﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AASDBContext : DbContext
    {
        public AASDBContext()
            : base("name=AASDBContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ClassSession> ClassSessions { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    }
}
