using Evolution.Core;
using Evolution.Model;
using Evolution.Services.HelperServices;
using Evolution.Services.UserServices;
using System;
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
        
        public HomeViewModel()
        {
            InitPageProperties();
        }       

        private void InitPageProperties()
        {
            try
            {
                UserModel User = Task.Run(() => AuthUserService.UserExists(HelperService.CurrentUser)).Result;
                
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
