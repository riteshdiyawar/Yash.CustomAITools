using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Yash.BusinessLogicExtractor
{
    public static class DLLExtract
    {
        public static void GetAllMethodFromDLL(string dllpath)
        {




            string projectPath = dllpath;
            string outputPath = @"E:\MethodList.txt"; // 🔁 Output file path

            var csFiles = Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories);
            var outputLines = new List<string>();

            foreach (var file in csFiles)
            {
                var code = File.ReadAllText(file);
                var tree = CSharpSyntaxTree.ParseText(code);
                var root = tree.GetRoot();

                var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var classDecl in classDeclarations)
                {
                    string className = classDecl.Identifier.Text;
                    var methods = classDecl.DescendantNodes().OfType<MethodDeclarationSyntax>();

                    foreach (var method in methods)
                    {
                        string methodName = method.Identifier.Text;
                        outputLines.Add($"{className}\t{methodName}");
                    }
                }
            }

            File.WriteAllLines(outputPath, outputLines);
            Console.WriteLine($"Method list written to: {outputPath}");
        }
    }









    public static class MethodCounter
    {
        public static int CountAllMethods(string assemblyPath)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath); int totalMethodCount = 0;
            foreach (Type type in assembly.GetTypes()) { totalMethodCount += type.GetMethods().Length; }
            return totalMethodCount;
        }

    }

}
