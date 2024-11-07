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
    private readonly IUserManager _userManager;

    public UpdateSubCategoryCommandHandler(ISubCategoryRepository subCategoryRepository, IUserManager userManager)
    {
        _subCategoryRepository = subCategoryRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();
        var subCategory = await _subCategoryRepository.GetAsync(s=>s.Id==request.Id);

        if (subCategory == null)
            throw new NotFoundException(UIMessage.NotFound("Sub-category"));

        if (await _subCategoryRepository.IsPropertyUniqueAsync(x => x.Name, request.Name, request.Id))
            throw new DomainException(UIMessage.AlreadyExsist("Name"));

        subCategory.SetDetailsForUpdate(request.Name, request.CategoryId, userId);

        await _subCategoryRepository.Update(subCategory);
        await _subCategoryRepository.Commit(cancellationToken);

        return true;
    }
}

