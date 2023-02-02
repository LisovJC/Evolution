using Evolution.Core;
using Evolution.Model;
using Evolution.Services.CloudStoreServices;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Evolution.Services.HelperServices
{
    public static class HelperService
    {
        #region Paths
        public static readonly string pathToSettingsFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Settings";
        public static readonly string pathToSettingsFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Settings\\appSettings.json";
        
        public static readonly string pathToUsersFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Users";
        
        public static readonly string pathToTasksFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Tasks";

        public static readonly string pathToCurrentStateDatasFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\CurrentStateDatas";
        public static readonly string pathToCurrentStateDatasFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\CurrentStateDatas\\currentDatas.json";
        #endregion      

        public static string CurrentUser { get; set; } = "";
        public static string IdEvolutionFolder { get; set; } = "";
        public static string IdUserFolder { get; set; } = "";
        public static string IdCommonTaskFolder { get; set; } = "";
        public static ObservableCollection<File> FilesAndFoldersInRootFolder { get; set; } = new();
        public static ObservableCollection<File> FilesAndFoldersInEvolutionFolder { get; set; } = new();
        public static ObservableCollection<File> FilesAndFoldersInUserFolder { get; set; } = new();
        public static ObservableCollection<File> FilesAndFoldersInCommonTasksFolder { get; set; } = new();
        public static List<UserModel> AllUsersInApp { get; set; } = new();

        public static TaskModel SelectTask { get; set; } = new();
        public static int index { get; set; } = -2;

        public static ObservableCollectionEX<TaskModel> GlobalTasksInCash { get; set; } = new ();

        public static async Task<bool> HelperUpdateData(string user)
        {
            try
            {
                AllUsersInApp.Clear();
                CurrentUser = user;
                AllUsersInApp = await Task.Run(() => FireBaseService.GetDataFromDataBase<UserModel>("UserAuthData\\Users"));
                
                //FilesAndFoldersInRootFolder = await Task.Run(() => GoogleDriveService.ListEntities());
                //IdEvolutionFolder = GetItemIDByName(FilesAndFoldersInRootFolder, "EVOLUTION");
                //FilesAndFoldersInEvolutionFolder = await Task.Run(() => GoogleDriveService.ListEntities(IdEvolutionFolder));
                //IdUserFolder = GetItemIDByName(FilesAndFoldersInEvolutionFolder, CurrentUser);
                //IdCommonTaskFolder = GetItemIDByName(FilesAndFoldersInEvolutionFolder, "common_folder");
                //FilesAndFoldersInCommonTasksFolder = await Task.Run(() => GoogleDriveService.ListEntities(IdCommonTaskFolder));
                //FilesAndFoldersInUserFolder = await Task.Run(() => GoogleDriveService.ListEntities(IdUserFolder));               
                //for (int i = 0; i < FilesAndFoldersInEvolutionFolder.Count; i++)
                //{
                //    if (FilesAndFoldersInEvolutionFolder[i].Name != "common_folder")
                //    {
                //        AllUsersInApp.Add(new()
                //        {
                //            Login = FilesAndFoldersInEvolutionFolder[i].Name
                //        });
                //    }
                //}
                return true;
            }
            catch (System.Exception ex)
            {

                Debug.WriteLine(ex.Message);
                return false;
            }            
        }
        
        public static string GetItemIDByName(ObservableCollection<File> FilesAndFolders, string name)
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
