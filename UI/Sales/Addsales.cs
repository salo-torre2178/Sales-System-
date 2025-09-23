using SalesSystem.BLL;
using SalesSystem.Entities;
using System;
using System.Windows.Forms;

namespace SalesSystem.UI.Sales
{
    public partial class Addsales : Form
    {
        private readonly SaleBLL saleBLL;

        public Addsales()
        {
            InitializeComponent();
            saleBLL = new SaleBLL();
        }

        private void Addsales_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }




        // 🔹 Botón Atrás
        //private void btnBack_Click(object sender, EventArgs e)
        //{
        //this.Close(); // al cerrarse, vuelve a SalesForm
        //}
    }
}
