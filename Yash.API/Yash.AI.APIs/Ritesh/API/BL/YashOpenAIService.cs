using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yash.CustomTool.API.Ritesh.Model;

namespace Yash.CustomTool.API.Ritesh.BL
{



    public interface IOpenAiService
    {
        Task<string> AnalyzeAsync(string type, string data);
    }

    public class YashOpenAIService : IOpenAiService
    {
        private readonly AzureOpenAIClient _openAiClient;
        private readonly string _deploymentName;

        public YashOpenAIService()
        {
            var endpoint = AIHelper.AzureOpenAI_Endpoint;
            var apiKey = AIHelper.AzureOpenAI_ApiKey;
            _deploymentName = AIHelper.AzureOpenAI_DeploymentName;

            _openAiClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        }

        public async Task<string> AnalyzeAsync(string type, string data)
        {
            try
            {
                var prompt = $"Analyze the share price of IEX in NSE";

                var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a skilled data analyst."),
                new UserChatMessage(prompt)
            };

                var chatClient = _openAiClient.GetChatClient(_deploymentName);
                var response = await chatClient.CompleteChatAsync(messages, new ChatCompletionOptions
                {
                    Temperature = 0.7f,
                    FrequencyPenalty = 0f,
                    PresencePenalty = 0f
                });

                return response.Value.Content.Last().Text;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        public async Task<string> AnalyzePromptAsync(string prompt)
        {
            try
            {
                var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a skilled data analyst."),
                new UserChatMessage(prompt)
            };

                var chatClient = _openAiClient.GetChatClient(_deploymentName);
                var response = await chatClient.CompleteChatAsync(messages, new ChatCompletionOptions
                {
                    Temperature = 0.7f,
                    FrequencyPenalty = 0f,
                    PresencePenalty = 0f
                });

                return response.Value.Content.Last().Text;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<string> AskQuestion(string prompt)
        {
            return "";
        }
    }
}