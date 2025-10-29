using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Yash.CustomTool.API.Ritesh.Model;
using System;
namespace Yash.CustomTool.API.Ritesh.Controllers


{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIAnalysisController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public OpenAIAnalysisController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("GenerateBusinessDocument")]
        public async Task<IActionResult> GenerateBusinessDocument()
        {
            try
            {
                string prompt = AIHelper.OPEN_AI_Prompt_GenerateBRD;
                string filePath = "E:\\CodeFile.txt";

                if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                    return BadRequest("Invalid file path.");

                var apiKey = AIHelper.OPEN_AI_Key;
                var assistantId = AIHelper.OPEN_AI_AssistantId;
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v2");

                // Step 1: Upload file
                using var uploadContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(filePath));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                uploadContent.Add(fileContent, "file", Path.GetFileName(filePath));
                uploadContent.Add(new StringContent("assistants"), "purpose");

                var uploadResponse = await client.PostAsync("https://api.openai.com/v1/files", uploadContent);
                var uploadJson = await uploadResponse.Content.ReadAsStringAsync();
                var fileId = JsonDocument.Parse(uploadJson).RootElement.GetProperty("id").GetString();


                var threadPayload = new
                {
                    messages = new[]
    {
        new
        {
            role = "user",
            content = prompt,
            file_ids = new[] { fileId }
        }
    }
                };

                var threadRequest = new StringContent(JsonSerializer.Serialize(threadPayload), Encoding.UTF8, "application/json");
                var threadResponse = await client.PostAsync("https://api.openai.com/v1/threads", threadRequest);
                var threadJson = await threadResponse.Content.ReadAsStringAsync();

                // Log the raw response
                Console.WriteLine("Thread Response JSON:");
                Console.WriteLine(threadJson);

                // Check for error before accessing 'id'
                var threadDoc = JsonDocument.Parse(threadJson);
                if (threadDoc.RootElement.TryGetProperty("error", out var error))
                {
                    var errorMessage = error.GetProperty("message").GetString();
                    return BadRequest($"OpenAI API error during thread creation: {errorMessage}");
                }

                // If no error, extract thread ID
                var threadId = threadDoc.RootElement.GetProperty("id").GetString();
                Console.WriteLine($"Thread ID: {threadId}");


                // Step 3: Run assistant on thread
                //..var runPayload = new { assistant_id = assistantId };


                var runPayload = new
                {
                    assistant_id = assistantId
                };

                var runContent = new StringContent(JsonSerializer.Serialize(runPayload), Encoding.UTF8, "application/json");




                var runResponse = await client.PostAsync(
                    $"https://api.openai.com/v1/threads/{threadId}/runs",
                    new StringContent(JsonSerializer.Serialize(runPayload), Encoding.UTF8, "application/json")
                );

                var runJson = await runResponse.Content.ReadAsStringAsync();
                var runId = JsonDocument.Parse(runJson).RootElement.GetProperty("id").GetString();

                // Step 4: Poll for completion
                string status;
                do
                {
                    await Task.Delay(2000);
                    var statusResponse = await client.GetAsync($"https://api.openai.com/v1/threads/{threadId}/runs/{runId}");
                    var statusJson = await statusResponse.Content.ReadAsStringAsync();
                    status = JsonDocument.Parse(statusJson).RootElement.GetProperty("status").GetString();
                } while (status != "completed");

                // Step 5: Get messages
                var messagesResponse = await client.GetAsync($"https://api.openai.com/v1/threads/{threadId}/messages");
                var messagesJson = await messagesResponse.Content.ReadAsStringAsync();


                return Content(messagesJson, "application/json");
            }
            catch (Exception ex)
            {

            }

            return Content("");
        }





        [HttpPost("analyze-from-path")]
        public async Task<IActionResult> AnalyzeFileFromPath()
        {
            try
            {
                string prompt = AIHelper.OPEN_AI_Prompt_GenerateBRD;
                string filePath = "E:\\CodeFile.txt";

                if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                    return BadRequest("Invalid file path.");

                var apiKey = AIHelper.OPEN_AI_Key;
                var assistantId = AIHelper.OPEN_AI_AssistantId;
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v2");

                // Step 1: Upload file
                using var uploadContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(filePath));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                uploadContent.Add(fileContent, "file", Path.GetFileName(filePath));
                uploadContent.Add(new StringContent("assistants"), "purpose");

                var uploadResponse = await client.PostAsync("https://api.openai.com/v1/files", uploadContent);
                var uploadJson = await uploadResponse.Content.ReadAsStringAsync();
                var fileId = JsonDocument.Parse(uploadJson).RootElement.GetProperty("id").GetString();

                // Step 2: Create thread
                var threadResponse = await client.PostAsync(
                    "https://api.openai.com/v1/threads",
                    new StringContent("{}", Encoding.UTF8, "application/json")
                );
                var threadJson = await threadResponse.Content.ReadAsStringAsync();
                var threadId = JsonDocument.Parse(threadJson).RootElement.GetProperty("id").GetString();

                // Step 3: Add message with prompt and file
                var messagePayload = new
                {
                    role = "user",
                    content = prompt,
                    file_ids = new[] { fileId }
                };

                var messageResponse = await client.PostAsync(
                    $"https://api.openai.com/v1/threads/{threadId}/messages",
                    new StringContent(JsonSerializer.Serialize(messagePayload), Encoding.UTF8, "application/json")
                );

                var messagesJson = await messageResponse.Content.ReadAsStringAsync();

                /*
                // Step 4: Run assistant
                var runPayload = new { assistant_id = assistantId };
                var runResponse = await client.PostAsync(
                    $"https://api.openai.com/v1/threads/{threadId}/runs",
                    new StringContent(JsonSerializer.Serialize(runPayload), Encoding.UTF8, "application/json")
                );
                var runJson = await runResponse.Content.ReadAsStringAsync();
                var runId = JsonDocument.Parse(runJson).RootElement.GetProperty("id").GetString();

                // Step 5: Poll for completion
                string status;
                do
                {
                    await Task.Delay(2000);
                    var statusResponse = await client.GetAsync($"https://api.openai.com/v1/threads/{threadId}/runs/{runId}");
                    var statusJson = await statusResponse.Content.ReadAsStringAsync();
                    status = JsonDocument.Parse(statusJson).RootElement.GetProperty("status").GetString();
                } while (status != "completed");

                // Step 6: Get messages
                var messagesResponse = await client.GetAsync($"https://api.openai.com/v1/threads/{threadId}/messages");
                var messagesJson = await messagesResponse.Content.ReadAsStringAsync();
                */

                return Content(messagesJson, "application/json");
            }
            catch (Exception ex)
            {
                
            }

            return Content("messagesJson", "application/json");
        }


    }
}