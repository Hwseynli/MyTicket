using MediatR;

namespace MyTicket.Application.Features.Commands.Tag.Category.Update;
public class UpdateCategoryCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
}