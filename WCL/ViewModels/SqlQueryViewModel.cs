using Microsoft.Data.SqlClient;
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
    class SqlQueryViewModel : INotifyPropertyChanged
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
        public SqlQueryViewModel()
        {
            ErrorStrig = string.Empty;

            CommandClearErrorString = new Command(OnClearErrorString);
            CommandExecuteSql = new Command(OnExecuteSql);  
        }
        #endregion

        #region Properies
        
        public bool IsHasError => !string.IsNullOrEmpty(_errorStrig);
        public bool IsHasErrorReg => !string.IsNullOrEmpty(_errorStrigReg);

        private string? _sqlQuery { get; set; }
        public string SqlQuery
        {
            get => _sqlQuery ?? string.Empty;
            set
            {
                _sqlQuery = value;
                OnPropertyChanged("SqlQuery");
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
        private ObservableCollection<DataRowView> _queryResults { get; set; }
        public ObservableCollection<DataRowView> QueryResults
        {
            get => _queryResults;
            set
            {
                _queryResults = value;
                OnPropertyChanged("QueryResults");
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
        public ICommand CommandExecuteSql { get; set; }
        private void OnExecuteSql()
        {
            try
            {
                using (var connection = new SqlConnection("your_connection_string"))
                {
                    connection.Open();

                    using (var command = new SqlCommand(SqlQuery, connection))
                    {
                        // Получаем данные
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Очищаем предыдущие результаты
                            QueryResults.Clear();

                            // Добавляем строки из DataTable в ObservableCollection
                            foreach (DataRow row in dataTable.Rows)
                            {
                                QueryResults.Add(row.Table.DefaultView[0]); // Добавляем каждую строку
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorStrig = $"Ошибка при выполнении SQL-запроса: {ex.Message}";
            }
        }


        #endregion
    }
}
