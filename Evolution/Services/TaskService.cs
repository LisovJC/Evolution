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

namespace Evolution.Services
{
    internal class TaskService
    {
        public static ObservableCollection<TaskModel> AllTasks { get; set; } = new();
        public static List<TaskModel> CommonTasks { get; set; } = new();
        public static ObservableCollection<File> TempTasks { get; set; } = new();
        public static TaskModel task = new();
        public static string PathToUserTasksFile = "";
        public static string PathToCommonTasksFile = $"{Environment.CurrentDirectory}\\Common_Tasks\\Common_Tasks.json";
        public static void CreateIssue(
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
                Categories = AddCategories(categories)
            };

            InitTaskJsonFiles(HelperService.CurrentUser);

            AllTasks = DataSaveLoad.LoadData<TaskModel>(PathToUserTasksFile);
            AllTasks.Add(task);
            DataSaveLoad.Serialize(AllTasks);
            AddCommonTask(HelperService.CurrentUser);
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
                return "Exists";
            }
        }

        private static async void AddCommonTask(string login)
        {
            CommonTasks = await GetCommonTasks();
            if (CommonTasks.Count == 0)
            {
                for (int i = 0; i < AllTasks.Count; i++)
                {
                    if (AllTasks[i].TypeTask == TypeTaskEdentity.global)
                    {                       
                         CommonTasks.Add(AllTasks[i]);                      
                    }
                }
            }
            else
            {
                for (int i = 0; i < AllTasks.Count; i++)
                {
                    if (AllTasks[i].TypeTask == TypeTaskEdentity.global)
                    {
                        for (int j = 0; j < CommonTasks.Count; j++)
                        {
                            if (CommonTasks[j] != AllTasks[i])
                            {
                                CommonTasks.Add(AllTasks[i]);
                            }
                        }
                    }
                }
            }

            DataSaveLoad.SaveDatas(PathToCommonTasksFile,CommonTasks);

            try
            {
                if(HelperService.GetItemIDByName(TempTasks, "Common_Tasks.json") != null)
                {
                    GoogleDriveService.Remove(HelperService.GetItemIDByName(TempTasks, "Common_Tasks.json"));
                }
                
                await GoogleDriveService.uploadFile(PathToCommonTasksFile, HelperService.IdCommonTaskFolder);
            }
            catch (Exception ex)
            {

                Debug.Write(ex.Message);
            }    
                                 
        }

        private static async Task<List<TaskModel>> GetCommonTasks()
        {            
            bool waiting = await HelperService.HelperUpdateData(HelperService.CurrentUser);           
            
            if (!System.IO.File.Exists(PathToCommonTasksFile))
            {
                var file = System.IO.File.Create(PathToCommonTasksFile);
                file.Close();
            }

            TempTasks = HelperService.FilesAndFoldersInCommonTasksFolder;          
            ObservableCollection<TaskModel> temp;

            if (TempTasks.Count == 0)
            {
                return CommonTasks;
            }
            else
            {
                await GoogleDriveService.Download(HelperService.GetItemIDByName(TempTasks, "Common_Tasks.json"), PathToCommonTasksFile);
                temp = DataSaveLoad.LoadData<TaskModel>(PathToCommonTasksFile);
            }
            for (int i = 0; i < temp.Count; i++)
            {
                CommonTasks.Add(temp[i]);
            }
            return CommonTasks;
            
        }
    }
}
