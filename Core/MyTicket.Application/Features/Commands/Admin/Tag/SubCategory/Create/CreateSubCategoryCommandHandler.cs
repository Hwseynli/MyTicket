using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Categories;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Create;
public class CreateSubCategoryCommandHandler : IRequestHandler<CreateSubCategoryCommand, bool>
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IUserManager _userManager;

    public CreateSubCategoryCommandHandler(ISubCategoryRepository subCategoryRepository, IUserManager userManager)
    {
        _subCategoryRepository = subCategoryRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(CreateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();
        var subCategory = new Domain.Entities.Categories.SubCategory();
        subCategory.SetDetails(request.Name, request.CategoryId, userId);

        await _subCategoryRepository.AddAsync(subCategory);
        await _subCategoryRepository.Commit(cancellationToken);

        return true;
    }
}

