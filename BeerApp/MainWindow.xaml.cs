using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
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
using System.IO;
using System.Diagnostics;
using System.Data.Entity.Spatial;
using Microsoft.Win32;
using System.Data.Entity.Migrations;

namespace BeerApp
{
    public partial class MainWindow : Window
    {
        private PivchanskiyEntities1 db; // Инициализация БД

        // Коллекции для корректной работы бд и DataGrid
        private ObservableCollection<Beer> _beers = new ObservableCollection<Beer>();
        private ObservableCollection<Manufacturer> _manufacturer = new ObservableCollection<Manufacturer>();
        private ObservableCollection<BeerIngredient> _beersIngredient = new ObservableCollection<BeerIngredient>();
        private ObservableCollection<BeerType> _beersType = new ObservableCollection<BeerType>();
        private ObservableCollection<Customer> _customer = new ObservableCollection<Customer>();
        private ObservableCollection<Ingredient> _ingredients = new ObservableCollection<Ingredient>();
        private ObservableCollection<Order> _order = new ObservableCollection<Order>();
        private ObservableCollection<OrderBeer> _orderBeers = new ObservableCollection<OrderBeer>();
        private ObservableCollection<ProductionProcess> _productionProcesses = new ObservableCollection<ProductionProcess>();
        private ObservableCollection<User> _user = new ObservableCollection<User>();
        private ObservableCollection<QueryBeer> _querybeer = new ObservableCollection<QueryBeer>();

        public MainWindow()
        {
            InitializeComponent();
            db = new PivchanskiyEntities1();
            LoadBeers();
        }

