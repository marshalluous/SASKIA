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

        [TestMethod]
        public void SimpleStructNameTypoTest()
        {
            const string source = "struct Appartment {}";
            TypoTest<StructDeclarationSyntax>(new TypoRefactoring(), source, "struct Apartment{}");
        }

        [TestMethod]
        public void StructNameTypoTest()
        {
            const string source = "struct AppartmentRoom {}";
            TypoTest<StructDeclarationSyntax>(new TypoRefactoring(), source, "struct ApartmentRoom{}");
        }

        [TestMethod]
        public void StructUnderlineNameTypoTest()
        {
            const string source = "struct Appartment_Room {}";
            TypoTest<StructDeclarationSyntax>(new TypoRefactoring(), source, "struct Apartment_Room{}");
        }

        [TestMethod]
        public void SimpleEnumNameTypoTest()
        {
            const string source = "enum Appartment {}";
            TypoTest<EnumDeclarationSyntax>(new TypoRefactoring(), source, "enum Apartment{}");
        }

        [TestMethod]
        public void EnumNameTypoTest()
        {
            const string source = "enum AppartmentRoom {}";
            TypoTest<EnumDeclarationSyntax>(new TypoRefactoring(), source, "enum ApartmentRoom{}");
        }

        [TestMethod]
        public void EnumUnerlineNameTypoTest()
        {
            const string source = "enum Appartment_Room {}";
            TypoTest<EnumDeclarationSyntax>(new TypoRefactoring(), source, "enum Apartment_Room{}");
        }

        [TestMethod]
        public void SimpleClassNameTypoTest()
        {
            const string source = "class Appartment {}";
            TypoTest<ClassDeclarationSyntax>(new TypoRefactoring(), source, "class Apartment{}");
        }

        [TestMethod]
        public void ClassNameTypoTest()
        {
            const string source = "class AppartmentRoom {}";
            TypoTest<ClassDeclarationSyntax>(new TypoRefactoring(), source, "class ApartmentRoom{}");
        }

        [TestMethod]
        public void ClassUnderlineNameTypoTest()
        {
            const string source = "class Appartment_Room {}";
            TypoTest<ClassDeclarationSyntax>(new TypoRefactoring(), source, "class Apartment_Room{}");
        }

        [TestMethod]
        public void SimplePropertyNameTypoTest()
        {
            const string source = "" +
                                  "class Apartment {" +
                                  " private int Rooom{ get; set; }" +
                                  "}";
            TypoTest<PropertyDeclarationSyntax>(new TypoRefactoring(), source, "private int Room{ get; set; }");
        }

        [TestMethod]
        public void PropertyNameTypoTest()
        {
            const string source = "" +
                                  "class Apartment {" +
                                  " private int RooomCount { get; set; }" +
                                  "}";
            TypoTest<PropertyDeclarationSyntax>(new TypoRefactoring(), source, "private int RoomCount{ get; set; }");
        }

        [TestMethod]
        public void SimpleUnderlinedPropertyNameTypoTest()
        {
            const string source = "class Apartment {" +
                                  " private int _Room{ get; set; }" +
                                  "}";
            TypoTest<PropertyDeclarationSyntax>(new TypoRefactoring(), source, string.Empty);
        }

        [TestMethod]
        public void UnderlinedPropertyNameTypoTest()
        {
            const string source = "class Apartment {" +
                                  " private int _RoomCount { get; set; }" +
                                  "}";
            TypoTest<PropertyDeclarationSyntax>(new TypoRefactoring(), source, string.Empty);
        }

        [TestMethod]
        public void SimpleLinkedPropertyFieldTypoTest()
        {
            const string source = "class Apartment {" +
                                  "private int _rooom;" +
                                  "private int RoomCount { get { return _rooom; } set{ _rooom=value; } }" +
                                  "}";
            TypoTest<FieldDeclarationSyntax>(new TypoRefactoring(), source, "private int _room;");
        }

        [TestMethod]
        public void LinkedPropertyFieldTypoTest()
        {
            const string source = "" +
                                  "class Apartment {" +
                                  "private int _rooomCount;" +
                                  "private int RoomCount { get { return _rooomCount; } set{ _rooomCount=value; } }" +
                                  "}";
            TypoTest<FieldDeclarationSyntax>(new TypoRefactoring(), source, "private int _roomCount;");
        }
        
        [TestMethod]
        public void SimpleFieldNameTypoTest()
        {
            const string source = "" +
                                  "class Apartment {" +
                                  " private int rooom;" +
                                  "}";
            TypoTest<VariableDeclaratorSyntax>(new TypoRefactoring(), source, "room");
        }

        [TestMethod]
        public void FieldNameTypoTest()
        {
            const string source = "" +
                                  "class Apartment {" +
                                  " private int rooomCount;" +
                                  "}";
            TypoTest<VariableDeclaratorSyntax>(new TypoRefactoring(), source, "roomCount");
        }

        [TestMethod]
        public void UnderlinedFieldNameTypoTest()
        {
            const string source = "" +
                                  "class Apartment {" +
                                  " private int _rooomCount;" +
                                  "}";
            TypoTest<VariableDeclaratorSyntax>(new TypoRefactoring(), source, "_roomCount");
        }

        [TestMethod]
        public void UnderlinedDoubleFieldNameTypoTest()
        {
            const string source = "" +
                                  "class Apartment {" +
                                  " private int _rooom_Count;" +
                                  "}";
            TypoTest<VariableDeclaratorSyntax>(new TypoRefactoring(), source, "_room_Count");
        }

        [TestMethod]
        public void SimpleInterfaceNameTypoTest()
        {
            const string source = "interface IAppartment {}";
            TypoTest<InterfaceDeclarationSyntax>(new TypoRefactoring(), source, "interface IApartment{}");
        }

        [TestMethod]
        public void InterfaceNameTypoTest()
        {
            const string source = "interface IAppartmentRoom {}";
            TypoTest<InterfaceDeclarationSyntax>(new TypoRefactoring(), source, "interface IApartmentRoom{}");
        }

        [TestMethod]
        public void InterfaceUnderlineNameTypoTest()
        {
            const string source = "interface IAppartment_Room {}";
            TypoTest<InterfaceDeclarationSyntax>(new TypoRefactoring(), source, "interface IApartment_Room{}");
        }
        
        [TestMethod]
        public void SimpleMethodNameTypoTest()
        {
            const string source = "class Apartment {" +
                                  " private void Rooom(){}" +
                                  "}";
            TypoTest<MethodDeclarationSyntax>(new TypoRefactoring(), source, "private void Room(){}");
        }

        [TestMethod]
        public void MethodNameTypoTest()
        {
            const string source = "class Apartment {" +
                                  " private void RooomCount(){}" +
                                  "}";
            TypoTest<MethodDeclarationSyntax>(new TypoRefactoring(), source, "private void RoomCount(){}");
        }

        [TestMethod]
        public void MethodUnderlineNameTypoTest()
        {
            const string source = "class Apartment {" +
                                  " private void Rooom_Count(){}" +
                                  "}";
            TypoTest<MethodDeclarationSyntax>(new TypoRefactoring(), source, "private void Room_Count(){}");
        }

        [TestMethod]
        public void OneCharacterWordTypoTest()
        {
            const string source = "class Apartment {" +
                                  "   private void XYBurnDown(){}" +
                                  "}";
            TypoTest<MethodDeclarationSyntax>(new TypoRefactoring(), source, string.Empty);
        }
    }
}
