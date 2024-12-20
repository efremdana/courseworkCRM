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
using MahApps.Metro.Controls;


namespace kursovia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            mainGrid.Children.Add(new View.TotalData());
        }

        private void OpenTotalData(object sender, RoutedEventArgs e)
        {
            this.mainGrid.Children.Clear();
            this.mainGrid.Children.Add(new View.TotalData());
        }

        private void OpenFinanceData(object sender, RoutedEventArgs e)
        {
            this.mainGrid.Children.Clear();
            this.mainGrid.Children.Add(new View.FinanceData());
        }

        private void OpenReclame(object sender, RoutedEventArgs e)
        {
            this.mainGrid.Children.Clear();
            this.mainGrid.Children.Add(new View.Reclame());
        }
        private void OpenCreateProjectView(object sender, RoutedEventArgs e)
        {
            this.mainGrid.Children.Clear();
            this.mainGrid.Children.Add(new View.CreateProjectView());
        }
    }
}
