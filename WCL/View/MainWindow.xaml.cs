using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WCL.ViewModels;

namespace WCL.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Инициализация каждой ViewModel
            var suppliersViewModel = new SuppliersViewModel();
            var reportsViewModel = new ReportsViewModel();
            var backupViewModel = new BackupViewModel();
            var sqlQueryViewModel = new SqlQueryViewModel();

            // Установка контекста данных для каждого TabItem
            SuppliersTab.DataContext = suppliersViewModel;
            ReportsTab.DataContext = reportsViewModel;
            BackupTab.DataContext = backupViewModel;
            SqlQueryTab.DataContext = sqlQueryViewModel;

            this.DataContext = new MainViewModel();

        }

        /// Обработчик события MouseLeftButtonDown для перетаскивания окна 
        private void Window_Move(object sender, MouseButtonEventArgs e)
        {
            //Пока кнопка зажата перетаскиваем окно
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        //обработка события нажатия на клавиатуру для вызова регистрации или логин
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(this.DataContext is MainViewModel viewModel)
            {
                if ((e.Key == Key.Enter))
                {
                    //если окно регистрации не выведено на экран
                    if(viewModel.VisibilityWindowTemp.HasFlag(Visibility.Collapsed))
                    {
                        viewModel.CommandLogIn.Execute(null);
                    }
                }
            }
        }

        private void AddSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            string supplierName = SupplierNameTextBox.Text;
            string contactName = ContactNameTextBox.Text;
            string phone = PhoneTextBox.Text;
            if (!(SuppliersTab.DataContext is SuppliersViewModel tmp)) return;
            if (!(this.DataContext is MainViewModel tmp2)) return;

            if (!string.IsNullOrEmpty(supplierName) && !string.IsNullOrEmpty(contactName) && !string.IsNullOrEmpty(phone))
            {
                tmp.AddSupplier(supplierName, contactName, phone);
                tmp2.VisibilityWindowTemp = Visibility.Collapsed;
            }
            else
            {
               tmp2.ErrorStringReg="Please fill all fields.";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.DataContext is MainViewModel tmp)) return;
            tmp.VisibilityWindowTemp = Visibility.Visible;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MainViewModel tmp2)
            {
                tmp2.VisibilityWindowTemp= Visibility.Visible;
                tmp2.VisibilityWindowAddwarehouse = Visibility.Visible;

            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MainViewModel tmp2)
            {
                tmp2.VisibilityWindowTemp= Visibility.Visible;
                tmp2.VisibilityWindowAddCustomer = Visibility.Visible;

            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MainViewModel tmp2)
            {
                tmp2.VisibilityWindowTemp= Visibility.Visible;
                tmp2.VisibilityWindowAddOrder = Visibility.Visible;

            }
        }
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            string customerName = CustomerNameTextBox.Text;
            string contactName = CustomerContactTextBox.Text;
            string phone = CustomerPhoneTextBox.Text;

            if (!(this.DataContext is MainViewModel tmp)) return;

            if (!string.IsNullOrEmpty(customerName) && !string.IsNullOrEmpty(contactName) && !string.IsNullOrEmpty(phone))
            {
                tmp.AddCustomer(customerName, contactName, phone);
                tmp.VisibilityWindowTemp = Visibility.Collapsed;
            }
            else
            {
                tmp.ErrorStringReg = "Please fill all fields.";
            }
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int customerId = int.Parse(CustomerIdTextBox.Text);
            int employeeId = int.Parse(EmployeeIdTextBox.Text);
            DateTime orderDate = DateTime.Parse(OrderDateTextBox.Text);
            string shipAddress = ShipAddressTextBox.Text;
            string shipCity = ShipCityTextBox.Text;

            if (!(this.DataContext is MainViewModel tmp)) return;

            if (customerId > 0 && employeeId > 0 && !string.IsNullOrEmpty(shipAddress) && !string.IsNullOrEmpty(shipCity))
            {
                tmp.AddOrder(customerId, employeeId, orderDate, shipAddress, shipCity);
                tmp.VisibilityWindowTemp = Visibility.Collapsed;
            }
            else
            {
                tmp.ErrorStringReg = "Please fill all fields.";
            }
        }

        private void AddWarehouseButton_Click(object sender, RoutedEventArgs e)
        {
            string warehouseName = WarehouseNameTextBox.Text;
            string location = WarehouseLocationTextBox.Text;

            if (!(this.DataContext is MainViewModel tmp)) return;

            if (!string.IsNullOrEmpty(warehouseName) && !string.IsNullOrEmpty(location))
            {
                tmp.AddWarehouse(warehouseName, location);
                tmp.VisibilityWindowTemp = Visibility.Collapsed;
            }
            else
            {
                tmp.ErrorStringReg = "Please fill all fields.";
            }
        }


    }
}