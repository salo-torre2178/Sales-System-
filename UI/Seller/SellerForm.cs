using SalesSystem.UI.Sales;
using System;
using System.Windows.Forms;

namespace SalesSystem.UI.Seller
{
    public partial class SellerForm : Form
    {
        private string _role;

        public SellerForm(string role) // 👈 recibe rol
        {
            InitializeComponent();
            _role = role;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Abrimos SalesForm con el rol actual
            SalesForm salesForm = new SalesForm(_role);
            salesForm.Show();
            this.Hide();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            ProductForm productForm = new ProductForm(_role); // 👈 pasamos rol real
            productForm.Show();
            this.Hide();
        }
    }
}
