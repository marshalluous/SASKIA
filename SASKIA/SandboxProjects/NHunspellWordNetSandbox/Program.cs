using NHunspell;
using Syn.WordNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NHunspellSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var hunspell = new Hunspell(GetFileInProjectFolder("en_us.aff"), GetFileInProjectFolder("en_us.dic")))
            {
                WordNetEngine engine = new WordNetEngine();
                var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                engine.LoadFromDirectory($"{assemblyPath}\\..\\..\\dict\\"); // extremely slow


                /* className test */
                var lastWord = GetLastWord("WordAnalyzeToolEat");
                CheckSpell(hunspell, lastWord);
                string wordType = GetWordType(engine, lastWord);
                if (wordType != "Noun")
                {
                    Console.WriteLine("Warning: Class name must be a noun");
                }
                Console.ReadKey();
            }

        }

        private static SynSet GetFirstSynSet(WordNetEngine engine, string lastWord)
        {
            var synSets = engine.GetSynSets(lastWord);
            if (synSets.Capacity < 1)
            {
                throw new ArgumentException(lastWord + " couldn't be found in WordNet database");
                // todo: orange/blau unterstreichen
            }
            else
            {
                return synSets[0];
            }
        }

        private static void CheckSpell(Hunspell hunspell, string lastWord)
        {
            if (!IsSpelledCorrect(hunspell, lastWord))
            {
                throw new ArgumentException(lastWord + "is not spelled correctly");
            }
        }

        private static string GetLastWord(string input)
        {
            List<string> wordList = SplitCamelCase(input);
            return wordList[wordList.Capacity - 1];
        }

        private static string GetWordType(WordNetEngine engine, string lastWord)
        {
            SynSet wordSet = GetFirstSynSet(engine, lastWord);
            return wordSet.PartOfSpeech.ToString();
        }

        private static List<string> SplitCamelCase(string input)
        {
            string words = Regex.Replace(input, @"(\p{Ll})(\P{Ll})", "$1 $2");
            return words.Split(' ').ToList();
        }

        private static string GetFileInProjectFolder(string fileName)
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return $"{assemblyPath}\\..\\..\\{fileName}";
        }

        private static void Analyze(Hunspell hunspell, string input)
        {
            var morphs = hunspell.Analyze(input);
            foreach (string morph in morphs)
            {
                Console.WriteLine("Morph is: " + morph);
            }
        }

        private static void GetStem(Hunspell hunspell, string input, string input2)
        {
            var generated = hunspell.Generate(input, input2);
            foreach (string stem in generated)
            {
                Console.WriteLine("Generated word is: " + stem);
            }
        }

        private static void MakeSuggestions(Hunspell hunspell, string input)
        {
            var suggestions = hunspell.Suggest(input);
            Console.WriteLine("There are " + suggestions.Count + " suggestions");
            foreach (string suggestion in suggestions)
            {
                Console.WriteLine("Suggestion is: " + suggestion);
            }
        }

        private static bool IsSpelledCorrect(Hunspell hunspell, string input)
        {
            return hunspell.Spell(input);
        }
    }
}
