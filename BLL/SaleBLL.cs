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

        public int CreateSale(Sale sale, List<SaleDetail> saleDetails)
        {
            // Validaciones de negocio
            if (sale == null) throw new ArgumentNullException(nameof(sale));
            if (saleDetails == null || saleDetails.Count == 0)
                throw new Exception("La venta debe tener al menos un detalle");

            // Calcular el total de la venta automáticamente
            decimal total = 0;
            foreach (var d in saleDetails)
            {
                if (d.Quantity <= 0) throw new Exception("Cantidad inválida en detalle de venta");
                if (d.UnitPrice <= 0) throw new Exception("Precio inválido en detalle de venta");

                total += d.Quantity * d.UnitPrice;
            }

            sale.Total = total;
            sale.SaleDate = DateTime.Now;

            // Llamada a DAL
            return saleDAL.AddSale(sale, saleDetails);
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
            int rows = saleDAL.Delete(saleId);  

            if (rows == 0)
            {
                throw new Exception("No se pudo eliminar la venta. Verifica que exista en la base de datos.");
            }
        }



    }
}
