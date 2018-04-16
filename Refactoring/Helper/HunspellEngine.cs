﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NHunspell;

namespace Refactoring.Helper
{
	public sealed class HunspellEngine
	{
		private string ExecutableAssemlyPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		public HunspellEngine()
		{
			Hunspell.NativeDllPath = ExecutableAssemlyPath;
		}

		public bool HasTypo(string wordString)
		{
			return ExecuteHunspellQuery(hunspell =>
			{
				var capitalWord = MorphWord(wordString, char.ToUpper);
				var smallWord = MorphWord(wordString, char.ToLower);
				return !(hunspell.Spell(capitalWord) || hunspell.Spell(smallWord));
			});
		}

		private string MorphWord(string word, Func<char, char> function)
		{
			var firstLetter = word.First();
			var morphedFirst = function(firstLetter);
			return word.Replace(firstLetter, morphedFirst);
		}

		public List<string> GetSuggestions(string word)
		{
			return ExecuteHunspellQuery(hunspell =>
			{
				var list = hunspell.Suggest(word).Take(5);
				List<string> suggestions = new List<string>();
				foreach (var suggestion in list)
				{
					suggestions.Add(suggestion.Replace(" ", ""));
				}
				return suggestions;
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
			return ExecutableAssemlyPath + $"\\{fileName}";
		}
	}
}
