using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Categories;

namespace MyTicket.Application.Features.Commands.Tag.Category.Update;
public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserManager _userManager;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUserManager userManager)
    {
        _categoryRepository = categoryRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();
        var category = await _categoryRepository.GetAsync(c=>c.Id==request.Id);

        if (category == null)
            throw new NotFoundException("Category not found.");

        if (!await _categoryRepository.IsPropertyUniqueAsync(c=>c.Name,request.Name,category.Id))
            throw new ValidationException();

        category.SetDetailsForUpdate(request.Name, userId);

        _categoryRepository.Update(category);
        await _categoryRepository.Commit(cancellationToken);

        return true;
    }
}

