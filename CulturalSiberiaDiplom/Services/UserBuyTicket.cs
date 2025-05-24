using System;
using CulturalSiberiaDiplom.Models;

namespace CulturalSiberiaDiplom.Services;

public static class UserBuyTicket
{
    public static async void BuyTicket(User user, Event? @event, Museum? museum)
    {
        await using var context = new CulturalSiberiaContext();
        
        var ticket = new Ticket
        {
            UserId = user.Id,
            StatusId = 1,
            PurchaseDate = DateTime.Now
        };

        if (@event != null)
        {
            ticket.TicketTargetTypeId = 1;
            ticket.EventId = @event.Id;
        }
        
        else if (museum != null)
        {
            ticket.TicketTargetTypeId = 2;
            ticket.MuseumId = museum.Id;
        }

        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();
    }
}