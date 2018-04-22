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

        public bool IsNoun(string word)
        {
            using (SQLiteCommand getWordFromDatabase = new SQLiteCommand(database))
            {
                getWordFromDatabase.CommandText = $"select * from entries where word = '{word}'";
                var reader = getWordFromDatabase.ExecuteReader();
                while (reader.Read())
                {
					var wordType = reader.GetString(WordTypeLocation);
					if (wordType.StartsWith("n")) return true;
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
					if (wordType.StartsWith("v")) return true;
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
					if (reader.GetString(WordTypeLocation).StartsWith("a")) return true;
				}
				return false;
			}
		}
    }
}
