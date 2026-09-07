using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;

namespace POS_system
{
    public partial class SP_StockDataContext
    {
        public List<Stock> sp_Search(string searchTerm)
        {
            List<Stock> results = new List<Stock>();

            using (SqlConnection conn = new SqlConnection(this.Connection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_Search", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Stock s = new Stock
                        {
                            StockID = Convert.ToInt32(reader["StockID"]),
                            ProductName = reader["ProductName"].ToString(),
                            Category = reader["Category"].ToString(),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            Material = reader["Material"] == DBNull.Value ? null : reader["Material"].ToString(),
                            DateAdded = Convert.ToDateTime(reader["DateAdded"])
                        };
                        results.Add(s);
                    }
                }
            }

            return results;
        }
    }
}