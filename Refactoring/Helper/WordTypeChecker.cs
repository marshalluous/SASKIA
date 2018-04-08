using System;
using System.Data.SQLite;

namespace Refactoring.Helper
{
	class WordTypeChecker
	{
		public bool IsNoun(string className)
		{
			using (SQLiteConnection dbConnection = new SQLiteConnection("Data Source=:memory:"))
			{
				dbConnection.Open();
				SetSameEncodingAsLocalDb(dbConnection);
				AttachLocalDbToMemoryDb(dbConnection);
				GetAllTableNames(dbConnection);
				GetAllColumnNames(dbConnection);
				return IsNoun(dbConnection, WordSplitter.GetLastWord(className));
			}
		}

		private bool IsNoun(SQLiteConnection dbConnection, string word)
		{
			using (SQLiteCommand getword = new SQLiteCommand(dbConnection))
			{
				getword.CommandText = $"select * from entries where word = '{word}'";
				var reader = getword.ExecuteReader();
				while (reader.Read())
				{
					if (reader.GetString(1).StartsWith("n"))
					{
						return true;
					}
				}
				return false;
			}
		}

		private void GetAllColumnNames(SQLiteConnection dbConnection)
		{
			using (SQLiteCommand getEntry = new SQLiteCommand(dbConnection))
			{
				getEntry.CommandText = "select * from entries";
				var reader = getEntry.ExecuteReader();
				Console.WriteLine("Column Names:");
				for (var i = 0; i < reader.FieldCount; i++)
				{
					Console.WriteLine(reader.GetName(i));
				}
			}
		}

		private void GetAllTableNames(SQLiteConnection dbConnection)
		{
			using (SQLiteCommand getTables = new SQLiteCommand(dbConnection))
			{
				getTables.CommandText = "select name from dictDb.sqlite_master where type='table'";
				SQLiteDataReader r = getTables.ExecuteReader();
				while (r.Read())
				{
					Console.WriteLine(r["name"]);
				}
			}
		}

		private void AttachLocalDbToMemoryDb(SQLiteConnection dbConnection)
		{
			using (SQLiteCommand inMemCommand = new SQLiteCommand(@"ATTACH '" +
								"Dictionary.db' " +
								"AS dictDb", dbConnection))
			{
				inMemCommand.ExecuteNonQuery();
			}
		}

		private void SetSameEncodingAsLocalDb(SQLiteConnection dbConnection)
		{
			using (SQLiteCommand pragma = new SQLiteCommand(dbConnection))
			{
				pragma.CommandText = "pragma encoding = \"UTF-16\"";
				pragma.ExecuteNonQuery();
			}
		}
	}
}
