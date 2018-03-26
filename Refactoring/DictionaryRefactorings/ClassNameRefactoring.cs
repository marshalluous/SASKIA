﻿using Microsoft.CodeAnalysis; using Microsoft.CodeAnalysis.CSharp; using Microsoft.CodeAnalysis.CSharp.Syntax; using NHunspell; using System; using System.Collections.Generic; using System.Data.SQLite; using System.IO; using System.Linq; using System.Reflection; using System.Text.RegularExpressions;  namespace Refactoring.DictionaryRefactorings { 	//public sealed class ClassNameRefactoring : IRefactoring 	//{ 	//	public string DiagnosticId => "SASKIA200"; 	//	public string Title => DiagnosticId; 	//	public string Description => Title;  	//	public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() => 	//		new[] { SyntaxKind.ClassDeclaration };  	//	public DiagnosticInfo DoDiagnosis(SyntaxNode node) 	//	{ 	//		var classNode = (ClassDeclarationSyntax)node; 	//		var className = classNode.Identifier.ValueText;  	//		bool error = false;  	//		using (var hunspell = new Hunspell(GetFileInProjectFolder("en_us.aff"), GetFileInProjectFolder("en_us.dic"))) 	//		{ 	//			bool allWordsSpelledCorrect = true; 	//			string lastWord = GetLastWord(className); 	//			var wordList = SplitCamelCase(className); 	//			SpellCheckAllWords(hunspell, wordList, ref allWordsSpelledCorrect); 	//			error = !allWordsSpelledCorrect && IsNoun(lastWord); 	//		}  	//		return error
	//			? DiagnosticInfo.CreateFailedResult("Class name is no noun or has a typo") 	//			: DiagnosticInfo.CreateSuccessfulResult(); 	//	}  	//	public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node) 	//	{ 	//		yield return node; 	//	}  	//	public SyntaxNode GetReplaceableNode(SyntaxToken token) 	//	{ 	//		return token.Parent; 	//	}

	//	/* Hunspell */

	//	private string GetFileInProjectFolder(string fileName) 	//	{ 	//		var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 	//		return $"Z:\\Development\\HSR\\SASKIA\\{fileName}"; 	//	}  	//	private string GetLastWord(string input) 	//	{ 	//		var wordList = SplitCamelCase(input); 	//		return wordList[wordList.Count - 1]; 	//	}  	//	private List<string> SplitCamelCase(string input) 	//	{ 	//		string words = Regex.Replace(input, @"(\p{Ll})(\P{Ll})", "$1 $2"); 	//		return words.Split(' ').ToList(); 	//	}  	//	private void SpellCheckAllWords(Hunspell hunspell, List<string> wordList, ref bool spelledCorrect) 	//	{ 	//		spelledCorrect = true; 	//		foreach (string word in wordList) 	//		{ 	//			if (!spelledCorrect) 	//				return; 	//			else 	//				spelledCorrect = CheckSpell(hunspell, word); 	//		} 	//	}  	//	private bool IsSpelledCorrect(Hunspell hunspell, string input) 	//	{ 	//		return hunspell.Spell(input); 	//	}  	//	private bool CheckSpell(Hunspell hunspell, string word) 	//	{ 	//		if (!IsSpelledCorrect(hunspell, word)) 	//		{ 	//			Console.WriteLine("Warning: '" + word + "' spelled incorrectly"); 	//			return false; 	//		} 	//		return true; 	//	}

	//	/* SQL */
	//	private bool IsNoun(string word) 	//	{ 	//		using (SQLiteConnection dbConnection = new SQLiteConnection("Data Source=:memory:")) 	//		{ 	//			dbConnection.Open(); 	//			SetSameEncodingAsLocalDb(dbConnection); 	//			AttachLocalDbToMemoryDb(dbConnection); 	//			GetAllTableNames(dbConnection); 	//			GetAllColumnNames(dbConnection); 	//			return GetSpecificWord(dbConnection, "Do"); 	//		} 	//	}  	//	private bool GetSpecificWord(SQLiteConnection dbConnection, string word) 	//	{ 	//		using (SQLiteCommand getword = new SQLiteCommand(dbConnection)) 	//		{ 	//			getword.CommandText = $"select * from entries where word = '{word}'"; 	//			var reader = getword.ExecuteReader(); 	//			bool isNoun = false; 	//			while (reader.Read()) 	//			{ 	//				if (reader.GetString(1).StartsWith("n")) 	//					isNoun = true; 	//			} 	//			return isNoun; 	//		} 	//	}  	//	private void GetAllColumnNames(SQLiteConnection dbConnection) 	//	{ 	//		using (SQLiteCommand getEntry = new SQLiteCommand(dbConnection)) 	//		{ 	//			getEntry.CommandText = "select * from entries"; 	//			var reader = getEntry.ExecuteReader(); 	//			Console.WriteLine("Column Names:"); 	//			for (var i = 0; i < reader.FieldCount; i++) 	//			{ 	//				Console.WriteLine(reader.GetName(i)); 	//			} 	//		} 	//	}  	//	private void GetAllTableNames(SQLiteConnection dbConnection) 	//	{ 	//		using (SQLiteCommand getTables = new SQLiteCommand(dbConnection)) 	//		{ 	//			getTables.CommandText = "select name from dictDb.sqlite_master where type='table'"; 	//			SQLiteDataReader r = getTables.ExecuteReader(); 	//			while (r.Read()) 	//			{ 	//				Console.WriteLine(r["name"]); 	//			} 	//		} 	//	}  	//	private void AttachLocalDbToMemoryDb(SQLiteConnection dbConnection) 	//	{ 	//		using (SQLiteCommand inMemCommand = new SQLiteCommand(@"ATTACH '" + 	//							"Dictionary.db' " + 	//							"AS dictDb", dbConnection)) 	//		{ 	//			inMemCommand.ExecuteNonQuery(); 	//		} 	//	}  	//	private void SetSameEncodingAsLocalDb(SQLiteConnection dbConnection) 	//	{ 	//		using (SQLiteCommand pragma = new SQLiteCommand(dbConnection)) 	//		{ 	//			pragma.CommandText = "pragma encoding = \"UTF-16\""; 	//			pragma.ExecuteNonQuery(); 	//		} 	//	} 	//} } 