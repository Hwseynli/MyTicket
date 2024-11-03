using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Categories;

namespace MyTicket.Application.Features.Commands.Tag.Category.Create;
public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserManager _userManager;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUserManager userManager)
    {
        _categoryRepository = categoryRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();
        var category = new Domain.Entities.Categories.Category();
        category.SetDetails(request.Name, userId);

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.Commit(cancellationToken);

        return true;
    }
}

