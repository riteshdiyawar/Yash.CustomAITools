using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using Yash.BusinessLogicExtractor;

namespace YashCustomToolRitesh
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class YashCustomToolController : ControllerBase

    {


        private readonly IConfiguration _configuration;

        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<YashCustomToolController> _logger;

        //public YashCustomToolController(IOptions<AppSettings> settings)
        //{
        //    _appSettings = settings;
        //}



        public YashCustomToolController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet("GetCodeImprovement")]
        public async Task<IActionResult> GetCodeImprovement
            (string ProjectPath = "E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode", string ProjectTechnologyType = "ASPXNET", string DatabaseConnection = "")
        {
            AIClass aIClass = new AIClass(ProjectPath, ProjectTechnologyType, DatabaseConnection);

            //string projectLocation = _configuration["ProjectLocation"].ToString();

            var openAiResponse = await aIClass.GetCodeImprovement(ProjectPath, ProjectTechnologyType);



            #region MD
            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(openAiResponse);

            // Set file name and content type
            string fileName = "Yash_CustomTools_CodeImprovement_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);
            #endregion

        }




        [HttpGet("GetClassDiagram")]
        public async Task<IActionResult> GetClassDiagram
            (string ProjectPath = "E:\\Project Applied\\Anchor.SuretyPortal", string ProjectTechnologyType = "ASPX.NET", string DatabaseConnection = "")
        {
            AIClass aIClass = new AIClass(ProjectPath, ProjectTechnologyType, DatabaseConnection);

            string projectLocation = ProjectPath;// _configuration["ProjectLocation"].ToString();

            var fileContent = await aIClass.GetClassDiagram(projectLocation);
            //aIClass.SaveDocuemnt(FileContent, "YashCustomTool_");




            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

            // Set file name and content type
            string fileName = "Yash_CustomTools_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);

            //return null;
        }


        [HttpGet("GetUnitTestGenerator")]
        public async Task<IActionResult> GetUnitTestGenerator
             (string ProjectPath = "E:\\Project Applied\\Anchor.SuretyPortal", string ProjectTechnologyType = "ASPX.NET", string DatabaseConnection = "")
        {
            AIClass aIClass = new AIClass(ProjectPath, ProjectTechnologyType, DatabaseConnection);

            string projectLocation = _configuration["ProjectLocation"].ToString();

            var fileContent = await aIClass.GetProjectDetails(projectLocation);
            //aIClass.SaveDocuemnt(FileContent, "YashCustomTool_");




            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

            // Set file name and content type
            string fileName = "Yash_CustomTools_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);

            //return null;
        }


        [HttpGet("GetProjectDiagram")]
        public async Task<IActionResult> GetProjectDiagram
             (string ProjectPath = "E:\\Project Applied\\Anchor.SuretyPortal", string ProjectTechnologyType = "ASPXNET", string DatabaseConnection = "")
        {
            AIClass aIClass = new AIClass(ProjectPath, ProjectTechnologyType, DatabaseConnection);

            //string projectLocation = _configuration["ProjectLocation"].ToString();

            var openAiResponse = await aIClass.GetProjectDiagram(ProjectPath, ProjectTechnologyType);
     


            #region MD
            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(openAiResponse);

            // Set file name and content type
            string fileName = "Yash_CustomTools_ProjectDiagram_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);
            #endregion

        }

        [HttpGet("GetProjectDetails")]
        public async Task<IActionResult> GetProjectDetails
            (string ProjectPath = "E:\\Project Applied\\Anchor.SuretyPortal", string ProjectTechnologyType = "ASPX.NET", string DatabaseConnection = "")

        {
            AIClass aIClass = new AIClass(ProjectPath, ProjectTechnologyType, DatabaseConnection);

            string projectLocation = _configuration["ProjectLocation"].ToString();

            var openAiResponse = await aIClass.GetProjectDetails(projectLocation);

            #region Word
            //// var openAiResponse = await GetOpenAIResponse(prompt);
            //if (string.IsNullOrWhiteSpace(openAiResponse))
            //    return BadRequest("Failed to generate content from OpenAI.");

            //var wordBytes = GenerateWordDocument(openAiResponse);

            //return File(wordBytes,
            //            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            //            "OpenAI_Generated_Document.docx");
            #endregion

            #region MD
            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(openAiResponse);

            // Set file name and content type
            string fileName = "Yash_CustomTools_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);
            #endregion




        }



        [HttpGet("GetProjectFeatureDetail")]
        public async Task<IActionResult> GetProjectDetail(string ProjectPath, string ProjectTechnologyType, string DatabaseConnection)
        {
            AIClass aIClass = new AIClass(ProjectPath, ProjectTechnologyType, DatabaseConnection);

            string projectLocation = _configuration["ProjectLocation"].ToString();

            var fileContent = await aIClass.GetProjectDetails(projectLocation);
            //aIClass.SaveDocuemnt(FileContent, "YashCustomTool_");




            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

            // Set file name and content type
            string fileName = "Yash_CustomTools_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);

            //return null;
        }

        [HttpGet("GetDatabaseDetailandImprovement")]
        public async Task<IActionResult> GetDatabaseDetailandImprovement
            (string ProjectPath = "E:\\Project Applied\\Anchor.SuretyPortal", string ProjectTechnologyType = "ASPX.NET", string DatabaseConnection = "")

        {
            AIClass aIClass = new AIClass(ProjectPath, ProjectTechnologyType, DatabaseConnection);

            string projectLocation = _configuration["ProjectLocation"].ToString();

            var fileContent = await aIClass.GetProjectDetails(projectLocation);
            //aIClass.SaveDocuemnt(FileContent, "YashCustomTool_");




            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

            // Set file name and content type
            string fileName = "Yash_CustomTools_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md";
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);

            //return null;
        }


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

        #endregion



    }
















}
