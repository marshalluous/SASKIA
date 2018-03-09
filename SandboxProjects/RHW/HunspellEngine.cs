using NHunspell;
using Syn.WordNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RHW
{
    public class HunspellEngine
    {
        private int wrongWords, correctNouns = 80; // logging

        public void CheckClassNames(List<string> classNames)
        {
            WordNetEngine engine = new WordNetEngine();
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            engine.LoadFromDirectory($"{assemblyPath}\\..\\..\\dict\\"); // extremely slow

            using (var hunspell = new Hunspell(GetFileInProjectFolder("en_us.aff"), GetFileInProjectFolder("en_us.dic")))
            {
                foreach (string className in classNames)
                {
                    if (className == "") continue;
                    bool lastWordIsSpelledCorrect = true;
                    string lastWord = GetLastWord(className);
                    var wordList = SplitCamelCase(className);
                    SpellCheckAllWords(hunspell, wordList, ref lastWordIsSpelledCorrect);
                    if (lastWordIsSpelledCorrect && GetWordType(engine, lastWord) != "Noun")
                    {
                        correctNouns--;
                        Console.WriteLine("Warning: Class name '" + className + "' must end with a noun");
                    }
                }
                Console.WriteLine($"Correct nouns: {correctNouns}, wrongWords: {wrongWords}");
            }
        }

        private void SpellCheckAllWords(Hunspell hunspell, List<string> wordList, ref bool lastStatus)
        {
            foreach (string word in wordList)
            {
                if (word == wordList.Last())
                {
                    lastStatus = CheckSpell(hunspell, word);
                    if (!lastStatus) wrongWords++;
                }
                else
                {
                    if (!CheckSpell(hunspell, word)) wrongWords++;
                }
            }
        }

        private bool CheckSpell(Hunspell hunspell, string word)
        {
            if (!IsSpelledCorrect(hunspell, word))
            {
                Console.WriteLine("Warning: '" + word + "' spelled incorrectly");
                return false;
            }
            return true;
        }

        private string GetWordType(WordNetEngine engine, string lastWord)
        {
            SynSet wordSet = GetFirstSynSet(engine, lastWord);
            if (wordSet.LexicalRelations.Count() >= 1)
            {
                return wordSet.PartOfSpeech.ToString();
            }
            else
            {
                return "";
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

        private string GetFileInProjectFolder(string fileName)
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return $"{assemblyPath}\\..\\..\\{fileName}";
        }

        private string GetLastWord(string input)
        {
            var wordList = SplitCamelCase(input);
            return wordList[wordList.Count - 1];
        }

        private bool IsSpelledCorrect(Hunspell hunspell, string input)
        {
            return hunspell.Spell(input);
        }

        private List<string> SplitCamelCase(string input)
        {
            string words = Regex.Replace(input, @"(\p{Ll})(\P{Ll})", "$1 $2");
            return words.Split(' ').ToList();
        }
    }
}
