﻿using Evolution.Model;
using Evolution.Services.CloudStoreServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using static Evolution.Enums.Enums;

namespace Evolution.Services.HelperServices
{
    public static class HelperService
    {
        /***ТУТЬ ХРАНИМ ВСЕ ПУТИ***/
        public static readonly string pathToSettingsFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Settings\\appSettings.json";
        /*=====================================================================*/
        public static readonly string pathToSettingsFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Settings";
        /*=====================================================================*/
        public static readonly string pathToUsersFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Users";
        /*=====================================================================*/
        public static readonly string pathToTasksFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Tasks";

        /***ОБЪЕКТ ВОШЕДШЕГО ПОЛЬЗОВАТЕЛЯ***/
        public static string CurrentUser { get; set; } = "";
        
        /***КОЛИЧЕСТВО ГЛОБАЛЬНЫХ ЗАДАЧ***/
        public static int CountTasksOfGlobalTaskList { get; set; } = 0;

        /***ВСЕ ПОЛЬЗОВАТЕЛИ В ПРИЛОЖЕНИИ***/
        public static List<UserModel> AllUsersInApp { get; set; } = new();

        /***ВСЕ ГЛОБАЛЬНЫЕ ЗАДАЧИ, КОТОРЫЕ ЗАГРУЗИЛИ И ХРАНИМ В КЕШЕ***/
        public static List<TaskModel> GlobalTasksInCash { get; set; } = new();


        /***ОБНОВЛЯЕМ ВСЕ ДАННЫЕ***/
        public static async Task<bool> HelperUpdateData(string user)
        {
            try
            {
                CurrentUser = user;
                AllUsersInApp = await Task.Run(() => FireBaseService.GetDataFromDataBase<UserModel>(TypeDatas.UserAuthData));
                CountTasksOfGlobalTaskList = await Task.Run(() => FireBaseService.GetCounterFromDataBase(TypeDatas.GlobalTasks));
                GlobalTasksInCash = await Task.Run(() => FireBaseService.GetDataFromDataBase<TaskModel>(TypeDatas.GlobalTasks, CountTasksOfGlobalTaskList));

                Debug.WriteLine($"\n^^^^^^^^^^^^^^^^^^^^^^^^^\n\t***** ВНИМАНИЕ. [HelperService]: Данные успешно получены и обновлены. ******\n");

                return true;
            }
            catch (System.Exception ex)
            {

                Debug.WriteLine(ex.Message);
                return false;
            }            
        }
    }
}
