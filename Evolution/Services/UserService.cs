using Evolution.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;

namespace Evolution.Services
{
    public static class UserService
    {
        private static bool isValidUserData = false;

        private static string PathToUserFolder = "";
        public static string PathToUserAuthFile = "";

        public static string currentUser = "";

        public static UserModel User = new();

        public static string CreateUser(string? login, string? password, string? confirmPassword, string Email = "none")
        {                     
                isValidUserData = IsValidUserData(login, password, confirmPassword, Email);

                if(isValidUserData)
                {
                    PathToUserFolder = InitUserJsonFiles(login);

                    if(PathToUserFolder == "Exists")
                    {
                        Debug.WriteLine("Ошибка. Пользователь существует.");
                        return "false";
                    }
                    else
                    {                       
                        Debug.WriteLine("Успешно. Пользователь создан.");
                        PathToUserAuthFile = $"{PathToUserFolder}\\user_auth.json";

                        User.Login = login;
                        User.Email = Email;
                        User.Password = password;
                        User.AccessRights = UserModel.Acces.asRegularUser;
                        DataSaveLoad.Serialize(User);
                        return PathToUserAuthFile;
                    }
                }
                else
                {
                    return "false";
                }
        }

        private static bool IsValidUserData(string? login, string? password, string? confirmPassword, string Email = "none")
        {
            var email = new EmailAddressAttribute();

            if (
                    login == null ||
                    password == null ||
                    confirmPassword == null
               )
            {
                Debug.WriteLine("Ошибка, один из параметрова null!\nПользователь не был создан.");
                return false;               
            }
            else if 
                (
                    (login.Trim().Length < 3) || login.Contains(" ") ||
                    (password.Trim().Length < 3) || password.Contains(" ") || confirmPassword != password ||
                    (!email.IsValid(Email) && Email != "none")
                )
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        private static string InitUserJsonFiles(string? login)
        {
            string folderPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Users\\{login}";

            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                var file = File.Create($"{folderPath}\\user_auth.json");
                file.Close();
                return folderPath;
            }
            else
            {
                return "Exists";
            }                      
        }       

        public static string GetUserLogin()
        {
            return currentUser;
        }
    }
}
