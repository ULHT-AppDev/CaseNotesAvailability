﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CNAModel : DbContext
    {
        public CNAModel()
            : base("name=CNAModel")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<AuditClincAnswer> AuditClincAnswers { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<Specilaty> Specilaties { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<UnavailableCaseNote> UnavailableCaseNotes { get; set; }
        public virtual DbSet<LoginRight> LoginRights { get; set; }
        public virtual DbSet<LoginRoleRight> LoginRoleRights { get; set; }
        public virtual DbSet<LoginRole> LoginRoles { get; set; }
        public virtual DbSet<LogAction> LogActions { get; set; }
        public virtual DbSet<LogDetail> LogDetails { get; set; }
        public virtual DbSet<LogError> LogErrors { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<LogSession> LogSessions { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<PersonRole> PersonRoles { get; set; }
        public virtual DbSet<PersonUniqueGuid> PersonUniqueGuids { get; set; }
        public virtual DbSet<Genericuser> Genericusers { get; set; }
        public virtual DbSet<List_ReasonUnavailable> List_ReasonUnavailable { get; set; }
        public virtual DbSet<RequiresImprovementActionPoint> RequiresImprovementActionPoints { get; set; }
        public virtual DbSet<RequiresImprovementDetail> RequiresImprovementDetails { get; set; }
        public virtual DbSet<userrole> userroles { get; set; }
    }
}