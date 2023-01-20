using Evolution.Model;
using Evolution.Services.CloudStoreServices;
using Evolution.Services.DataSaveLoadServices;
using Evolution.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Evolution.Services.UserServices
{
    public class AuthUserService
    {
        private static string PathToUserFolder = "";

        public static UserModel User = new();

        public static async Task<bool> SignIn(string login, string password)
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
                HelperService.AllUsersInApp = await FireBaseService.GetDataFromDataBase<UserModel>("UserAuthData/Users");
                List<UserModel> AllUsersInApp = new();
                AllUsersInApp = HelperService.AllUsersInApp;
                if (AllUsersInApp.Count > 0)
                {
                    foreach (var user in AllUsersInApp)
                    {
                        if (user.Login == login) return true;
                        else return false;
                    }
                }
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
