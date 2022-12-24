﻿using ControlzEx.Standard;
using Evolution.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using File = Google.Apis.Drive.v3.Data.File;
using System.IO;
using System.Threading.Tasks;

namespace Evolution.Services.UserServices
{
    public static class CreateUserService
    {
        private static string PathToUserFolder = "";
        public static string PathToUserAuthFile = "";
        public static string EVOLUTIONFolder = "";

        private static ObservableCollection<File> FilesAndFolders = new ObservableCollection<File>();

        public static UserModel User = new();

        public static string CreateUser(string? login, string? password, string? confirmPassword, string Email = "none")
        {

            if (IsValidUserData(login, password, confirmPassword, Email))
            {
                PathToUserFolder = CreateUserJSON(login);

                if (PathToUserFolder == "Exists")
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
                    User.DateCreate = DateTime.Now;
                    
                    DataSaveLoad.Serialize(User);
                    
                    CreateUserFolderInGDrive(login, true);
                    
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

        private static string CreateUserJSON(string? login)
        {
            string folderPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Users\\{login}";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                var file = System.IO.File.Create($"{folderPath}\\user_auth.json");
                file.Close();
                return folderPath;
            }
            else
            {
                return "Exists";
            }
        }

        public static async void CreateUserFolderInGDrive(string login, bool isCreate = false)
        {
                                 
            FilesAndFolders = await GoogleDriveService.ListEntities(); //Получаем весь список файлов и папок в корне
            
            EVOLUTIONFolder = GetItemIDByName("EVOLUTION"); //айди папки EVOLUTION
            
            if(isCreate) await GoogleDriveService.CreateFolder(login, EVOLUTIONFolder); //Создание папки пользователя        

            FilesAndFolders = await GoogleDriveService.ListEntities(EVOLUTIONFolder); //Весь список файлов и папок в ПАПКЕ EVOLUTION
            string UserFolder = GetItemIDByName(login); //Получение айпи папки пользователя
            GetUserInfoService.SetUserGDriveFolder(UserFolder);

            if (isCreate) await GoogleDriveService.uploadFile($"{AppDomain.CurrentDomain.BaseDirectory}\\Users\\{login}\\user_auth.json", UserFolder); //Загрузка файла данных о пользователе в его папку


            //GoogleDriveService.Remove(GetFolderIDByName("FFF").Result);//Удаление папки в папке
        }

        private static string GetItemIDByName(string name)
        {
            for (int i = 0; i < FilesAndFolders.Count; i++)
            {
                if (FilesAndFolders[i].Name == name)
                {
                    Debug.WriteLine(FilesAndFolders[i].Name);
                    return FilesAndFolders[i].Id;
                }
            }

            return null;
        }
    }
}
