using System;
using System.Windows.Forms;

namespace SalesSystem.UI.Sales
{
    public partial class EditSales : Form
    {
        private int _saleId;

        public EditSales(int saleId)   
        {
            InitializeComponent();
            _saleId = saleId;
        }



    private void EditSales_Load(object sender, EventArgs e)
        {
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {

        }
    }
}
