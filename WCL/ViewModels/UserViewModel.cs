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
    class UserViewModel : User, INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region Properies

        public new int UserID
        {
            get => base.UserID;
            set
            {
                base.UserID = value;
                OnPropertyChanged("Reports");
            }
        }

        private string _FIO { get; set; }
        public string FIO
        {
            get => _FIO;
            set
            {
                _FIO = value;
                OnPropertyChanged("FIO");
            }
        }


    public new string Username
        {
            get => base.Username;
            set
            {
                base.Username = value;
                OnPropertyChanged("Username");
            }
        }
        public new string Password
        {
            get => base.Password;
            set
            {
                base.Password = value;
                OnPropertyChanged("Password");
            }
        }

        public new bool IsAdmin
        {
            get => base.IsAdmin;
            set
            {
                base.IsAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }

        public new bool IsLogIn
        {
            get => base.IsLogIn;
            set
            {
                base.IsLogIn = value;
                OnPropertyChanged("IsLogIn");
            }
        }
        
        #endregion

    }
}

