using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Yash.BusinessLogicExtractor
{
       
 
        public class BusinessLogicExtractor
        {
            public string ExtractBusinessLogic(string methodCode)
            {
                // Parse the method code
                var tree = CSharpSyntaxTree.ParseText(methodCode);
                var root = tree.GetRoot() as CompilationUnitSyntax;

                // Find the method declaration
                var method = root.DescendantNodes().OfType<MethodDeclarationSyntax>().FirstOrDefault();
                if (method == null)
                {
                    throw new InvalidOperationException("No method found in the provided code.");
                }

                // Extract business logic (for simplicity, assume all statements are business logic)
                //Console.WriteLine("Method Name: " + method.Identifier.Text);
                //Console.WriteLine("Method Name: " + method.Body.Statements);


                if (method != null)
                {
                    Console.WriteLine("Method Name: " + method.Identifier.Text);
                    Console.WriteLine("Business Logic:");

                    foreach (var statement in method.Body.Statements)
                    {
                        Console.WriteLine(statement.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("No method found in the provided code.");
                }

                var businessLogicStatements = method.Body.Statements;

                // Create a new class for the business logic
                var businessLogicClass = SyntaxFactory.ClassDeclaration("BusinessLogic")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddMembers(
                        SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "Execute")
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .WithBody(SyntaxFactory.Block(businessLogicStatements))
                    );

                // Generate the new class code
                var newRoot = root.AddMembers(businessLogicClass);
                return newRoot.NormalizeWhitespace().ToFullString();

            }


            public string ExtractBusinessMethods(string methodCode)
            {
                string code = methodCode;// File.ReadAllText(filePath); // Read the content of the file
                SyntaxTree tree = CSharpSyntaxTree.ParseText(code); // Parse the code into a syntax tree
                CompilationUnitSyntax root = tree.GetCompilationUnitRoot(); // Get the root of the tree

                // Find all method declarations in the syntax tree
                var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                string allMethods = "";
                foreach (var methodDeclaration in methodDeclarations)
                {
                    // Extract method information
                    //string methodName = methodDeclaration.Identifier.Text;
                    //string returnType = methodDeclaration.ReturnType.ToString();
                    //string methodBody = methodDeclaration.Body.ToString();

                    //Console.WriteLine($"Method Name: {methodName}");
                    //Console.WriteLine($"Return Type: {returnType}");
                    //Console.WriteLine($"Method Body: {methodBody}");
                    //Console.WriteLine("--------------------");

                    allMethods = methodDeclaration.ToFullString() + "\n\r" + allMethods;


                }
                return allMethods;
            }
        }
     
}
