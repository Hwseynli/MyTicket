using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Exceptions;

namespace MyTicket.Persistence.Concrete;
public class SmsManager:ISmsManager
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _client;

    public SmsManager(IConfiguration configuration, HttpClient client)
    {
        _configuration = configuration;
        _client = client;
    }

    public async Task SendSmsAsync(string phone, string subject, string body)
    {

        var smsSettings = _configuration.GetSection("Sms");
        var text = subject + "\n" + body;

        var apiUrl = smsSettings["ApiUrl"].Replace("{AccountSid}", smsSettings["AccountSid"]);
        var accountSid = smsSettings["AccountSid"];
        var authToken = smsSettings["AuthToken"];
        // Set Authorization Header with Basic Authentication
        var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{accountSid}:{authToken}"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

        // Prepare SMS data (this depends on the API you're using)
        var payload = new Dictionary<string, string>
    {
        { "To", phone },
        { "From", "YourTwilioPhoneNumber" }, // Twilio-da qeydiyyatdan keçmiş telefon nömrənizi buraya yazın
        { "Body", text }
    };

        var content = new FormUrlEncodedContent(payload);

        // Send POST request
        var response = await _client.PostAsync(apiUrl, content);

        // Check if the request was successful
        if (!response.IsSuccessStatusCode)
            throw new UnAuthorizedException("Failed to send SMS.");
    }
}

