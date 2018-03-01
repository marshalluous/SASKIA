using NHunspell;
using System;

namespace NHanspellSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NHunspell functions demonstration");
            Console.WriteLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯");
            Console.WriteLine();

            using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
            {
                Console.WriteLine("Hunspell - Spell Checking Functions");
                Console.WriteLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯");

                Console.WriteLine("Check if the word 'Recommendation' is spelled correct");
                bool correct = hunspell.Spell("Recommendatio");
                Console.WriteLine("Recommendation is spelled " + (correct ? "correct" : "wrong"));
                Console.ReadKey();

                //Console.WriteLine("");
                //Console.WriteLine("Make suggestions for the misspelled word 'Recommendatio'");
                //List<string> suggestions = hunspell.Suggest("Recommendatio");
                //Console.WriteLine("There are " + suggestions.Count.ToString() + " suggestions");
                //foreach (string suggestion in suggestions)
                //{
                //    Console.WriteLine("Suggestion is: " + suggestion);
                //}


                //Console.WriteLine("");
                //Console.WriteLine("Find the word stem of the word 'decompressed'");
                //List<string> stems = hunspell.Stem("decompressed");
                //foreach (string stem in stems)
                //{
                //    Console.WriteLine("Word Stem is: " + stem);
                //}

                //Console.WriteLine("");
                //Console.WriteLine("Generate the plural of 'girl' by providing sample 'boys'");
                //List<string> generated = hunspell.Generate("girl", "boys");
                //foreach (string stem in generated)
                //{
                //    Console.WriteLine("Generated word is: " + stem);
                //}

                //Console.WriteLine("");
                //Console.WriteLine("Analyze the word 'decompressed'");
                //List<string> morphs = hunspell.Analyze("decompressed");
                //foreach (string morph in morphs)
                //{
                //    Console.WriteLine("Morph is: " + morph);
                //}

            }
        }
    }
}
