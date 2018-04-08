using Refactoring.Helper;
using System.Linq;
using System.Text;

namespace Refactoring.Refactorings.TypeIdentifierConvention
{
    internal static class IdentifierChecker
    {
        public static bool IsUpperCamelCase(string identifierName)
        {
            var wordList = WordSplitter.GetSplittedWordList(identifierName);
            return wordList.All(word => char.IsUpper(word[0]));
        }

        public static bool IsLowerCamelCase(string identifierName)
        {
            var wordList = WordSplitter.GetSplittedWordList(identifierName);
            return char.IsLower(wordList[0][0]) && wordList.Skip(1).All(word => char.IsUpper(word[0]));
        }
        
        public static string ToUpperCamelCaseIdentifier(string identifierName)
        {
            if (string.IsNullOrEmpty(identifierName))
                return string.Empty;
            return FixUnderlines(char.ToUpper(identifierName[0]) + identifierName.Substring(1));
        }

        public static string FixUnderlines(string identifierName)
        {
            if (string.IsNullOrEmpty(identifierName))
                return identifierName;
            
            bool upperNext = false;
            var result = new StringBuilder();

            for (var index = 0; index < identifierName.Length; ++index)
            {
                if (identifierName[index] == '_')
                {
                    upperNext = true;
                }
                else
                {
                    var nextChar = identifierName[index];

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