using MediatR;

namespace MyTicket.Application.Features.Commands.Admin.Setting.Update;
public class UpdateSettingCommand:IRequest<bool>
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}