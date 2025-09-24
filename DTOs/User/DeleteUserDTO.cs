using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.DTOs.User
{
    public class DeleteUserDTO
    {
        public int Id { get; set; }

        public DeleteUserDTO(int ID)
        {
            Id = ID;
        }
    }
}
