using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Refactoring.WordHelper
{
	internal static class WordSplitter
	{
		public static string GetLastWord(string input) => 
		    GetSplittedWordList(input).Last();

	    public static List<string> GetSplittedWordList(string input)
		{
			var words = Regex
				.Replace(input, @"(\p{Ll})(\P{Ll})", "$1 $2")
				.Replace("_","_ ");
			return words.Split(' ').ToList();
		}
	}
}
