using FireSharp.Config;
using FireSharp;
using System.Diagnostics;
using FireSharp.Response;
using Evolution.Model;
using System.Collections.Generic;
using FireSharp.Extensions;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Evolution.Services
{
    public static class FirebaseService
    {
        private static FirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "7ObrgEm3lNF073bppXmCYsboUhdVSBqonvvLqIyp",
            BasePath = "https://evolution-5d406-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        private static FirebaseClient client;
        public static List<MessageModel> Usermessages;

        static FirebaseService()
        {
            client = new FirebaseClient(config);
        }

        public static async void PushMessageToDataBase<T>(T data)
        {
            try
            {
                await client.PushAsync("MessagesData/Lisov", data);
            }
            catch (System.Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public static async void GetMessagesFromDataBase()
        {
            while(true)
            {
                await Task.Delay(100);
                try
                {                    
                    FirebaseResponse response = await client.GetAsync("MessagesData/Lisov");
                    
                    Dictionary<string, MessageModel> messages = JsonConvert.DeserializeObject<Dictionary<string, MessageModel>>(response.Body.ToString());
                    Usermessages = messages.Select(x => x.Value).ToList();                   
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message + " ERROR!");
                }
            }            
        }

        public static List<MessageModel> GetMessages()
        {
            return Usermessages;
        }
    }
}
