using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Migrations;
using Echomedproject.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Echomedproject.DAL.Contexts
{
    public class EckomedDbContext:IdentityDbContext
    {

        public EckomedDbContext(DbContextOptions<EckomedDbContext>options): base (options){}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{ 
        //    optionsBuilder.UseSqlServer("Server=.;database=EckoMEd;trusted_Connection=true;trustservercertificate=true");

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hospital)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.Id)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascading delete

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Department)
                .WithMany(d => d.Rooms)
                .HasForeignKey(r => r.Id)
                .OnDelete(DeleteBehavior.Cascade); // Allows cascading delete
        }

        public DbSet<DataEntry> dataEntry { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Hospitals> hospitals { get; set; }
        public DbSet<Patients> Patients { get; set; }

        public DbSet<Invoice> invoices { get; set; }
        public DbSet<LabTest> labTests { get; set; }

        public DbSet<prescription> prescriptions    { get; set; }
        public DbSet<Records> Records { get; set; } 
        public DbSet<Room> rooms { get; set; } 
        public DbSet<Scans> Scans { get; set; } 

        public DbSet<AppUsers> Users { get; set; } 
        public DbSet<PatientHospital> patientHospital { get; set; }
        public DbSet<Users> Appusers { get; set; }
        public DbSet<Medicine> Midicine { get; set; }

        public DbSet<Charge> Charge { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<Pharmacy> pharmacies { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<PharmacyAcc> pharmacyAccs { get; set; }

        public DbSet<Request> requests { get; set; }
        public DbSet <Note> notes { get; set; }


    }
}
