using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Warehouse
{
    class User
    {
        public int id { get; set; }

        private string login, email, pass;

        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Password
        {
            get { return pass; }
            set { pass = value; }
        }

        public User() { }

        public static void RegisterUser(string login, string email, string pass)
        {
            ApplicationContext db = new ApplicationContext();
            User user = new User(login, email, pass);
            db.Users.Add(user);
            db.SaveChanges();
        }
        public static void WriteUserData(User authUser)
        {
            File.WriteAllText("user.txt", "");
            File.AppendAllText("user.txt", authUser.Login);
            File.AppendAllText("user.txt", "\n" + authUser.Password);
            File.AppendAllText("user.txt", "\n" + authUser.Email);
        }
        public User(string login, string email, string pass)
        {
            this.login = login;
            this.email = email;
            this.pass = pass;
        }
        /*
        public override string ToString()
        {
            return "Пользователь"+Login+" ,Email "+Email;
        }
        */
    }
}