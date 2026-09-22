using Azure;
using Azure.AI.OpenAI;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
using SharpToken;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yash.BusinessLogicExtractor;
using Yash.CustomTool.API.Ritesh.BL;
using Yash.CustomTool.API.Ritesh.Model;


namespace YashCustomToolRitesh
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class YashCustomToolController : ControllerBase

    {
        private readonly AzureOpenAIClient _openAiClient;
        private readonly string _deploymentName;

        private readonly IConfiguration _configuration;

        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<YashCustomToolController> _logger;

        //public YashCustomToolController(IOptions<AppSettings> settings)
        //{
        //    _appSettings = settings;
        //}


        //private string endpoint = "https://az-adgm-myadgm-azureopenai.openai.azure.com";
        //private string apiKey = "5861ce85085f4f2285d2cd1dc1e7f62b";
       
        // Create OpenAI Client
          


        public YashCustomToolController(IConfiguration configuration)
        {
            _configuration = configuration;
            _openAiClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        }


        #region Ritesh

        [HttpGet("GetControlInfo")]
        public async Task<IActionResult> GetControlInfo(string ProjectPath = "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\Data", string ProjectTechnologyType = "ASPXNET", string DatabaseConnection = "")
        {
            string projectCode = "";

            if (ProjectTechnologyType.ToUpper() == "ASPXNET")
            {
                var extractor = new AspNetCodeExtractor();
                projectCode = extractor.GetCodeWebFormControls(ProjectPath);
            }
            else if (ProjectTechnologyType.ToUpper() == "ANGULAR")
            {
                var extractor = new AngularCodeExtractor();
                projectCode = extractor.GetCodeWebFormControls(ProjectPath);
            }
            else
            {
                return BadRequest("Unsupported Project Technology Type.");
            }

            string AIrequest = AIHelper.OPEN_AI_Prompt_GetControls + Environment.NewLine + projectCode;

            YashOpenAIService yashOpenAIService = new YashOpenAIService();

            var openAiResponse = await yashOpenAIService.AnalyzePromptAsync(AIrequest);

            openAiResponse = openAiResponse.Replace("```json", "").Replace("```", "");

            return Content(openAiResponse, "application/json");

            



        }

        [HttpGet("GetCodeImprovement")]
        public async Task<IActionResult> GetCodeImprovement
            (string ProjectPath = "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode", string ProjectTechnologyType = "ASPXNET", string DatabaseConnection = "")
        {
            string projectCode = "";

            if (ProjectTechnologyType.ToUpper() == "ASPXNET")
            {
                var extractor = new AspNetCodeExtractor();
                projectCode = extractor.ExtractCode(ProjectPath);
            }
            else if (ProjectTechnologyType.ToUpper() == "ANGULAR")
            {
                var extractor = new AngularCodeExtractor();
                projectCode = extractor.ExtractCode(ProjectPath);
            }
            else
            {
                return BadRequest("Unsupported Project Technology Type.");
            }

            string AIrequest = AIHelper.OPEN_AI_Prompt_GetCodeImprovementSummary + Environment.NewLine + projectCode;

            YashOpenAIService yashOpenAIService = new YashOpenAIService();

            var openAiResponse = await yashOpenAIService.AnalyzePromptAsync(AIrequest);

            openAiResponse = openAiResponse.Replace("```json", "").Replace("```", "");

            return Content(openAiResponse, "application/json");

            //return Content(GetMethodImprovmentList(), "application/json");

        }



        [HttpGet("GetCodeImprovementDetail")]
        public async Task<IActionResult> GetCodeImprovementDetail(string MethodName, string ProjectPath = "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode")
        {


            CodeExtractor codeExtractor = new CodeExtractor();
            string projectCode = codeExtractor.GetMethodBodyByMethodName(MethodName, ProjectPath);

            string AIrequest = AIHelper.OPEN_AI_Prompt_GetMethodImprovementDetail + Environment.NewLine + projectCode;

            YashOpenAIService yashOpenAIService = new YashOpenAIService();

            var openAiResponse = await yashOpenAIService.AnalyzePromptAsync(AIrequest);

            #region MD
            //Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(openAiResponse);

            //Set file name and content type
            string fileName = "Yash_CustomTools_CodeImprovement_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);
            #endregion

            //return Ok("");
        }



        [HttpGet("GetProjectDiagram")]
        public async Task<IActionResult> GetProjectDiagram
         (string ProjectPath = "E:\\Project Applied\\Anchor.SuretyPortal", string ProjectTechnologyType = "ASPXNET", string DatabaseConnection = "")
        {
            string projectCode = "";

            if (ProjectTechnologyType.ToUpper() == "ASPXNET")
            {
                var extractor = new AspNetCodeExtractor();
                projectCode = extractor.GetAllFileName(ProjectPath);
            }
            else if (ProjectTechnologyType.ToUpper() == "ANGULAR")
            {
                var extractor = new AngularCodeExtractor();
                projectCode = extractor.GetAllFileName(ProjectPath);
            }
            else
            {
                return BadRequest("Unsupported Project Technology Type.");
            }

            string AIrequest = AIHelper.OPEN_AI_Prompt_DesignArch + Environment.NewLine + projectCode;

            YashOpenAIService yashOpenAIService = new YashOpenAIService();

            var openAiResponse = await yashOpenAIService.AnalyzePromptAsync(AIrequest);

            //var openAiResponse = "";


            #region MD
            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(openAiResponse);

            // Set file name and content type
            string fileName = "Yash_CustomTools_ProjectDiagram_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);
            #endregion

        }




        [HttpGet("GetProjectFeatureDetail")]
        public async Task<IActionResult> GetProjectFeatureDetail(string ProjectPath = "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\FEATURE", string ProjectTechnologyType = "ASPXNET", string DatabaseConnection = "")
        {
            //AIClass aIClass = new AIClass(ProjectPath, ProjectTechnologyType, DatabaseConnection);



            //string projectCode = "";

            //if (ProjectTechnologyType.ToUpper() == "ASPXNET")
            //{
            //    var extractor = new AspNetCodeExtractor();
            //    projectCode = extractor.GetPageLevelCode(ProjectPath);
            //}
            //else if (ProjectTechnologyType.ToUpper() == "ANGULAR")
            //{
            //    var extractor = new AngularCodeExtractor();
            //    projectCode = extractor.GetPageCode(ProjectPath);
            //}
            //else
            //{
            //    return BadRequest("Unsupported Project Technology Type.");
            //}

            //string AIrequest = AIHelper.OPEN_AI_Prompt_PageLevelFeature + Environment.NewLine + projectCode;

            //YashOpenAIService yashOpenAIService = new YashOpenAIService();

            //var openAiResponse = await yashOpenAIService.AnalyzePromptAsync(AIrequest);


            //// Convert string to byte array
            //byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(openAiResponse);

            //// Set file name and content type
            //string fileName = "Yash_CustomTools_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            //string contentType = "application/octet-stream";


            var openAiResponse = await GenerateResponseFromQuestion(_openAiClient);

            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(openAiResponse);

            // Set file name and content type
            string fileName = "Yash_CustomTools_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);

            //return null;
        }




        [HttpGet("GetProjectDetails")]
        public async Task<IActionResult> GetProjectDetails
            (string ProjectPath = "E:\\Project Applied\\Anchor.SuretyPortal", string ProjectTechnologyType = "ASPX.NET", string DatabaseConnection = "")

        {
            string projectCode = "";

            if (ProjectTechnologyType.ToUpper() == "ASPXNET")
            {
                var extractor = new AspNetCodeExtractor();
                projectCode = extractor.GetProjectCode(ProjectPath);
            }
            else if (ProjectTechnologyType.ToUpper() == "ANGULAR")
            {
                var extractor = new AngularCodeExtractor();
                projectCode = extractor.GetProjectCode(ProjectPath);
            }
            else
            {
                return BadRequest("Unsupported Project Technology Type.");
            }

            string AIrequest = AIHelper.OPEN_AI_Prompt_DesignArch + Environment.NewLine + projectCode;

            YashOpenAIService yashOpenAIService = new YashOpenAIService();

            var openAiResponse = await yashOpenAIService.AnalyzePromptAsync(AIrequest);



            #region MD
            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(openAiResponse);

            // Set file name and content type
            string fileName = "Yash_CustomTools_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);
            #endregion




        }





        #endregion



        #region "Sample"

        [HttpGet("SampleMDFileDownload")]
        public IActionResult SampleMDFileDownload(string ProjectPath = "E:\\Project Applied\\Anchor.SuretyPortal", string ProjectTechnologyType = "ASPX.NET", string DatabaseConnection = "")
        {


            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes("fileContent");

            // Set file name and content type
            string fileName = "Yash_CustomTools_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);
        }
        [HttpGet("SampleDownload")]
        public IActionResult DownloadWordDocument()
        {
            var fileName = "SampleDocument.docx";

            var fileBytes = System.Text.Encoding.UTF8.GetBytes("System.IO.File.ReadAllBytes(filePath);");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }


        [HttpGet("DownloadGeneratedWordDoc")]
        public IActionResult DownloadGeneratedWordDoc()
        {
            byte[] wordBytes;

            using (var memStream = new MemoryStream())
            {
                using (var wordDoc = WordprocessingDocument.Create(memStream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                {
                    var mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = new Body();

                    // Add content
                    body.Append(new Paragraph(new Run(new Text("Hello Ritesh, this is your generated Word document!"))));
                    body.Append(new Paragraph(new Run(new Text("Generated on: " + DateTime.Now.ToString("f")))));

                    mainPart.Document.Append(body);
                    mainPart.Document.Save();
                }

                wordBytes = memStream.ToArray();
            }

            return File(wordBytes,
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        "GeneratedDocument.docx");
        }

        #endregion

        #region Private Method
        private byte[] GenerateWordDocument(string content)
        {
            using var memStream = new MemoryStream();
            using (var wordDoc = WordprocessingDocument.Create(memStream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
            {
                var mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = new Body();

                foreach (var line in content.Split('\n'))
                {
                    body.Append(new Paragraph(new Run(new Text(line.Trim()))));
                }

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }

            return memStream.ToArray();
        }



        private string GetControls()
        {
            string JSON = @"
[
    {
        ""ControlName"": ""asp:ScriptManager"",
        ""Count"": 1
    },
    {
        ""ControlName"": ""asp:HiddenField"",
        ""Count"": 11
    },
    {
        ""ControlName"": ""asp:Button"",
        ""Count"": 22
    },
    {
        ""ControlName"": ""asp:TextBox"",
        ""Count"": 35
    },
    {
        ""ControlName"": ""asp:RadioButton"",
        ""Count"": 8
    },
    {
        ""ControlName"": ""asp:CheckBox"",
        ""Count"": 24
    },
    {
        ""ControlName"": ""asp:DropDownList"",
        ""Count"": 23
    },
    {
        ""ControlName"": ""ajaxtoolkit:AutoCompleteExtender"",
        ""Count"": 1
    },
    {
        ""ControlName"": ""asp:UpdateProgress"",
        ""Count"": 1
    },
    {
        ""ControlName"": ""asp:UpdatePanel"",
        ""Count"": 1
    },
    {
        ""ControlName"": ""asp:Panel"",
        ""Count"": 4
    },
    {
        ""ControlName"": ""asp:Repeater"",
        ""Count"": 1
    },
    {
        ""ControlName"": ""asp:Label"",
        ""Count"": 33
    },
    {
        ""ControlName"": ""asp:GridView"",
        ""Count"": 3
    },
    {
        ""ControlName"": ""uc1:accountinfo"",
        ""Count"": 1
    }
]
";
            return JSON;
        }

        private string GetMethodImprovmentList()
        {
            string JSON = @"[
  {
    ""methodName"": ""GenerateSiriusInvoice"",
    ""severity"": ""High"",
    ""improvement"": ""Performance"",
    ""suggestion"": ""Avoid using exceptions for control flow. Replace `throw ex;` with proper error logging and rethrow to preserve stack trace.""
  },
  {
    ""methodName"": ""GenerateInvoiceWithoutSirius"",
    ""severity"": ""High"",
    ""improvement"": ""Performance"",
    ""suggestion"": ""Avoid using exceptions for control flow. Replace `throw ex;` with proper error logging and rethrow to preserve stack trace.""
  },
  {
    ""methodName"": ""SelectBondInvoiceDetailsContract"",
    ""severity"": ""Medium"",
    ""improvement"": ""Performance"",
    ""suggestion"": ""Consider using async/await with `SqlCommand` to improve scalability of database calls.""
  },
  {
    ""methodName"": ""SelectBondInvoiceDetails"",
    ""severity"": ""Medium"",
    ""improvement"": ""Performance"",
    ""suggestion"": ""Consider using async/await with `SqlCommand` to improve scalability of database calls.""
  },
  {
    ""methodName"": ""SelectBondInvoiceDetailsInternational"",
    ""severity"": ""Medium"",
    ""improvement"": ""Performance"",
    ""suggestion"": ""Consider using async/await with `SqlCommand` to improve scalability of database calls.""
  },
  {
    ""methodName"": ""Select"",
    ""severity"": ""High"",
    ""improvement"": ""Readability"",
    ""suggestion"": ""Reduce nested try-catch blocks and split into smaller methods to improve readability and maintainability.""
  },
  {
    ""methodName"": ""InsertInvoiceErrorLog"",
    ""severity"": ""Low"",
    ""improvement"": ""Error Handling"",
    ""suggestion"": ""Add error logging to capture exceptions instead of silently ignoring them.""
  },
  {
    ""methodName"": ""GetInvoiceRequestInfo"",
    ""severity"": ""Medium"",
    ""improvement"": ""Performance"",
    ""suggestion"": ""Consider using parameterized queries and async calls for database operations.""
  },
  {
    ""methodName"": ""GetAutomationCurrencyReport"",
    ""severity"": ""High"",
    ""improvement"": ""Structure"",
    ""suggestion"": ""Refactor to separate data access logic into a repository class and avoid mixing business logic with database operations.""
  },
  {
    ""methodName"": ""CreateBondTransactionLogRecord"",
    ""severity"": ""Medium"",
    ""improvement"": ""Structure"",
    ""suggestion"": ""Add proper error handling and logging for exceptions to improve reliability of the method.""
  },
  {
    ""methodName"": ""Insert"",
    ""severity"": ""Medium"",
    ""improvement"": ""Validation"",
    ""suggestion"": ""Add input validation to ensure parameters are sanitized before inserting into the database.""
  },
  {
    ""methodName"": ""SelectAll"",
    ""severity"": ""Low"",
    ""improvement"": ""Readability"",
    ""suggestion"": ""Add XML comments to explain the purpose and usage of the method.""
  },
  {
    ""methodName"": ""GetEmailDesc"",
    ""severity"": ""Low"",
    ""improvement"": ""Error Handling"",
    ""suggestion"": ""Ensure the returned data is checked for null or empty values before accessing to avoid runtime exceptions.""
  },
  {
    ""methodName"": ""GetEmails"",
    ""severity"": ""Low"",
    ""improvement"": ""Error Handling"",
    ""suggestion"": ""Add null checks for the dataset and ensure proper exception handling for database operations.""
  }
]
";
            return JSON;
        }
        #endregion


        // Function to Generate Response from Question
        static async Task<string>  GenerateResponseFromQuestion(AzureOpenAIClient client)
        {

            var deploymentName = "gpt-35-turbo-myadgm-dev";
            var encodingName = Model.GetEncodingNameForModel(deploymentName);
            var encoding = GptEncoding.GetEncodingForModel(deploymentName);


            var fileContent = string.Empty;

            string ProjectPath = "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\FEATURE";
            var extractor = new AspNetCodeExtractor();
            fileContent = extractor.GetPageLevelCode(ProjectPath);
            // Optionally, store the file content or split it into chunks if it's too large
            string[] chunks = SplitFileIntoChunks(fileContent, 10000); // 2000 characters per chunk, adjust as needed

            // Process each chunk and summerize the content
            List<string> summerizeContentList = new List<string>();

            foreach (var chunk in chunks)
            {
                var summerizeContent = await SummerizeContent(client, chunk, deploymentName);
                summerizeContentList.Add(summerizeContent);
            }

            // Combine the summerize contents from all chunks
            
            var summerizeContentFinal = string.Join("\n", summerizeContentList);

            // Ask Question on Summerized Content
            var finalResponse = await AskQuestion(client, summerizeContentFinal, AIHelper.OPEN_AI_Prompt_GenerateBRD, deploymentName);

            return finalResponse 
                + Environment.NewLine +"--------------------------------------Details----------------------------------------------------------------" + Environment.NewLine+ summerizeContentFinal;

        }

        // Split file content into chunks based on the character limit
        private static string[] SplitFileIntoChunks(string content, int maxChunkSize)
        {
            var chunks = new List<string>();
            var currentChunk = new StringBuilder();

            foreach (var word in content.Split(' '))
            {
                if (currentChunk.Length + word.Length > maxChunkSize)
                {
                    chunks.Add(currentChunk.ToString());
                    currentChunk.Clear();
                }
                currentChunk.Append(word + " ");
            }

            if (currentChunk.Length > 0)
            {
                chunks.Add(currentChunk.ToString());
            }

            return chunks.ToArray();
        }

        private static async Task<string> SummerizeContent(AzureOpenAIClient client, string chunk, string deploymentName)
        {
            try
            {
                string prompt = $"{chunk}\n\n"+ AIHelper.OPEN_AI_Prompt_PageLevelFeature;

                // Send the prompt to Azure OpenAI
                var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a skilled data analyst."),
                new UserChatMessage(prompt)
            };
                var chatClient = client.GetChatClient(deploymentName);
                var response = await chatClient.CompleteChatAsync(messages, new ChatCompletionOptions()
                {
                    Temperature = (float)0.7,
                    FrequencyPenalty = (float)0,
                    PresencePenalty = (float)0,
                });
                var chatResponse = response.Value.Content.Last().Text;
                return chatResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error asking question: {ex.Message}");
                return string.Empty;
            }
        }

        // Ask a specific question about a chunk
        private static async Task<string> AskQuestion(AzureOpenAIClient client, string summerizedContent, string question, string deploymentName)
        {
            try
            {
                string prompt = $"{summerizedContent}\n\nQuestion: {question}\nAnswer:";

                // Send the prompt to Azure OpenAI
                var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a skilled data analyst."),
                new UserChatMessage(prompt)
            };
                var chatClient = client.GetChatClient(deploymentName);
                var response = await chatClient.CompleteChatAsync(messages, new ChatCompletionOptions()
                {
                    Temperature = (float)0.7,
                    FrequencyPenalty = (float)0,
                    PresencePenalty = (float)0,
                });
                var chatResponse = response.Value.Content.Last().Text;
                return chatResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error asking question: {ex.Message}");
                return string.Empty;
            }
        }
    }
















}
