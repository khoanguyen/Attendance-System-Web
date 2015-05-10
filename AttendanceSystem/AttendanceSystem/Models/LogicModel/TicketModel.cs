using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models.LogicModel
{
    public class TicketModel
    {
        public string QrCode { get; set; }

        public TicketModel() { }
        public TicketModel(Ticket ticket)
        {
            if (ticket != null) this.QrCode = Convert.ToBase64String(ticket.QrCode);
        }
    }
}