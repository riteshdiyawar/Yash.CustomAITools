
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using YashCustomToolRitesh;

namespace Yash.BusinessLogicExtractor
{


    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class RequestBody
    {
        public string model { get; set; }
        public Message[] messages { get; set; }
    }


    public class AIClass
    {
        public string _ProjectPath = "";
        public string _ProjectTechnologyType = "";
        public string _ProjectDatabaseConnection = "";
        private IEnumerable<string> filePaths;

        public AIClass()
        {
        }
        public AIClass(string ProjectPath, string ProjectTechnologyType, string ProjectDatabaseConnection)
        {
            _ProjectPath = ProjectPath;
            _ProjectTechnologyType = ProjectTechnologyType;
            _ProjectDatabaseConnection = ProjectDatabaseConnection;
        }

        #region Common
        public async Task<string> consumeAPIAsync(string FileName, RequestBody requestBody)
        {
            string responseString = "";
            string responseContent = "";
            try
            {

                var key = @"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";
                var endpoint = "https://api.openai.com/v1/chat/completions";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

                var response = await httpClient.PostAsJsonAsync(endpoint, requestBody);
                responseContent = await response.Content.ReadAsStringAsync();


                using var doc = JsonDocument.Parse(responseContent);
                responseString = doc.RootElement
                             .GetProperty("choices")[0]
                             .GetProperty("message")
                             .GetProperty("content")
                             .GetString();
                //var imageUrl = doc.RootElement.GetProperty("image_url");// ("image_url", out var imageElement) ? imageElement.GetString() : null;
                return responseString;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                responseString = responseContent + ex.ToString();
            }

            return responseString;
        }

        public void SaveDocuemnt(string Content = "TEST", string FileName = "TEST", string FileType = "MD")
        {


            string path = @"E:\Yash\Yash.BusinessLogicExtractor\OutputFiles\";
            string filePath = path + FileName + DateTime.Now.ToString("yyyyMMdd") + ".md";

            if (FileType == "TXT")
            {
            }
            else if (FileType == "MD")
            {
                CreateMarkdownFile(filePath, Content);
            }
            else if (FileType == "DOC")
            {
            }

            Console.WriteLine("File has been created...");

        }

        public void CreateDocument()
        {
            //// Create a new WordprocessingDocument
            //using (WordprocessingDocument document = WordprocessingDocument.Create(filePath,
            //    WordprocessingDocumentType.Document))
            //{
            //    // Add a MainDocumentPart to the document
            //    MainDocumentPart mainPart = document.AddMainDocumentPart();

            //    // Add a Document element to the MainDocumentPart
            //    Document doc = new Document();
            //    mainPart.Document = doc;

            //    // Add a Body element to the document
            //    Body body = new Body();
            //    doc.Body = body;

            //    // Add a Paragraph to the body
            //    Paragraph paragraph = new Paragraph();
            //    body.Append(paragraph);

            //    // Add a Run to the paragraph
            //    Run run = new Run();
            //    paragraph.Append(run);

            //    // Add Text to the run
            //    Text text = new Text(Document);
            //    run.Append(text);

            //    // Save the document
            //    document.Save();
            //}
        }
        public void CreateMarkdownFile(string filePath, string content)
        {
            try
            {
                // Use WriteAllText to create or overwrite the file and write content
                File.WriteAllText(filePath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        #endregion

        #region Ritesh

        public async Task<string> GetProjectDiagram(string projectPath, string TechnologyType)
        {
            //read the files 

            string folderPath = projectPath;
            AIClass aIClass = new AIClass();
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

                if (TechnologyType.ToLower() == "AspxNet".ToLower())
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
                }

                string Prompt = "I am Software Architect, i want to generate high-level conceptual architecture diagram for a Web Forms project.Based on the data";

                var requestBody = new RequestBody
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                        {
                            new Message { role = "system", content = "You are a helpful assistant." },
                            new Message { role = "user", content = Prompt + Environment.NewLine+ Summary  },
                            //new Message { role = "user", content = "Please give only summary." }
                            }
                };
                // pass to AI 
                string FilesSummary = await aIClass.consumeAPIAsync("projectFileName", requestBody); ;


                return FilesSummary;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing folder {folderPath}: ex.Message");
            }

            return "";
            // generate summary 

        }
        public async Task<string> GetCodeImprovement(string projectPath, string TechnologyType)
        {
            string FilesSummary = "";
            string folderPath = projectPath;// "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\"; // Replace with the actual folder path
            AIClass aIClass = new AIClass();
            try
            {
                // Get all file paths in the folder
                CodeExtractor businessLogicExtractor = new CodeExtractor();

                if (TechnologyType.ToLower() == "AspxNet".ToLower())
                {

                    // Get all .cs and .aspx files
                    filePaths = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                       .Where(file => file.EndsWith(".cs") || file.EndsWith(".aspx"))
                                       .ToArray();
                }

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



                #region "Calling Open AI"
                string Prompt = "Summarize key method-level improvements in the code briefly.";

                var requestBody = new RequestBody
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                        {
                            new Message { role = "system", content = "You are a helpful assistant." },
                            new Message { role = "user", content = Prompt + Environment.NewLine+ allCode  },
                            //new Message { role = "user", content = "Please give only summary." }
                            }
                };
                // pass to AI 
                FilesSummary = await aIClass.consumeAPIAsync("projectFileName", requestBody); ;


                #endregion
            }
            catch (Exception ex)
            {
                FilesSummary = ex.ToString();
            }

