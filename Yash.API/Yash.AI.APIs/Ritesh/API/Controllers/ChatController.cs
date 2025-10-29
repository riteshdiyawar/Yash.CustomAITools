using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Yash.BusinessLogicExtractor;
using Yash.CustomTool.API.Ritesh.Model;
using Yash.CustomTool.API.Ritesh.Service;
using YashCustomToolRitesh;


namespace Yash.CustomTool.API.Ritesh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {


        private readonly GeminiService _geminiService;

        public ChatController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }


        [HttpGet("GetCodeImprovement")]
        public async Task<IActionResult> GetCodeImprovement
                  (string ProjectPath = "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode", string ProjectTechnologyType = "ASPXNET", string DatabaseConnection = "")

        {


            GeminiChatRequest request;

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

            string Prompt = @"Analyze my code and provide suggestions to improve code quality, performance, and maintainability.
             I want the output in JSON format so I can bind it to a grid in Angular.
             The JSON should include the following columns:
             methodName: Name of the method

             severity: Severity level of the issue (e.g., Low, Medium, High)
             improvement: Area of improvement (e.g., readability, performance, structure)
             suggestion: Specific suggestion for improvement";

            //"Provide suggestions  for improving code quality, performance, and maintainability. I want just summary only"
            request = new GeminiChatRequest
            {
                Prompt = $"" + Prompt + Environment.NewLine + projectCode
            };

            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt is required.");

            var response = await _geminiService.GetChatResponseAsync(request.Prompt);

            #region MD
            // Convert string to byte array
            // byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(response);

            // Set file name and content type
            //string fileName = "Yash_CustomTools_CodeImprovement_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            //string contentType = "application/octet-stream";

            //return File(fileBytes, contentType, fileName);
            #endregion

            response = response.Replace("```json", "").Replace("```", "");
            return Content(response, "application/json");

            //string objJson = @"[  
            //            { 
            //            ""methodName"": ""foo"",    
            //            ""severity"": ""High"",
            //            ""improvement"": ""Refactor"",
            //            ""suggestion"": ""Use better naming""
            //            }
            //        ]";
            //return Content(objJson);



        }



        [HttpGet("GetCodeImprovementDetail")]
        public async Task<IActionResult> GetCodeImprovementDetail(string MethodName,string ProjectPath = "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode")        
        {
            GeminiChatRequest request;

            CodeExtractor codeExtractor = new CodeExtractor();  
        

            string projectCode = codeExtractor.GetMethodBodyByMethodName(MethodName, ProjectPath);


            string Prompt = @"Please analyze my code and provide detailed suggestions to improve its quality. Also, generate an auto-corrected version of the method with best practices applied.";




            request = new GeminiChatRequest
            {
                Prompt = $"" + Prompt + Environment.NewLine + projectCode
            };

            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt is required.");

            var response = await _geminiService.GetChatResponseAsync(request.Prompt);

            #region MD
            //Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(response);

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

            GeminiChatRequest request;



            string ImageUrl = "http://googleusercontent.com/image_generation_content/0";

            string LocalFilePath = "E:\\SuretyBondArchitecture.png";


            string projectFiles = "";

            if (ProjectTechnologyType.ToUpper() == "ASPXNET")
            {
                var extractor = new AspNetCodeExtractor();
                projectFiles = extractor.ExtractFilesStructure(ProjectPath);
            }
            else if (ProjectTechnologyType.ToUpper() == "ANGULAR")
            {
                var extractor = new AngularCodeExtractor();
                projectFiles = extractor.ExtractCode(ProjectPath);
            }
            else
            {
                return BadRequest("Unsupported Project Technology Type.");
            }

            #region API Calling
            request = new GeminiChatRequest
            {
                Prompt = $"I am Software Architect, i want to generate architecture diagram for a project. Based on the files available in Project" + Environment.NewLine + projectFiles
            };

            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt is required.");
            var response = await _geminiService.GetChatResponseAsync(request.Prompt);

            request = new GeminiChatRequest
            {
                Prompt = $"can you give me diagram only" + Environment.NewLine + response
            };

            response = await _geminiService.GetChatResponseAsImageAsync(request.Prompt);

            await DownloadImageAsync(ImageUrl, LocalFilePath);
            //#region MD
            //// Convert string to byte array
            //byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(response);

            //// Set file name and content type
            //string fileName = "Yash_CustomTools_ProjectDiagram_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            //string contentType = "application/octet-stream";

            //return File(fileBytes, contentType, fileName);
            //#endregion


            #endregion

            return null;
        }




        public static async Task DownloadImageAsync(string url, string filePath)
        {
            // Use a static HttpClient for better performance
            using (var httpClient = new HttpClient())
            {
                // 1. Get the image data as a stream
                using (var stream = await httpClient.GetStreamAsync(url))
                {
                    // 2. Open a file stream to write the content locally
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // 3. Copy the content from the HTTP stream to the file stream
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }
        }




    }



}





