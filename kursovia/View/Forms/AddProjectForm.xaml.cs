using domain;
using kursovia.View.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace kursovia.View.Forms
{
    /// <summary>
    /// Логика взаимодействия для AddProjectForm.xaml
    /// </summary>
    public partial class AddProjectForm : UserControl
    {
        public Project Project { get; private set; }
        public AddProjectForm()
        {
            InitializeComponent();
            Customers = GetCustomers();
            ComboBoxCustomers.ItemsSource = Customers;
        }
        public List<string> Customers { get; set; }
        public DatabaseModel Model = new DatabaseModel();
        public string SelectedCustomerNumber { get; set; }

        private List<string> GetCustomers()
        {
            List<CustomerModel> customerModels = Model.GetAllCustomers().ToList();
            List<string> customers = new List<string>();
            foreach (CustomerModel customer in customerModels)
            {
                string customerName = customer.Number + ". " + customer.Name;
                customers.Add(customerName);
            }
            return customers;
        }

        private void NewCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CustomerDialog();
            dialog.ShowDialog();
            if (dialog.DialogResult == false)
            {
                if (dialog.Customer != null)
                {
                    Project.Client = dialog.Customer;
                    Model.AddCustomer(dialog.Customer);
                }
            }
        }

        private void SaveProjectButton_Click(object sender, RoutedEventArgs e)
        {
            Model.AddProject(Project);
        }
    }
}
