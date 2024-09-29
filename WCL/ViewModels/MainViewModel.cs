using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WCL.Helpers;
using WCL.Models;

namespace WCL.ViewModels
{
    class MainViewModel: INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            ErrorStrig = string.Empty;
            VisibilityWindowTemp = Visibility.Collapsed;
            VisibilityWindowAddOrder = Visibility.Collapsed;
            VisibilityWindowAddwarehouse= Visibility.Collapsed;
            VisibilityWindowAddOrder = Visibility.Collapsed;
            VisibilityWindowAddCustomer = Visibility.Collapsed;

            User = new UserViewModel();
            CommandCloseWindow = new Command(OnCloseWindow);
            CommandMinimizeWindow = new Command(OnMinimizeWindow);
            CommandMaximizeRestoreWindow = new Command(OnMaximizeRestoreWindow);

            CommandLogIn = new Command(OnLogIn);
            CommandLogOut = new Command(OnLogOut);
            CommandClearErrorString = new Command(OnClearErrorString);
            CommandChangeVisibilityWindowRegistration= new Command(OnChangeVisibilityWindowRegistration);

            LoadWarehouses();
            LoadOrders();
            LoadCustomers();
        }
        #endregion

        #region Properies

        public bool IsHasError => !string.IsNullOrEmpty(_errorStrig);
        public bool IsHasErrorReg => !string.IsNullOrEmpty(_errorStrigReg);
        private List<Customer> _customer { get; set; }
        public List<Customer> Customers
        {
            get => _customer ?? new List<Customer>();
            set
            {
                _customer = value;
                OnPropertyChanged("Customers");
            }
        }

        private List<Order> _orders { get; set; }
        public List<Order> Orders
        {
            get => _orders ?? new List<Order>();
            set
            {
                _orders = value;
                OnPropertyChanged("Orders");
            }
        }

        private UserViewModel _user { get; set; }
        public UserViewModel User
        {
            get => _user ?? new UserViewModel();
            set
            {
                _user = value;
                OnPropertyChanged("IsHasError");
            }
        }
        private List<Warehouse> _warehouse { get; set; }
        public List<Warehouse> Warehouses
        {
            get => _warehouse ?? new List<Warehouse>();
            set
            {
                _warehouse = value;
                OnPropertyChanged("Warehouses");
            }
        }

        private List<UserViewModel> _users = new List<UserViewModel>
        {
            new UserViewModel { UserID = 1, Username = "a", Password = "a", IsAdmin = true,FIO ="Lobas Nikita Victorovich" },
            new UserViewModel { UserID = 2, Username = "user", Password = "user123", IsAdmin = false ,FIO ="Lobas Nikita Victorovich" }
        };

        private string? _errorStrig { get; set; }
        public string ErrorStrig
        {
            get => _errorStrig ?? string.Empty;
            set
            {
                _errorStrig = value;
                OnPropertyChanged("IsHasError");
                OnPropertyChanged("ErrorStrig");
            }
        }
        private string? _errorStrigReg { get; set; }
        public string ErrorStringReg
        {
            get => _errorStrigReg ?? string.Empty;
            set
            {
                _errorStrigReg = value;
                OnPropertyChanged("IsHasErrorReg");
                OnPropertyChanged("ErrorStringReg");
            }
        }
        private Visibility _visibilityWindowTemp { get; set; }
        public Visibility VisibilityWindowTemp
        {
            get => _visibilityWindowTemp;
            set
            {
                _visibilityWindowTemp = value;
                OnPropertyChanged("VisibilityWindowTemp");
            }
        }
        private Visibility _visibilityWindowAddOrder { get; set; }
        public Visibility VisibilityWindowAddOrder
        {
            get => _visibilityWindowAddOrder;
            set
            {
                _visibilityWindowAddOrder = value;
                OnPropertyChanged("VisibilityWindowAddOrder");
            }
        }

        private Visibility _visibilityWindowAddCustomer { get; set; }
        public Visibility VisibilityWindowAddCustomer
        {
            get => _visibilityWindowAddCustomer;
            set
            {
                _visibilityWindowAddCustomer = value;
                OnPropertyChanged("VisibilityWindowAddCustomer");
            }
        }

        private Visibility _visibilityWindowAddwarehouse { get; set; }
        public Visibility VisibilityWindowAddwarehouse
        {
            get => _visibilityWindowAddwarehouse;
            set
            {
                _visibilityWindowAddwarehouse = value;
                OnPropertyChanged("VisibilityWindowAddwarehouse");
            }
        }


        private int _activetab { get; set; }
        public int Activetab
        {
            get => _activetab;
            set
            {
                _activetab = value;

                switch (_activetab)
                {
                    case 0: break;
                    case 1: LoadWarehouses(); break;
                    case 2: LoadOrders(); break;
                    case 4: LoadCustomers(); break;
                    case 5: break;
                    case 6: break;
                    
                    default: break;
                }

                OnPropertyChanged("Activetab");
            }
        }
        #endregion

        #region Command

        /// <summary> Свернуть окно</summary>
        public ICommand CommandMinimizeWindow { get; set; }
        private void OnMinimizeWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        
        /// <summary> Свернуть окно</summary>
        public ICommand CommandCloseWindow { get; set; }
        private void OnCloseWindow()
        {
            Application.Current.MainWindow.Close();
        }

        /// <summary> Развернуть или воcстановить окно</summary>
        public ICommand CommandMaximizeRestoreWindow { get; set; }
        private void OnMaximizeRestoreWindow()
        {
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow.WindowState == WindowState.Maximized)
            {
                mainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                mainWindow.WindowState = WindowState.Maximized;
            }
        }
        public void AddCustomer(string customerName, string contactName, string phone)
        {
            var customer = new Customer { CustomerName = customerName, ContactName = contactName, Phone = phone };
            using (var context = new ProductCompanyContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }
            VisibilityWindowAddCustomer= Visibility.Collapsed;
            LoadCustomers();

        }

        public async Task AddOrder(int customerId, int employeeId, DateTime orderDate, string shipAddress, string shipCity)
        {
            string sql = "INSERT INTO Orders (CustomerID, EmployeeID, OrderDate, ShipAddress, ShipCity) VALUES (@p0, @p1, @p2, @p3, @p4)";
            using (var context = new ProductCompanyContext())
            {
                await context.Database.ExecuteSqlRawAsync(sql, customerId, employeeId, orderDate, shipAddress, shipCity);
            }

            VisibilityWindowAddOrder = Visibility.Collapsed;
            LoadOrders();

        }

        public void AddWarehouse(string warehouseName, string location)
        {
            var warehouse = new Warehouse { WarehouseName = warehouseName, Location = location };
            using (var context = new ProductCompanyContext())
            {
                context.Warehouses.Add(warehouse);
                context.SaveChanges();
            }
            VisibilityWindowAddwarehouse = Visibility.Collapsed;
            LoadWarehouses();
        }

        private void LoadCustomers()
        {
            using (var context = new ProductCompanyContext())
            {
                Customers = context.Customers.ToList();
            }
        }

        private void LoadOrders()
        {
            using (var context = new ProductCompanyContext())
            {
                Orders = context.Orders.ToList();
            }
        }

        private void LoadWarehouses()
        {
            using (var context = new ProductCompanyContext())
            {
                Warehouses = context.Warehouses.ToList();
            }
        }

        /// <summary> Изменить видимость окна регистрации</summary>
        public ICommand CommandChangeVisibilityWindowRegistration { get; set; }
        private void OnChangeVisibilityWindowRegistration()
        {
            CommandClearErrorString.Execute(null);
            //инвертируем видимость окна
            VisibilityWindowTemp = VisibilityWindowTemp.HasFlag(Visibility.Collapsed)
                ? Visibility.Visible 
                : Visibility.Collapsed;
        }

        /// <summary> Авторизация пользователя по введенным даным</summary>
        public ICommand CommandLogIn { get; set; }
        private void OnLogIn()
        {
            try
            {
                //продожаем только если нет ошибок
                var _user = _users.SingleOrDefault(x => x.Username == User.Username && x.Password == User.Password);

                if(_user != null)
                { 
                    User.FIO = _user.FIO;
                    User.IsLogIn = true;
                }
                else
                {
                    ErrorStrig = "Введены неверные данные";
                }
            }
            catch (Exception ex) 
            {
                ErrorStrig = "Ошибка входа в систему";
            }
        }

        /// <summary> Авторизация выхода пользователя из системы</summary>
        public ICommand CommandLogOut { get; set; }
        private void OnLogOut()
        {
            try
            {
                if(User != null)
                    User.IsLogIn = false;
            }
            catch
            {
                ErrorStrig = "Ошибка выхода из системы";
            }
        }

        /// <summary> очистка строки ошибки</summary>
        public ICommand CommandClearErrorString { get; set; }
        private void OnClearErrorString()
        {
            try
            {
                ErrorStrig = string.Empty;
                ErrorStringReg = string.Empty;
            }
            catch
            {
                ErrorStrig = "Ошибка выхода из системы";
            }
        }


        #endregion
    }
}
