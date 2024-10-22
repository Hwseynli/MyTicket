using MediatR;

namespace MyTicket.Application.Features.Commands.Admin.Setting.Create;
public class CreateSettingCommand:IRequest<bool>
{
    public string Key { get; set; }
    public string Value { get; set; }
}