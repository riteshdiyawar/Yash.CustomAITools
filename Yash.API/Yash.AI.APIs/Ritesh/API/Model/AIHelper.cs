using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Yash.BusinessLogicExtractor;
using DocumentFormat.OpenXml.Vml;

namespace Yash.CustomTool.API.Ritesh.Model
{
    public static class AIHelper
    {
        public static string OPEN_AI_Prompt_GenerateBRD = @"Based on the uploaded file,Summarize the following ASP.NET Web Forms code into a business-oriented description suitable for non-technical stakeholders.";

        public static string OPEN_AI_Key = @"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";


        public static string OPEN_AI_Prompt_GetControls = @"This is a Web Forms application. Could you please analyze this .aspx page and provide a count of all UI controls present? I need the output in JSON format, showing each control type and its count — something I can bind to a grid." +             "The JSON should include the following columns 1. Control Name 2. Count"            ;

        public static string OPEN_Gemini_Key = @"AIzaSyD7QnlTuyTJ1RWmK2DA8okmGyBLSiJvgDo";


        public static string OPEN_AI_AssistantId = @"asst_ANrq46eDqR5zLIMf5op6EiUO";

        public static string AssistantName = "";



        public static async Task<string> consumeAPIAsync(string FileName, RequestBody requestBody)
        {
            string responseString = "";
            string responseContent = "";
            try
            {

                //var key = @"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";
                var endpoint = "https://api.openai.com/v1/chat/completions";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OPEN_AI_Key);

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
