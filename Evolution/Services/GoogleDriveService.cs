using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using static Google.Apis.Drive.v3.DriveService;

namespace Evolution.Services
{
    public class GoogleDriveService
    {
        protected static string[] scopes = { DriveService.Scope.Drive };
        protected static UserCredential credential;
        static string ApplicationName = "EvolutionApp";
        protected static DriveService service;
        protected static FileExtensionContentTypeProvider fileExtensionProvider;

        private static async Task AuthenticateAsync()
        {
            using (var stream =
                new FileStream($"{Environment.CurrentDirectory}\\client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "lisovfox2002@gmail.com",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                fileExtensionProvider = new FileExtensionContentTypeProvider();
            }

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public static async Task<ObservableCollection<Google.Apis.Drive.v3.Data.File>> ListEntities(string id = "root")
        {
            await AuthenticateAsync();
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 100;
            listRequest.Fields = "nextPageToken, files(id, name, parents, createdTime, modifiedTime, mimeType)";
            listRequest.Q = $"'{id}' in parents";

            return new ObservableCollection<Google.Apis.Drive.v3.Data.File>(listRequest.Execute().Files);
        }

        public static async Task<Google.Apis.Drive.v3.Data.File> CreateFolder(string name, string id = "root")
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = name,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new[] { id }
            };

            var request = service.Files.Create(fileMetadata);
            request.Fields = "id, name, parents, createdTime, modifiedTime, mimeType";

            return request.Execute();
        }

        public static async Task<Google.Apis.Drive.v3.Data.File> uploadFile(string _uploadFile, string id, string _descrp = "Uploaded with .NET!")
        {
            if (System.IO.File.Exists(_uploadFile))
            {
                Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
                body.Name = System.IO.Path.GetFileName(_uploadFile);
                body.Description = _descrp;
                body.MimeType = GetMimeType(_uploadFile);
                body.Parents = new List<string> { id };// UN comment if you want to upload to a folder(ID of parent folder need to be send as paramter in above method)
                byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.CreateMediaUpload request = service.Files.Create(body, stream, GetMimeType(_uploadFile));
                    request.SupportsTeamDrives = true;
                   
                    await request.UploadAsync();
                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message, "Error Occured");
                    return null;
                }
            }
            else
            {
                Debug.WriteLine("The file does not exist.", "404");
                return null;
            }
        }

        public static void Remove(string id)
        {
            service.Files.Delete(id).Execute();
        }

        private static string GetMimeType(string fileName) 
        { 
            string mimeType = "application/unknown"; 
            string ext = System.IO.Path.GetExtension(fileName).ToLower(); 
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext); 
            if (regKey != null && regKey.GetValue("Content Type") != null) 
                mimeType = regKey.GetValue("Content Type").ToString(); 
            System.Diagnostics.Debug.WriteLine(mimeType); return mimeType; 
        }


    }
}
