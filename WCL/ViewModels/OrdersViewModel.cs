using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    class OrdersViewModel : INotifyPropertyChanged
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
        public OrdersViewModel()
        {
            ErrorStrig = string.Empty;

            CreateOrderCommand = new RelayCommand(CreateOrder);
            CancelOrderCommand = new RelayCommand(CancelOrder, CanCancelOrder);
            CommandClearErrorString = new Command(OnClearErrorString);
        }
        #endregion

        #region Properies
        public ObservableCollection<Order> Orders { get; set; }

        public Command CreateOrderCommand { get; }
        public Command CancelOrderCommand { get; }
        public bool IsHasError => !string.IsNullOrEmpty(_errorStrig);
        public bool IsHasErrorReg => !string.IsNullOrEmpty(_errorStrigReg);
        public Order SelectedOrder { get; set; }

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
        private Visibility _visibilityWindowRegistration { get; set; }
        public Visibility VisibilityWindowRegistration
        {
            get => _visibilityWindowRegistration ;
            set
            {
                _visibilityWindowRegistration = value;
                OnPropertyChanged("VisibilityWindowRegistration");
            }
        }
        #endregion

        #region Command
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
        private void CreateOrder()
        {
            // Логика создания заказа
            Orders.Add(new Order { Id = 1, ClientName = "Иванов", OrderDate = DateTime.Now, Status = "Открыт" });
        }

        private void CancelOrder()
        {
            // Логика отмены заказа
            if (SelectedOrder != null)
            {
                Orders.Remove(SelectedOrder);
            }
        }

        private bool CanCancelOrder()
        {
            return SelectedOrder != null;
        }


        #endregion
    }
}
