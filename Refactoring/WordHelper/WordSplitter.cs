using System.Collections.Generic;
using System.Linq;

namespace Refactoring.WordHelper
{
	internal static class WordSplitter
	{
		public static string GetLastWord(string input) => 
		    GetSplittedWordList(input).Last();
        
	    public static IEnumerable<string> GetSplittedWordList(string identifier)
	    {
	        var word = string.Empty;
            
	        foreach (var currentChar in identifier)
	        {
	            if (char.IsUpper(currentChar))
	            {
	                if (!string.IsNullOrEmpty(word))
	                    yield return word;
	                word = string.Empty;
                }

	            if (currentChar == '_')
	            {
	                if (!string.IsNullOrEmpty(word))
	                    yield return word;
	                yield return "_";
	                word = string.Empty;
	            }
	            else
	            {
	                word += currentChar;
                }
	        }

	        yield return word;
	    }
	}
}
