using System.IO;
using System.Reflection;

namespace NHunspellApplied
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = File.ReadAllText(GetSourceFilePath());
            // split a single word on each capital letter
            using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
            {
            }
        }
        private static string GetSourceFilePath()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return $"{assemblyPath}\\..\\..\\source.txt";
        }
    }
}
