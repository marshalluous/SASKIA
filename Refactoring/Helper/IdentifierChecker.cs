using System.Linq;
using System.Text;
using Refactoring.WordHelper;

namespace Refactoring.Helper
{
    internal static class IdentifierChecker
    {
        public static bool IsUpperCamelCase(string identifierName)
        {
            var wordList = WordSplitter.GetSplittedWordList(identifierName);
            return wordList.All(word => char.IsUpper(word[0]) || char.IsDigit(word[0]));
        }

        public static bool IsLowerCamelCase(string identifierName)
        {
            var wordList = WordSplitter.GetSplittedWordList(identifierName);
            return char.IsLower(wordList[0][0]) && wordList.Skip(1).All(word => char.IsUpper(word[0]));
        }
        
        public static string ToUpperCamelCaseIdentifier(string identifierName)
        {
            return string.IsNullOrEmpty(identifierName) ? 
                string.Empty : FixUnderlines(char.ToUpper(identifierName[0]) + identifierName.Substring(1));
        }

        public static string FixUnderlines(string identifierName)
        {
            if (string.IsNullOrEmpty(identifierName))
                return identifierName;
            
            var upperNext = false;
            var result = new StringBuilder();

            foreach (var currentChar in identifierName)
            {
                if (currentChar == '_')
                {
                    upperNext = true;
                }
                else
                {
                    var nextChar = currentChar;

                    if (upperNext)
                    {
                        nextChar = char.ToUpper(nextChar);
                        upperNext = false;
                    }

                    result.Append(nextChar);
                }
            }

            return result.ToString();
        }
    }
}