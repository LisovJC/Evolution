using Evolution.Core;
using Evolution.Model;
using Evolution.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using static Evolution.Model.TaskModel;

namespace Evolution.Services
{
    internal class TaskService
    {
        public static ObservableCollection<TaskModel> AllTasks { get; set; } = new();
        public static ObservableCollection<TaskModel> GlobalTasks { get; set; } = new();
        public static TaskModel task = new();
        public static string PathToUserTasksFile = "";
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

            InitTaskJsonFiles(GetUserInfoService.GetUserLogin());

            AllTasks = DataSaveLoad.LoadData<TaskModel>(PathToUserTasksFile);
            AllTasks.Add(task);
            DataSaveLoad.Serialize(AllTasks);
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
                var file = File.Create($"{folderPath}\\Tasks.json");
                file.Close();
                return folderPath;
            }
            else
            {
                return "Exists";
            }
        }

        private static void AddGlobalTask()
        {
            for (int i = 0; i < AllTasks.Count; i++)
            {
                if (AllTasks[i].TypeTask == TypeTaskEdentity.global)
                {
                    GlobalTasks.Add(AllTasks[i]);
                }
            }
        }

        private static void GetGlobalTask()
        {

        }
    }
}
