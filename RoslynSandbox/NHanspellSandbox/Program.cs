using NHunspell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NHanspellSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
            {
                //string input = "Recommendation";
                //Console.WriteLine("Check if the word '" + input + "' is spelled correct");
                //bool correct = IsSpelledCorrect(hunspell, input);
                //Console.WriteLine(input + " is spelled " + (correct ? "correct" : "wrong"));
                //if (!correct)
                //{
                //    MakeSuggestions(hunspell, input);
                //}
                //Console.ReadKey();

                //input = "Recommendatio";
                //Console.WriteLine("Check if the word '" + input + "' is spelled correct");
                //correct = IsSpelledCorrect(hunspell, input);
                //Console.WriteLine(input + " is spelled " + (correct ? "correct" : "wrong"));
                //if (!correct)
                //{
                //    MakeSuggestions(hunspell, input);
                //}
                //Console.ReadKey();

                //input = "decompressed";
                //Console.WriteLine("Find the word stem of the word '" + input + "'");
                //List<string> stems = hunspell.Stem(input);
                //foreach (string stem in stems)
                //{
                //    Console.WriteLine("Word Stem is: " + stem);
                //}
                //Console.ReadKey();


                //input = "girl";
                //string input2 = "boys";
                //Console.WriteLine("Generate the plural of '" + input + "' by providing sample '" + input2 + "'");
                //GetStem(hunspell, input, input2);
                //Console.ReadKey();

                //input = "decompressed";
                //Console.WriteLine("Analyze the word '" + input + "'");
                //Analyze(hunspell, input);
                //Console.ReadKey();


                var source = File.ReadAllText(GetSourceFilePath());
                Console.WriteLine(SplitCamelCase("PascalCase"));
                Console.WriteLine(SplitCamelCase("camelCase"));
                Console.ReadKey();
            }
        }

        private static string SplitCamelCase(string input)
        {
            return Regex.Replace(input, @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        private static string GetSourceFilePath()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return $"{assemblyPath}\\..\\..\\source.txt";
        }

        private static void Analyze(Hunspell hunspell, string input)
        {
            List<string> morphs = hunspell.Analyze(input);
            foreach (string morph in morphs)
            {
                Console.WriteLine("Morph is: " + morph);
            }
        }

        private static void GetStem(Hunspell hunspell, string input, string input2)
        {
            List<string> generated = hunspell.Generate(input, input2);
            foreach (string stem in generated)
            {
                Console.WriteLine("Generated word is: " + stem);
            }
        }

        private static void MakeSuggestions(Hunspell hunspell, string input)
        {
            List<string> suggestions = hunspell.Suggest(input);
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