            return FilesSummary;
        }

        public async Task<string> GetProjectDetails(string projectPath)
        {
            //read the files 
            string finaloutput = "";
            string folderPath = projectPath;// "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\"; // Replace with the actual folder path
            AIClass aIClass = new AIClass();
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



                #region "Calling Open AI"

                string prompt = @"Summarize the following ASP.NET Web Forms code into a business-oriented description suitable for non-technical stakeholders.";
                var requestBody = new RequestBody
                {
                    model = "gpt -4o-mini",
                    messages = new[]
                                     {
                            new Message { role = "system", content = "You are a helpful assistant." },
                            new Message { role = "user", content = prompt + Environment.NewLine+ allCodes  },
                    }
                };

                //Here are summaries of different modules of a Web Forms project.Please combine them into a single, cohesive business - oriented overview.
                //finaloutput = await aIClass.consumeAPIAsync("projectFileName", requestBody);
                return finaloutput;

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing folder {folderPath}: {ex.Message}");
            }
            return "";         
        }



        public async Task<char[]> GetProjectFeatureDetail(string projectLocation)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Praveen

        public async Task<string> GetClassDiagram(string projectPath)
        {
            //read the files 

            string folderPath = projectPath;
            AIClass aIClass = new AIClass();
            try
            {


                Console.WriteLine("Enter the root path of your ASP.NET project:");
                string rootPath = projectPath;

                if (!Directory.Exists(rootPath))
                {
                    Console.WriteLine("Invalid path.");
                    return "";
                }

                string Summary = "";
                Console.WriteLine("\n📁 Project Structure Summary:\n");

                var aspxPages = Directory.GetFiles(rootPath, "*.aspx", SearchOption.AllDirectories);
                var codeBehind = Directory.GetFiles(rootPath, "*.aspx.cs", SearchOption.AllDirectories);
                var classes = Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories)
                                       .Where(f => !f.EndsWith(".aspx.cs"))
                                       .ToArray();
                var configs = Directory.GetFiles(rootPath, "*.config", SearchOption.AllDirectories);

                //Summary = Summary + $"UI Pages (.aspx): {aspxPages.Length}";
                //foreach (var file in aspxPages)
                //    Summary = Summary + (" - " + Path.GetFileName(file));

                //Summary = Summary + ($"\nCode-behind (.aspx.cs): {codeBehind.Length}");
                //foreach (var file in codeBehind)
                //    Summary = Summary + (" - " + Path.GetFileName(file));

                Summary = Summary + ($"\nBusiness/Data Classes (.cs): {classes.Length}");
                foreach (var file in classes)
                    Summary = Summary + (" - " + Path.GetFileName(file));

                //Summary = Summary + ($"\nConfig Files: {configs.Length}");
                //foreach (var file in configs)
                //    Summary = Summary + (" - " + Path.GetFileName(file));

                string Prompt = "Please  generate the Class Diagram (Conceptual)  high-level based on the file structure for below content";

                //string Prompt =  ("\n Please  generate the Architecture Diagram (Conceptual)  for me in MS word for below content!");

                var requestBody = new RequestBody
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                        {
                            new Message { role = "system", content = "You are a helpful assistant." },
                            new Message { role = "user", content = Prompt + Environment.NewLine+ Summary  },
                            //new Message { role = "user", content = "Please give only summary." }
                            }
                };
                // pass to AI 
                string singleFileSummary = "";


                singleFileSummary = await aIClass.consumeAPIAsync("projectFileName", requestBody);
                return singleFileSummary;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing folder {folderPath}: ex.Message");
            }

            return "";
            // generate summary 

        }

        internal async Task<char[]> GetUnitTestGenerator(string projectLocation)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
