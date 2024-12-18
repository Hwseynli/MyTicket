﻿using MyTicket.Domain.Entities.Users;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Domain.Entities.Places;
public class Place:Editable<User>
{
    public string Name { get; private set; }
    public string Address { get; private set; }
    public List<PlaceHall> PlaceHalls { get; set; }

    public void SetDetails(string name, string address,int createdById)
    {
        Name = name.Capitalize();
        Address = address;
        PlaceHalls = new List<PlaceHall>();
        SetAuditDetails(createdById);
    }

    public void AddHall(PlaceHall hall)
    {
        PlaceHalls.Add(hall);
    }

    public void SetDetailsForUpdate(string name,string address,int updateById)
    {
        Name = name.Capitalize();
        Address = address;
        SetEditFields(updateById);
    }
}