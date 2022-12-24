using Evolution.Command;
using Evolution.Core;
using Evolution.Services.UserServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using File = Google.Apis.Drive.v3.Data.File;

namespace Evolution.ViewModel.Windows
{
    internal class SignViewModel : ObservableObject
    {

        #region VisibilityProperties
        private Visibility _signUpVisibility = Visibility.Collapsed;

        public Visibility SignUpVisibility
        {
            get => _signUpVisibility;
            set => Set(ref _signUpVisibility, value);
        }

        private string _labelSign = "Sign in";

        public string LabelSign
        {
            get => _labelSign;
            set => Set(ref _labelSign, value);
        }

        private bool _isEnableSignIn = true;

        public bool isEnableSignIn
        {
            get => _isEnableSignIn;
            set => Set(ref _isEnableSignIn, value);
        }

        private bool _isEnableSignUp = true;

        public bool isEnableSignUp
        {
            get => _isEnableSignUp;
            set => Set(ref _isEnableSignUp, value);
        }

        private bool _isEnableGetStarted = false;

        public bool isEnableGetStarted
        {
            get => _isEnableGetStarted;
            set => Set(ref _isEnableGetStarted, value);
        }

        private Visibility _reqired = Visibility.Collapsed;

        public Visibility Reqired
        {
            get => _reqired;
            set => Set(ref _reqired, value);
        }

        private Visibility _iconVisibility = Visibility.Visible;

        public Visibility IconVisibility
        {
            get => _iconVisibility;
            set => Set(ref _iconVisibility, value);
        }
        #endregion

        #region Fields
        private string _login = "";

        public string Login
        {
            get => _login;
            set { Set(ref _login, value); IsValidData(); }
        }

        private string _password = "";

        public string Password
        {
            get => _password;
            set { Set(ref _password, value); IsValidData(); }
        }

        private string _confrimPassword = "";

        public string ConfirmPassword
        {
            get => _confrimPassword;
            set { Set(ref _confrimPassword, value); IsValidData(); }
        }

        private string _email = "none";

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        private string _contentRegButton = "Sign Up";

        public string ContentRegButton
        {
            get => _contentRegButton;
            set => Set(ref _contentRegButton, value);
        }

        #endregion

        #region GoogleDrive
        ObservableCollection<File> FilesAndFolders = new ObservableCollection<File>();

        private static string _EVOLUTIONFolder = "";
        private static string _UserFolder = "";
        #endregion

        #region Commands
        public RelayCommand SignInCommand { get; set; }
        public RelayCommand SignUpCommand { get; set; }
        public RelayCommand GetStartedCommand { get; set; }
        public RelayCommand CloseAppCommand { get; set; }
        #endregion        
        public SignViewModel()
        {
            #region Commands
            SignInCommand = new(o =>
            {
                if (AuthUserService.SignIn(Login, Password))
                {
                    MainWindow mainWindow = new();
                    mainWindow.Owner = Application.Current.MainWindow;
                    mainWindow.Show();
                    Application.Current.MainWindow.Hide();
                }
                else
                {
                    Debug.WriteLine("Непредвиденная ошибка входа!");
                }
            });

            SignUpCommand = new(o =>
            {
                if (SignUpVisibility == Visibility.Collapsed)
                {
                    SignUpVisibility = Visibility.Visible;
                    LabelSign = "Registration";
                    isEnableSignIn = false;
                    Reqired = Visibility.Visible;
                    ContentRegButton = "Cancel";
                    IconVisibility = Visibility.Collapsed;
                }
                else
                {
                    SignUpVisibility = Visibility.Collapsed;
                    LabelSign = "Sign in";
                    isEnableSignIn = true;
                    Reqired = Visibility.Collapsed;
                    ContentRegButton = "Sign Up";
                    IconVisibility = Visibility.Visible;
                }

            });

            GetStartedCommand = new(o =>
            {
                SignUpVisibility = Visibility.Collapsed;
                LabelSign = "Sign in";
                isEnableSignIn = true;
                Reqired = Visibility.Collapsed;
                ContentRegButton = "Sign Up";
                IconVisibility = Visibility.Visible;

                string userSucces = "";
                userSucces = CreateUserService.CreateUser(Login, Password, ConfirmPassword, Email);

                if (userSucces != "false")
                {
                    Debug.WriteLine("Succes!");
                    ConfirmPassword = string.Empty;
                    Email = string.Empty;                    
                }
            });

            CloseAppCommand = new(o =>
            {
                Application.Current.Shutdown();
            });
            #endregion                      
        }

        private void IsValidData()
        {
            try
            {
                if (Login.Length > 2 && Password.Length > 2 && Password == ConfirmPassword)
                {
                    isEnableGetStarted = true;
                }
                else
                {
                    isEnableGetStarted = false;
                }
            }
            catch (System.Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }            
        }
           
        //async void Test()
        //{
        //    FilesAndFolders = await GoogleDriveService.ListEntities();
        //    string id_Evo = GetItemIDByName("EVOLUTION").Result;
        //    FilesAndFolders = await GoogleDriveService.ListEntities(id_Evo);
        //    id_Evo = GetItemIDByName("Lisov").Result;
        //    FilesAndFolders = await GoogleDriveService.ListEntities(id_Evo);
        //    id_Evo = GetItemIDByName("user_auth.json").Result;

            
        //    GoogleDriveService.Download(id_Evo, "C:\\Users\\lisov\\source\\repos\\Evolution_1\\Evolution\\bin\\ee.json"); //Загрузка и сейв файла.                         
        //}
    }
}
