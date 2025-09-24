using SalesSystem.BLL;
using SalesSystem.DAL;
using SalesSystem.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SalesSystem.UI.Sales
{
    public partial class Addsales : Form
    {
        private readonly SaleBLL saleBLL;
        private List<SaleDetail> details;
        private readonly ProductBLL productBLL;

        public Addsales()
        {
            InitializeComponent();
            saleBLL = new SaleBLL();
            details = new List<SaleDetail>();
            productBLL = new ProductBLL();
        }

        private void Addsales_Load(object sender, EventArgs e)
        {
            dgvDetails.Columns.Clear();
            dgvDetails.AutoGenerateColumns = false;
            dgvDetails.AllowUserToAddRows = false;

            dgvDetails.Columns.Add("ProductID", "ID Producto");
            dgvDetails.Columns.Add("ProductName", "Producto");
            dgvDetails.Columns.Add("Quantity", "Cantidad");
            dgvDetails.Columns.Add("UnitPrice", "Precio Unitario");
            dgvDetails.Columns.Add("SubTotal", "SubTotal");


            var products = productBLL.GetAllProducts();

            cmbProducts.DataSource = products;
            cmbProducts.DisplayMember = "ProductName";
            cmbProducts.ValueMember = "ProductID";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProducts.SelectedItem == null)
                {
                    MessageBox.Show("Seleccione un producto.");
                    return;
                }

                if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida.");
                    return;
                }

                Product selectedProduct = (Product)cmbProducts.SelectedItem;

                var detail = new SaleDetail
                {
                    ProductID = selectedProduct.ProductID,
                    Quantity = qty,
                    UnitPrice = selectedProduct.Price
                };

                details.Add(detail);

                dgvDetails.Rows.Add(
                    detail.ProductID,
                    selectedProduct.ProductName,
                    detail.Quantity,
                    detail.UnitPrice,
                    detail.Quantity * detail.UnitPrice
                );

                CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar detalle: " + ex.Message);
            }
        }


        private void CalcularTotal()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["Quantity"].Value != null && row.Cells["UnitPrice"].Value != null)
                {
                    int qty;
                    decimal price;

                    int.TryParse(row.Cells["Quantity"].Value.ToString(), out qty);
                    decimal.TryParse(row.Cells["UnitPrice"].Value.ToString(), out price);

                    decimal subtotal = qty * price;
                    row.Cells["SubTotal"].Value = subtotal;

                    total += subtotal;
                }
            }

            lblTotal.Text = total.ToString("N2");
        }


        private void btnSaveSale_Click(object sender, EventArgs e)
        {
            try
            {
                if (details == null || details.Count == 0)
                {
                    MessageBox.Show("Debes agregar al menos un detalle de venta.");
                    return;
                }

                // Crear la venta
                var sale = new Sale
                {
                    CustomerID = string.IsNullOrEmpty(txtCustomerID.Text) ? (int?)null : Convert.ToInt32(txtCustomerID.Text),
                    SellerID = Convert.ToInt32(txtSellerID.Text),
                    SaleDate = DateTime.Now
                    // El Total lo calcula la BLL automáticamente
                };

                // Guardar la venta con sus detalles
                int saleId = saleBLL.CreateSale(sale, details);

                MessageBox.Show($"✅ Venta registrada con ID: {saleId}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar todo después de guardar
                details.Clear();
                dgvDetails.Rows.Clear();
                lblTotal.Text = "0";
                txtCustomerID.Clear();
                txtSellerID.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la venta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SalesForm salesForm;

        public Addsales(SalesForm form)
        {
            InitializeComponent();
            saleBLL = new SaleBLL();
            details = new List<SaleDetail>();
            productBLL = new ProductBLL();
            salesForm = form;
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.Close();      // cierra Addsales
            salesForm.Show();  // vuelve al SalesForm
        }
    }
}
