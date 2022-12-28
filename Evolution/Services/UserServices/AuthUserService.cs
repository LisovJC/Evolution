using Evolution.Model;
using System;
using System.Diagnostics;
using System.IO;

namespace Evolution.Services.UserServices
{
    public class AuthUserService
    {
        private static string PathToUserFolder = "";

        public static UserModel User = new();

        public static bool SignIn(string login, string password)
        {
            PathToUserFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Users\\{login}";

            if (UserExists(PathToUserFolder))
            {
                User = DataSaveLoad.LoadDataUser<UserModel>(PathToUserFolder + "\\user_auth.json");
                if (IsCorrectUserData(login, password, User))
                {                                     
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static bool UserExists(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsCorrectUserData(string? login, string? password, UserModel user)
        {
            if (
                    login == null ||
                    password == null
               )
            {
                Debug.WriteLine("Ошибка, один из параметрова null!\nОшибка входа.");
                return false;
            }
            else if
                (
                    (login.Trim().Length >= 2) && !login.Contains(" ") && login == user.Login &&
                    (password.Trim().Length >= 2) && !password.Contains(" ") && password == user.Password
                )
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
