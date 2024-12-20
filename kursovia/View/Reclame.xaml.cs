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
using domain;
using kursovia.Domain;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.Wpf;

namespace kursovia.View
{
    /// <summary>
    /// Логика взаимодействия для Reclame.xaml
    /// </summary>
    public partial class Reclame : UserControl
    {
        public Reclame()
        {
            InitializeComponent();
            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;
            YearTextBox.Text = DateTime.Now.Year.ToString();

            ReclameList = GetReclameList(int.Parse(YearTextBox.Text));
            NewCustomers = GetListNewCustomer(int.Parse(YearTextBox.Text));

            this.DataContext = this;
            ReclameViewModelCost = CreatePlotModelCost(int.Parse(YearTextBox.Text), ReclameList);
            ReclameViewModelCustomer = CreatePlotModelCustomer(int.Parse(YearTextBox.Text), NewCustomers);

            this.ReclameDataGrid.ItemsSource = GetDataForGridModel(MonthComboBox.SelectedIndex + 1, NewCustomers);
        }

        public PlotModel ReclameViewModelCost { get; set; }
        public PlotModel ReclameViewModelCustomer { get; set; }
        private List<ReclameDataModel> ReclameList { get; set; }
        private Dictionary<int,List<Customer>> NewCustomers { get; set; }

        private readonly DatabaseModel Model = new DatabaseModel();

        private List<ReclameDataModel> GetReclameList(int year)
        {
            List<TurnoverModel> turnoverModels = Model.GetAllTurnovers(year).ToList();
            List<ReclameDataModel> reclameList = new List<ReclameDataModel>();
            foreach (var turnoverModel in turnoverModels)
            {
                if (turnoverModel.Name.ToLower().Contains("реклама")) 
                {
                    ReclameDataModel reclameItem = new ReclameDataModel
                    {
                        Source = turnoverModel.Name.Split(' ')[1],
                        Day = turnoverModel.Day,
                        Month = turnoverModel.Month,
                        Year = turnoverModel.Year,
                        Cost = turnoverModel.Denomination
                    };
                    reclameList.Add(reclameItem);
                }
            }
            return reclameList;
        }

        private Dictionary<int,List<Customer>> GetListNewCustomer(int year)
        {
            List<ProjectModel> projectModels = Model.GetAllProjects().ToList();
            Dictionary<int, List<Customer>> newCustomers = new Dictionary<int, List<Customer>>();
            List<Customer> listCustomers = new List<Customer>();
            foreach (var project in projectModels.Where(p => DateTime.Parse(p.DateStartWork).Year == year))
            {
                Customer newCustomer = Model.GetCustomerByNumber(project.CustomerNumber);
                newCustomer.Projects = Model.GetProjectsByCustomerNumber(newCustomer.Number);
                if (newCustomer.Projects.All(p => DateTime.Parse(p.DateStartWork).Year >= year))
                {
                    listCustomers.Add(newCustomer);
                }
            }
            HashSet<Customer> setCustomers = new HashSet<Customer>();
            foreach (var month in Enumerable.Range(1, 12))
            {
                List<Customer> customers = new List<Customer>(
                    listCustomers.Where(i => !setCustomers.Contains(i) && i.Projects.All(p => DateTime.Parse(p.DateStartWork).Month == month)));
                foreach (var i in customers) setCustomers.Add(i);
                newCustomers.Add(month, customers);
            }
            return newCustomers;
        }

        private List<DataGridModel> GetDataForGridModel(int month, Dictionary<int, List<Customer>> newCustomers)
        {
            List<DataGridModel> data = new List<DataGridModel>();
            HashSet<string> sources = new HashSet<string>();
            foreach (var item in ReclameList) sources.Add(item.Source);
            foreach (var source in sources)
            {
                int count = newCustomers[month].Where(c => c.Traffic == source).Count();
                decimal cost = ReclameList
                    .FindAll(i => i.Source == source && i.Month == month)
                    .Where(i => i.Month == 5)
                    .Sum(i => i.Cost);
                data.Add(new DataGridModel
                {
                    Source = source,
                    Cost = cost,
                    CountCustomers = count,
                });
            }
            return data;
        }

        private PlotModel CreatePlotModelCost(int year, List<ReclameDataModel> reclames)
        {
            Legend legend = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
            };
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Месяц"
            };
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Сумма"
            };

            PlotModel plotModel = new PlotModel();
            plotModel.Legends.Add(legend);
            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);

            Dictionary<int, decimal> dataCostReclame = new Dictionary<int, decimal>();
            foreach (var month in Enumerable.Range(1,12)) 
            {
                dataCostReclame.Add(month, reclames.Where(r => r.Month == month).Sum(r => r.Cost));
            } 

            string[] months = { "Янв", "Фев", "Март", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" };

            var costSeries = new LineSeries
            {
                Title = "Затраты на рекламу", // Название для легенды
                Color = OxyColors.Red, // Цвет линии
                MarkerType = MarkerType.Circle, // Тип маркера
                MarkerSize = 4, // Размер маркера
                MarkerStroke = OxyColors.Black, // Цвет обводки маркера
                MarkerFill = OxyColors.Red,
            };
            foreach (var month in Enumerable.Range(1, 12))
            {
                categoryAxis.Labels.Add(months[month - 1]);
                costSeries.Points.Add(new DataPoint(month - 1, (double)dataCostReclame[month]));
            }
            plotModel.Series.Add(costSeries);
            return plotModel;
        }

        private PlotModel CreatePlotModelCustomer(int year, Dictionary<int, List<Customer>> newCustomers)
        {
            Legend legend = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Месяц"
            };
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Заказчиков"
            };

            PlotModel plotModel = new PlotModel();
            plotModel.Legends.Add(legend);
            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);

            string[] months = { "Янв", "Фев", "Март", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" };

            var customersSeries = new LineSeries
            {
                Title = "Новые заказчики", // Название для легенды
                Color = OxyColors.LightGreen, // Цвет линии
                MarkerType = MarkerType.Circle, // Тип маркера
                MarkerSize = 4, // Размер маркера
                MarkerStroke = OxyColors.Black, // Цвет обводки маркера
                MarkerFill = OxyColors.LightGreen,
            };
            foreach (var month in Enumerable.Range(1, 12))
            {
                categoryAxis.Labels.Add(months[month - 1]);
                customersSeries.Points.Add(new DataPoint(month - 1, newCustomers[month].Count));
            }
            plotModel.Series.Add(customersSeries);
            return plotModel;
        }

        private void Button_Click_Mark(object sender, RoutedEventArgs e)
        {
            int year = int.Parse(YearTextBox.Text);
            int month = MonthComboBox.SelectedIndex + 1;
            ReclameList = GetReclameList(year);
            NewCustomers = GetListNewCustomer(year);
            this.ReclameDataGrid.ItemsSource = GetDataForGridModel(month, NewCustomers);

            GraphCost.Model = CreatePlotModelCost(year, ReclameList);
            GraphCustomers.Model = CreatePlotModelCustomer(year, NewCustomers);
        }
        private class DataGridModel
        {
            public string Source { get; set; }
            public decimal Cost { get; set; }
            public int CountCustomers { get; set; }
        }
    }
}
