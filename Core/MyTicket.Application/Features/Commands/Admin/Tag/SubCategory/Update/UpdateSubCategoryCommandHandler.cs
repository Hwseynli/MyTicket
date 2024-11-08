using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Categories;
using MyTicket.Domain.Exceptions;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Update;
public class UpdateSubCategoryCommandHandler : IRequestHandler<UpdateSubCategoryCommand, bool>
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserManager _userManager;

    public UpdateSubCategoryCommandHandler(ISubCategoryRepository subCategoryRepository, IUserManager userManager, ICategoryRepository categoryRepository)
    {
        _subCategoryRepository = subCategoryRepository;
        _userManager = userManager;
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(UpdateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();

        if (request.CategoryIds == null || !request.CategoryIds.Any())
            throw new DomainException("At least one category is required.");

        var subCategory = await _subCategoryRepository.GetAsync(s=>s.Id==request.Id,"Categories");

        if (subCategory == null)
            throw new NotFoundException(UIMessage.NotFound("Sub-category"));

        var categories = await _categoryRepository.GetAllAsync(sc => request.CategoryIds.Contains(sc.Id)&& !subCategory.Categories.Any(x => x.Id == sc.Id));

        if (await _subCategoryRepository.IsPropertyUniqueAsync(x => x.Name, request.Name, request.Id))
            throw new DomainException(UIMessage.AlreadyExsist("Name"));

        subCategory.SetDetailsForUpdate(request.Name, categories, userId);

        await _subCategoryRepository.Update(subCategory);
        await _subCategoryRepository.Commit(cancellationToken);

        return true;
    }
}

