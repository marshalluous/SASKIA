using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryDbProj
{
	class Program
	{
		static void Main(string[] args)
		{
			using (SQLiteConnection dbConnection = new SQLiteConnection("Data Source=:memory:"))
			{
				dbConnection.Open();
				SetSameEncodingAsLocalDb(dbConnection);
				AttachLocalDbToMemoryDb(dbConnection);
				GetAllTableNames(dbConnection);
				GetAllColumnNames(dbConnection);
				GetSpecificWord(dbConnection, "Do");
				Console.ReadKey();
			}
		}

		private static void GetSpecificWord(SQLiteConnection dbConnection, string word)
		{
			using (SQLiteCommand getword = new SQLiteCommand(dbConnection))
			{
				getword.CommandText = $"select * from entries where word = '{word}'";
				var reader = getword.ExecuteReader();
				while (reader.Read())
				{
					Console.WriteLine(reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2));
				}
			}
		}

		private static void GetAllColumnNames(SQLiteConnection dbConnection)
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

		private static void GetAllTableNames(SQLiteConnection dbConnection)
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

		private static void AttachLocalDbToMemoryDb(SQLiteConnection dbConnection)
		{
			using (SQLiteCommand inMemCommand = new SQLiteCommand(@"ATTACH '" +
								Assembly.GetExecutingAssembly().Location.Replace("\\DictionaryDbProj.exe", "") + "\\Dictionary.db' " +
								"AS dictDb", dbConnection))
			{
				inMemCommand.ExecuteNonQuery();
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
	}
}
