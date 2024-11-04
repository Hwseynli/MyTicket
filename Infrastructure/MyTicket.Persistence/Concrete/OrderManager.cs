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

namespace MyTicket.Persistence.Concrete;
public class OrderManager : IOrderManager
{
    private readonly IPromoCodeRepository _promoCodeRepository;

    public OrderManager(IPromoCodeRepository promoCodeRepository)
    {
        _promoCodeRepository = promoCodeRepository;
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

    public async Task Payment(string token_visa, string email, string firstName, string lastName, string phoneNumber, decimal orderTotalAmount)
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
            var fontTitle = new XFont("Verdana", 14, XFontStyle.Bold);
            var fontRegular = new XFont("Verdana", 12, XFontStyle.Regular);
            var fontSmall = new XFont("Verdana", 10, XFontStyle.Regular);

            // İlk səhifə - Order məlumatları
            var firstPage = doc.AddPage();
            var graphics = XGraphics.FromPdfPage(firstPage);

            graphics.DrawString($"Order ID: {order.Id}", fontTitle, XBrushes.Black, new XPoint(30, 50));
            graphics.DrawString($"Total Amount: ${order.TotalAmount + discountAmount}", fontRegular, XBrushes.Black, new XPoint(30, 80));

            if (order.PromoCode != null)
            {
                decimal discountedAmount = discountAmount; // Endirim hesablanır
                graphics.DrawString($"Promo Code: {order.PromoCode.UniqueCode}", fontRegular, XBrushes.Black, new XPoint(30, 110));
                graphics.DrawString($"Discounted Amount: ${discountedAmount}", fontRegular, XBrushes.Red, new XPoint(30, 140));
            }

            // Final Price calculation
            decimal finalPrice = order.TotalAmount; // Assuming TotalAmount already has discount applied
            graphics.DrawString($"Final Price: ${finalPrice}", fontRegular, XBrushes.Green, new XPoint(30, 170));


            // Hər bilet üçün ayrıca səhifə əlavə edilir
            foreach (var ticket in order.Tickets)
            {
                var ticketPage = doc.AddPage();
                var ticketGraphics = XGraphics.FromPdfPage(ticketPage);

                // Bilet məlumatları
                ticketGraphics.DrawString("Ticket", fontTitle, XBrushes.Black, new XPoint(30, 30));
                ticketGraphics.DrawString($"Name: {ticket.User.FirstName} {ticket.User.LastName}", fontRegular, XBrushes.Black, new XPoint(30, 70));
                ticketGraphics.DrawString($"Event: {ticket.Event.Title}", fontRegular, XBrushes.Black, new XPoint(30, 100));
                ticketGraphics.DrawString($"Date: {ticket.Event.StartTime:MMM dd, yyyy HH:mm}", fontRegular, XBrushes.Black, new XPoint(30, 130));
                ticketGraphics.DrawString($"Seat: {ticket.Seat}", fontRegular, XBrushes.Black, new XPoint(30, 160));
                ticketGraphics.DrawString($"Price: ${ticket.Price}", fontRegular, XBrushes.Black, new XPoint(30, 190));

                // Barkod yaradılması (SkiaSharp və ZXing ilə)
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Height = 50,
                        Width = 200,
                        Margin = 1
                    }
                };

                // Barkodu SKBitmap-ə çevir
                var barcodeBitmap = barcodeWriter.Write(ticket.UniqueCode);
                using (var image = SKImage.FromBitmap(barcodeBitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var barcodeStream = new MemoryStream())
                {
                    data.SaveTo(barcodeStream);
                    barcodeStream.Position = 0;

                    // PDF-də barkod olaraq göstər
                    var barcodeImage = XImage.FromStream(() => new MemoryStream(barcodeStream.ToArray()));
                    ticketGraphics.DrawImage(barcodeImage, 30, 220, 200, 50);
                }

                ticketGraphics.DrawString($"Code: {ticket.UniqueCode}", fontSmall, XBrushes.Black, new XPoint(30, 340));
            }

            doc.Save(ms);
        }

        return ms.ToArray();
    }

}

