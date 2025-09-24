using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.DTOs.User
{
    public class UserLoginDTO
    {
        public string Email {  get; set; }
        public string Password { get; set; }

        public UserLoginDTO(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
    }
}
