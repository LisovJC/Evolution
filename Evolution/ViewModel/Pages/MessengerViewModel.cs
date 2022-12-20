using Evolution.Command;
using Evolution.Core;
using Evolution.Model;
using Evolution.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Evolution.ViewModel.Pages
{
    internal class MessengerViewModel : ObservableObject
    {
        //public static ObservableCollectionEX<MessageModel> MessageList { get; set; }
        //public List<MessageModel> test { get; set; }


        //public RelayCommand SendCommand { get; set; }
        //public RelayCommand UpdateCommand { get; set; }

        //private string _message;

        //public string Message
        //{
        //    get => _message;
        //    set => Set(ref _message, value);
        //}

        //public MessengerViewModel()
        //{
        //    MessageList = new();
        //    GetMsgs();


        //    SendCommand = new(o => 
        //    {                                                       
        //        try
        //        {
        //            MessageModel msg = new MessageModel { Message = Message, Sender = UserService.GetUserLogin() + ":" };                    
        //        }
        //        catch (Exception ex)
        //        {

        //            Debug.WriteLine(ex.Message);
        //        }
        //        Message = String.Empty;
        //    });
           
        //} 
        
        //public async void GetMsgs()
        //{
        //    while(true)
        //    {
        //        await Task.Delay(500);
        //        test = FirebaseService.GetMessages();
        //        if(test != null)
        //        {
        //            if (test.Count != MessageList.Count)
        //            {
        //                MessageList.Clear();
        //                foreach (var item in test)
        //                {
        //                    MessageList.Add(item);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
