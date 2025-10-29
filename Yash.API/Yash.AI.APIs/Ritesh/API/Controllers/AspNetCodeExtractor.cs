using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yash.BusinessLogicExtractor;

namespace Yash.CustomTool.API.Ritesh.Controllers
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

        public string ExtractFilesStructure(string projectPath)
        {

            string projectFiles = "";
            string Summary = "Please select the Proper Technology";


            string folderPath = projectPath;//     
            try
            {


               
                string rootPath = projectPath;
                if (!Directory.Exists(rootPath))
                {
                    Console.WriteLine("Invalid path.");
                    return "";
                }



                Summary = "";
                var aspxPages = Directory.GetFiles(rootPath, "*.aspx", SearchOption.AllDirectories);
                var codeBehind = Directory.GetFiles(rootPath, "*.aspx.cs", SearchOption.AllDirectories);
                var classes = Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories)
                                       .Where(f => !f.EndsWith(".aspx.cs"))
                                       .ToArray();
                var configs = Directory.GetFiles(rootPath, "*.config", SearchOption.AllDirectories);

                Summary = Summary + $"UI Pages (.aspx): {aspxPages.Length}";
                foreach (var file in aspxPages)
                    Summary = Summary + (" - " + Path.GetFileName(file));

                Summary = Summary + ($"\nCode-behind (.aspx.cs): {codeBehind.Length}");
                foreach (var file in codeBehind)
                    Summary = Summary + (" - " + Path.GetFileName(file));

                Summary = Summary + ($"\nBusiness/Data Classes (.cs): {classes.Length}");
                foreach (var file in classes)
                    Summary = Summary + (" - " + Path.GetFileName(file));

                Summary = Summary + ($"\nConfig Files: {configs.Length}");
                foreach (var file in configs)
                    Summary = Summary + (" - " + Path.GetFileName(file));


                projectFiles = Summary;

            }
            catch (Exception ex)
            {
                projectFiles = ex.ToString();

            }


            return projectFiles;
        }

    }
}
