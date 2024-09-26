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
    class ReportsViewModel : INotifyPropertyChanged
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
        public ReportsViewModel()
        {
            ErrorStrig = string.Empty;
            GenerateReportCommand = new Command(GenerateReport);
            ExportToExcelCommand = new Command(ExportToExcel);
            ExportToWordCommand = new Command(ExportToWord);
            CommandClearErrorString = new Command(OnClearErrorString);
        }
        #endregion

        #region Properies
        public ObservableCollection<Report> Reports { get; set; }

        public Command GenerateReportCommand { get; }
        public Command ExportToExcelCommand { get; }
        public Command ExportToWordCommand { get; }
        public bool IsHasError => !string.IsNullOrEmpty(_errorStrig);
        public bool IsHasErrorReg => !string.IsNullOrEmpty(_errorStrigReg);
        private ObservableCollection<Report> _reports { get; set; }
        public ObservableCollection<Report> Reports
        {
            get => _reports ?? new ObservableCollection<Report>();
            set
            {
                _reports = value;
                OnPropertyChanged("Reports");
            }
        }
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
        /// <summary> очистка строки ошибки</summary>
        public ICommand CommandLoadReports { get; set; }
        private void OnLoadReports()
        {
            try
            {
                using (var context = new ProductCompanyContext())
                {
                    // Запрашиваем отчеты из базы данных
                    var reportsFromDb = context.Reports.ToList();

                    // Очищаем коллекцию перед добавлением новых данных
                    Reports.Clear();

                    foreach (var report in reportsFromDb)
                    {
                        Reports.Add(report); // Добавляем каждый отчет в коллекцию
                    }
}
            }
            catch (Exception ex)
            {
                ErrorStrig = "Ошибка выхода из системы";
            }
        }

        private void GenerateReport()
        {
            // Логика генерации отчета
            Reports.Add(new Report { Id = 1, ReportName = "Отчет", CreationDate = DateTime.Now });
        }

        #endregion
    }
}
