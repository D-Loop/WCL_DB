using Microsoft.Data.SqlClient;
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
using System.Windows.Forms;
using System.Windows.Input;
using WCL.Helpers;
using WCL.Models;

namespace WCL.ViewModels
{
    class BackupViewModel : INotifyPropertyChanged
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
        public BackupViewModel()
        {
            ErrorStrig = string.Empty;
            CommandBrowseBackupPath= new Command(OnBrowseBackupPath);
            CommandExecuteBackup = new Command(OnExecuteBackup);
            CommandExecuteRestore = new Command(OnExecuteRestore);
            CommandClearErrorString = new Command(OnClearErrorString);
        }
        #endregion

        #region Properies
        
        public bool IsHasError => !string.IsNullOrEmpty(_errorStrig);
        public bool IsHasErrorReg => !string.IsNullOrEmpty(_errorStrigReg);
        private string? _backupFilePath { get; set; }
        public string BackupFilePath
        {
            get => _backupFilePath ?? string.Empty;
            set
            {
                _backupFilePath = value;
                OnPropertyChanged("BackupFilePath");
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

        public ICommand CommandExecuteBackup { get; set; }
        private void OnExecuteBackup()
        {
            if (string.IsNullOrEmpty(BackupFilePath))
            {
                System.Windows.MessageBox.Show("Пожалуйста, укажите путь для резервной копии.");
                return;
            }

            try
            {
                using (var connection = new SqlConnection("your_connection_string"))
                {
                    connection.Open();
                    string backupQuery = $"BACKUP DATABASE [YourDatabase] TO DISK = '{BackupFilePath}'";
                    using (var command = new SqlCommand(backupQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                System.Windows.MessageBox.Show("Создание резервной копии завершено успешно!");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при создании резервной копии: {ex.Message}");
            }
        }

        public ICommand CommandBrowseBackupPath { get; set; }
        private void OnBrowseBackupPath()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog()
                {
                    Filter = "Backup files (*.bak)|*.bak",
                    DefaultExt = "bak",
                    FileName = "DatabaseBackup.bak"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    BackupFilePath = saveFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при восстановлении базы данных: {ex.Message}");
            }
        }

        public ICommand CommandExecuteRestore { get; set; }
        private void OnExecuteRestore()
        {
            if (string.IsNullOrEmpty(BackupFilePath))
            {
                System.Windows.MessageBox.Show("Пожалуйста, выберите файл резервной копии для восстановления.");
                return;
            }

            try
            {
                using (var connection = new SqlConnection("your_connection_string"))
                {
                    connection.Open();
                    string restoreQuery = $"RESTORE DATABASE [YourDatabase] FROM DISK = '{BackupFilePath}' WITH REPLACE";
                    using (var command = new SqlCommand(restoreQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                System.Windows.MessageBox.Show("База данных восстановлена успешно!");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при восстановлении базы данных: {ex.Message}");
            }
        }

        #endregion
    }
}
