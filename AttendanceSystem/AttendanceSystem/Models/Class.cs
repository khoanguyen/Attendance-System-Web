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
    
    public partial class Class
    {
        public Class()
        {
            this.ClassSessions = new HashSet<ClassSession>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfessorName { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<System.TimeSpan> ExcusedTime { get; set; }
    
        public virtual ICollection<ClassSession> ClassSessions { get; set; }
    }
}