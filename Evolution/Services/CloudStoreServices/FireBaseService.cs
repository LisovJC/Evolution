using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp;
using Evolution.Model;
using Evolution.Services.HelperServices;

namespace Evolution.Services.CloudStoreServices
{
    internal class FireBaseService
    {
        private static FirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "7ObrgEm3lNF073bppXmCYsboUhdVSBqonvvLqIyp",
            BasePath = "https://evolution-5d406-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        private static FirebaseClient client;

        static FireBaseService()
        {
            client = new FirebaseClient(config);
        }

        public static async void PushToDataBase<T>(T data, string dataType, string path = null)
        {
            if (path == null) path = "Tasks";
            try
            {
                await client.PushAsync(dataType + "/" + path, data);
            }
            catch (System.Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public static async Task<List<T>> GetDataFromDataBase<T>(string path)
        {
            List<T> Data = new();
            
            try
            {
               FirebaseResponse response = await client.GetAsync(path);

               Dictionary<string, T> messages = JsonConvert.DeserializeObject<Dictionary<string, T>>(response.Body.ToString());
               Data = messages.Select(x => x.Value).ToList();
               return Data;
            }
            catch (System.Exception ex)
            {
               Debug.WriteLine(ex.Message + " ERROR!");
               return null;
            }
        }

        //public static List<MessageModel> GetMessages()
        //{
        //    return Usermessages;
        //}
    }
}
