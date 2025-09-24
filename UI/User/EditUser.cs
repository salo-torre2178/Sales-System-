using SalesSystem.BLL;
using SalesSystem.DTOs.User; 
using System;
using System.Windows.Forms;

namespace SalesSystem.UI.User
{
    public partial class EditUser : Form
    {
        private readonly UserBLL userBLL;
        private readonly int userId;
        private readonly UserForm userForm;

        public EditUser(int id, UserForm form)
        {
            InitializeComponent();
            userBLL = new UserBLL();
            userId = id;
            userForm = form;

            // Cargar datos del usuario
            var user = userBLL.GetById(userId); // Aquí sigues usando la entidad para leer
            if (user != null)
            {
                txtName.Text = user.FullName;
                txtidentity.Text = user.IdentityNumber.ToString();
                txtphone.Text = user.Phone;
                txtaddress.Text = user.Address;
                txtemail.Text = user.Email;
                txtpassword.Text = user.PasswordHash;
                cmbrole.SelectedItem = user.Role;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var updateDto = new UpdateUserDTO
                {
                    UserID = userId,
                    FullName = txtName.Text,
                    IdentityNumber = long.Parse(txtidentity.Text),
                    Phone = txtphone.Text,
                    Address = txtaddress.Text,
                    Email = txtemail.Text,
                    PasswordHash = txtpassword.Text,
                    Role = cmbrole.SelectedItem?.ToString()
                };

                userBLL.UpdateUser(updateDto);

                MessageBox.Show("✅ Usuario actualizado correctamente.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar usuario: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            userForm?.Show();
        }
    }
}
