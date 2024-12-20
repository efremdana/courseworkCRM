using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace domain
{
    public class MyDbContext : DbContext
    {
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<DocumentationTypeModel> DocumentationTypes { get; set; }
        public DbSet<TypeTurnoverModel> TypesTurnover { get; set; }
        public DbSet<TurnoverModel> Turnovers { get; set; }

        public MyDbContext() : base("name=SQLiteConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectModel>().HasKey(p => p.ProjectNumber);
            modelBuilder.Entity<CustomerModel>().HasKey(c => c.Number);
            modelBuilder.Entity<DocumentationTypeModel>().HasKey(d => d.DocumentationName);
            modelBuilder.Entity<TypeTurnoverModel>().HasKey(type => type.NameTurnover);
            modelBuilder.Entity<TurnoverModel>().HasKey(t => t.Id);

            modelBuilder.Entity<ProjectModel>().Property(p => p.ProjectNumber).IsRequired();
            modelBuilder.Entity<ProjectModel>().Property(p => p.ProjectName).IsRequired();
            modelBuilder.Entity<ProjectModel>().Property(p => p.Cost).IsRequired();
            modelBuilder.Entity<ProjectModel>().Property(p => p.DocumentationType).IsRequired();
            modelBuilder.Entity<ProjectModel>().Property(p => p.Inginner).IsRequired();
            modelBuilder.Entity<ProjectModel>().Property(p => p.CustomerNumber).IsRequired();
            modelBuilder.Entity<ProjectModel>().Property(p => p.Contract).IsRequired();
            modelBuilder.Entity<ProjectModel>()
                .HasRequired(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerNumber);
            modelBuilder.Entity<ProjectModel>()
                .HasRequired(p => p.Type)
                .WithMany()
                .HasForeignKey(p => p.DocumentationType);

            modelBuilder.Entity<CustomerModel>().Property(c => c.Number).IsRequired();
            modelBuilder.Entity<CustomerModel>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<CustomerModel>().Property(c => c.PhoneNumber).IsRequired();

            modelBuilder.Entity<DocumentationTypeModel>().Property(d => d.DocumentationName).IsRequired();

            modelBuilder.Entity<TypeTurnoverModel>().Property(type => type.NameTurnover).IsRequired();

            modelBuilder.Entity<TurnoverModel>().Property(t => t.Id).IsRequired();
            modelBuilder.Entity<TurnoverModel>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<TurnoverModel>().Property(t => t.Day).IsRequired();
            modelBuilder.Entity<TurnoverModel>().Property(t => t.Month).IsRequired();
            modelBuilder.Entity<TurnoverModel>().Property(t => t.Year).IsRequired();
            modelBuilder.Entity<TurnoverModel>().Property(t => t.Denomination).IsRequired();
            modelBuilder.Entity<TurnoverModel>()
                .HasRequired(t => t.Type)
                .WithMany()
                .HasForeignKey(t => t.TypeTurnover);
        }
    }

    [Table("Projects")]
    public class ProjectModel
    {
        public string ProjectNumber { get; set; }
        public string ProjectName { get; set; }
        public decimal Cost { get; set; }
        public string DocumentationType { get; set; }
        public string Inginner { get; set; }
        public int CustomerNumber { get; set; }
        private string _city;
        public string City
        {
            get => _city;
            set => _city = value ?? "";
        }
        public string Contract { get; set; }
        private string _dateStartWork;
        public string DateStartWork
        {
            get => _dateStartWork;
            set => _dateStartWork = value ?? "";
        }
        private string _dateEndWork;
        public string DateEndWork
        {
            get => _dateEndWork;
            set => _dateEndWork = value ?? "";
        }

        [ForeignKey("CustomerNumber")]
        public virtual CustomerModel Customer { get; set; }

        [ForeignKey("DocumentationType")]
        public virtual DocumentationTypeModel Type { get; set; }

    }

    [Table("Customers")]
    public class CustomerModel
    {
        public int Number { get; set; }
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => _firstName = value ?? "";
        }
        public string Name { get; set; }
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => _lastName = value ?? "";
        }
        public string PhoneNumber { get; set; }
        private string _email;
        public string Email
        {
            get => _email;
            set => _email = value ?? "";
        }
        private string _company;
        public string Company
        {
            get => _company;
            set => _company = value ?? "";
        }
        private string _traffic;
        public string Traffic
        {
            get => _traffic;
            set => _traffic = value ?? "";
        }
        public int? TIN { get; set; }
    }

    [Table("DocumentationTypes")]
    public class DocumentationTypeModel
    {
        public string DocumentationName { get; set; }
    }

    [Table("TypesTurnover")]
    public class TypeTurnoverModel
    {
        public string NameTurnover { get; set; }
    }

    [Table("FinancialTurnovers")]
    public class TurnoverModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Denomination { get; set; }
        public string TypeTurnover { get; set; }

        [ForeignKey("TypeTurnover")]
        public virtual TypeTurnoverModel Type { get; set; }
    }
}
