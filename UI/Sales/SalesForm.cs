using SalesSystem.BLL;
using SalesSystem.Entities;
using SalesSystem.UI.Seller;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SalesSystem.UI.Sales
{
    public partial class SalesForm : Form
    {
        private readonly SaleBLL saleBLL;
        private readonly string userRole;

        public SalesForm(string role)
        {
            InitializeComponent();
            saleBLL = new SaleBLL();
            userRole = role;
        }

        private void SalesForm_Load(object sender, EventArgs e)
        {
            LoadSales();

            if (userRole == "Administrator")
            {
                // 🔹 Solo eliminar visible
                btnAdd.Visible = false;
                pictureBox3.Visible = false;

                btnEdit.Visible = false;
                pictureBox4.Visible = false;

                btnDelete.Visible = true;
                pictureBox5.Visible = true;

                // 🔹 Centrar botón eliminar con su icono
                int totalWidth = pictureBox5.Width + 5 + btnDelete.Width;
                int startX = (this.ClientSize.Width - totalWidth) / 2;

                pictureBox5.Left = startX;
                btnDelete.Left = pictureBox5.Right + 5;
            }
            else if (userRole == "Seller")
            {
                // 🔹 Mostrar agregar y editar
                btnAdd.Visible = true;
                pictureBox3.Visible = true;

                btnEdit.Visible = true;
                pictureBox4.Visible = true;

                btnDelete.Visible = false;
                pictureBox5.Visible = false;

                // 🔹 Centrar agregar y editar
                int totalWidth = pictureBox3.Width + btnAdd.Width + 20 + pictureBox4.Width + btnEdit.Width;
                int startX = (this.ClientSize.Width - totalWidth) / 2;

                pictureBox3.Left = startX;
                btnAdd.Left = pictureBox3.Right + 5;

                pictureBox4.Left = btnAdd.Right + 20;
                btnEdit.Left = pictureBox4.Right + 5;
            }
            else
            {
                // Ningún permiso
                btnAdd.Visible = false;
                pictureBox3.Visible = false;
                btnEdit.Visible = false;
                pictureBox4.Visible = false;
                btnDelete.Visible = false;
                pictureBox5.Visible = false;
            }
        }



        private void LoadSales()
        {
            var sales = saleBLL.GetAllSales();
            dataGridView1.DataSource = sales;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int saleId = (int)dataGridView1.CurrentRow.Cells["SaleID"].Value;

                var confirm = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar este registro?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    saleBLL.DeleteSale(saleId);
                    MessageBox.Show("Venta eliminada correctamente ✅");
                    LoadSales();
                }
            }
            else
            {
                MessageBox.Show("Seleccione una venta para eliminar.");
            }
        }

        // 🔹 Abrir AddSales
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Addsales addForm = new Addsales();
            addForm.FormClosed += (s, args) => LoadSales(); // recarga al cerrar
            addForm.ShowDialog();
        }

        // 🔹 Abrir EditSales
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int saleId = (int)dataGridView1.CurrentRow.Cells["SaleID"].Value;

                EditSales editForm = new EditSales(saleId); // 🔹 aquí sí le pasamos el ID
                // si después necesitas pasar el ID, puedes modificar el constructor de EditSales
                editForm.FormClosed += (s, args) => LoadSales();
                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione una venta para editar.");
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            Addsales addForm = new Addsales();
            addForm.FormClosed += (s, args) => LoadSales(); // recarga al cerrar
            addForm.ShowDialog();
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int saleId = (int)dataGridView1.CurrentRow.Cells["SaleID"].Value;

                EditSales editForm = new EditSales(saleId);  // 👈 ahora sí pasa el ID
                editForm.FormClosed += (s, args) => LoadSales();
                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione una venta para editar.");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (userRole == "Seller")
            {
                SellerForm sellerForm = new SellerForm(userRole);
                sellerForm.Show();
            }
            else if (userRole == "Administrator")
            {
                AdminForm adminForm = new AdminForm();
                adminForm.Show();
            }

            this.Close(); // cerrar SalesForm
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int saleId = (int)dataGridView1.CurrentRow.Cells["SaleID"].Value;

                var confirm = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar esta venta?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    saleBLL.DeleteSale(saleId);
                    MessageBox.Show("Venta eliminada correctamente ✅", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSales(); // refresca el DataGridView
                }
                else
                {
                    MessageBox.Show("La venta no fue eliminada.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Seleccione una venta para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
