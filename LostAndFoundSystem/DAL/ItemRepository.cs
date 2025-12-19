using System;
using System.Collections.Generic;
using System.Data.SQLite;
using LostAndFoundSystem.Models;

namespace LostAndFoundSystem.DAL
{
    /// <summary>
    /// Repository for item data access
    /// </summary>
    public class ItemRepository
    {
        /// <summary>
        /// Add a new item to the database
        /// </summary>
        public int AddItem(Item item)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = @"
                    INSERT INTO Items (Title, CategoryID, Description, FoundDate, FoundLocationID, Status, FinderNotes, LoggedByUserID, CreatedDate)
                    VALUES (@Title, @CategoryID, @Description, @FoundDate, @FoundLocationID, @Status, @FinderNotes, @LoggedByUserID, @CreatedDate);
                    SELECT last_insert_rowid();";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", item.Title);
                    command.Parameters.AddWithValue("@CategoryID", item.CategoryID);
                    command.Parameters.AddWithValue("@Description", item.Description ?? string.Empty);
                    command.Parameters.AddWithValue("@FoundDate", item.FoundDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@FoundLocationID", item.FoundLocationID);
                    command.Parameters.AddWithValue("@Status", item.Status);
                    command.Parameters.AddWithValue("@FinderNotes", item.FinderNotes ?? string.Empty);
                    command.Parameters.AddWithValue("@LoggedByUserID", item.LoggedByUserID);
                    command.Parameters.AddWithValue("@CreatedDate", item.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Get all items with category and location names
        /// </summary>
        public List<Item> GetAllItems()
        {
            var items = new List<Item>();

            using (var connection = DatabaseManager.GetConnection())
            {
                string query = @"
                    SELECT i.*, c.CategoryName, l.LocationName 
                    FROM Items i
                    LEFT JOIN Categories c ON i.CategoryID = c.CategoryID
                    LEFT JOIN Locations l ON i.FoundLocationID = l.LocationID
                    ORDER BY i.FoundDate DESC";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(MapReaderToItem(reader));
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Search items with filters
        /// </summary>
        public List<Item> SearchItems(string keyword, int? categoryId, int? locationId, string status)
        {
            var items = new List<Item>();

            using (var connection = DatabaseManager.GetConnection())
            {
                string query = @"
                    SELECT i.*, c.CategoryName, l.LocationName 
                    FROM Items i
                    LEFT JOIN Categories c ON i.CategoryID = c.CategoryID
                    LEFT JOIN Locations l ON i.FoundLocationID = l.LocationID
                    WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query += " AND (i.Title LIKE @Keyword OR i.Description LIKE @Keyword)";
                }

                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    query += " AND i.CategoryID = @CategoryID";
                }

                if (locationId.HasValue && locationId.Value > 0)
                {
                    query += " AND i.FoundLocationID = @LocationID";
                }

                if (!string.IsNullOrWhiteSpace(status))
                {
                    query += " AND i.Status = @Status";
                }

                query += " ORDER BY i.FoundDate DESC";

                using (var command = new SQLiteCommand(query, connection))
                {
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                    }
                    if (categoryId.HasValue)
                    {
                        command.Parameters.AddWithValue("@CategoryID", categoryId.Value);
                    }
                    if (locationId.HasValue)
                    {
                        command.Parameters.AddWithValue("@LocationID", locationId.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(status))
                    {
                        command.Parameters.AddWithValue("@Status", status);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(MapReaderToItem(reader));
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Get item by ID
        /// </summary>
        public Item GetItemById(int itemId)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = @"
                    SELECT i.*, c.CategoryName, l.LocationName 
                    FROM Items i
                    LEFT JOIN Categories c ON i.CategoryID = c.CategoryID
                    LEFT JOIN Locations l ON i.FoundLocationID = l.LocationID
                    WHERE i.ItemID = @ItemID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemID", itemId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToItem(reader);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Update item status
        /// </summary>
        public void UpdateItemStatus(int itemId, string status)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "UPDATE Items SET Status = @Status WHERE ItemID = @ItemID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@ItemID", itemId);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get items older than specified days
        /// </summary>
        public List<Item> GetItemsByAgeThreshold(int days)
        {
            var items = new List<Item>();
            DateTime thresholdDate = DateTime.Now.AddDays(-days);

            using (var connection = DatabaseManager.GetConnection())
            {
                string query = @"
                    SELECT i.*, c.CategoryName, l.LocationName 
                    FROM Items i
                    LEFT JOIN Categories c ON i.CategoryID = c.CategoryID
                    LEFT JOIN Locations l ON i.FoundLocationID = l.LocationID
                    WHERE i.Status = 'Active' AND i.FoundDate <= @ThresholdDate
                    ORDER BY i.FoundDate ASC";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ThresholdDate", thresholdDate.ToString("yyyy-MM-dd HH:mm:ss"));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(MapReaderToItem(reader));
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Get items in same category from last N days
        /// </summary>
        public List<Item> GetRecentItemsByCategory(int categoryId, int daysBack)
        {
            var items = new List<Item>();
            DateTime thresholdDate = DateTime.Now.AddDays(-daysBack);

            using (var connection = DatabaseManager.GetConnection())
            {
                string query = @"
                    SELECT i.*, c.CategoryName, l.LocationName 
                    FROM Items i
                    LEFT JOIN Categories c ON i.CategoryID = c.CategoryID
                    LEFT JOIN Locations l ON i.FoundLocationID = l.LocationID
                    WHERE i.CategoryID = @CategoryID AND i.FoundDate >= @ThresholdDate
                    ORDER BY i.FoundDate DESC";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryID", categoryId);
                    command.Parameters.AddWithValue("@ThresholdDate", thresholdDate.ToString("yyyy-MM-dd HH:mm:ss"));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(MapReaderToItem(reader));
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Map database reader to Item object
        /// </summary>
        private Item MapReaderToItem(SQLiteDataReader reader)
        {
            return new Item
            {
                ItemID = Convert.ToInt32(reader["ItemID"]),
                Title = reader["Title"].ToString(),
                CategoryID = Convert.ToInt32(reader["CategoryID"]),
                CategoryName = reader["CategoryName"]?.ToString() ?? "",
                Description = reader["Description"]?.ToString() ?? "",
                FoundDate = DateTime.Parse(reader["FoundDate"].ToString()),
                FoundLocationID = Convert.ToInt32(reader["FoundLocationID"]),
                LocationName = reader["LocationName"]?.ToString() ?? "",
                Status = reader["Status"].ToString(),
                FinderNotes = reader["FinderNotes"]?.ToString() ?? "",
                LoggedByUserID = Convert.ToInt32(reader["LoggedByUserID"]),
                CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString())
            };
        }

        /// <summary>
        /// Get count of items by status
        /// </summary>
        public Dictionary<string, int> GetItemCountByStatus()
        {
            var counts = new Dictionary<string, int>();

            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "SELECT Status, COUNT(*) as Count FROM Items GROUP BY Status";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        counts[reader["Status"].ToString()] = Convert.ToInt32(reader["Count"]);
                    }
                }
            }

            return counts;
        }
    }
}
