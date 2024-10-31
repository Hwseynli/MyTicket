namespace MyTicket.Domain.Entities.Payments;
public class PaymentRequest
{
    public string OrderCode { get; set; } // Order ilə əlaqələndirilməli
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "AZN";
    public string Language { get; set; } = "az"; // dil parametrləri
    public string ReturnUrl { get; set; } // Ödəniş tamamlandıqdan sonra qaytarılma URL-i
}

