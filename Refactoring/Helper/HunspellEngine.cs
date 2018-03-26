using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NHunspell;

namespace Refactoring.Helper
{
	public sealed class HunspellEngine
	{
		public bool HasTypo(string className)
		{
			bool error = false;  			using (var hunspell = new Hunspell(GetFileInProjectFolder("en_us.aff"), GetFileInProjectFolder("en_us.dic"))) 			{ 				bool allWordsSpelledCorrect = true; 				string lastWord = WordSplitter.GetLastWord(className); 				var wordList = WordSplitter.GetSplittedWordList(className); 				SpellCheckAllWords(hunspell, wordList, ref allWordsSpelledCorrect); 				error = !allWordsSpelledCorrect; 			}

			return error;
		}

		private string GetFileInProjectFolder(string fileName) 		{ 			var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 			return $"Z:\\Development\\HSR\\SASKIA\\{fileName}"; 		}  		private void SpellCheckAllWords(Hunspell hunspell, List<string> wordList, ref bool spelledCorrect) 		{ 			spelledCorrect = true; 			foreach (string word in wordList) 			{ 				if (!spelledCorrect) 					return; 				else 					spelledCorrect = hunspell.Spell(word); 			} 		}
	}
}
