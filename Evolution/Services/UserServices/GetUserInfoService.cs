namespace Evolution.Services.UserServices
{
    internal class GetUserInfoService
    {

        private static string currentUser = "";
        private static string idUserFolder = "";

        public static void SetUserLogin(string login)
        {
            currentUser = login;
        }

        public static void SetUserGDriveFolder(string UserFolder)
        {
            idUserFolder = UserFolder;
        }

        public static string GetUserLogin()
        {
            return currentUser;
        }

        public static string GetUserGDriveFolder()
        {
            if(idUserFolder == "") CreateUserService.CreateUserFolderInGDrive(currentUser);
            
            return idUserFolder;
        }
    }
}
