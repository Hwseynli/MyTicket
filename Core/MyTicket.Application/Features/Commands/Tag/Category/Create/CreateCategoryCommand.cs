using MediatR;

namespace MyTicket.Application.Features.Commands.Tag.Category.Create;
public class CreateCategoryCommand : IRequest<bool>
{
    public string Name { get; set; }
}

