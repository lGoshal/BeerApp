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

namespace BeerApp
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AddBeerWindow : Window
    {
        public string BeerName { get; set; }
        public int BeerTypeID { get; set; }
        public int ManufacturerID { get; set; }
        public decimal ABV { get; set; }
        public decimal Price { get; set; }
        public DateTime ProductionDate { get; set; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Соберите данные и установите свойства BeerName, BeerTypeID и другие
            DialogResult = true;
        }
    }
}
