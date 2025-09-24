using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using SalesSystem.Entities;

namespace SalesSystem.DAL
{
    public class SaleDAL
    {
        private readonly string connectionString;

        public SaleDAL()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["SalesSystemDB"]
                .ConnectionString;
        }

        // Crear nueva venta
        public int AddSale(Sale sale, List<SaleDetail> saleDetails)
        {
            if (sale == null) throw new ArgumentNullException(nameof(sale));
            if (saleDetails == null || saleDetails.Count == 0) throw new Exception("La venta debe tener al menos un detalle");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Insertar venta y recuperar SaleID
                    string saleQuery = @"INSERT INTO Sale (SaleDate, CustomerID, SellerID, Total)
                                         OUTPUT INSERTED.SaleID
                                         VALUES (@SaleDate, @CustomerID, @SellerID, @Total)";

                    SqlCommand cmdSale = new SqlCommand(saleQuery, conn, transaction);
                    cmdSale.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
                    cmdSale.Parameters.AddWithValue("@CustomerID", (object)sale.CustomerID ?? DBNull.Value);
                    cmdSale.Parameters.AddWithValue("@SellerID", sale.SellerID);
                    cmdSale.Parameters.AddWithValue("@Total", sale.Total);

                    int saleId = (int)cmdSale.ExecuteScalar();

                    // 2. Insertar detalles de la venta
                    string detailQuery = @"INSERT INTO SaleDetail (SaleID, ProductID, Quantity, UnitPrice)
                                           VALUES (@SaleID, @ProductID, @Quantity, @UnitPrice)";

                    foreach (var detail in saleDetails)
                    {
                        SqlCommand cmdDetail = new SqlCommand(detailQuery, conn, transaction);
                        cmdDetail.Parameters.AddWithValue("@SaleID", saleId);
                        cmdDetail.Parameters.AddWithValue("@ProductID", detail.ProductID);
                        cmdDetail.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        cmdDetail.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);

                        cmdDetail.ExecuteNonQuery();
                    }

                    // 3. Confirmar transacción
                    transaction.Commit();

                    return saleId;
                }
                catch
                {
                    // Si algo falla, revertir
                    transaction.Rollback();
                    throw;
                }
            }
        }

// Obtener todas las ventas
        public List<Sale> GetAllSales()
        {
            List<Sale> sales = new List<Sale>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SaleID, SaleDate, CustomerID, SellerID, Total FROM Sale";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sales.Add(new Sale
                    {
                        SaleID = (int)reader["SaleID"],
                        SaleDate = (DateTime)reader["SaleDate"],
                        CustomerID = reader["CustomerID"] as int?,
                        SellerID = (int)reader["SellerID"],
                        Total = (decimal)reader["Total"]
                    });
                }
            }

            return sales;
        }

        // Obtener venta por ID
        public Sale GetSaleById(int id)
        {
            Sale sale = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SaleID, SaleDate, CustomerID, SellerID, Total FROM Sale WHERE SaleID = @SaleID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SaleID", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    sale = new Sale
                    {
                        SaleID = (int)reader["SaleID"],
                        SaleDate = (DateTime)reader["SaleDate"],
                        CustomerID = reader["CustomerID"] as int?,
                        SellerID = (int)reader["SellerID"],
                        Total = (decimal)reader["Total"]
                    };
                }
            }

            return sale;
        }

        // Actualizar venta
        public void UpdateSale(Sale sale)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Sale 
                                 SET SaleDate=@SaleDate, CustomerID=@CustomerID, SellerID=@SellerID, Total=@Total
                                 WHERE SaleID=@SaleID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SaleID", sale.SaleID);
                cmd.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
                cmd.Parameters.AddWithValue("@CustomerID", (object)sale.CustomerID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SellerID", sale.SellerID);
                cmd.Parameters.AddWithValue("@Total", sale.Total);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Eliminar venta
        public int Delete(int saleId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Primero eliminamos los detalles de la venta
                string queryDetails = "DELETE FROM SaleDetail WHERE SaleID = @SaleID";
                SqlCommand cmdDetails = new SqlCommand(queryDetails, conn);
                cmdDetails.Parameters.AddWithValue("@SaleID", saleId);
                cmdDetails.ExecuteNonQuery();

                // Ahora eliminamos la venta
                string querySale = "DELETE FROM Sale WHERE SaleID = @SaleID";
                SqlCommand cmdSale = new SqlCommand(querySale, conn);
                cmdSale.Parameters.AddWithValue("@SaleID", saleId);

                return cmdSale.ExecuteNonQuery();
            }
        }
    }
}
