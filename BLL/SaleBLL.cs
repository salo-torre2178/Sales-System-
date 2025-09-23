using SalesSystem.DAL;
using SalesSystem.Entities;
using System;
using System.Collections.Generic;

namespace SalesSystem.BLL
{
    public class SaleBLL
    {
        private readonly SaleDAL saleDAL;

        public SaleBLL()
        {
            saleDAL = new SaleDAL();
        }

        public string AddSale(Sale sale)
        {
            if (sale == null)
                return "La venta no puede ser nula";

            if (sale.Total <= 0)
                return "El total debe ser mayor que 0";

            try
            {
                saleDAL.AddSale(sale);
                return "Venta registrada correctamente ✅";
            }
            catch (Exception ex)
            {
                return $"Error al registrar la venta: {ex.Message}";
            }
        }

        public List<Sale> GetAllSales()
        {
            return saleDAL.GetAllSales();
        }

        public Sale GetSaleById(int id)
        {
            return saleDAL.GetSaleById(id);
        }

        public void UpdateSale(Sale sale)
        {
            saleDAL.UpdateSale(sale);
        }

        public void DeleteSale(int saleId)
        {
            int rows = saleDAL.Delete(saleId);  // 🔹 aquí definimos 'rows'

            if (rows == 0)
            {
                throw new Exception("No se pudo eliminar la venta. Verifica que exista en la base de datos.");
            }
        }

    }
}
