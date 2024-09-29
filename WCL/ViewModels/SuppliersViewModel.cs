using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Printing;
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
    class SuppliersViewModel : INotifyPropertyChanged
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
        public SuppliersViewModel()
        {
            ErrorStrig = string.Empty;
            VisibilityWindowAddSuppliers = Visibility.Collapsed;
            Suppliers = new ObservableCollection<Supplier>();
            CommandClearErrorString = new Command(OnClearErrorString);
            CommandChangeVisibility=new Command(OnChangeVisibility);
            LoadSuppliers();

        }
        #endregion

        #region Properies
        private ObservableCollection<Supplier> _suppliers { get; set; }
        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers ;
            set
            {
                _suppliers = value;
                OnPropertyChanged("Suppliers");
            }
        }

        public Command RemoveSupplierCommand { get; }

        public bool IsHasError => !string.IsNullOrEmpty(_errorStrig);
        public bool IsHasErrorReg => !string.IsNullOrEmpty(_errorStrigReg);

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
        private Visibility _visibilityWindowAddSuppliers { get; set; }
        public Visibility VisibilityWindowAddSuppliers
        {
            get => _visibilityWindowAddSuppliers;
            set
            {
                _visibilityWindowAddSuppliers = value;
                OnPropertyChanged("VisibilityWindowAddSuppliers");
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

        public ICommand CommandChangeVisibility{ get; set; }
        private void OnChangeVisibility()
        {
            try
            {
                VisibilityWindowAddSuppliers = VisibilityWindowAddSuppliers  == Visibility.Visible ? Visibility.Collapsed: Visibility.Visible;
            }
            catch
            {
                ErrorStrig = "Ошибка выхода из системы";
            }
        }

        public void AddSupplier(string supplierName, string contactName, string phone)
        {
            using (var context = new ProductCompanyContext())
            {
                // Создаем нового поставщика
                var supplier = new Supplier
                {
                    SupplierName = supplierName,
                    ContactName = contactName,
                    Phone = phone
                };

                // Добавляем в контекст
                context.Suppliers.Add(supplier);

                // Сохраняем изменения в базе данных
                context.SaveChanges();
            }

            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            Suppliers = null;
            using (var context = new ProductCompanyContext())
            {
                Suppliers  = new ObservableCollection<Supplier>(context.Suppliers);
            }
        }

        private void RemoveSupplier()
        {
            // Логика удаления поставщика
            if (SelectedSupplier != null)
            {
                Suppliers.Remove(SelectedSupplier);
            }
        }

        private bool CanRemoveSupplier()
        {
            return SelectedSupplier != null;
        }

        public Supplier SelectedSupplier { get; set; }

        #endregion
    }
}
