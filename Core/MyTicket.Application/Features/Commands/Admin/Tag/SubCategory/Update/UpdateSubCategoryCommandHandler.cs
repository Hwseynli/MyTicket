using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Categories;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Update;
public class UpdateSubCategoryCommandHandler : IRequestHandler<UpdateSubCategoryCommand, bool>
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IUserManager _userManager;

    public UpdateSubCategoryCommandHandler(ISubCategoryRepository subCategoryRepository, IUserManager userManager)
    {
        _subCategoryRepository = subCategoryRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoryRepository.GetAsync(s=>s.Id==request.Id);

        if (subCategory == null)
            throw new NotFoundException("SubCategory not found.");

        subCategory.SetDetailsForUpdate(request.Name, request.CategoryId, _userManager.GetCurrentUserId());

        await _subCategoryRepository.Update(subCategory);
        await _subCategoryRepository.Commit(cancellationToken);

        return true;
    }
}

