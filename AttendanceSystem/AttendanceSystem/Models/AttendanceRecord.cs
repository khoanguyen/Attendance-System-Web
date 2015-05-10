//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AttendanceSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AttendanceRecord
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SessionId { get; set; }
        public int TicketId { get; set; }
        public System.DateTime RecordDate { get; set; }
        public System.DateTimeOffset CheckinTime { get; set; }
    
        public virtual ClassSession ClassSession { get; set; }
        public virtual Student Student { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
