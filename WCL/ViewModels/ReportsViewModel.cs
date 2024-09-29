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
            CommandClearErrorString = new Command(OnClearErrorString);
        }
        #endregion

        #region Properies
        public Command GenerateReportCommand { get; }
        public Command ExportToExcelCommand { get; }
        public Command ExportToWordCommand { get; }
        public bool IsHasError => !string.IsNullOrEmpty(_errorStrig);
        public bool IsHasErrorReg => !string.IsNullOrEmpty(_errorStrigReg);
        private ObservableCollection<SalesReport> _reports { get; set; }
        public ObservableCollection<SalesReport> Reports
        {
            get => _reports ?? new ObservableCollection<SalesReport>();
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
        public ObservableCollection<string> ReportTypes { get; set; }
        public string SelectedReportType { get; set; }
        public string ReportStatus { get; set; }

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
                    //Запрашиваем отчеты из базы данных
                   var reportsFromDb = context.SalesReports.ToList();

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

        private void ExportToExcel()
        {
            // Логика для экспорта данных в Excel
            //var excelApp = new Application();
            //Workbook workbook = excelApp.Workbooks.Add();
            //Worksheet worksheet = workbook.Sheets[1];

            //// Пример заполнения данными
            //worksheet.Cells[1, 1] = "Пример отчета";
            //workbook.SaveAs("Отчет.xlsx");
            //workbook.Close();
            //excelApp.Quit();

            //ReportStatus = "Отчет экспортирован в Excel!";
            //OnPropertyChanged(nameof(ReportStatus));
        }
        private void GenerateReport()
        {
            // Логика генерации отчета
            //Reports.Add(new Report { Id = 1, ReportName = "Отчет", CreationDate = DateTime.Now });
        }

        #endregion
    }
}

