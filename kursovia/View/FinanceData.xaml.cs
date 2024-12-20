using domain;
using kursovia.Domain;
using kursovia.Domain.MyComparer;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
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
using OxyPlot.Legends;

namespace kursovia.View
{
    /// <summary>
    /// Логика взаимодействия для FinanceData.xaml
    /// </summary>
    public partial class FinanceData : UserControl
    {
        public FinanceData()
        {
            InitializeComponent();
            this.Unloaded += FinanceData_Unloaded;

            this.MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;
            this.YearTextBox.Text = DateTime.Now.Year.ToString();

            Turnovers = GetTurnovers(Model.GetAllTurnovers(int.Parse(YearTextBox.Text)).ToList());
            TurnoversDataGrid.ItemsSource = Turnovers.Where(t => t.Month == MonthComboBox.SelectedIndex + 1);

            var model = CreatePlotModel(int.Parse(YearTextBox.Text), Turnovers);

            this.DataContext = this;
            FinanceViewModelProperty = model;
        }

        public PlotModel FinanceViewModelProperty { get; set; }

        private readonly DatabaseModel Model = new DatabaseModel();

        public List<Turnover> Turnovers { get; set; }

        private List<Turnover> GetTurnovers(List<TurnoverModel> turnoverModels)
        {
            List<Turnover> turnovers = new List<Turnover>();
            foreach (TurnoverModel turnoverModel in turnoverModels)
            {
                Turnover turnover = new Turnover
                {
                    Id = turnoverModel.Id,
                    Name = turnoverModel.Name,
                    Day = turnoverModel.Day,
                    Month = turnoverModel.Month,
                    Year = turnoverModel.Year,
                    Denomination = turnoverModel.Denomination,
                    TypeTurnover = turnoverModel.TypeTurnover,
                };
                turnovers.Add(turnover);
            }
            return turnovers;
        }

        private PlotModel CreatePlotModel(int year, List<Turnover> turnovers)
        {
            Legend legend = new Legend 
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.RightTop,
                LegendOrientation = LegendOrientation.Vertical,
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
                Title = "Сумма"
            };

            PlotModel plotModel = new PlotModel();
            plotModel.Legends.Add(legend);
            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);

            Dictionary<int, decimal> dataIncomes = GetDataIncomesForGraph(year, turnovers);
            Dictionary<int, decimal> dataExpenses = GetDataExpensesForGraph(year, turnovers);

            string[] months = { "Янв", "Фев", "Март", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" };

            var incomeSeries = new LineSeries 
            {
                Title = "Доход", // Название для легенды
                Color = OxyColors.LightGreen, // Цвет линии
                MarkerType = MarkerType.Circle, // Тип маркера
                MarkerSize = 4, // Размер маркера
                MarkerStroke = OxyColors.Black, // Цвет обводки маркера
                MarkerFill = OxyColors.LightGreen,
            };
            var expenseSeries = new LineSeries 
            {
                Title = "Расход", // Название для легенды
                Color = OxyColors.Red, // Цвет линии
                MarkerType = MarkerType.Circle, // Тип маркера
                MarkerSize = 4, // Размер маркера
                MarkerStroke = OxyColors.Black, // Цвет обводки маркера
                MarkerFill = OxyColors.Red,
            };
            foreach (var month in Enumerable.Range(1, 12))
            {
                categoryAxis.Labels.Add(months[month - 1]);
                incomeSeries.Points.Add(new DataPoint(month - 1, (double)dataIncomes[month]));
                expenseSeries.Points.Add(new DataPoint(month - 1, (double)dataExpenses[month]));
            }
            plotModel.Series.Add(incomeSeries);
            plotModel.Series.Add(expenseSeries);
            return plotModel;
        }

        private Dictionary<int, decimal> GetDataIncomesForGraph(int year, List<Turnover> turnovers)
        {
            Dictionary<int, decimal> data = new Dictionary<int, decimal>();
            foreach (var month in Enumerable.Range(1, 12))
            {
                decimal totalIncome = turnovers.Where(t => t.TypeTurnover == "Доход" && t.Month == month && t.Year == year).Sum(t => t.Denomination);
                data.Add(month, totalIncome);
            }
            return data;
        }
        private Dictionary<int, decimal> GetDataExpensesForGraph(int year, List<Turnover> turnovers)
        {
            Dictionary<int, decimal> data = new Dictionary<int, decimal>();
            foreach (var month in Enumerable.Range(1, 12))
            {
                decimal totalExpense = turnovers.Where(t => t.TypeTurnover == "Расход" && t.Month == month && t.Year == year).Sum(t => t.Denomination);
                data.Add(month, totalExpense);
            }
            return data;
        }

        private void FinanceData_Unloaded(object sender, RoutedEventArgs e)
        {
            Model.ClouseConnection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Form.NewTurnovers != null)
                if (Form.NewTurnovers.Count != 0)
                {
                    Model.AddTurnover(Form.NewTurnovers);
                    Form.NewTurnovers.Clear();
                }
        }

        private void Button_Click_Mark(object sender, RoutedEventArgs e)
        {
            int year = int.Parse(YearTextBox.Text);
            int month = MonthComboBox.SelectedIndex + 1;
            Turnovers = GetTurnovers(Model.GetAllTurnovers(year).ToList());

            var model = CreatePlotModel(year, Turnovers);
            this.GraphTurnover.Model = model;

            TurnoversDataGrid.ItemsSource = Turnovers.Where(t => t.Month == month);
        }
    }
}
