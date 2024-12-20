using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain
{
    public class DatabaseModel
    {
        private string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=YourDatabase.mdb;";

        private readonly MyDbContext _dbContext;

        public DatabaseModel()
        {
            _dbContext = new MyDbContext(connectionString);
        }

        // SELECT
        public IQueryable<Project> GetAllProjects()
        {
            return (IQueryable<Project>)_dbContext.Projects;
        }
        public IQueryable<Project> GetAllCustomers()
        {
            return (IQueryable<Project>)_dbContext.Customers;
        }

        // INSERT
        public void AddProject(Project project)
        {
            MyDbContext.Project projectTable = new MyDbContext.Project();
            projectTable.ProjectNumber = project.ProjectNumber;
            projectTable.ProjectName = project.ProjectName;
            projectTable.CustomerNumber = project.Client.Number;
            projectTable.City = project.City;
            projectTable.Cost = project.Cost;
            projectTable.Contract = project.Contract;
            projectTable.DocumentationType = project.DocumentationType;
            projectTable.DateStartWork = project.DateStartWork;
            projectTable.DateEndWork = project.DateEndWork;
            projectTable.Inginner = project.Inginner;
            _dbContext.Projects.Add(projectTable);
            _dbContext.SaveChanges();
        }
        public void AddCustomer(Customer customer)
        {
            MyDbContext.Customer customerTable = new MyDbContext.Customer();
            customerTable.Number = customer.Number;
            customerTable.FirstName = customer.FirstName;
            customerTable.Name = customer.Name;
            customerTable.LastName = customer.LastName;
            customerTable.PhoneNumber = customer.PhoneNumber;
            customerTable.Email = customer.Email;
            customerTable.Company = customer.Company;
            customerTable.Traffic = customer.Traffic;
            customerTable.TIN = customer.TIN;
            _dbContext.Customers.Add(customerTable);
            _dbContext.SaveChanges();
        }

        // UPDATE
        public void UpdateProject(Project project)
        {
            var existingProject = _dbContext.Projects.FirstOrDefault(p => p.ProjectNumber == project.ProjectNumber);
            if (existingProject != null)
            {
                existingProject.ProjectName = project.ProjectName;
                existingProject.CustomerNumber = project.Client.Number;
                existingProject.City = project.City;
                existingProject.Cost = project.Cost;
                existingProject.Contract = project.Contract;
                existingProject.DocumentationType = project.DocumentationType;
                existingProject.DateStartWork = project.DateStartWork;
                existingProject.DateEndWork = project.DateEndWork;
                existingProject.Inginner = project.Inginner;
                _dbContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Проект не найден.");
            }
        }
        public void UpdateCustomer(Customer customer)
        {
            var existingCustomer = _dbContext.Customers.FirstOrDefault(p => p.Number == customer.Number);
            if (existingCustomer != null)
            {
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.Name = customer.Name;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.PhoneNumber = customer.PhoneNumber;
                existingCustomer.Email = customer.Email;
                existingCustomer.Company = customer.Company;
                existingCustomer.Traffic = customer.Traffic;
                existingCustomer.TIN = customer.TIN;
                _dbContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Закзчик не найден.");
            }
        }
    }
}
