﻿using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Domain.Entities.Places;
public class Seat : Editable<User>
{
    public int RowNumber { get; private set; }
    public int SeatNumber { get; private set; }
    public SeatType SeatType { get; set; }
    public decimal Price { get; private set; }
    public int PlaceHallId { get; private set; }
    public PlaceHall? PlaceHall { get; private set; }
    public List<Ticket> Tickets { get; private set; }

    public void SetDetail(int rowNumber, int seatNumber, SeatType seatType, decimal price,int placeHallId, int userId)
    {
        if (rowNumber <= 0 || seatNumber <= 0)
            throw new DomainException(UIMessage.GreaterThanZero("Row and seat numbers"));
        if (price <= 0)
            throw new DomainException(UIMessage.GreaterThanZero("Price"));

        PlaceHallId = placeHallId;
        RowNumber = rowNumber;
        SeatNumber = seatNumber;
        SeatType = seatType;
        Price = price;
        Tickets = new List<Ticket>();
        SetAuditDetails(userId);
    }
    public void SetDetailForUpdate(int rowNumber, int seatNumber, SeatType seatType, decimal price, int modifiedById)
    {
        if (rowNumber <= 0 || seatNumber <= 0)
            throw new DomainException(UIMessage.GreaterThanZero("Row and seat numbers"));
        if (price <= 0)
            throw new DomainException(UIMessage.GreaterThanZero("Price"));

        RowNumber = rowNumber;
        SeatNumber = seatNumber;
        SeatType = seatType;
        Price = price;
        SetEditFields(modifiedById);
    }

    public SeatType DetermineSeatType(int row, int totalRows)
    {
        int front = totalRows / 3;
        int middle = front * 2;

        // Here we define the type of each row
        if (row <= front)
            return SeatType.FrontRow;
        else if (row > front && row <= middle)
            return SeatType.MiddleRow;
        else
            return SeatType.BackRow;
    }

    public decimal CalculateSeatPrice(SeatType seatType, decimal price=100)
    {
        decimal finelPrice = price;
        switch (seatType)
        {
            case SeatType.FrontRow:
                finelPrice = price * 2;
                return finelPrice;
            case SeatType.MiddleRow:
                finelPrice = price + (price * 3 / 4);
                return finelPrice;
            case SeatType.BackRow:
                finelPrice = price;
                return finelPrice;
            default:
                throw new DomainException(UIMessage.ValidProperty("Seat type"));
        }
    }
}

