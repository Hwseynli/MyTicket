using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using System.Text.Json;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Orders;
using MyTicket.Domain.Entities.PromoCodes;
using MyTicket.Domain.Exceptions;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SkiaSharp;
using Stripe;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Persistence.Concrete;
public class OrderManager : IOrderManager
{
    private readonly IPromoCodeRepository _promoCodeRepository;
    private readonly BankClient _client;

    public OrderManager(IPromoCodeRepository promoCodeRepository, BankClient client)
    {
        _promoCodeRepository = promoCodeRepository;
        _client = client;
    }

    public async Task<PromoCode> GetPromoCodeByIdAsync(int promoCodeId, int userId)
    {
        if (promoCodeId <= 0)
            throw new BadRequestException("promoCodeId is invalid");
        PromoCode promoCode = await _promoCodeRepository.GetAsync(x => x.Id == promoCodeId, nameof(promoCode.UserPromoCodes));
        if (promoCode == null)
            throw new NotFoundException("PromoCode not fount");
        if(promoCode.UserPromoCodes.Any(x => x.UserId == userId))
            throw new BadRequestException("Eyni promocodu 2 ci defe itifadə edə bilmərsiniz)");
        return promoCode;
    }
    public StringContent PaymentForKapital(decimal totalAmount, string orderCode)
    {
        var order = new
        {
            order = new
            {
                typeRid = "Order_SMS",
                amount = totalAmount.ToString("F2"),
                currency = "AZN",
                language = "az",
                description = orderCode,
                hppRedirectUrl = $"https://luxride.themepanthers.com/lux/",
                hppCofCapturePurposes = new[] { "Cit" }
            }
        };

        var json = JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return content;
    }

    public async Task PaymentForStripe(string token_visa, string email, string firstName, string lastName, string phoneNumber, decimal orderTotalAmount)
    {
        var optionCust = new CustomerCreateOptions
        {
            Email = email,
            Name = firstName + " " + lastName,
            Phone = phoneNumber
        };
        var serviceCust = new CustomerService();
        Customer customer = serviceCust.Create(optionCust);

        // Stripe ödənişi
        var chargeOptions = new ChargeCreateOptions
        {
            Amount = (long)(orderTotalAmount * 100),
            Currency = "USD",
            Description = "Ticket Order Payment",
            Source = token_visa, // Frontend-dən alınan Source burada istifadə olunmalıdır
            ReceiptEmail = email
        };

        var serviceCharge = new ChargeService();
        Charge charge = await serviceCharge.CreateAsync(chargeOptions);

        if (charge.Status != "succeeded")
            throw new DomainException("Ödəniş uğursuz oldu.");
    }

    public byte[] GenerateReceipt(Domain.Entities.Orders.Order order, decimal discountAmount)
    {
        using var ms = new MemoryStream();
        using (var doc = new PdfDocument())
        {
            // Define fonts and colors
            var fontTitle = new XFont("Verdana", 16, XFontStyle.Bold);
            var fontRegular = new XFont("Verdana", 12, XFontStyle.Regular);
            var fontSmall = new XFont("Verdana", 10, XFontStyle.Regular);

            var colorGray = XBrushes.Gray;
            var colorBlue = XBrushes.Blue;
            var colorGreen = XBrushes.Green;
            var colorRed = XBrushes.Red;

            // First Page - Order Information
            var firstPage = doc.AddPage();
            var graphics = XGraphics.FromPdfPage(firstPage);

            graphics.DrawString("Order Receipt", fontTitle, colorBlue, new XPoint(30, 40));

            // Draw a separating line
            graphics.DrawLine(XPens.Gray, 30, 60, 540, 60);

            graphics.DrawString($"Order ID: {order.Id}", fontRegular, colorGray, new XPoint(30, 80));
            graphics.DrawString($"Total Amount: ${order.TotalAmount + discountAmount}", fontRegular, colorGray, new XPoint(30, 100));

            // Display Promo Code and Discount Info
            if (order.PromoCode != null)
            {
                decimal discountedAmount = discountAmount;
                graphics.DrawString($"Promo Code: {order.PromoCode.UniqueCode}", fontRegular, colorGreen, new XPoint(30, 130));
                graphics.DrawString($"Discounted Amount: ${discountedAmount}", fontRegular, colorRed, new XPoint(30, 150));
            }
            else
            {
                graphics.DrawString("Promo Code: Not Applied", fontRegular, colorGray, new XPoint(30, 130));
                graphics.DrawString($"Discounted Amount: ${discountAmount}", fontRegular, colorRed, new XPoint(30, 150));
            }

            // Final Price
            decimal finalPrice = order.TotalAmount;
            graphics.DrawString($"Final Price: ${finalPrice}", fontTitle, colorBlue, new XPoint(30, 180));

            // Add pages for each ticket
            foreach (var ticket in order.Tickets)
            {
                var ticketPage = doc.AddPage();
                var ticketGraphics = XGraphics.FromPdfPage(ticketPage);

                // Header for ticket
                ticketGraphics.DrawString("Ticket Information", fontTitle, colorBlue, new XPoint(30, 40));
                ticketGraphics.DrawLine(XPens.Gray, 30, 60, 540, 60);

                // Ticket details
                ticketGraphics.DrawString($"Name: {ticket.User.FirstName} {ticket.User.LastName}", fontRegular, colorGray, new XPoint(30, 80));
                ticketGraphics.DrawString($"Event: {ticket.Event.Title}", fontRegular, colorGray, new XPoint(30, 110));
                ticketGraphics.DrawString($"Date: {ticket.Event.StartTime:MMM dd, yyyy HH:mm}", fontRegular, colorGray, new XPoint(30, 140));
                ticketGraphics.DrawString($"Seat: {ticket.Seat.SeatNumber}", fontRegular, colorGray, new XPoint(30, 170));
                ticketGraphics.DrawString($"Row: {ticket.Seat.RowNumber}", fontRegular, colorGray, new XPoint(30, 200));
                ticketGraphics.DrawString($"Place Hall: {ticket.Seat.PlaceHall.Name}", fontRegular, colorGray, new XPoint(30, 230));
                ticketGraphics.DrawString($"Location: {ticket.Seat.PlaceHall.Place.Name}", fontRegular, colorGray, new XPoint(30, 260));
                ticketGraphics.DrawString($"Price: ${ticket.Price}", fontRegular, colorBlue, new XPoint(30, 290));

                // Generate barcode for ticket
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Height = 50,
                        Width = 220,
                        Margin = 1
                    }
                };

                var barcodeBitmap = barcodeWriter.Write(ticket.UniqueCode);
                using (var image = SKImage.FromBitmap(barcodeBitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var barcodeStream = new MemoryStream())
                {
                    data.SaveTo(barcodeStream);
                    barcodeStream.Position = 0;

                    var barcodeImage = XImage.FromStream(() => new MemoryStream(barcodeStream.ToArray()));
                    ticketGraphics.DrawImage(barcodeImage, 30, 320, 200, 50);
                }

                ticketGraphics.DrawString($"Code: {ticket.UniqueCode}", fontSmall, colorGray, new XPoint(30, 380));
            }

            doc.Save(ms);
        }

        return ms.ToArray();
    }

}

