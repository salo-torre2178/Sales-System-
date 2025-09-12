﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public long IdentityNumber { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Role { get; set; } 
    }

}
