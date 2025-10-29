using System.Net.Http;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;



namespace Yash.CustomTool.API.Ritesh.Service
{



    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "AIzaSyB_3FzJaiRRXFK-Ni2k_9ljOKJG4-XJFTw";

        public GeminiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetChatResponseAsync(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
            };

            string text;

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent");

            request.Headers.Add("X-Goog-Api-Key", _apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                text = $"Gemini API Error: {response.StatusCode} - {error}";
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(responseStream);

            text = doc.RootElement
               .GetProperty("candidates")[0]
               .GetProperty("content")
               .GetProperty("parts")[0]
               .GetProperty("text")
               .GetString();

            return text ?? "No response from Gemini";
        }






        // NOTE: You need to implement the surrounding GeminiApiClient class 
        //       and the _apiKey/_httpClient variables as shown in the previous response.

        public async Task<string> GetChatResponseAsImageAsync(string prompt)
        {
            // ... (Your requestBody construction remains the same) ...
            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent");

            request.Headers.Add("X-Goog-Api-Key", _apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error: {response.StatusCode} - {error}");
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(responseStream);

            // --- MODIFIED LOGIC TO EXTRACT TEXT OR IMAGE URL ---
            try
            {
                var candidate = doc.RootElement.GetProperty("candidates")[0];

                // 1. Check for Text (Standard Response)
                if (candidate.TryGetProperty("content", out var contentElement) &&
                    contentElement.TryGetProperty("parts", out var partsElement) &&
                    partsElement.GetArrayLength() > 0 &&
                    partsElement[0].TryGetProperty("text", out var textElement))
                {
                    return textElement.GetString() ?? "No text response.";
                }

                // 2. Check for an Image URL (from a Tool/Function Call Response)
                if (candidate.TryGetProperty("parts", out partsElement) &&
                    partsElement.GetArrayLength() > 0 &&
                    partsElement[0].TryGetProperty("functionResponse", out var funcResponseElement) &&
                    funcResponseElement.TryGetProperty("response", out var responseDataElement))
                {
                    // This is a deep dive into the 'google:search' tool's response structure.
                    // Image URL is usually nested in the 'media' property of one of the search results.
                    if (responseDataElement.TryGetProperty("results", out var resultsElement) &&
                        resultsElement.GetArrayLength() > 0)
                    {
                        foreach (var result in resultsElement.EnumerateArray())
                        {
                            if (result.TryGetProperty("media", out var mediaElement) &&
                                mediaElement.GetArrayLength() > 0)
                            {
                                if (mediaElement[0].TryGetProperty("image", out var imageElement) &&
                                    imageElement.TryGetProperty("url", out var urlElement))
                                {
                                    // Return the first found image URL
                                    return urlElement.GetString() ?? "No valid image URL found.";
                                }
                            }
                        }
                    }
                }

                // If neither text nor a structured tool response was found
                return "Could not extract text or image URL from response.";
            }
            catch (Exception)
            {
                // Fallback for unexpected JSON structure (e.g., error or block reason)
                return "Failed to parse API response structure.";
            }
        }

    }
}
