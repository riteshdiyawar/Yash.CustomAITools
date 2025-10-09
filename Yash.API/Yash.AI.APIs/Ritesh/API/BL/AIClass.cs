using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

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
        public async Task<string> consumeAPIAsync(string FileName, RequestBody requestBody)
        {

            try
            {
                //"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"

                var K = ""; /*@"sk-proj-UsfKrHWDAo5HeypN-gaRP6eFpzquWu-0sy8pz62Y5KME-9_gLs0OziaUQmvRU0Fgy1GydLyQHT3BlbkFJLnoZupaNxpE4aKRrOOIJUoqoAYJ2EdiRoakQ0n3_MV8h5QIKzM5-K8JaAKJNRwHYk_z4JQfqsA";
*/
                var endpoint = "https://api.openai.com/v1/chat/completions";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

                //var requestBody = new
                //{
                //    //model = "gpt-4", // or "gpt-3.5-turbo"
                //    //model = "gpt-3.5-turbo",
                //    model = "gpt-4o-mini",


                //    messages = new[]
                //    {
                //new { role = "system", content = "You are a helpful assistant." },
                //new { role = "user", content = "Explain business logic summary provided code in C#. "+ Environment.NewLine + Code  },
                //new { role = "user", content = "Please give only summary." }
                //    }
                //};

                var response = await httpClient.PostAsJsonAsync(endpoint, requestBody);
                var responseContent = await response.Content.ReadAsStringAsync();


                using var doc = JsonDocument.Parse(responseContent);
                var reply = doc.RootElement
                               .GetProperty("choices")[0]
                               .GetProperty("message")
                               .GetProperty("content")
                               .GetString();

                return reply;
                //Second Command 

                /*

                                var requestBody2 = new
                                {
                                    //model = "gpt-4", // or "gpt-3.5-turbo"
                                    //model = "gpt-3.5-turbo",
                                    model = "gpt-4o-mini",


                                    messages = new[]
                                    {
                                        new { role = "system", content = "You are a helpful assistant." },
                                        new { role = "user", content = "Please give only summary." }
                                    }
                                };


                                var response2 = await httpClient.PostAsJsonAsync(endpoint, requestBody2);
                                var responseContent2 = await response.Content.ReadAsStringAsync();


                                using var doc2 = JsonDocument.Parse(responseContent2);
                                var reply2 = doc2.RootElement
                                               .GetProperty("choices")[0]
                                               .GetProperty("message")
                                               .GetProperty("content")
                                               .GetString();


                                Console.WriteLine("Copilot says:\n" + reply);
                */
                //   SaveDocuemnt(reply, FileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return "";
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
        public async Task<string> GetProjectDiagram(string projectPath)
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

                string Prompt = "Please  generate the Architecture Diagram (Conceptual)  high-level breakdown of  Web Forms project architecture based on the file structure for below content";

                //string Prompt =  ("\n Please  generate the Architecture Diagram (Conceptual)  for me in MS word for below content!");

                var requestBody = new RequestBody
                {
                    model = "gpt-3.5-turbo" , //"gpt-4o-mini",
                    messages = new[]
                        {
                            new Message { role = "system", content = "You are a helpful assistant." },
                            new Message { role = "role", content = Prompt + Environment.NewLine+ Summary  },
                            //new Message { role = "user", content = "Please give only summary." }
                            }
                };
                // pass to AI 
                string singleFileSummary = "";


                singleFileSummary = await aIClass.consumeAPIAsync("projectFileName", requestBody);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing folder {folderPath}: ex.Message");
            }

            return "";
            // generate summary 

        }
        public async Task<string> ProcessCodeFiles(string projectPath)
        {
            //read the files 

            string folderPath = projectPath;// "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\"; // Replace with the actual folder path
            AIClass aIClass = new AIClass();
            try
            {
                // Get all file paths in the folder
                BusinessLogicExtractor businessLogicExtractor = new BusinessLogicExtractor();
                string[] filePaths = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);
                //string[] filePaths = Directory.GetFiles(folderPath);
                string allMethodsCodes = "";
                string ConsolidatefileSummary = "";
                foreach (string filePath in filePaths)
                {

                    try
                    {
                        string singleFileSummary = "";

                        // Read the content of the file
                        string fileContent = File.ReadAllText(filePath);
                        string projectFileName = Path.GetFileName(filePath);

                        allMethodsCodes = businessLogicExtractor.ExtractBusinessMethods(fileContent);
                        // Process the file content (e.g., print it)



                        var requestBody = new RequestBody
                        {
                            model = "gpt-4o-mini",
                            messages = new[]
                         {
                            new Message { role = "system", content = "You are a helpful assistant." },
                            new Message { role = "user", content = "Explain business logic summary provided code in C#. " + Environment.NewLine+ allMethodsCodes + "Code" },
                            new Message { role = "user", content = "Please give only summary." }
                            }
                        };
                        // pass to AI 



                        singleFileSummary = await aIClass.consumeAPIAsync(projectFileName, requestBody);

                        // combine the result 
                        ConsolidatefileSummary = singleFileSummary + ConsolidatefileSummary;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                    }
                }


                //                Great! Please upload your ASP.NET Web Forms project files(or a ZIP of the solution folder).Once I have the files, I’ll:

                //                Analyze the structure of your.aspx, .cs, and other relevant files.
                //Identify key components like UI pages, business logic, data access layers, and configuration.
                //Generate a clean architecture diagram that visually represents how everything connects.

                // pass combine result

                var requestBody1 = new RequestBody
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                       {
                            new Message { role = "system", content = "You are a helpful assistant." },
                            new Message { role = "user", content = "do the analysis and provide Project description and Business Logic also"+ConsolidatefileSummary + Environment.NewLine  },
                            //new Message { role = "user", content = "Please give only summary." }
                            }
                };
                // pass to AI 


                var finaloutput = await aIClass.consumeAPIAsync(ConsolidatefileSummary, requestBody1);

                return finaloutput;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing folder {folderPath}: {ex.Message}");
            }

            return "";



            // generate summary 

        }
    }
}
