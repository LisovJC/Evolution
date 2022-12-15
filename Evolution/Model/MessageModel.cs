using Evolution.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Model
{
    public class MessageModel : ObservableObject
    {
		private string _message;

		public string Message
		{
			get => _message;
			set => Set(ref _message, value);
		}

        private string _sender;

        public string Sender
        {
            get => _sender;
            set => Set(ref _sender, value);
        }

    }
}
