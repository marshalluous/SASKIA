using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Refactoring.Helper
{
	public sealed class WordSplitter
	{
		public static string GetLastWord(string input)
		{
			var wordList = GetSplittedWordList(input);
			return wordList[wordList.Count - 1];
		}

		public static List<string> GetSplittedWordList(string input)
		{
			string words = Regex.Replace(input, @"(\p{Ll})(\P{Ll})", "$1 $2");
			return words.Split(' ').ToList();
		}
	}
}
