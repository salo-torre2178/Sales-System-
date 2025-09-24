using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.DTOs.Product
{
    public class DeleteProductDTO
    {
        public int ProductID { get; set; }

        public DeleteProductDTO(int productId)
        {
            ProductID = productId;
        }
    }

}
