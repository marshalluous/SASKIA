using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace Refactoring.WordHelper
{
    public class DictionaryClass
    {
        private static SQLiteConnection database;

        public static SQLiteConnection Database {
            get {
                if (database != null && database.State == System.Data.ConnectionState.Open)
                    return database;

                database = new SQLiteConnection("Data Source=:memory:");
                database.Open();
                SetSameEncodingAsLocalDb(database);
                AttachLocalDbToMemoryDb(database);
                return database;
            }
        }

        private static void SetSameEncodingAsLocalDb(SQLiteConnection dbConnection)
        {
            using (var pragma = new SQLiteCommand(dbConnection))
            {
                pragma.CommandText = "pragma encoding = \"UTF-16\"";
                pragma.ExecuteNonQuery();
            }
        }

        private static void AttachLocalDbToMemoryDb(SQLiteConnection dbConnection)
        {
            using (var inMemCommand = new SQLiteCommand(@"ATTACH '" +
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Dictionary.db' " +
                                "AS dictDb", dbConnection))
            {
                inMemCommand.ExecuteNonQuery();
            }
        }
    }
}
