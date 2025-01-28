using System.Data.SqlClient;

namespace ProductManagementSystemDashboard.Models
{
    public class DAL
    {
        private readonly string _connectionString;
        public DAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //Method to create a new product
        public int InsertProduct(Products product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using(SqlCommand command = new SqlCommand("sp_InsertProduct", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Address", product.Address);
                    command.Parameters.AddWithValue("@Country", product.Country);
                    command.Parameters.AddWithValue("@State", product.State);
                    command.Parameters.AddWithValue("@City", product.City);
                    command.Parameters.AddWithValue("@ProductionDocument", product.ProductionDocument);
                    command.Parameters.AddWithValue("@CreatedOn", product.CreatedOn);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result != null ? (int)result : 0;
                }
            }
        }

        //Method to get a product by id
        public Products GetProducts(int id)
        {
            Products products = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetProductById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            products = new Products
                            {
                                Id = (int)reader["Id"],
                                ProductName = reader["ProductName"].ToString(),
                                Address = reader["Address"].ToString(),
                                Country = reader["Country"].ToString(),
                                State = (decimal)reader["State"],
                                City = reader["City"].ToString(),
                                ProductionDocument = reader["ProductionDocument"].ToString(),
                                CreatedOn = reader["CreatedOn"].ToString()
                            };
                        }
                    }
                }
            }
            return products;
        }

        //Method to get all products
        public List<Products> GetProducts()
        {
            List<Products> ListProducts = new List<Products>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("getAllProduct", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           Products products = new Products
                            {
                                Id = (int)reader["Id"],
                                ProductName = reader["ProductName"].ToString(),
                                Address = reader["Address"].ToString(),
                                Country = reader["Country"].ToString(),
                                State = (decimal)reader["State"],
                                City = reader["City"].ToString(),
                                ProductionDocument = reader["ProductionDocument"].ToString(),
                                CreatedOn = reader["CreatedOn"].ToString()
                            };
                            ListProducts.Add(products);
                        }
                    }
                }
            }
            return ListProducts;
        }

        //Method to update a product
        public bool UpdateProduct(Products product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UpdateProduct", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", product.Id);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Address", product.Address);
                    command.Parameters.AddWithValue("@Country", product.Country);
                    command.Parameters.AddWithValue("@State", product.State);
                    command.Parameters.AddWithValue("@City", product.City);
                    command.Parameters.AddWithValue("@ProductionDocument", product.ProductionDocument);
                    command.Parameters.AddWithValue("@CreatedOn", product.CreatedOn);
                    connection.Open();
                    int i = command.ExecuteNonQuery();
                    return i >= 1;
                }
            }
        }
    }
}
