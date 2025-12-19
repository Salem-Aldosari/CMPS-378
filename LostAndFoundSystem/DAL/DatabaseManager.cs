using System;
using System.Data.SQLite;
using System.IO;

namespace LostAndFoundSystem.DAL
{
    public class DatabaseManager
    {
        private static readonly string DatabaseFile = "LostAndFound.db";
        public static readonly string ConnectionString = $"Data Source={DatabaseFile};Version=3;";

        public static void InitializeDatabase()
        {
            bool isNewDatabase = !File.Exists(DatabaseFile);
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                if (isNewDatabase)
                {
                    CreateTables(connection);
                    InsertSampleData(connection);
                }
            }
        }

        private static void CreateTables(SQLiteConnection connection)
        {
            string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    Role TEXT NOT NULL,
                    Email TEXT,
                    ContactInfo TEXT,
                    IsActive INTEGER DEFAULT 1,
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                )";

            string createCategoriesTable = @"
                CREATE TABLE IF NOT EXISTS Categories (
                    CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CategoryName TEXT UNIQUE NOT NULL,
                    Description TEXT
                )";

            string createLocationsTable = @"
                CREATE TABLE IF NOT EXISTS Locations (
                    LocationID INTEGER PRIMARY KEY AUTOINCREMENT,
                    LocationName TEXT UNIQUE NOT NULL,
                    Description TEXT
                )";

            string createItemsTable = @"
                CREATE TABLE IF NOT EXISTS Items (
                    ItemID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    CategoryID INTEGER NOT NULL,
                    Description TEXT,
                    FoundDate DATETIME NOT NULL,
                    FoundLocationID INTEGER NOT NULL,
                    Status TEXT DEFAULT 'Active',
                    FinderNotes TEXT,
                    LoggedByUserID INTEGER,
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
                    FOREIGN KEY (FoundLocationID) REFERENCES Locations(LocationID),
                    FOREIGN KEY (LoggedByUserID) REFERENCES Users(UserID)
                )";

            string createClaimsTable = @"
                CREATE TABLE IF NOT EXISTS Claims (
                    ClaimID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ItemID INTEGER NOT NULL,
                    ClaimerUserID INTEGER,
                    ClaimerName TEXT NOT NULL,
                    ClaimDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                    VerificationStatus TEXT DEFAULT 'Pending',
                    VerifierNotes TEXT,
                    ContactInfo TEXT,
                    ItemDescription TEXT,
                    FOREIGN KEY (ItemID) REFERENCES Items(ItemID),
                    FOREIGN KEY (ClaimerUserID) REFERENCES Users(UserID)
                )";

            string createIndexes = @"
                CREATE INDEX IF NOT EXISTS idx_items_category ON Items(CategoryID);
                CREATE INDEX IF NOT EXISTS idx_items_location ON Items(FoundLocationID);
                CREATE INDEX IF NOT EXISTS idx_items_status ON Items(Status);
                CREATE INDEX IF NOT EXISTS idx_items_founddate ON Items(FoundDate);
                CREATE INDEX IF NOT EXISTS idx_claims_item ON Claims(ItemID);
                CREATE INDEX IF NOT EXISTS idx_claims_status ON Claims(VerificationStatus);
            ";

            ExecuteNonQuery(connection, createUsersTable);
            ExecuteNonQuery(connection, createCategoriesTable);
            ExecuteNonQuery(connection, createLocationsTable);
            ExecuteNonQuery(connection, createItemsTable);
            ExecuteNonQuery(connection, createClaimsTable);
            ExecuteNonQuery(connection, createIndexes);
        }

        private static void InsertSampleData(SQLiteConnection connection)
        {
            // Generate proper BCrypt hashes
            string adminHash = BCrypt.Net.BCrypt.HashPassword("admin123");
            string staffHash = BCrypt.Net.BCrypt.HashPassword("staff123");
            string studentHash = BCrypt.Net.BCrypt.HashPassword("student123");

            // Insert users with properly hashed passwords
            string insertAdmin = $"INSERT INTO Users (Username, PasswordHash, Role, Email) VALUES ('admin', '{adminHash}', 'Admin', 'admin@campus.edu')";
            string insertStaff = $"INSERT INTO Users (Username, PasswordHash, Role, Email) VALUES ('staff', '{staffHash}', 'Staff', 'staff@campus.edu')";
            string insertStudent = $"INSERT INTO Users (Username, PasswordHash, Role, Email) VALUES ('student', '{studentHash}', 'Student', 'student@campus.edu')";
            
            string insertCategories = @"
                INSERT INTO Categories (CategoryName, Description) VALUES 
                ('Electronics', 'Phones, tablets, laptops, chargers, headphones'),
                ('Clothing', 'Jackets, shirts, pants, shoes, hats'),
                ('Books', 'Textbooks, notebooks, binders'),
                ('Personal Items', 'Wallets, bags, backpacks'),
                ('Keys', 'Car keys, room keys, key chains'),
                ('Accessories', 'Jewelry, watches, glasses'),
                ('Sports Equipment', 'Balls, rackets, gym gear'),
                ('Other', 'Miscellaneous items')";

            string insertLocations = @"
                INSERT INTO Locations (LocationName, Description) VALUES 
                ('Library', 'Main campus library'),
                ('Cafeteria', 'Student dining hall'),
                ('Gym', 'Recreation center'),
                ('Parking Lot', 'Campus parking areas'),
                ('Classroom Buildings', 'Academic buildings'),
                ('Student Center', 'Student union building'),
                ('Dorms', 'Residence halls'),
                ('Other', 'Other campus locations')";

            string insertSampleItems = @"
                INSERT INTO Items (Title, CategoryID, Description, FoundDate, FoundLocationID, Status, FinderNotes, LoggedByUserID) VALUES 
                ('iPhone 13', 1, 'Black iPhone with cracked screen protector', '2025-11-18 14:30:00', 1, 'Active', 'Found on study table', 1),
                ('Blue Jacket', 2, 'Navy blue windbreaker, size M', '2025-11-17 09:15:00', 2, 'Active', 'Left on chair', 1),
                ('Chemistry Textbook', 3, 'Chemistry 101 textbook, 3rd edition', '2025-11-16 16:45:00', 5, 'Active', 'Found in classroom 204', 1),
                ('Black Backpack', 4, 'JanSport backpack with laptop compartment', '2025-11-15 10:20:00', 7, 'Active', 'Found in hallway', 1),
                ('Car Keys', 5, 'Toyota key with Pacman keychain', '2025-11-14 11:00:00', 4, 'Claimed', 'Found near parking entrance', 1)";

            ExecuteNonQuery(connection, insertAdmin);
            ExecuteNonQuery(connection, insertStaff);
            ExecuteNonQuery(connection, insertStudent);
            ExecuteNonQuery(connection, insertCategories);
            ExecuteNonQuery(connection, insertLocations);
            ExecuteNonQuery(connection, insertSampleItems);
        }

        private static void ExecuteNonQuery(SQLiteConnection connection, string query)
        {
            using (var command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
