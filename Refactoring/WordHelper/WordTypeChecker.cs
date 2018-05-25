using System;
using System.Data.SQLite;

namespace Refactoring.WordHelper
{
    internal sealed class WordTypeChecker
	{
        private readonly SQLiteConnection database;
        private const int WordTypeLocation = 1;

        public WordTypeChecker(SQLiteConnection database)
        {
            this.database = database;
        }

	    public bool IsDefinitivelyNoun(string word) =>
	        CheckWords(word, wordType => wordType.Contains("n.") &&
                !(wordType.Contains("v.") || wordType.Contains("adv.") || wordType.Contains("adj.")));

	    private bool IsWordType(string word, string wordTypeFlag) => 
	        CheckWords(word, wordType => wordType.Contains(wordTypeFlag));

	    private bool CheckWords(string word, Predicate<string> wordTypeChecker)
	    {
	        using (var getWordFromDatabase = new SQLiteCommand(database))
	        {
	            var reader = CreateReader(word, getWordFromDatabase);
                while (reader.Read())
	            {
	                var wordType = reader.GetString(WordTypeLocation);

	                if (wordTypeChecker(wordType))
	                    return true;
	            }
                return false;
	        }
        }

        private static SQLiteDataReader CreateReader(string word, SQLiteCommand getWordFromDatabase)
        {
            getWordFromDatabase.CommandText = CreateWordCommand(word);
            return getWordFromDatabase.ExecuteReader();
        }

        public bool IsNoun(string word) =>
	        IsWordType(word, "n.");

	    public bool IsVerb(string word) =>
	        IsWordType(word, "v.");

	    public bool IsAdverb(string word) =>
	        IsWordType(word, "adv.");

	    public bool IsAdjective(string word) =>
	        IsWordType(word, "adj.");

	    private static string CreateWordCommand(string word) =>
	        $"select * from entries where word = '{word}'";
    }
}