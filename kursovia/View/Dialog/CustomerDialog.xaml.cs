using domain;
using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace kursovia.View.Dialog
{
    /// <summary>
    /// Логика взаимодействия для CustomerDialog.xaml
    /// </summary>
    public partial class CustomerDialog : MetroWindow
    {
        public Customer Customer { get; private set; }
        public CustomerDialog()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            // Сохранение данных из формы в объект Customer
            Customer.Number = int.Parse(this.NumeberCustomer.Text);
            Customer.FirstName = FirstNameCustomer.Text;
            Customer.Name = NameCustomer.Text;
            Customer.LastName = LastNameCustomer.Text;
            Customer.PhoneNumber = PhoneCustomer.Text;
            Customer.Email = EmailCustomer.Text;
            Customer.Company = CompanyCustomer.Text;
            Customer.Traffic = TrafficCustomer.Text;
            if (int.TryParse(TINCustomer.Text, out int tin))
            {
                Customer.TIN = tin;
            }
            else
            {
                Customer.TIN = null;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
