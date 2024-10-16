using MediatR;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Create;
public class CreateSubCategoryCommand : IRequest<bool>
{
    public string Name { get; set; }
    public int CategoryId { get; set; }
}

