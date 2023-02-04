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
using System.Data.Common;

namespace Evolution.Services.TaskServices
{
    internal class TaskService
    {
        public static ObservableCollection<TaskModel> AllTasks { get; set; } = new();
        /*=====================================================================*/
        public static TaskModel NewTask = new();
        /*=====================================================================*/
        public static string PathToUserTasksFile = "";
        
        public static async void CreateTask(
            string title,
            string assigned,
            double plannedTimeCosts,
            string description,
            string OtherCategory,
            string deadLine,
            TypeTaskEdentity typeTask,
            List<Category> categories)
        {
            NewTask = new TaskModel
            {
                Title = title,
                Description = description,
                Assigned = assigned,
                PlannedTimeCosts = plannedTimeCosts,
                OtherCategory = OtherCategory,
                DeadLine = deadLine,
                TypeTask = typeTask,
                DateCreate = DateTime.Now.ToString("d"),
                Categories = AddCategories(categories),
                Creator = "автор: " + HelperService.CurrentUser
            };

            switch(NewTask.TypeTask)
            {
                case TypeTaskEdentity.local:
                    {
                        InitTaskJsonFiles(HelperService.CurrentUser);
                        AllTasks = DataSaveLoad.LoadData<TaskModel>(PathToUserTasksFile);
                        AllTasks.Add(NewTask);
                        DataSaveLoad.Serialize(AllTasks);
                    }
                    break;
                case TypeTaskEdentity.global:
                    {                       
                        try
                        {
                           List<TaskModel> a = new();
                           a = await Task.Run(() => FireBaseService.GetDataFromDataBase<TaskModel>("GlobalTask\\Tasks"));
                           int c = 0;
                           if(a != null && a.Count != 0)
                            {
                                c = a.Count;
                            }
                            else
                            {
                                NewTask.ID = 0;
                            }
                           NewTask.ID = c;
                           await Task.Run(() => FireBaseService.PushToDataBase(NewTask, $"GlobalTask\\Tasks\\{NewTask.ID}"));
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

        public static async void UpdateTaskList(TaskModel NewTask)
        {
            try
            {
                //for (int i = 0; i < HelperService.GlobalTasksInCash.Count; i++)
                //{
                //    if (HelperService.GlobalTasksInCash[i] == NewTask)
                //    {
                //        HelperService.GlobalTasksInCash[i] = NewTask;
                //    }
                //}
                await Task.Run(() => FireBaseService.UpdateToDataBase(NewTask, "GlobalTask"));
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
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
            string folderPath = $"{HelperService.pathToTasksFolder}\\{login}";
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
                return "error: This file exists!";
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        //public static async Task<ObservableCollection<TaskModel>> GetCommonTasks()
        //{
        //    await HelperService.HelperUpdateData(HelperService.CurrentUser);

        //    if (!System.IO.File.Exists(PathToCommonTasksFile))
        //    {
        //        var file = System.IO.File.Create(PathToCommonTasksFile);
        //        file.Close();
        //    }

        //    FilesAndFoldersInCommonTasksFolder = HelperService.FilesAndFoldersInCommonTasksFolder;

        //    if (FilesAndFoldersInCommonTasksFolder.Count == 0)
        //    {
        //        return CommonTasks;
        //    }
        //    else
        //    {
        //        await GoogleDriveService.Download(HelperService.GetItemIDByName(
        //            FilesAndFoldersInCommonTasksFolder,
        //            "Common_Tasks.json"),
        //            PathToCommonTasksFile);
        //        CommonTasks = DataSaveLoad.LoadData<TaskModel>(PathToCommonTasksFile);
        //        return CommonTasks;
        //    }
        //}
    }
}
