using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace Refactoring
{
    public class Dictionary
    {
        private static SQLiteConnection database;

        public static SQLiteConnection Database {
            get {
                if (database == null || database.State != System.Data.ConnectionState.Open)
                {
                    database = new SQLiteConnection("Data Source=:memory:");
                    database.Open();
                    SetSameEncodingAsLocalDb(database);
                    AttachLocalDbToMemoryDb(database);
                }
                return database;
            }
        }

        private static void SetSameEncodingAsLocalDb(SQLiteConnection dbConnection)
        {
            using (SQLiteCommand pragma = new SQLiteCommand(dbConnection))
            {
                pragma.CommandText = "pragma encoding = \"UTF-16\"";
                pragma.ExecuteNonQuery();
            }
        }

        private static void AttachLocalDbToMemoryDb(SQLiteConnection dbConnection)
        {
            using (SQLiteCommand inMemCommand = new SQLiteCommand(@"ATTACH '" +
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Dictionary.db' " +
                                "AS dictDb", dbConnection))
            {
                inMemCommand.ExecuteNonQuery();
            }
        }
    }
}
