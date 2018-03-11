using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class ExpressionEvaluatorTesting
    {
        [TestMethod]
        public void EvaluateMathExpression()
        {
            var actual = ExpressionEvaluator.Evaluate("4 + 245 - (12 * 4)");
            Assert.AreEqual(201, actual);
        }

        [TestMethod]
        public void EvaluateBooleanExpression()
        {
            var actual = ExpressionEvaluator.Evaluate("(false && !true) || true");
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void StringConcatExpression()
        {
            var actual = ExpressionEvaluator.Evaluate("'h' + \"allo\"");
            Assert.AreEqual("hallo", actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionEvaluatorException))]
        public void MethodCallEvaluator()
        {
            ExpressionEvaluator.Evaluate("Console.WriteLine('a');");
        }
    }
}
