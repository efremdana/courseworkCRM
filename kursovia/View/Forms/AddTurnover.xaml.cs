using kursovia.Domain;
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
    /// Логика взаимодействия для AddTurnover.xaml
    /// </summary>
    public partial class AddTurnover : UserControl
    {
        public List<Turnover> NewTurnovers { get; set; }
        public AddTurnover()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Turnover newTurnover = new Turnover
            {
                Name = NameTextBox.Text,
                Day = int.Parse(DayComboBox.Text),
                Month = DateTime.ParseExact(
                    MonthComboBox.Text, 
                    "MMMM", 
                    System.Globalization.CultureInfo.InvariantCulture
                    )
                .Month,
                Year = int.Parse(YearTextBox.Text),
                TypeTurnover = TypeComboBox.Text,
                Denomination = decimal.Parse(DenominationTextBox.Text)
            };
            NewTurnovers.Add(newTurnover);
        }
    }
}
