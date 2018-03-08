using System;

namespace RHW
{
    class Program
    {
        static void Main(string[] args)
        {
            var roslyn = new RoslynEngine();
            var tree = roslyn.GetSyntaxTree("source.txt");
            var compilation = roslyn.GetCompilation(tree);
            var classNames = roslyn.GetClassNames(tree);
            var hunspell = new HunspellEngine();
            hunspell.CheckClassNames(classNames);
            Console.ReadKey();
        }
    }
}
