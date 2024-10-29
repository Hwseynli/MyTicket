using System.Text;
using System.Text.Json;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Domain.Entities.Orders;

namespace MyTicket.Persistence.Concrete;
public class PaymentManager : IPaymentManager
{
    private readonly HttpClient _httpClient;
    private const string PaymentUrl = "https://api.birbank.business/payment"; // Dəyişdirmək lazımdır

    public PaymentManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> ProcessPaymentAsync(Order order)
    {
        var paymentRequest = new
        {
            orderId = order.Id,
            amount = order.TotalAmount,
            currency = "AZN",
            description = "Order Payment"
        };

        var requestContent = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(PaymentUrl, requestContent);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(responseContent); // Ensure `PaymentResponse` matches API schema

            if (paymentResponse?.Status=="success")
            {
                order.MarkAsPaid();
                return paymentResponse?.Status ?? "Payment initialization failed";
            }
        }

        throw new Exception("Failed to initialize payment.");
    }

    public async Task<bool> CompleteOrderPayment(Order order)
    {
        var paymentService = new PaymentManager(new HttpClient());
        bool paymentResult = await paymentService.ProcessPaymentAsync(order)=="success";

        if (!paymentResult)
        {
            throw new Exception("Ödəniş baş tutmadı.");
        }

        // Əlavə məlumatları yadda saxlamaq üçün burada order statusunu yeniləyə bilərsiniz.
        return paymentResult;
    }

}
public class PaymentResponse
{
    public string Status { get; set; }
}
