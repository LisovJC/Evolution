﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Evolution.Core;
using Evolution.Model;
using Evolution.ViewModel.Pages;
using System;
using Evolution.Services.UserServices;

namespace Evolution.Services
{
    public class DataSaveLoad
    {

        #region Saver
        public static void Serialize(object o)
        {
            if (CreateUserService.User != null || TaskService.task != null)
            {
                if (o.GetType() == CreateUserService.User.GetType())
                {
                    SaveDatas(CreateUserService.PathToUserAuthFile, o);
                }

                if (o.GetType() == TaskService.AllTasks.GetType())
                {
                    SaveDatas(TaskService.PathToUserTasksFile, o);
                }

                //if (MessengerViewModel.MessageList != null)
                //{
                //    if (o.GetType() == MessengerViewModel.MessageList.GetType())
                //    {
                //        SaveDatas($"{AppDomain.CurrentDomain.BaseDirectory}\\Messages\\Messages.json", o);
                //    }
                //}

                //if (SettingsViewModel.settingsObj != null)
                //{
                //    if (o.GetType() == SettingsViewModel.settingsObj.GetType())
                //    {
                //        SaveDatas(JsonPathSettings, o);
                //    }
                //}

                //if (MainViewModelPage.CurrentDatas != null)
                //{
                //    if (o.GetType() == MainViewModelPage.CurrentDatas.GetType())
                //    {
                //        SaveDatas(JsonPathCurrentData, o);
                //    }
                //}

                //if (CreatePageViewModel.AdvancedPlans != null)
                //{
                //    if (o.GetType() == CreatePageViewModel.AdvancedPlans.GetType())
                //    {
                //        SaveDatas(JsonPathAdvancedPlansData, o);
                //    }
                //}
            }
        }

            public static void SaveDatas(string path, object o)
            {
                if (path != null)
                {
                    string dir = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir))
                    {
                        _ = Directory.CreateDirectory(dir);
                    }

                    using (StreamWriter file = File.CreateText(path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, o);
                    }
                }
            }
            #endregion

            #region Validation
            public static bool IsValidJson(string stringValue)
            {
                if (File.Exists(stringValue))
                {
                    var value = File.ReadAllText(stringValue).Trim();
                    if ((value.StartsWith("{") && value.EndsWith("}")) ||
                        (value.StartsWith("[") && value.EndsWith("]")))
                    {
                        try
                        {
                            JToken obj = JToken.Parse(value);
                            return true;
                        }
                        catch (JsonReaderException)
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            #endregion

            #region Loader
            public static ObservableCollectionEX<T> LoadData<T>(string path)
            {
                if (!IsValidJson(path))
                {
                    ObservableCollectionEX<T> objects = new ObservableCollectionEX<T>();
                    return objects;
                }

                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<ObservableCollectionEX<T>>(json);
            }

            public static UserModel LoadDataUser<T>(string path)
            {
                if (!IsValidJson(path))
                {
                    UserModel obj = new();
                    return obj;
                }

                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<UserModel>(json);
            }
            #endregion
        }
    } 
