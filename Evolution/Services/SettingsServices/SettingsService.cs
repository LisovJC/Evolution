using AutostartManagement;
using Evolution.Model;
using Evolution.Services.DataSaveLoadServices;
using Evolution.Services.HelperServices;
using System;
using System.IO;

namespace Evolution.Services.SettingsServices
{
    public static class SettingsService
    {
        public static SettingsModel sm { get; set; } = new()
        {
            RememberMeForAuth = false,
            Login = "",
            Password = ""
        };

        public static void CreateSettingsFile()
        {
            string pathToSettingsFile = HelperService.pathToSettingsFile;

            if(!File.Exists(pathToSettingsFile))
            {
                var file = File.Create(pathToSettingsFile);
                file.Close();
               
                DataSaveLoad.Serialize(sm);
            }
        }

        public static void UpdateSettingsFile(string login, string password, bool isRememberMe)
        {
            sm.RememberMeForAuth = isRememberMe;
            sm.Password = password;
            sm.Login = login;

            DataSaveLoad.Serialize(sm);
        }

        public static void AutoRunState(bool autoRunState = false)
        {
            string shortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\Evolution";
            string ExeFilePath = Environment.CurrentDirectory + "\\Evolution.exe";

            if(autoRunState)
            {
                var registerShortcutForAllUser = true;
                var autostartManager = new AutostartManager(shortCutPath, ExeFilePath, registerShortcutForAllUser);
                autostartManager.EnableAutostart();
            }
            else
            {
                var registerShortcutForAllUser = true;
                var autostartManager = new AutostartManager(shortCutPath, ExeFilePath, registerShortcutForAllUser);
                autostartManager.DisableAutostart();
            }
        }
    }
}
