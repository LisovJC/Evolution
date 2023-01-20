using Evolution.Model;
using Evolution.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using static Evolution.Model.TaskModel;
using File = Google.Apis.Drive.v3.Data.File;
using System.Diagnostics;
using Evolution.Services.CloudStoreServices;
using Evolution.Services.DataSaveLoadServices;
using ControlzEx.Standard;

namespace Evolution.Services.TaskServices
{
    internal class TaskService
    {
        public static ObservableCollection<TaskModel> AllTasks { get; set; } = new();
        public static ObservableCollection<TaskModel> CommonTasks { get; set; } = new();
        public static ObservableCollection<File> FilesAndFoldersInCommonTasksFolder { get; set; } = new();
        public static TaskModel task = new();
        public static string PathToUserTasksFile = "";
        public static string PathToCommonTasksFile = $"{Environment.CurrentDirectory}\\Common_Tasks\\Common_Tasks.json";
        public static async void CreateTask(
            string Title,
            string Assigned,
            double PlannedTimeCosts,
            string Description,
            string OtherCategory,
            Priority priority,
            TypeTaskEdentity typeTask,
            List<Category> categories)
        {
            task = new TaskModel
            {
                Title = Title,
                Description = Description,
                Assigned = Assigned,
                PlannedTimeCosts = PlannedTimeCosts,
                OtherCategory = OtherCategory,
                TaskPriority = priority,
                TypeTask = typeTask,
                DateCreate = DateTime.Now,
                Categories = AddCategories(categories),
                Creator = HelperService.CurrentUser
            };

            switch(task.TypeTask)
            {
                case TypeTaskEdentity.local:
                    {
                        InitTaskJsonFiles(HelperService.CurrentUser);
                        AllTasks = DataSaveLoad.LoadData<TaskModel>(PathToUserTasksFile);
                        AllTasks.Add(task);
                        DataSaveLoad.Serialize(AllTasks);
                    }
                    break;
                case TypeTaskEdentity.global:
                    {
                        ////CommonTasks = await GetCommonTasks();

                        ////CommonTasks.Add(task);
                        ////DataSaveLoad.SaveDatas(PathToCommonTasksFile, CommonTasks);
                        //try
                        //{
                        //    if (HelperService.GetItemIDByName(FilesAndFoldersInCommonTasksFolder, "Common_Tasks.json") != null)
                        //    {
                        //        GoogleDriveService.Remove(HelperService.GetItemIDByName(FilesAndFoldersInCommonTasksFolder, "Common_Tasks.json"));
                        //    }

                        //    await GoogleDriveService.uploadFile(PathToCommonTasksFile, HelperService.IdCommonTaskFolder);
                        //}
                        //catch (Exception ex)
                        //{

                        //    Debug.Write(ex.Message);
                        //}
                        try
                        {
                           await Task.Run(() => FireBaseService.PushToDataBase(task,"GlobalTask"));
                           await Task.Run(() => HelperService.HelperUpdateData(HelperService.CurrentUser));
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine(ex.Message);
                        }
                    }
                    break;

            }       
        }

        private static List<Category> AddCategories(List<Category> Categories)
        {
            List<Category> finalCategories = new();
            for (int i = 0; i < Categories.Count; i++)
            {
                if (Categories[i].isSelect)
                {
                    finalCategories.Add(Categories[i]);
                }
            }
            return finalCategories;
        }

        private static string InitTaskJsonFiles(string? login)
        {
            string folderPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Users\\{login}\\Tasks";
            PathToUserTasksFile = $"{folderPath}\\Tasks.json";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                var file = System.IO.File.Create($"{folderPath}\\Tasks.json");
                file.Close();
                return folderPath;
            }
            else
            {
                return "This file exists!";
            }
        }

        public static async Task<ObservableCollection<TaskModel>> GetCommonTasks()
        {
            await HelperService.HelperUpdateData(HelperService.CurrentUser);

            if (!System.IO.File.Exists(PathToCommonTasksFile))
            {
                var file = System.IO.File.Create(PathToCommonTasksFile);
                file.Close();
            }

            FilesAndFoldersInCommonTasksFolder = HelperService.FilesAndFoldersInCommonTasksFolder;

            if (FilesAndFoldersInCommonTasksFolder.Count == 0)
            {
                return CommonTasks;
            }
            else
            {
                await GoogleDriveService.Download(HelperService.GetItemIDByName(
                    FilesAndFoldersInCommonTasksFolder,
                    "Common_Tasks.json"),
                    PathToCommonTasksFile);
                CommonTasks = DataSaveLoad.LoadData<TaskModel>(PathToCommonTasksFile);
                return CommonTasks;
            }
        }
    }
}
