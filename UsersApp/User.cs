using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace UsersApp
{
    public class User
    {
        public int id { get; set; }
        private string Login, Password, Email;

        public string login
        {
            get { return Login; }
            set { Login = value; }
        }

        public string password
        {
            get { return Password; }
            set { Password = value; }
        }
        public string email
        {
            get { return Email; }
            set { Email = value; }
        }

        public User() { }
        public User(string login, string password, string email)
        {
            Login = login;
            Password = password;
            Email = email;
        }
    }
}
