using MyTicket.Domain.Common;

namespace MyTicket.Domain.Entities.Orders;
public class Payment : BaseEntity
{
    public decimal Amount { get; private set; }
    public int OrderId { get; private set; }
    public Order Order { get; private set; }
    public string Status { get; private set; } // e.g., "Pending", "Completed", "Failed"
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public Payment(decimal amount, int orderId)
    {
        Amount = amount;
        OrderId = orderId;
        Status = "Pending";
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        Status = "Completed";
        CompletedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = "Failed";
    }
}

