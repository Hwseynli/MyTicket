using MediatR;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Update;
public class UpdateSubCategoryCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> CategoryIds { get; set; }
}

