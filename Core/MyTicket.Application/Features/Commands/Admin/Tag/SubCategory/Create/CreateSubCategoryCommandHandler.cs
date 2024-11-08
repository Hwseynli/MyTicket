using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Categories;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Create;
public class CreateSubCategoryCommandHandler : IRequestHandler<CreateSubCategoryCommand, bool>
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserManager _userManager;

    public CreateSubCategoryCommandHandler(ISubCategoryRepository subCategoryRepository, IUserManager userManager, ICategoryRepository categoryRepository)
    {
        _subCategoryRepository = subCategoryRepository;
        _userManager = userManager;
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(CreateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();

        if (request.CategoryIds == null || !request.CategoryIds.Any())
            throw new DomainException("At least one category is required.");


        var categories = await _categoryRepository.GetAllAsync(sc => request.CategoryIds.Contains(sc.Id));

        var subCategory = new Domain.Entities.Categories.SubCategory();
        subCategory.SetDetails(request.Name, categories, userId);

        await _subCategoryRepository.AddAsync(subCategory);
        await _subCategoryRepository.Commit(cancellationToken);

        return true;
    }
}

