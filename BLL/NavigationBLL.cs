using SalesSystem.DTOs.User;
using SalesSystem.UI;
using SalesSystem.UI.Seller;
using System;
using System.Windows.Forms;

namespace SalesSystem.BLL
{
    public class NavigationBLL
    {
        public Form ShowFormByRole(UserLoginResponseDTO user)
        {
            switch (user.Role)
            {
                case "Administrator":
                    return new AdminForm();

                case "Seller":
                    return new SellerForm(user.Role);

                default:
                    throw new Exception("Rol no conocido");
            }
        }
    }
}
