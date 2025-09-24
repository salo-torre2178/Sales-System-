using SalesSystem.BLL;
using SalesSystem.DTOs.User;
using SalesSystem.Entities;
using SalesSystem.UI.Products;
using SalesSystem.UI.Seller;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SalesSystem.UI
{
    public partial class ProductForm : Form
    {
        private readonly ProductBLL productBLL;
        private AdminForm _adminForm;
        private string _role;

        public ProductForm()
        {
            InitializeComponent();
            productBLL = new ProductBLL();
            LoadProducts();
        }

        // Constructor con rol
        public ProductForm(string role)
        {
            InitializeComponent();
            productBLL = new ProductBLL();
            LoadProducts();
            _role = role;

            // 👇 si el rol es vendedor, ocultamos botones e íconos
            if (_role == "Seller" || _role == "Vendedor")
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;

                // oculta los íconos asociados
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                pictureBox5.Visible = false;
            }
        }

        public ProductForm(AdminForm adminForm)
        {
            InitializeComponent();
            productBLL = new ProductBLL();
            LoadProducts();
            _adminForm = adminForm;
            _role = "Admin";
        }

        private void LoadProducts()
        {
            List<Product> products = productBLL.GetAllProducts();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = products;

            dataGridView1.Columns["ProductID"].HeaderText = "ID";
            dataGridView1.Columns["ProductName"].HeaderText = "Name";
            dataGridView1.Columns["Description"].HeaderText = "Description";
            dataGridView1.Columns["Price"].HeaderText = "Price";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddProduct add = new AddProduct();
            add.FormClosed += (s, args) => LoadProducts();
            add.ShowDialog();
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int productID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ProductID"].Value);
                EditProduct editProduct = new EditProduct(productID);
                editProduct.Show();
                this.Close();
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("⚠️ Debes seleccionar un producto para eliminar.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int productID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ProductID"].Value);

                // DTO para encapsular la eliminación
                DeleteUserDTO user = new DeleteUserDTO(productID);

                DialogResult result = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar este registro?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    productBLL.DeleteProduct(user);

                    MessageBox.Show("✅ Producto eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refrescar formulario
                    this.Close();
                    ProductForm productForm = new ProductForm(_role);
                    productForm.Show();
                }
            }
            catch (Exception ex)
            {
                // El mensaje viene ya tratado desde el BLL
                MessageBox.Show(ex.Message, "Error al eliminar",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnBack_Click_1(object sender, EventArgs e)
        {
            if (_role == "Seller" || _role == "Vendedor")
            {
                SellerForm seller = new SellerForm(_role); // 👈 aquí pasamos el rol
                seller.Show();
                this.Close();
            }
            else
            {
                AdminForm admin = new AdminForm();
                admin.Show();
                this.Close();
            }
        }

    }
}
