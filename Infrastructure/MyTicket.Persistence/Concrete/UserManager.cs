using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Domain.Entities.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyTicket.Persistence.Concrete;
public class UserManager : IUserManager
{
    private readonly IClaimManager _claimManager;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public UserManager(IClaimManager claimManager, IConfiguration configuration, IUserRepository userRepository)
    {
        _claimManager = claimManager;
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public (string token, DateTime expireAt) GenerateTJwtToken(User user)
    {
        var claims = new List<Claim> {
            //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role.Name) // Add this claim for the role
        };
        claims.AddRange(_claimManager.GetUserClaims(user));
        var jwtSettings = _configuration.GetSection("JWTSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
        var creadentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireAt = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireAt"]));
        var token = new JwtSecurityToken
            (claims: claims,
            expires: expireAt,
            signingCredentials: creadentials
            );
        var tokenHandler = new JwtSecurityTokenHandler();
        return (tokenHandler.WriteToken(token), expireAt);
    }

    public async Task<User> GetCurrentUser(params string[]? includes)
    {
        int userId = _claimManager.GetCurrentUserId();
        if (userId <= 0)
            throw new BadRequestException();
        var user = await _userRepository.GetAsync(x => x.Id == userId, includes);
        if (user == null)
            throw new UnAuthorizedException();
        return user;
    }

    public async Task<int> GetCurrentUserId()
    {
        return (await GetCurrentUser()).Id;
    }
}
