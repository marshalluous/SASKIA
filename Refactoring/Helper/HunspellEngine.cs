using System;
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
			return ExecuteHunspellQuery(hunspell => {
				var wordList = WordSplitter.GetSplittedWordList(className);
				foreach (string word in wordList)
				{
					if (!hunspell.Spell(word))
					{
						return true;
					}
				}
				return false;
			});
		}

		public IEnumerable<string> GetSuggestions(string word)
		{
			return ExecuteHunspellQuery(hunspell => {
				return hunspell.Suggest(word);
			});
		}

		private T ExecuteHunspellQuery<T>(Func<Hunspell, T> query)
		{
			using (var hunspell = new Hunspell(GetFileInProjectFolder("en_us.aff"), GetFileInProjectFolder("en_us.dic")))
			{
				return query(hunspell);
			}
		}

		private string GetFileInProjectFolder(string fileName)
		{
			var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			return $"Z:\\Development\\HSR\\SASKIA\\{fileName}";
		}
	}
}
