using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
 

namespace Yash.BusinessLogicExtractor
{
    public class OpenAIService
    {
        //private readonly HttpClient _httpClient;
        private readonly string _apiKey = @"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";

        //public OpenAIService(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        public async Task<string> AnalyzeBatchAsync(RequestBody requestBody)
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
    }
}