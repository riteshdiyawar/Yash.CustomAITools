using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[controller]")]
public class OpenAIDocumentController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OpenAIDocumentController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadGeneratedDoc()
    {
        var prompt = "Write a professional summary about the benefits of using AI in software development.";

        var openAiResponse = await GetOpenAIResponse(prompt);
        if (string.IsNullOrWhiteSpace(openAiResponse))
            return BadRequest("Failed to generate content from OpenAI.");

        var wordBytes = GenerateWordDocument(openAiResponse);

        return File(wordBytes,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "OpenAI_Generated_Document.docx");
    }

    private async Task<string> GetOpenAIResponse(string prompt)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://api.openai.com/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR_OPENAI_API_KEY");

        var requestBody = new
        {
            model = "gpt-4",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("v1/chat/completions", content);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement
                  .GetProperty("choices")[0]
                  .GetProperty("message")
                  .GetProperty("content")
                  .GetString();
    }

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
}