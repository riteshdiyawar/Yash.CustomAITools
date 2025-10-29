using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenAI;
using RagApi.Services;
using System.IO;
using System.Threading.Tasks;
 
namespace RagApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CodeController : ControllerBase
    {
        private readonly RagService _ragService;

        public CodeController(IConfiguration config)
        {
            var apiKey = @"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";
            _ragService = new RagService(apiKey);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCode()
        {
           
            var chunks = await _ragService.ChunkAndEmbedAsync();
            return Ok(new { message = $"Uploaded and indexed {chunks.Count} chunks." });
        }

        [HttpPost("query")]
        public async Task<IActionResult> QueryCode( )
        {
            string question = "Summarize the following ASP.NET Web Forms code into a business-oriented description suitable for non-technical stakeholders.";
            var answer = await _ragService.AskQuestionAsync(question);
            return Ok(new { answer });
        }
    }
}