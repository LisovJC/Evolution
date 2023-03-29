using Evolution.Command;
using Evolution.Core;
using Evolution.Model;
using Evolution.Services.HelperServices;
using Evolution.Services.SettingsServices;
using Evolution.Services.UserServices;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Evolution.ViewModel.Pages
{
    internal class HomeViewModel : ObservableObject
    {
        #region UserProperties
        private string _login;

        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private string _registrationDate;

        public string RegistrationDate
        {
            get  => _registrationDate;
            set => Set(ref _registrationDate, value); 
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }
        #endregion

        private bool _isAutoRun = false;

        public bool IsAutoRun
        {
            get => _isAutoRun;
            set => Set(ref _isAutoRun, value);
        }

        private bool _isRememberMe = false;

        public bool IsRememberMe
        {
            get => _isRememberMe;
            set => Set(ref _isRememberMe, value);
        }

        public RelayCommand SetAutoRunStateCommand { get; set; }
        public RelayCommand SetRememberMeStateCommand { get; set; }

        public ObservableCollection<TaskModel> GlobalTasks { get; set; } = new() { new (){ Creator = "Lisov", Title = " test1" }};

        public HomeViewModel()
        {
            InitPageProperties();

            SettingsModel sm = SettingsService.GetSettings();
            IsAutoRun = sm.AutoRun;
            IsRememberMe = sm.RememberMeForAuth;

            SetAutoRunStateCommand = new(o =>
            {
                SettingsService.SetAutoRunSettings(IsAutoRun);
            });

            SetRememberMeStateCommand = new(o =>
            {
                string login = HelperService.CurrentUser;
                string password = HelperService.GetUserObject(HelperService.CurrentUser).Password;
                SettingsService.SetRememberDataForAuthSettings(login, password, IsRememberMe);
            });
        }       

        private void InitPageProperties()
        {
            try
            {
                UserModel User = Task.Run(() => AuthUserService.UserExists(HelperService.CurrentUser)).Result; //TODO вызывает подтормаживания

                Login = User.Login;
                Password = User.Password;
                RegistrationDate = User.DateCreate.ToString("d");
                Email = User.Email;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "\nОшибка загрузки данных HomePage.");
            }           
        }
    }
}
