using System.Net.Http.Headers;
using System.Text;

namespace MyTicket.Infrastructure.Utils;
public class BankClient
{
    private readonly HttpClient _httpClient;

    public BankClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://txpgtst.kapitalbank.az/api/");

        var basicAuth = Convert.ToBase64String(Encoding.ASCII.GetBytes("TerminalSys/kapital:kapital123"));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
    }

    public async Task<string> GetAsync(string request) => await _httpClient.GetStringAsync(request);
    public async Task<HttpResponseMessage> PostAsync(string request, StringContent content) => await _httpClient.PostAsync(request, content);
}

