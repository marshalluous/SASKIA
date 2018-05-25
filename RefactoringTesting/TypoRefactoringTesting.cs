using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring;
using Refactoring.Refactorings.DictionaryRefactoring;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public class TypoRefactoringTesting
    {
        private static void TypoTest<T>(IRefactoring refactoring, string source, string expected)
        {
            TestHelper.TestCodeFix<T>(refactoring, source, expected);
        }

        private static void ListCompare(IList<string> first, IList<string> second)
        {
            Assert.IsTrue(first != null && second != null && first.Count() == second.Count());

            for (var index = 0; index < first.Count(); ++index)
            {
                Assert.AreEqual(first[index], second[index]);
            }
        }

        [TestMethod]
        public void SimpleStructNameTypoTest()
        {
            var source = "struct Appartment {}";
            TypoTest<StructDeclarationSyntax>(new TypoRefactoring(), source, "struct Apartment{}");
        }

        [TestMethod]
        public void StructNameTypoTest()
        {
            var source = "struct AppartmentRoom {}";
            TypoTest<StructDeclarationSyntax>(new TypoRefactoring(), source, "struct ApartmentRoom{}");
        }

        [TestMethod]
        public void StructUnderlineNameTypoTest()
        {
            var source = "struct Appartment_Room {}";
            TypoTest<StructDeclarationSyntax>(new TypoRefactoring(), source, "struct Apartment_Room{}");
        }

        [TestMethod]
        public void SimpleEnumNameTypoTest()
        {
            var source = "enum Appartment {}";
            TypoTest<EnumDeclarationSyntax>(new TypoRefactoring(), source, "enum Apartment{}");
        }

        [TestMethod]
        public void EnumNameTypoTest()
        {
            var source = "enum AppartmentRoom {}";
            TypoTest<EnumDeclarationSyntax>(new TypoRefactoring(), source, "enum ApartmentRoom{}");
        }

        [TestMethod]
        public void EnumUnerlineNameTypoTest()
        {
            var source = "enum Appartment_Room {}";
            TypoTest<EnumDeclarationSyntax>(new TypoRefactoring(), source, "enum Apartment_Room{}");
        }

        [TestMethod]
        public void SimpleClassNameTypoTest()
        {
            var source = "class Appartment {}";
            TypoTest<ClassDeclarationSyntax>(new TypoRefactoring(), source, "class Apartment{}");
        }

        [TestMethod]
        public void ClassNameTypoTest()
        {
            var source = "class AppartmentRoom {}";
            TypoTest<ClassDeclarationSyntax>(new TypoRefactoring(), source, "class ApartmentRoom{}");
        }

        [TestMethod]
        public void ClassUnderlineNameTypoTest()
        {
            var source = "class Appartment_Room {}";
            TypoTest<ClassDeclarationSyntax>(new TypoRefactoring(), source, "class Apartment_Room{}");
        }

        [TestMethod]
        public void SimplePropertyNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private int Rooom{ get; set; }" +
                "}";
            TypoTest<PropertyDeclarationSyntax>(new TypoRefactoring(), source, "private int Room{ get; set; }");
        }

        [TestMethod]
        public void PropertyNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private int RooomCount { get; set; }" +
                "}";
            TypoTest<PropertyDeclarationSyntax>(new TypoRefactoring(), source, "private int RoomCount{ get; set; }");
        }

        [TestMethod]
        public void SimpleUnderlinedPropertyNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private int _Room{ get; set; }" +
                "}";
            TypoTest<PropertyDeclarationSyntax>(new TypoRefactoring(), source, "private int Room{ get; set; }");
        }

        [TestMethod]
        public void UnderlinedPropertyNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private int _RoomCount { get; set; }" +
                "}";
            TypoTest<PropertyDeclarationSyntax>(new TypoRefactoring(), source, "private int RoomCount{ get; set; }");
        }

        [TestMethod]
        public void SimpleLinkedPropertyFieldTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    "private int _rooom;" +
                    "private int RoomCount { get { return _rooom; } set{ _rooom=value; } }" +
                "}";
            TypoTest<FieldDeclarationSyntax>(new TypoRefactoring(), source, "private int _room;");
        }

        [TestMethod]
        public void LinkedPropertyFieldTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    "private int _rooomCount;" +
                    "private int RoomCount { get { return _rooomCount; } set{ _rooomCount=value; } }" +
                "}";
            TypoTest<FieldDeclarationSyntax>(new TypoRefactoring(), source, "private int _roomCount;");
        }

        [TestMethod]
        public void ComplexLinkedPropertyFieldTypoTest()
        {
            var source =
                "class Apartment {" +
                    "private int _rooomCount;" +
                    "private int RoomCount { get { return _rooomCount; } set{ _rooomCount=value; } }" +
                "}";
            var expected =
                "class Apartment {" +
                    "private int _roomCount;" +
                    "private int RoomCount { get { return _roomCount; } set{ _roomCount=value; } }" +
                "}";
            throw new NotImplementedException("");
        }

        [TestMethod]
        public void SimpleFieldNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private int rooom;" +
                "}";
            TypoTest<VariableDeclaratorSyntax>(new TypoRefactoring(), source, "room");
        }

        [TestMethod]
        public void FieldNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private int rooomCount;" +
                "}";
            TypoTest<VariableDeclaratorSyntax>(new TypoRefactoring(), source, "roomCount");
        }

        [TestMethod]
        public void UnderlinedFieldNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private int _rooomCount;" +
                "}";
            TypoTest<VariableDeclaratorSyntax>(new TypoRefactoring(), source, "_roomCount");
        }

        [TestMethod]
        public void UnderlinedDoubleFieldNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private int _rooom_Count;" +
                "}";
            TypoTest<VariableDeclaratorSyntax>(new TypoRefactoring(), source, "_room_Count");
        }

        [TestMethod]
        public void SimpleInterfaceNameTypoTest()
        {
            var source = "interface IAppartment {}";
            TypoTest<InterfaceDeclarationSyntax>(new TypoRefactoring(), source, "interface IApartment{}");
        }

        [TestMethod]
        public void InterfaceNameTypoTest()
        {
            var source = "interface IAppartmentRoom {}";
            TypoTest<InterfaceDeclarationSyntax>(new TypoRefactoring(), source, "interface IApartmentRoom{}");
        }

        [TestMethod]
        public void InterfaceUnderlineNameTypoTest()
        {
            var source = "interface IAppartment_Room {}";
            TypoTest<InterfaceDeclarationSyntax>(new TypoRefactoring(), source, "interface IApartment_Room{}");
        }

        [TestMethod]
        public void InterfaceDoubleUnderlineNameTypoTest()
        {
            var source = "interface I_Appartment_Room {}";
            TypoTest<InterfaceDeclarationSyntax>(new TypoRefactoring(), source, "interface I_Apartment_Room{}");
        }

        [TestMethod]
        public void SimpleMethodNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private void Rooom(){}" +
                "}";
            TypoTest<MethodDeclarationSyntax>(new TypoRefactoring(), source, "private void Room(){}");
        }

        [TestMethod]
        public void MethodNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private void RooomCount(){}" +
                "}";
            TypoTest<MethodDeclarationSyntax>(new TypoRefactoring(), source, "private void RoomCount(){}");
        }

        [TestMethod]
        public void MethodUnderlineNameTypoTest()
        {
            var source = "" +
                "class Apartment {" +
                    " private void Rooom_Count(){}" +
                "}";
            TypoTest<MethodDeclarationSyntax>(new TypoRefactoring(), source, "private void Room_Count(){}");
        }
    }
}
