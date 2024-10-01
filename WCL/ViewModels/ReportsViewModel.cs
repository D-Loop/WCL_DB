using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using WCL.Helpers;
using WCL.Models;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace WCL.ViewModels
{
    public class SupplyReport
    {
        [Key]
        public int SupplyID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime SupplyDate { get; set; }
        public int SupplierID { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
    }
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
            Reports = new ObservableCollection<SupplyReport>();
            CommandClearErrorString = new Command(OnClearErrorString);
            GenerateReportCommand = new Command(GenerateSupplyReport);
            ExportToExcelCommand = new Command(ExportToExcel);
            ExportToWordCommand = new Command(ExportToWord);
        }
        #endregion

        #region Properies

        public ICommand GenerateReportCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand ExportToWordCommand { get; }
        private void ExportToWord()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Documents (*.docx)|*.docx",
                Title = "Сохранить отчет как"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Создание документа Word по указанному пути
                using (var document = DocX.Create(saveFileDialog.FileName))
                {
                    var title = document.InsertParagraph("Отчет по поставкам")
                                         .FontSize(20)
                                         .Bold()
                                         .Alignment = Alignment.center; 

                    var table = document.InsertTable(Reports.Count + 1, 5);
                    table.Rows[0].Cells[0].Paragraphs[0].Append("ID поставки");
                    table.Rows[0].Cells[1].Paragraphs[0].Append("ID товара");
                    table.Rows[0].Cells[2].Paragraphs[0].Append("Количество");
                    table.Rows[0].Cells[3].Paragraphs[0].Append("Дата поставки");
                    table.Rows[0].Cells[4].Paragraphs[0].Append("Поставщик");

                    for (int i = 0; i < Reports.Count; i++)
                    {
                        var report = Reports[i];
                        table.Rows[i + 1].Cells[0].Paragraphs[0].Append(report.SupplyID.ToString());
                        table.Rows[i + 1].Cells[1].Paragraphs[0].Append(report.ProductID.ToString());
                        table.Rows[i + 1].Cells[2].Paragraphs[0].Append(report.Quantity.ToString());
                        table.Rows[i + 1].Cells[3].Paragraphs[0].Append(report.SupplyDate.ToShortDateString());
                        table.Rows[i + 1].Cells[4].Paragraphs[0].Append(report.SupplierName);
                    }

                    document.Save();
                }
            }
        }

        private void GenerateSupplyReport()
        {
            var reportData = new List<SupplyReport>();

            using (var context = new ProductCompanyContext())
            {
                var supplies = context.SupplyPlans.Include(s => s.Product).Include(s => s.Supplier).ToList();

                foreach (var supply in supplies)
                {
                    reportData.Add(new SupplyReport
                    {
                        SupplyID = supply.SupplyPlanID, // Обновите здесь
                        ProductID = supply.ProductID,
                        Quantity = supply.PlannedQuantity, // Проверьте, что используете нужное поле
                        SupplyDate = supply.SupplyDate, // Проверьте, что используете нужное поле
                        SupplierID = supply.SupplierID,
                        ProductName = supply.Product.ProductName,
                        SupplierName = supply.Supplier.SupplierName
                    });
                }
            }

            Reports = new ObservableCollection<SupplyReport>(reportData);
            // Здесь можно дополнительно вызвать метод для отображения отчета, например, ExportToExcel.
        }

        public bool IsHasError => !string.IsNullOrEmpty(_errorStrig);
        public bool IsHasErrorReg => !string.IsNullOrEmpty(_errorStrigReg);
        private ObservableCollection<SupplyReport> _reports { get; set; }
        public ObservableCollection<SupplyReport> Reports
        {
            get => _reports ?? new ObservableCollection<SupplyReport>();
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
        private List<SupplyReport> GetSupplyReportData()
        {
            using (var context = new ProductCompanyContext())
            {
                return context.SupplyPlans
                    .Include(s => s.Product)
                    .Include(s => s.Supplier)
                    .Select(supply => new SupplyReport
                    {
                        SupplyID = supply.SupplyPlanID,
                        ProductID = supply.ProductID,
                        Quantity = supply.PlannedQuantity,
                        SupplyDate = supply.SupplyDate,
                        SupplierID = supply.SupplierID,
                        ProductName = supply.Product.ProductName,
                        SupplierName = supply.Supplier.SupplierName
                    }).ToList();
            }
        }

        private void ExportToExcel()
        {
            var supplies = GetSupplyReportData(); // Получаем данные для отчета
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Supply Report");

                // Заголовки столбцов
                worksheet.Cells[1, 1].Value = "Supply ID";
                worksheet.Cells[1, 2].Value = "Product ID";
                worksheet.Cells[1, 3].Value = "Quantity";
                worksheet.Cells[1, 4].Value = "Supply Date";
                worksheet.Cells[1, 5].Value = "Supplier ID";
                worksheet.Cells[1, 6].Value = "Product Name";
                worksheet.Cells[1, 7].Value = "Supplier Name";

                // Заполнение данными
                int row = 2;
                foreach (var supply in supplies)
                {
                    worksheet.Cells[row, 1].Value = supply.SupplyID;
                    worksheet.Cells[row, 2].Value = supply.ProductID;
                    worksheet.Cells[row, 3].Value = supply.Quantity;
                    worksheet.Cells[row, 4].Value = supply.SupplyDate;
                    worksheet.Cells[row, 5].Value = supply.SupplierID;
                    worksheet.Cells[row, 6].Value = supply.ProductName;
                    worksheet.Cells[row, 7].Value = supply.SupplierName;
                    row++;
                }

                // Сохранение файла
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Сохранить отчет как"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, package.GetAsByteArray());
                }
            }
        }


        #endregion
    }
}

