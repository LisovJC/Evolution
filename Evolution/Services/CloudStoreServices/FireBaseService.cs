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
        //public static List<MessageModel> Usermessages;

        static FireBaseService()
        {
            client = new FirebaseClient(config);
        }

        public static async void PushToDataBase<T>(T data, string user, string dataType)
        {
            if (user == null) user = "Tasks";
            try
            {
                await client.PushAsync(dataType + "/" + user, data);
            }
            catch (System.Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public static async void GetMessagesFromDataBase()
        {
            while (true)
            {
                await Task.Delay(100);
                try
                {
                    FirebaseResponse response = await client.GetAsync("MessagesData/Lisov");

                    //Dictionary<string, MessageModel> messages = JsonConvert.DeserializeObject<Dictionary<string, MessageModel>>(response.Body.ToString());
                    //Usermessages = messages.Select(x => x.Value).ToList();
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message + " ERROR!");
                }
            }
        }

        //public static List<MessageModel> GetMessages()
        //{
        //    return Usermessages;
        //}
    }
}
