using domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Collections;

namespace kursovia.View
{
    /// <summary>
    /// Логика взаимодействия для TotalData.xaml
    /// </summary>
    public partial class TotalData : UserControl
    {
        public TotalData()
        {
            InitializeComponent();
            this.Unloaded += TotalData_Unloaded;

            Projects = GetProjectsList(Model.GetAllProjects().ToList());
            projectsDataGrid.ItemsSource = Projects;

            YearTextBox.Text = DateTime.Now.Year.ToString();
            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;

            DataTotal = new List<TotalDataGridModel>
            {
                new TotalDataGridModel { Name = "Количество договоров", Data = (object)Projects.Count() },
                new TotalDataGridModel { Name = "Сумма дохода", Data = (object)Projects.Sum(p => p.Cost) }
            };
            TotalDataGrid.ItemsSource = DataTotal;
        }

        public List<TotalDataGridModel> DataTotal { get; set; }

        private readonly DatabaseModel Model = new DatabaseModel();

        public List<Project> Projects { get; set; }

        private List<Project> GetProjectsList(List<ProjectModel> projects)
        {
            List<Project> resoult = new List<Project>();
            foreach (ProjectModel project in projects)
            {
                Project projectList = new Project
                {
                    ProjectNumber = project.ProjectNumber,
                    ProjectName = project.ProjectName,
                    Cost = project.Cost,
                    City = project.City,
                    Client = Model.GetCustomerByNumber(project.CustomerNumber),
                    DocumentationType = project.DocumentationType,
                    Inginner = project.Inginner,
                    Contract = project.Contract,
                    DateStartWork = project.DateStartWork,
                    DateEndWork = project.DateEndWork,
                };
                resoult.Add(projectList);
            }
            return resoult;
        }

        public class TotalDataGridModel
        {
            public string Name { get; set; }
            public object Data { get; set; }
        }

        private void Button_Click_Mark(object sender, RoutedEventArgs e)
        {

        }

        private void TotalData_Unloaded(object sender, RoutedEventArgs e)
        {
            Model.ClouseConnection();
        }
    }
}
