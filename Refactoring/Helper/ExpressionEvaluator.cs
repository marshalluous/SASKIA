using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Refactoring.Helper
{
    internal static class ExpressionEvaluator
    {
        private const string ClassName = "Class";
        private const string MethodName = "Method";

        public static object Evaluate(string expression)
        {
            var compilerResult = Compile(expression);

            if (compilerResult.Errors.Count > 0)
                throw new ExpressionEvaluatorException("expression cannot be compiled");

            return Invoke(compilerResult);
        }

        private static object Invoke(CompilerResults compilerResult)
        {
            var assembly = compilerResult.CompiledAssembly;
            var instance = assembly.CreateInstance(ClassName);
            var method = instance?.GetType().GetMethod(MethodName);
            return method?.Invoke(instance, null);
        }

        private static string CreateCodeTemplate(string code)
        {
            return $"public class {ClassName}{{ public object {MethodName}(){{ return {code}; }}}}";
        }

        private static CompilerResults Compile(string code)
        {
            var codeProvider = new CSharpCodeProvider();
            return codeProvider.CompileAssemblyFromSource(CreateCompilerParameters(),
                CreateCodeTemplate(code));
        }

        private static CompilerParameters CreateCompilerParameters()
        {
            return new CompilerParameters
            {
                CompilerOptions = "/t:library",
                GenerateInMemory = true
            };
        }
    }
}
