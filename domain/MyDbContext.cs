using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace domain
{
    public class MyDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DocumentationType> DocumentationTypes { get; set; }

        public MyDbContext(string connection) : base(connection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Определение ограничений, индексов и других настроек для моделей, если необходимо
            modelBuilder.Entity<Project>().HasKey(p => p.ProjectNumber);
            modelBuilder.Entity<Customer>().HasKey(c => c.Number);
            modelBuilder.Entity<DocumentationType>().HasKey(d => d.DocumentationName);

            modelBuilder.Entity<Project>().Property(p => p.ProjectNumber).IsRequired();
            modelBuilder.Entity<Project>().Property(p => p.ProjectName).IsRequired();
            modelBuilder.Entity<Project>().Property(p => p.Cost).IsRequired();
            modelBuilder.Entity<Project>().Property(p => p.DocumentationType).IsRequired();
            modelBuilder.Entity<Project>().Property(p => p.Inginner).IsRequired();
            modelBuilder.Entity<Project>().Property(p => p.Contract).IsRequired();
            modelBuilder.Entity<Project>()
                .HasRequired(p => p.Client)
                .WithMany()
                .HasForeignKey(p => p.CustomerNumber);
            modelBuilder.Entity<Project>()
                .HasRequired(p => p.Type)
                .WithMany()
                .HasForeignKey(p => p.DocumentationType);

            modelBuilder.Entity<Customer>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.PhoneNumber).IsRequired();
        }

        [Table("Projects")]
        public class Project
        {
            public string ProjectNumber { get; set; }
            public string ProjectName { get; set; }
            public decimal Cost { get; set; }
            public string DocumentationType { get; set; }
            public string Inginner { get; set; }
            public int CustomerNumber { get; set; }
            public string City { get; set; }
            public string Contract { get; set; }
            public string DateStartWork { get; set; }
            public string DateEndWork { get; set; }


            [ForeignKey("CustomerNumber")]
            public virtual Customer Client { get; set; }

            [ForeignKey("DocumentationType")]
            public virtual DocumentationType Type { get; set; }
        }

        [Table("Customers")]
        public class Customer
        {
            public int Number { get; set; }
            public string FirstName { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string Company { get; set; }
            public string Traffic { get; set; }
            public int TIN { get; set; }
        }

        [Table("DocumentationTypes")]
        public class DocumentationType
        {
            public string DocumentationName { get; set; }
        }
    }
}
