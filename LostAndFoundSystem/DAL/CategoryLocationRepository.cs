using System;
using System.Collections.Generic;
using System.Data.SQLite;
using LostAndFoundSystem.Models;

namespace LostAndFoundSystem.DAL
{
    /// <summary>
    /// Repository for category data access
    /// </summary>
    public class CategoryRepository
    {
        /// <summary>
        /// Get all categories
        /// </summary>
        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Categories ORDER BY CategoryName";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryID = Convert.ToInt32(reader["CategoryID"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            Description = reader["Description"]?.ToString() ?? ""
                        });
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// Add a new category
        /// </summary>
        public int AddCategory(Category category)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = @"
                    INSERT INTO Categories (CategoryName, Description)
                    VALUES (@CategoryName, @Description);
                    SELECT last_insert_rowid();";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@Description", category.Description ?? "");

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Update a category
        /// </summary>
        public void UpdateCategory(Category category)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "UPDATE Categories SET CategoryName = @CategoryName, Description = @Description WHERE CategoryID = @CategoryID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@Description", category.Description ?? "");
                    command.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        public void DeleteCategory(int categoryId)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "DELETE FROM Categories WHERE CategoryID = @CategoryID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryID", categoryId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// Repository for location data access
    /// </summary>
    public class LocationRepository
    {
        /// <summary>
        /// Get all locations
        /// </summary>
        public List<Location> GetAllLocations()
        {
            var locations = new List<Location>();

            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Locations ORDER BY LocationName";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        locations.Add(new Location
                        {
                            LocationID = Convert.ToInt32(reader["LocationID"]),
                            LocationName = reader["LocationName"].ToString(),
                            Description = reader["Description"]?.ToString() ?? ""
                        });
                    }
                }
            }

            return locations;
        }

        /// <summary>
        /// Add a new location
        /// </summary>
        public int AddLocation(Location location)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = @"
                    INSERT INTO Locations (LocationName, Description)
                    VALUES (@LocationName, @Description);
                    SELECT last_insert_rowid();";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocationName", location.LocationName);
                    command.Parameters.AddWithValue("@Description", location.Description ?? "");

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Update a location
        /// </summary>
        public void UpdateLocation(Location location)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "UPDATE Locations SET LocationName = @LocationName, Description = @Description WHERE LocationID = @LocationID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocationName", location.LocationName);
                    command.Parameters.AddWithValue("@Description", location.Description ?? "");
                    command.Parameters.AddWithValue("@LocationID", location.LocationID);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Delete a location
        /// </summary>
        public void DeleteLocation(int locationId)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "DELETE FROM Locations WHERE LocationID = @LocationID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocationID", locationId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
