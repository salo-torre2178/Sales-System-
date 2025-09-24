using SalesSystem.BLL;
using SalesSystem.DTOs.User;
using SalesSystem.Entities;
using SalesSystem.UI;
using SalesSystem.UI.Seller; // 👈 importa el namespace donde está SellerForm
using System;
using System.Windows.Forms;

namespace SalesSystem
{
    public partial class Login : Form
    {
        private readonly UserBLL userBLL;
        private readonly NavigationBLL navigationBLL;

        public Login()
        {
            InitializeComponent();
            userBLL = new UserBLL();
            navigationBLL = new NavigationBLL();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            UserLoginDTO userLoginDTO = new UserLoginDTO(email,password);

            try
            {
                UserLoginResponseDTO user = userBLL.Login(userLoginDTO);

                MessageBox.Show(userBLL.GetWelcomeMessage(user));

                Form form = navigationBLL.ShowFormByRole(user);

                form.Show();
                this.Hide(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message);
            }
        }
    }
}
