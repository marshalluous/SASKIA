using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHW
{
    class Program
    {
        static void Main(string[] args)
        {
            var roslyn = new RoslynEngine();
            var tree = roslyn.GetSyntaxTree("source.txt");
            var compilation = roslyn.GetCompilation(tree);
            List<string> classNames = roslyn.GetClassNames(tree);
        }
    }
}
