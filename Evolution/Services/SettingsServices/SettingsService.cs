using Evolution.Model;
using Evolution.Services.DataSaveLoadServices;
using Evolution.Services.HelperServices;
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
            string pathToSettingsFolder = HelperService.pathToSettingsFolder;
            string pathToSettingsFile = pathToSettingsFolder + "//appSettings.json";

            if (!Directory.Exists(pathToSettingsFolder))
            {
                Directory.CreateDirectory(pathToSettingsFolder);
            }

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
    }
}
