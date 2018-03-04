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

                //string input = "decompressed";
                //Console.WriteLine("Analyze the word '" + input + "'");
                //Analyze(hunspell, input);
                //Console.ReadKey();


                //var source = File.ReadAllText(GetFileInProjectFolder("source.txt"));

                var words = SplitCamelCase("WordAnalyzeTool");
                var word = words[words.Capacity - 1];
                Console.WriteLine(word);
                Console.WriteLine(GetWordType(engine.GetSynSets(word)));
                Console.ReadKey();
            }

        }

        private static string GetWordType(List<SynSet> set)
        {
            return set[0].PartOfSpeech.ToString();
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
