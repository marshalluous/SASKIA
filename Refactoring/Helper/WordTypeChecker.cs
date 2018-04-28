using System.Data.SQLite;

namespace Refactoring.Helper
{
    class WordTypeChecker
	{
        private SQLiteConnection database;
        private const int WordTypeLocation = 1;

        public WordTypeChecker(SQLiteConnection database)
        {
            this.database = database;
        }

        public bool IsDefinitivelyNoun(string word)
        {
            using (SQLiteCommand getWordFromDatabase = new SQLiteCommand(database))
            {
                getWordFromDatabase.CommandText = $"select * from entries where word = '{word}'";
                var reader = getWordFromDatabase.ExecuteReader();
                while (reader.Read())
                {
                    var wordType = reader.GetString(WordTypeLocation);
                    if (wordType.Contains("n.") &&
                        !(wordType.Contains("v.") || 
                            wordType.Contains("adv.") ||
                            wordType.Contains("adj.")))
                        return true;
                }
                return false;
            }
        }

        public bool IsNoun(string word)
        {
            using (SQLiteCommand getWordFromDatabase = new SQLiteCommand(database))
            {
                getWordFromDatabase.CommandText = $"select * from entries where word = '{word}'";
                var reader = getWordFromDatabase.ExecuteReader();
                while (reader.Read())
                {
					var wordType = reader.GetString(WordTypeLocation);
					if (wordType.Contains("n.")) return true;
                }
                return false;
            }
        }

        public bool IsVerb(string word)
        {
            using (SQLiteCommand getWordFromDatabase = new SQLiteCommand(database))
            {
                getWordFromDatabase.CommandText = $"select * from entries where word = '{word}'";
                var reader = getWordFromDatabase.ExecuteReader();
                while (reader.Read())
				{
					var wordType = reader.GetString(WordTypeLocation);
					if (wordType.Contains("v.")) return true;
                }
                return false;
            }
        }

        public bool IsAdverb(string word)
        {
            using (SQLiteCommand getWordFromDatabase = new SQLiteCommand(database))
            {
                getWordFromDatabase.CommandText = $"select * from entries where word = '{word}'";
                var reader = getWordFromDatabase.ExecuteReader();
                while (reader.Read())
                {
                    var wordType = reader.GetString(WordTypeLocation);
                    if (wordType.Contains("adv.")) return true;
                }
                return false;
            }
        }

        public bool IsAdjective(string word)
        {
            using (SQLiteCommand getWordFromDatabase = new SQLiteCommand(database))
            {
                getWordFromDatabase.CommandText = $"select * from entries where word = '{word}'";
                var reader = getWordFromDatabase.ExecuteReader();
                while (reader.Read())
                {
                    var wordType = reader.GetString(WordTypeLocation);
                    if (wordType.Contains("adj.")) return true;
                }
                return false;
            }
        }
    }
}
