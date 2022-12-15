using Evolution.Core;

namespace Evolution.Model
{
    public class UserModel : ObservableObject
    {
		private string _login;

		public string Login
		{
			get => _login;
			set => Set(ref _login, value);
		}

        private string _password;

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        private Acces _accessRights;

        public Acces AccessRights
        {
            get => _accessRights;
            set => Set(ref _accessRights, value);
        }

        public enum Acces
        {
            asRegularUser,
            asAdministrator
        }


    }
}
