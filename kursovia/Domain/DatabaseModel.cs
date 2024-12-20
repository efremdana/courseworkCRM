using kursovia.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace domain
{
    public class DatabaseModel
    {

        private readonly MyDbContext _dbContext;

        public DatabaseModel()
        {
            _dbContext = new MyDbContext();
        }

        // SELECT
        public IQueryable<ProjectModel> GetAllProjects()
        {
            return _dbContext.Projects;
        }
        public IQueryable<CustomerModel> GetAllCustomers()
        {
            return _dbContext.Customers;
        }

        public IQueryable<TurnoverModel> GetAllTurnovers(int year)
        {
            return _dbContext.Turnovers.Where(t => t.Year == year);
        }

        public Customer GetCustomerByNumber(int number)
        {
            CustomerModel customerModel = _dbContext.Customers.Where(c => c.Number == number).FirstOrDefault();
            return new Customer
            {
                Number = customerModel.Number,
                FirstName = customerModel.FirstName,
                Name = customerModel.Name,
                LastName = customerModel.LastName,
                PhoneNumber = customerModel.PhoneNumber,
                Email = customerModel.Email,
                Company = customerModel.Company,
                TIN = customerModel.TIN,
                Traffic = customerModel.Traffic,
            };
        }

        public List<Project> GetProjectsByCustomerNumber(int customerNumber)
        {
            List<Project> projects = new List<Project>();
            List<ProjectModel> projectModels = _dbContext.Projects.Where(p => p.CustomerNumber == customerNumber).ToList();
            foreach (ProjectModel projectModel in projectModels)
            {
                projects.Add(new Project
                {
                    ProjectNumber = projectModel.ProjectNumber,
                    ProjectName = projectModel.ProjectName,
                    Cost = projectModel.Cost,
                    DocumentationType = projectModel.DocumentationType,
                    Inginner = projectModel.Inginner,
                    City = projectModel.City,
                    Contract = projectModel.Contract,
                    DateStartWork = projectModel.DateStartWork,
                    DateEndWork = projectModel.DateEndWork,
                });
            }
            return projects;
        }

        // INSERT
        public void AddProject(Project project)
        {
            ProjectModel projectTable = new ProjectModel
            {
                ProjectNumber = project.ProjectNumber,
                ProjectName = project.ProjectName,
                CustomerNumber = project.Client.Number,
                City = project.City,
                Cost = project.Cost,
                Contract = project.Contract,
                DocumentationType = project.DocumentationType,
                DateStartWork = project.DateStartWork,
                DateEndWork = project.DateEndWork,
                Inginner = project.Inginner
            };
            _dbContext.Projects.Add(projectTable);
            _dbContext.SaveChanges();
        }
        public void AddCustomer(Customer customer)
        {
            CustomerModel customerTable = new CustomerModel
            {
                Number = customer.Number,
                FirstName = customer.FirstName,
                Name = customer.Name,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Company = customer.Company,
                Traffic = customer.Traffic,
                TIN = customer.TIN
            };
            _dbContext.Customers.Add(customerTable);
            _dbContext.SaveChanges();
        }

        public void AddTurnover(List<Turnover> turnovers)
        {
            foreach (Turnover turnover in turnovers)
            {
                TurnoverModel turnoverTable = new TurnoverModel
                {
                    Name = turnover.Name,
                    Day = turnover.Day,
                    Month = turnover.Month,
                    Year = turnover.Year,
                    TypeTurnover = turnover.TypeTurnover,
                    Denomination = turnover.Denomination
                };
                _dbContext.Turnovers.Add(turnoverTable);
            }
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

        public void ClouseConnection()
        {
            _dbContext.Database.Connection.Close();
        }
    }
}
