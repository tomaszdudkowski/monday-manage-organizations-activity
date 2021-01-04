using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mondayWebApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace mondayWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>()
                .HasKey(e => e.AddressID);

            modelBuilder.Entity<Employee>()
                .HasOne<Address>(p => p.EmployeeAddress)
                .WithOne(r => r.Employee)
                .HasForeignKey<Address>(s => s.EmployeeID);

            modelBuilder.Entity<Employee>()
                .HasOne<Department>(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Employee>()
                .HasOne<Project>(p => p.Project)
                .WithMany(e => e.Employees)
                .HasForeignKey(p => p.ProjectID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Task>()
                .HasOne<Project>(p => p.Project)
                .WithMany(t => t.Tasks)
                .HasForeignKey(p => p.ProjectID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
