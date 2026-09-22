using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yash.BusinessLogicExtractor;
using Yash.CustomTool.API.Ritesh.Model;
using YashCustomToolRitesh;

namespace Yash.CustomTool.API.Ritesh.BL
{
    public class AspNetCodeExtractor
    {
        private IEnumerable<string> filePaths;
        public AspNetCodeExtractor()
        {
        }

        public string ExtractCode(string projectPath)
        {

            string FilesSummary = "";
            string folderPath = projectPath;// 
            string allMethodsCodes = "";
            string ConsolidatefileSummary = "";
            try
            {



                // Get all .cs and .aspx files
                filePaths = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                   .Where(file => file.EndsWith(".cs")) //|| file.EndsWith(".aspx")
                                   .ToArray();

                foreach (string filePath in filePaths)
                {

                    try
                    {
                        // Read the content of the file
                        string fileContent = File.ReadAllText(filePath);
                        //string projectFileName = Path.GetFileName(filePath);

                        allMethodsCodes = allMethodsCodes + Environment.NewLine + fileContent;
                        // Process the file content (e.g., print it)

                    }
                    catch (Exception ex)
                    {
                        FilesSummary = ex.Message;
                    }
                }
                FilesSummary = allMethodsCodes;
            }
            catch (Exception ex)
            {
                FilesSummary = ex.Message;
            }

            return FilesSummary;
        }

        internal string GetAllFileName(string projectPath)
        {
            string folderPath = projectPath;
            //AIClass aIClass = new AIClass();
            try
            {


                Console.WriteLine("Enter the root path of your ASP.NET project:");
                string rootPath = projectPath;

                if (!Directory.Exists(rootPath))
                {
                    Console.WriteLine("Invalid path.");
                    return "";
                }

                string Summary = "Please select the Proper Technology";
                Console.WriteLine("\n📁 Project Structure Summary:\n");

                //if (TechnologyType.ToLower() == "AspxNet".ToLower())
                {
                    Summary = "";
                    var aspxPages = Directory.GetFiles(rootPath, "*.aspx", SearchOption.AllDirectories);
                    var codeBehind = Directory.GetFiles(rootPath, "*.aspx.cs", SearchOption.AllDirectories);
                    var classes = Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories)
                                           .Where(f => !f.EndsWith(".aspx.cs"))
                                           .ToArray();
                    var configs = Directory.GetFiles(rootPath, "*.config", SearchOption.AllDirectories);

                    Summary = Summary + $"UI Pages (.aspx): {aspxPages.Length}";
                    foreach (var file in aspxPages)
                        Summary = Summary + " - " + Path.GetFileName(file);

                    Summary = Summary + $"\nCode-behind (.aspx.cs): {codeBehind.Length}";
                    foreach (var file in codeBehind)
                        Summary = Summary + " - " + Path.GetFileName(file);

                    Summary = Summary + $"\nBusiness/Data Classes (.cs): {classes.Length}";
                    foreach (var file in classes)
                        Summary = Summary + " - " + Path.GetFileName(file);

                    Summary = Summary + $"\nConfig Files: {configs.Length}";
                    foreach (var file in configs)
                        Summary = Summary + " - " + Path.GetFileName(file);
                }



                //var requestBody = new RequestBody
                //{
                //    model = "gpt-4o-mini",
                //    messages = new[]
                //        {
                //            new Message { role = "system", content = "You are a helpful assistant." },
                //            new Message { role = "user", content = Prompt + Environment.NewLine+ Summary  },
                //            //new Message { role = "user", content = "Please give only summary." }
                //            }
                //};
                // pass to AI 
                //string FilesSummary = await AIHelper.consumeAPIAsync("projectFileName", requestBody); ;


                return Summary;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing folder {folderPath}: ex.Message");
                return ex.Message.ToString();
            }

        }

        internal string GetCodeWebFormControls(string projectPath)
        {
            string rootPath = projectPath;


            // Get all .cs and .aspx files
            filePaths = Directory.GetFiles(projectPath, "*.*", SearchOption.AllDirectories)
                               .Where(file => file.EndsWith(".aspx"))
                               .ToArray();


            //string[] filePaths = Directory.GetFiles(folderPath);
            string allCode = "";

            foreach (string filePath in filePaths)
            {

                try
                {

                    // Read the content of the file
                    string fileContent = File.ReadAllText(filePath);
                    string projectFileName = Path.GetFileName(filePath);

                    allCode = allCode + Environment.NewLine + fileContent;
                    // Process the file content (e.g., print it)

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                }
            }
            return allCode;

        }
        internal string GetProjectCode(string projectPath)
        {
            //read the files 
            string finaloutput = "";
            string folderPath = projectPath;// "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\"; // Replace with the actual folder path
            //AIClass aIClass = new AIClass();
            try
            {
                // Get all file paths in the folder
                CodeExtractor businessLogicExtractor = new CodeExtractor();
                // Get all .cs and .aspx files
                var filePaths = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                         .Where(
                                        file => file.EndsWith(".cs") ||
                                        file.EndsWith(".aspx") ||
                                        file.EndsWith(".config")

                                        )
                                         .ToArray();
                string allCodes = "";
                foreach (string filePath in filePaths)
                {
                    try
                    {                        // Read the content of the file
                        string fileContent = File.ReadAllText(filePath);
                        allCodes = allCodes + Environment.NewLine + "File Name: " + Path.GetFileName(filePath) + Environment.NewLine + fileContent;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                    }
                }


                return allCodes;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing folder {folderPath}: {ex.Message}");
            }
            return "";
        }


        internal string GetPageLevelCode(string projectPath)
        {
            //read the files             
            string folderPath = projectPath;// "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\"; // Replace with the actual folder path
            //AIClass aIClass = new AIClass();
            try
            {
                // Get all file paths in the folder
                CodeExtractor businessLogicExtractor = new CodeExtractor();
                // Get all .cs and .aspx files
                var filePaths = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                         .Where(
                                        file => file.EndsWith(".cs") || file.EndsWith(".aspx")


                                        )
                                         .ToArray();
                string allCodes = "";
                foreach (string filePath in filePaths)
                {
                    try
                    {                        // Read the content of the file
                        string fileContent = File.ReadAllText(filePath);
                        allCodes = allCodes + Environment.NewLine + "File Name: " + Path.GetFileName(filePath) + Environment.NewLine + fileContent;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                    }
                }


                return allCodes;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing folder {folderPath}: {ex.Message}");
            }
            return "";
        }


    }
}
