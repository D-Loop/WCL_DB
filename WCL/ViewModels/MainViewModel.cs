using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
            try
            {
                ErrorStrig = string.Empty;
                VisibilityWindowTemp = Visibility.Collapsed;
                VisibilityWindowAddOrder = Visibility.Collapsed;
                VisibilityWindowAddwarehouse= Visibility.Collapsed;
                VisibilityWindowAddOrder = Visibility.Collapsed;
                VisibilityWindowAddCustomer = Visibility.Collapsed;
                VisibilityWindowAddSupply = Visibility.Collapsed;
                VisibilityWindowAddStockForecasts = Visibility.Collapsed;

                User = new UserViewModel();
                CommandCloseWindow = new Command(OnCloseWindow);
                CommandMinimizeWindow = new Command(OnMinimizeWindow);
                CommandMaximizeRestoreWindow = new Command(OnMaximizeRestoreWindow);
                ExecuteQueryCommand = new Command(ExecuteQuery);

                CommandLogIn = new Command(OnLogIn);
                CommandLogOut = new Command(OnLogOut);
                CommandClearErrorString = new Command(OnClearErrorString);
                CommandChangeVisibilityWindowRegistration= new Command(OnChangeVisibilityWindowRegistration);
                BrowseBackupPathCommand = new Command(BrowseBackupPath);
                BackupCommand = new Command(BackupDatabase);
                RestoreCommand = new Command(RestoreDatabase);

                LoadWarehouses();
                LoadOrders();
                LoadCustomers();
                LoadProducts();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Вывод ошибки для диагностики
            }
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
        private List<SupplyPlan> _supplyPlan { get; set; }
        public List<SupplyPlan> SupplyPlan
        {
            get => _supplyPlan ?? new List<SupplyPlan>();
            set
            {
                _supplyPlan = value;
                OnPropertyChanged("SupplyPlan");
            }
        }
        private List<Product> _products { get; set; }
        public List<Product> Products
        {
            get => _products ?? new List<Product>();
            set
            {
                _products = value;
                OnPropertyChanged("Products");
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
            new UserViewModel { UserID = 1, Username = "Admin", Password = "admin", IsAdmin = true,FIO ="Lobas Mikita Victorovich" },
            new UserViewModel { UserID = 2, Username = "User", Password = "user", IsAdmin = false ,FIO ="Lobas Nikita Victorovich" }
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


        private Visibility _visibilityWindowAddSupply { get; set; }
        public Visibility VisibilityWindowAddSupply
        {
            get => _visibilityWindowAddSupply;
            set
            {
                _visibilityWindowAddSupply = value;
                OnPropertyChanged("VisibilityWindowAddSupply");
            }
        }


        private Visibility _visibilityWindowAddStockForecasts { get; set; }
        public Visibility VisibilityWindowAddStockForecasts
        { 
            get => _visibilityWindowAddStockForecasts;
            set
            {
                _visibilityWindowAddStockForecasts = value;
                OnPropertyChanged("VisibilityWindowAddStockForecasts");
            }
        }
        public ICommand BrowseBackupPathCommand { get; }
        public ICommand BackupCommand { get; }
        public ICommand RestoreCommand { get; }

        private string _backupPath;
        public string BackupPath
        {
            get => _backupPath;
            set
            {
                _backupPath = value;
                OnPropertyChanged(nameof(BackupPath));
            }
        }

        private string _sqlQueryTextBoxText { get; set; }
        public string SqlQueryTextBoxText
        {
            get => _sqlQueryTextBoxText;
            set
            {
                _sqlQueryTextBoxText = value;
                OnPropertyChanged("SqlQueryTextBoxText");
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
        public ICommand ExecuteQueryCommand { get; set; }

        private DataTable _queryResults;
        public DataTable QueryResults
        {
            get => _queryResults;
            set
            {
                _queryResults = value;
                OnPropertyChanged(nameof(QueryResults));
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
                    case 5: LoadSupplyPlans(); break;
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
        public async Task<List<StockForecast>> LoadStockForecasts()
        {
            using (var context = new ProductCompanyContext())
            {
                return await context.StockForecasts.Include(sf => sf.Product).ToListAsync();
            }
        }

        private void BrowseBackupPath()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Backup Files (*.bak)|*.bak",
                DefaultExt = ".bak"
            };

            if (dialog.ShowDialog() == true)
            {
                BackupPath = dialog.FileName;
            }
        }
        private void RestoreDatabase()
        {
            if (string.IsNullOrWhiteSpace(BackupPath))
            {
                // Обработка ошибки
                return;
            }

            using (var context = new ProductCompanyContext())
            {
                try
                {
                    // Установка базы данных в режим единого пользователя
                    string setSingleUserQuery = $"USE master; ALTER DATABASE [{context.Database.GetDbConnection().Database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                    string restoreQuery = $"RESTORE DATABASE [{context.Database.GetDbConnection().Database}] FROM DISK = '{BackupPath}' WITH REPLACE;";
                    string setMultiUserQuery = $"ALTER DATABASE [{context.Database.GetDbConnection().Database}] SET MULTI_USER;";

                    context.Database.OpenConnection();

                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        // Выполнение запроса на установку в режим единого пользователя
                        command.CommandText = setSingleUserQuery;
                        command.ExecuteNonQuery();

                        // Выполнение запроса на восстановление базы данных
                        command.CommandText = restoreQuery;
                        command.ExecuteNonQuery();

                        // Возврат базы данных в режим многопользовательского
                        command.CommandText = setMultiUserQuery;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    context.Database.CloseConnection();
                }
            }
        }

        private void BackupDatabase()
        {
            if (string.IsNullOrWhiteSpace(BackupPath))
            {
                // Обработка ошибки
                return;
            }

            using (var context = new ProductCompanyContext())
            {
                try
                {
                    string query = $"BACKUP DATABASE [{context.Database.GetDbConnection().Database}] TO DISK = '{BackupPath}'";
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = query;
                        context.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        // Успех
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    context.Database.CloseConnection();
                }
            }
        }

        public void BackupDatabase(string backupFilePath)
        {
            using (var context = new ProductCompanyContext())
            {
                try
                {
                    string query = $"BACKUP DATABASE [{context.Database.GetDbConnection().Database}] TO DISK = '{backupFilePath}'";
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = query;
                        context.Database.OpenConnection();
                        command.ExecuteNonQuery(); // Выполнение команды
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    context.Database.CloseConnection();
                }
            }
        }

        public async Task AddStockForecast(int productId, DateTime forecastDate, int predictedQuantity)
        {
            var stockForecast = new StockForecast
            {
                ProductID = productId,
                ForecastDate = forecastDate,
                PredictedQuantity = predictedQuantity
            };

            using (var context = new ProductCompanyContext())
            {
                context.StockForecasts.Add(stockForecast);
                await context.SaveChangesAsync();
            }
        }


        public async Task AddSupplyPlan(int productId, DateTime plannedDate, int plannedQuantity, int supplierId)
        {
            var supplyPlan = new SupplyPlan
            {
                ProductID = productId,
                SupplyDate = plannedDate,
                PlannedQuantity = plannedQuantity,
                SupplierID = supplierId
            };

            using (var context = new ProductCompanyContext())
            {
                context.SupplyPlans.Add(supplyPlan);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<SupplyPlan>> LoadSupplyPlans()
        {
            using (var context = new ProductCompanyContext())
            {
                return await context.SupplyPlans.Include(sp => sp.Product).Include(sp => sp.Supplier).ToListAsync();
            }
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
        private void LoadProducts()
        {
            using (var context = new ProductCompanyContext())
            {
                Products = context.Products.ToList();
            }
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

        private void ExecuteQuery()
        {
            var query = SqlQueryTextBoxText; // Получаем SQL-запрос
            using (var context = new ProductCompanyContext())
            {
                try
                {
                    // Создаем DataTable для хранения результатов
                    DataTable dataTable = new DataTable();

                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = query; // Устанавливаем CommandText
                        context.Database.OpenConnection(); // Открываем соединение

                        using (var reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader); // Загружаем данные в DataTable
                        }
                    }

                    // Присваиваем результат в QueryResults
                    QueryResults = dataTable;
                }
                catch (Exception ex)
                {
                    // Обработка ошибок
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    context.Database.CloseConnection(); // Закрываем соединение
                }
            }
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
                    User.IsAdmin = _user.IsAdmin;
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
