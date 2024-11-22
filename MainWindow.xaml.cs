using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Configuration;
namespace BeerApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BeerRepository _beerRepository;

        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["PivchanskiyConnection"].ConnectionString;
            _beerRepository = new BeerRepository(connectionString);
            LoadBeers();
        }

        // Обработчик кнопки экспорта
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = ((DataView)BeerDataGrid.ItemsSource).ToTable();

            // Открываем диалоговое окно для выбора места сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.Title = "Сохранить отчет в Excel";
            saveFileDialog.FileName = "BeersReport.xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportToExcel(dataTable, saveFileDialog.FileName);
            }
        }

        // Метод для экспорта данных в Excel
        private void ExportToExcel(DataTable dataTable, string filePath)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(dataTable, "Beers");
                workbook.SaveAs(filePath);
            }

            MessageBox.Show("Отчет успешно экспортирован.", "Экспорт в Excel", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadBeers()
        {
            BeerDataGrid.ItemsSource = _beerRepository.GetAllBeers().DefaultView;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddBeerWindow addWindow = new AddBeerWindow();
            if (addWindow.ShowDialog() == true)
            {
                _beerRepository.AddBeer(addWindow.BeerName, addWindow.BeerTypeID, addWindow.ManufacturerID, addWindow.ABV, addWindow.Price, addWindow.ProductionDate);
                LoadBeers();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (BeerDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)BeerDataGrid.SelectedItem;
                AddBeerWindow editWindow = new AddBeerWindow
                {
                    BeerID = (int)row["BeerID"],
                    BeerName = (string)row["Name"],
                    BeerTypeID = (int)row["BeerTypeID"],
                    ManufacturerID = (int)row["ManufacturerID"],
                    ABV = (decimal)row["ABV"],
                    Price = (decimal)row["Price"],
                    ProductionDate = (DateTime)row["ProductionDate"]
                };

                if (editWindow.ShowDialog() == true)
                {
                    _beerRepository.UpdateBeer(editWindow.BeerID, editWindow.BeerName, editWindow.BeerTypeID, editWindow.ManufacturerID, editWindow.ABV, editWindow.Price, editWindow.ProductionDate);
                    LoadBeers();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BeerDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)BeerDataGrid.SelectedItem;
                int beerId = (int)row["BeerID"];
                _beerRepository.DeleteBeer(beerId);
                LoadBeers();
            }
        }
    }
}
