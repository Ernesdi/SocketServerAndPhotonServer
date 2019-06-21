using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    class User
    {
        public int Id;
        public string Username;
        public string Password;

        public User(int Id, string Username, string Password)
        {
            this.Id = Id;
            this.Username = Username;
            this.Password = Password;
        }


    }
}