        // События для всего
        private void DataGrid_AutoGenerationColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "BeerType" || e.PropertyName == "Manufacturer" || e.PropertyName == "Ingredient" ||
                e.PropertyName == "User" || e.PropertyName == "Order" || e.PropertyName == "BeerIngredient" ||
                e.PropertyName == "OrderBeer" || e.PropertyName == "ProductionProcess" || e.PropertyName == "Beer" ||
                e.PropertyName == "Customer")
            {
                e.Cancel = true;
            }
            if (e.PropertyName == "link")
            {
                DataGridTemplateColumn imageColumn = new DataGridTemplateColumn
                {
                    Header = "Изображение"
                };
                var imageFactory = new FrameworkElementFactory(typeof(Image));
                imageFactory.SetBinding(Image.SourceProperty, new Binding("link"));
                imageFactory.SetValue(Image.WidthProperty, 100.0);
                imageFactory.SetValue(Image.HeightProperty, 100.0);
                imageColumn.CellTemplate = new DataTemplate
                {
                    VisualTree = imageFactory
                };
                e.Column = imageColumn;
            }
        }

        //Метод загрузки данных в DataGrid
        private void LoadBeers()
        {
            string selectedOption = ((ComboBoxItem)cmbTable.SelectedItem)?.Content.ToString();
            switch (selectedOption)
            {
                case "Beer":
                    _beers = new ObservableCollection<Beer>(db.Beer.ToList());
                    dgProduct.ItemsSource = _beers;
                    db.SaveChanges();
                    break;
                case "Manufacturer":
                    _manufacturer = new ObservableCollection<Manufacturer>(db.Manufacturer.ToList());
                    dgProduct.ItemsSource = _manufacturer;
                    db.SaveChanges();
                    break;
                case "BeerIngridient":
                    _beersIngredient = new ObservableCollection<BeerIngredient>(db.BeerIngredient.ToList());
                    dgProduct.ItemsSource = _beersIngredient;
                    db.SaveChanges();
                    break;
                case "BeerType":
                    _beersType = new ObservableCollection<BeerType>(db.BeerType.ToList());
                    dgProduct.ItemsSource = _beersType;
                    db.SaveChanges();
                    break;
                case "Customer":
                    _customer = new ObservableCollection<Customer>(db.Customer.ToList());
                    dgProduct.ItemsSource = _customer;
                    db.SaveChanges();
                    break;
                case "Ingridient":
                    _ingredients = new ObservableCollection<Ingredient>(db.Ingredient.ToList());
                    dgProduct.ItemsSource = _ingredients;
                    db.SaveChanges();
                    break;
                case "OrderBeer":
                    _orderBeers = new ObservableCollection<OrderBeer>(db.OrderBeer.ToList());
                    dgProduct.ItemsSource = _orderBeers;
                    db.SaveChanges();
                    break;
                case "Order":
                    _order = new ObservableCollection<Order>(db.Order.ToList());
                    dgProduct.ItemsSource = _order;
                    db.SaveChanges();
                    break;
                case "ProductionProcess":
                    _productionProcesses = new ObservableCollection<ProductionProcess>(db.ProductionProcess.ToList());
                    dgProduct.ItemsSource = _productionProcesses;
                    db.SaveChanges();
                    break;
                case "User":
                    _user = new ObservableCollection<User>(db.User.ToList());
                    dgProduct.ItemsSource = _user;
                    db.SaveChanges();
                    break;
                case "LINQ Beer":
                    var queryBeer = from be in db.Beer
                                join bet in db.BeerType on be.BeerTypeID equals bet.BeerTypeID
                                select new
                                {
                                    be.Name,
                                    bet.TypeName,
                                    bet.Description,
                                    be.ABV,
                                    be.Price,
                                    be.ProductionDate,

                                };
                    dgProduct.ItemsSource = queryBeer.ToList();
                    break;
                case "LINQ User":
                    var queryUser = from us in db.User
                                    join cu in db.Customer on us.CustomerID equals cu.CustomerID
                                    select new
                                    {
                                        us.Login,
                                        us.Password,
                                        cu.Name,
                                        cu.Email
                                    };
                    dgProduct.ItemsSource= queryUser.ToList();
                    break;
                case "LINQ Ingredients":
                    var queryIngred = from ingre in db.Ingredient
                                      join BeerIng in db.BeerIngredient on ingre.IngredientID equals BeerIng.IngredientID
                                      join be in db.Beer on BeerIng.BeerID equals be.BeerID
                                      select new
                                      {
                                          be.BeerID,
                                          ingre.Name,
                                          ingre.Supplier,
                                          BeerIng.Quantity
                                      };
                    dgProduct.ItemsSource = queryIngred.ToList();
                    break;
                case "LINQ Order":
                    var queryOrder = from or in db.Order
                                     join cu in db.Customer on or.CustomerID equals cu.CustomerID
                                     select new
                                     {
                                         cu.Name,
                                         or.OrderDate,
                                         or.TotalAmount
                                     };
                    dgProduct.ItemsSource = queryOrder.ToList();
                    break;
                case "LINQ ProductionProcess":
                    var queryProductionProcess = from pp in db.ProductionProcess
                                                 join be in db.Beer on pp.BeerID equals be.BeerID
                                                 select new
                                                 {
                                                     be.Name,
                                                     pp.StepNumber,
                                                     pp.Description,
                                                     pp.DurationHours
                                                 };
                    dgProduct.ItemsSource = queryProductionProcess.ToList();
                    break;
                default:
                    _beers = new ObservableCollection<Beer>(db.Beer.ToList());
                    dgProduct.ItemsSource = _beers;
                    db.SaveChanges();
                    break;
            }
        }

        // Кнопка сохранения
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            if (dgProduct.SelectedItem is Beer selectedBeer)
            {
                db.Entry(selectedBeer).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            LoadBeers();
        }

        // Авто-загрузка начальной таблицы в DataGrid
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new PivchanskiyEntities1();
            dgProduct.ItemsSource = db.Beer.ToList();
        }

        // Метод определения импорта и экспорта
        private void ImpExp(object sender, RoutedEventArgs e)
        {
            string selectedOption = ((ComboBoxItem)ImpExpBox.SelectedItem)?.Content.ToString();
            switch (selectedOption)
            {
                case "Экспорт в CSV":
                    ExportToCsv();
                    break;
                case "Импорт из CSV":
                    ImportFromCsv();
                    break;
                case "Экспорт в Excel":
                    ExportToExcel();
                    break;
                case "Импорт из Excel":
                    ImportFromExcel();
                    break;
            }
        }

        // Экспорт в CSV
        private void ExportToCsv()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("wpfdata.csv", false, Encoding.UTF8))
                {
                    // Запись заголовков
                    var headers = dgProduct.Columns.Select(column => $"\"{column.Header}\"");
                    sw.WriteLine(string.Join(",", headers));

                    // Запись данных
                    foreach (var item in dgProduct.Items)
                    {
                        if (item != null)
                        {
                            var row = dgProduct.Columns.Select(column =>
                            {
                                var cellContent = column.GetCellContent(item) as TextBlock;
                                return $"\"{cellContent?.Text}\"";
                            });
                            sw.WriteLine(string.Join(",", row));
                        }
                    }
                }

                // Открытие файла после сохранения
                Process.Start(new ProcessStartInfo("wpfdata.csv") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте CSV: {ex.Message}");
            }
        }

        // Импорт из CSV
        private void ImportFromCsv()
        {
            try
            {
                using (StreamReader sr = new StreamReader("wpfdata.csv", Encoding.UTF8))
                {
                    string line;
                    bool isHeader = true;

                    // Чтение заголовков
                    while ((line = sr.ReadLine()) != null)
                    {
                        var values = line.Split(',');

                        if (isHeader)
                        {
                            isHeader = false;
                            continue; // Пропускаем заголовок, если DataGrid уже настроен
                        }

                        // Создаем объект `Beer` и заполняем его значениями из `values`
                        Beer newBeer = new Beer
                        {
                            Name = values[0].Trim('"'),
                            BeerTypeID = int.Parse(values[1]),
                            ManufacturerID = int.Parse(values[2]),
                            ABV = decimal.Parse(values[3]),
                            Price = decimal.Parse(values[4]),
                            ProductionDate = DateTime.Parse(values[5])
                        };

                        // Добавление в коллекцию и в базу данных
                        _beers.Add(newBeer);
                        db.Beer.Add(newBeer);
                    }
                    db.SaveChanges();
                    LoadBeers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при импорте CSV: {ex.Message}");
            }
        }

        // Экспорт в Exel
        private void ExportToExcel()
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;

            try
            {
                // Экспорт заголовков
                for (int i = 0; i < dgProduct.Columns.Count; i++)
                {
                    workSheet.Cells[1, i + 1] = dgProduct.Columns[i].Header;
                }

                // Экспорт данных
                for (int i = 0; i < dgProduct.Items.Count; i++)
                {
                    for (int j = 0; j < dgProduct.Columns.Count; j++)
                    {
                        var cellContent = dgProduct.Columns[j].GetCellContent(dgProduct.Items[i]) as TextBlock;
                        workSheet.Cells[i + 2, j + 1] = cellContent?.Text;
                    }
                }

                // Сохранение и открытие Excel
                string filePath = "wpfdata.xlsx";
                workSheet.SaveAs(filePath);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Excel: {ex.Message}");
            }
            finally
            {
                // Освобождение ресурсов Excel
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        // Импорт из Exel
        private void ImportFromExcel()
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = null;

            try
            {
                workbook = excelApp.Workbooks.Open("wpfdata.xlsx");
                Microsoft.Office.Interop.Excel._Worksheet worksheet = workbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range range = worksheet.UsedRange;

                for (int i = 2; i <= range.Rows.Count; i++)
                {
                    Beer newBeer = new Beer
                    {
                        Name = (range.Cells[i, 1] as Microsoft.Office.Interop.Excel.Range)?.Text,
                        BeerTypeID = int.Parse((range.Cells[i, 2] as Microsoft.Office.Interop.Excel.Range)?.Text),
                        ManufacturerID = int.Parse((range.Cells[i, 3] as Microsoft.Office.Interop.Excel.Range)?.Text),
                        ABV = decimal.Parse((range.Cells[i, 4] as Microsoft.Office.Interop.Excel.Range)?.Text),
                        Price = decimal.Parse((range.Cells[i, 5] as Microsoft.Office.Interop.Excel.Range)?.Text),
                        ProductionDate = DateTime.Parse((range.Cells[i, 6] as Microsoft.Office.Interop.Excel.Range)?.Text)
                    };

                    _beers.Add(newBeer);
                    db.Beer.Add(newBeer);
                }
                db.SaveChanges();
                LoadBeers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при импорте из Excel: {ex.Message}");
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        // Кнопка добавления
        private void Button_Add(object sender, RoutedEventArgs e)
        {
            string selectedOption = ((ComboBoxItem)cmbTable.SelectedItem)?.Content.ToString();
            switch (selectedOption)
            {
                case "Beer":
                    Beer newBeer = new Beer
                    {
                        Name = "Новое Пиво",
                        BeerTypeID = 1,
                        ManufacturerID = 1,
                        ABV = 5.0m,
                        Price = 100m,
                        ProductionDate = DateTime.Now
                    };
                    _beers.Add(newBeer);
                    db.Beer.Add(newBeer);
                    db.SaveChanges();
                    LoadBeers();
                    break;
                case "BeerIngredient":
                    BeerIngredient newBeerIngredient = new BeerIngredient
                    {
                        IngredientID = 0,
                        Quantity = 0
                    };
                    _beersIngredient.Add(newBeerIngredient);
                    db.BeerIngredient.Add(newBeerIngredient);
                    db.SaveChanges();
                    LoadBeers();
                    break;
                case "BeerType":
                    BeerType newBeerType = new BeerType
                    {
                        TypeName = "type",
                        Description = "description"
                    };
                    _beersType.Add(newBeerType);
                    db.BeerType.Add(newBeerType);
                    db.SaveChanges();
                    LoadBeers();
                    break;
                case "Customer":
                    Customer newCustomer = new Customer
                    {
                        Name = "Иванов Иван Иванович",
                        Phone = "89000000000",
                        Email = "ivanov@mail.ru",
                        Address = "Pushkina 12"
                    };
                    _customer.Add(newCustomer);
                    db.Customer.Add(newCustomer);
                    db.SaveChanges();
                    LoadBeers();
                    break;
                case "Manufacturer":
                    Manufacturer newManufacturer = new Manufacturer
                    {
                        Name = "Name",
                        Country = "Country",
                        EstablishedYear = 0
                    };
                    _manufacturer.Add(newManufacturer);
                    db.Manufacturer.Add(newManufacturer);
                    db.SaveChanges();
                    LoadBeers();
                    break;
                case "Order":
                    Order newOrder = new Order
                    {
                        CustomerID = 1,
                        OrderDate = new DateTime(0),
                        TotalAmount = 0,
                    };
                    _order.Add(newOrder);
                    db.Order.Add(newOrder);
                    db.SaveChanges();
                    LoadBeers();
                    break;
                case "User":
                    Customer newCustomer1 = new Customer
                    {
                        Name = "Иванов Иван Иванович",
                        Phone = "89000000000",
                        Email = "ivanov@mail.ru",
                        Address = "Pushkina 12"
                    };
                    db.Customer.Add(newCustomer1);
                    db.SaveChanges();
                    User newUser = new User
                    {
                        Login = "Новый пользователь",
                        Password = "Password",
                        CustomerID = 3
                    };
                    db.User.Add(newUser);        
                    db.SaveChanges();              
                    LoadBeers();                   
                    break;
                default:
                    _beers = new ObservableCollection<Beer>(db.Beer.ToList());
                    dgProduct.ItemsSource = _beers;
                    db.SaveChanges();
                    break;
            }
        }
       
        // Кнопка удаления
        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            string selectedOption = ((ComboBoxItem)cmbTable.SelectedItem)?.Content.ToString();
            switch (selectedOption)
            {
                case "Beer":
                    if (dgProduct.SelectedItem is Beer selectedBeer)
                    {
                        db.Beer.Remove(selectedBeer);
                        db.SaveChanges();
                        LoadBeers();
                    }
                    else
                    {MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);}
                    break;
                case "BeerIngredient":
                    if (dgProduct.SelectedItem is BeerIngredient selectedBeerIngredient)
                    {
                        db.BeerIngredient.Remove(selectedBeerIngredient);
                        db.SaveChanges();
                        LoadBeers();
                    }
                    else
                    {MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);}
                    break;
                case "BeerType":
                    if (dgProduct.SelectedItem is BeerType selectedBeerType)
                    {
                        db.BeerType.Remove(selectedBeerType);
                        db.SaveChanges();
                        LoadBeers();
                    }
                    else
                    { MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);}
                    break;
                case "Customer":
                    if (dgProduct.SelectedItem is Customer selectedCustomer)
                    {
                        db.Customer.Remove(selectedCustomer);
                        db.SaveChanges();
                        LoadBeers();
                    }
                    else
                    {MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);}
                    break;
                case "Ingredients":
                    if (dgProduct.SelectedItem is Ingredient selectedIngredient)
                    {
                        db.Ingredient.Remove(selectedIngredient);
                        db.SaveChanges();
                        LoadBeers();
                    }
                    else
                    {MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);}
                    break;
                case "Manufacturer":
                    if (dgProduct.SelectedItem is Manufacturer selectedManufacturer)
                    {
                        db.Manufacturer.Remove(selectedManufacturer);
                        db.SaveChanges();
                        LoadBeers();
                    }
                    else
                    {MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);}
                    break;
                case "Order":
                    if (dgProduct.SelectedItem is Order selectedOrder)
                    {
                        db.Order.Remove(selectedOrder);
                        db.SaveChanges();
                        LoadBeers();
                    }
                    else
                    {MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);}
                    break;
                case "OrderBeer":
                    if (dgProduct.SelectedItem is OrderBeer selectedOrderBeer)
                    {
                        db.OrderBeer.Remove(selectedOrderBeer);
                        db.SaveChanges();
                        LoadBeers();
                    }
                    else
                    {MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);}
                    break;
                case "ProductionProcess":
                    if (dgProduct.SelectedItem is ProductionProcess selectedProductionProcess)
                    {
                        db.ProductionProcess.Remove(selectedProductionProcess);
                        db.SaveChanges();
                        LoadBeers();
                    }
                    else
                    {MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);}
                    break;
                case "User":
                    if (dgProduct.SelectedItem is User selectedUser)
                    {
                        db.User.Remove(selectedUser);
                        db.SaveChanges();
                        LoadBeers();
                    }
                    else
                    {MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);}
                    break;
                default:
                    MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

        // Кнопка сортировки
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedOption = ((ComboBoxItem)cmbSortOptions.SelectedItem)?.Content.ToString();
            if (selectedOption == null)
            {
                MessageBox.Show("Пожалуйста, выберите критерий сортировки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            switch (selectedOption)
            {
                case "По названию":
                    dgProduct.ItemsSource = new ObservableCollection<Beer>(db.Beer.OrderBy(b => b.Name).ToList());
                    break;
                case "По крепости":
                    dgProduct.ItemsSource = new ObservableCollection<Beer>(db.Beer.OrderBy(b => b.ABV).ToList());
                    break;
                case "По цене":
                    dgProduct.ItemsSource = new ObservableCollection<Beer>(db.Beer.OrderBy(b => b.Price).ToList());
                    break;
                case "По дате производства":
                    dgProduct.ItemsSource = new ObservableCollection<Beer>(db.Beer.OrderBy(b => b.ProductionDate).ToList());
                    break;
                default:
                    MessageBox.Show("Некорректный выбор критерия сортировки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

        private void Button_LoadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                // Получаем путь к выбранному изображению
                string imagePath = openFileDialog.FileName;

                // Получаем выбранный объект Beer из DataGrid (предполагаю, что вы выбрали строку)
                Beer selectedBeer = dgProduct.SelectedItem as Beer;

                if (selectedBeer != null)
                {
                    // Устанавливаем путь к изображению
                    selectedBeer.link = imagePath;

                    // Обновляем базу данных
                    db.SaveChanges();

                    // Если вы хотите, чтобы в DataGrid отображалась картинка, то надо обновить модель
                    dgProduct.Items.Refresh();  // Обновляем отображение DataGrid
                }
                else
                {
                    MessageBox.Show("Выберите строку для редактирования.");
                }
            }
        }
    }
}