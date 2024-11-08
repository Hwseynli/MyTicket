using MediatR;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Create;
public class CreateSubCategoryCommand : IRequest<bool>
{
    public string Name { get; set; }
    public List<int> CategoryIds { get; set; } //there should be 1 thousand attempts
}

