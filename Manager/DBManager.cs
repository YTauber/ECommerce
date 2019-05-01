using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Manager
{
    public class DBManager
    {
        private string _connectionString;

        public DBManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Products> GetProductsByCategoryId(int categoryId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT  * FROM Products WHERE CategoryId = @id";
            cmd.Parameters.AddWithValue("@id", categoryId);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Products> products = new List<Products>();
            while (reader.Read())
            {
                products.Add(new Products
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["Name"],
                    PictureName = (string)reader["PictureName"],
                    Description = (string)reader["Description"],
                    Price = (decimal)reader["Price"],
                    CategotyId = (int)reader["CategoryId"]
                });
            }

            connection.Close();
            connection.Dispose();
            return products;
        }

        public IEnumerable<Products> GetAllProducts()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT  * FROM Products";
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Products> products = new List<Products>();
            while (reader.Read())
            {
                products.Add(new Products
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["Name"],
                    PictureName = (string)reader["PictureName"],
                    Description = (string)reader["Description"],
                    Price = (decimal)reader["Price"],
                    CategotyId = (int)reader["CategoryId"]
                });
            }

            connection.Close();
            connection.Dispose();
            return products;
        }

        public Products GetProductById(int productId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT TOP 1 * FROM Products WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", productId);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            
            if (!reader.Read())
            {
                return null;
            }

            Products product = new Products
            {
                Id = (int)reader["id"],
                Name = (string)reader["Name"],
                PictureName = (string)reader["PictureName"],
                Description = (string)reader["Description"],
                Price = (decimal)reader["Price"],
                CategotyId = (int)reader["CategoryId"]
            };

            connection.Close();
            connection.Dispose();
            return product;
        }

        public IEnumerable<Categoties> GetCategories()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT  * FROM Categories";
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Categoties> categoties = new List<Categoties>();
            while (reader.Read())
            {
                categoties.Add(new Categoties
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["Name"]
                });
            }

            connection.Close();
            connection.Dispose();
            return categoties;
        }

        public IEnumerable<CartItems> GetProductsByCartId(int cartId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Products p JOIN CartItems ci 
                                                        ON p.id = ci.ItemId
		                                                WHERE ci.CartId = @id";
            cmd.Parameters.AddWithValue("@id", cartId);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<CartItems> cartItems = new List<CartItems>();
            while (reader.Read())
            {
                cartItems.Add(new CartItems
                {
                    ItemId = (int)reader["ItemId"],
                    CartId = (int)reader["CartId"],
                    Quantity = (int)reader["Quantity"],
                    Name = (string)reader["Name"],
                    PictureName = (string)reader["PictureName"],
                    Description = (string)reader["Description"],
                    Price = (decimal)reader["Price"],
                    CategotyId = (int)reader["CategoryId"]
                });
            }

            connection.Close();
            connection.Dispose();
            return cartItems;
        }

        public void InsertCategory(Categoties Category)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Categories VALUES (@name) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@name", Category.Name);
            connection.Open();
            Category.Id = (int)(decimal)cmd.ExecuteScalar();
            connection.Close();
            connection.Dispose();
        }

        public void UpdateCategory(Categoties Category)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"Update Categories SET Name = @name WHERE id = @id";
            cmd.Parameters.AddWithValue("@name", Category.Name);
            cmd.Parameters.AddWithValue("@id", Category.Id);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }

        public void InsertProduct(Products products)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Products VALUES (@name, @pictureName, @description, @price, @categoryid) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@name", products.Name);
            cmd.Parameters.AddWithValue("@pictureName", products.PictureName);
            cmd.Parameters.AddWithValue("@description", products.Description);
            cmd.Parameters.AddWithValue("@price", products.Price);
            cmd.Parameters.AddWithValue("@categoryid", products.CategotyId);
            connection.Open();
            products.Id = (int)(decimal)cmd.ExecuteScalar();
            connection.Close();
            connection.Dispose();
        }

        public void InsertCartItem(CartItems caeritems)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO CartItems VALUES (@itemId, @cartId, @quantity)";
            cmd.Parameters.AddWithValue("@itemId", caeritems.ItemId);
            cmd.Parameters.AddWithValue("@cartId", caeritems.CartId);
            cmd.Parameters.AddWithValue("@quantity", caeritems.Quantity);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }

        public void InsertCart(Carts cart)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Carts VALUES (@date) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@date", cart.Date);
            connection.Open();
            cart.Id = (int)(decimal)cmd.ExecuteScalar();
            connection.Close();
            connection.Dispose();
        }

        public void InsertUser(string HashPassword)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Admins VALUES (@HashPassword)";
            cmd.Parameters.AddWithValue("@HashPassword", HashPassword);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }

        public bool CheckPassword(string hashPassword)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT TOP 1 * FROM Admins";
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return false;
            }
            string HashPasword = (string)reader["HashPassword"];
            connection.Close();
            connection.Dispose();

            return BCrypt.Net.BCrypt.Verify(hashPassword, HashPasword);
        }


    }
}
