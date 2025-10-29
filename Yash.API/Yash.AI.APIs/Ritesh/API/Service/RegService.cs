using OpenAI;
using OpenAI.Embeddings;
using RagApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Yash.CustomTool.API.Ritesh.Model;
using OpenAI.Chat;
using System.IO;

namespace RagApi.Services
{
    public class RagService
    {
        private readonly OpenAIClient _client;
        private readonly List<CodeChunk> _chunks = new();

        public RagService(string apiKey)
        {
            _client = new OpenAIClient(new OpenAIAuthentication(apiKey));
        }

        public async Task<List<CodeChunk>> ChunkAndEmbedAsync(  int linesPerChunk = 20)
        {
            string code = File.ReadAllText("E:\\Mail.cs");


            var lines = code.Split('\n');
            var chunks = new List<string>();

            for (int i = 0; i < lines.Length; i += linesPerChunk)
            {
                chunks.Add(string.Join("\n", lines.Skip(i).Take(linesPerChunk)));
            }

            //foreach (var chunk in chunks)
            //{

            //    var embedding = await _client.EmbeddingsEndpoint.CreateEmbeddingAsync(chunk, "text-embedding-ada-002");
            //    _chunks.Add(new CodeChunk
            //    {
            //        Text = chunk,
            //        Embedding = embedding.Data[0].Embedding.Select(d => (float)d).ToArray()
            //    });

            //}


            foreach (var chunk in chunks)
            {
                try
                {
                    var embeddingResponse = await _client.EmbeddingsEndpoint.CreateEmbeddingAsync(chunk, "text-embedding-ada-002");

                    if (embeddingResponse?.Data?.Count > 0)
                    {
                        var embedding = embeddingResponse.Data[0].Embedding.Select(d => (float)d).ToArray();

                        _chunks.Add(new CodeChunk
                        {
                            Text = chunk,
                            Embedding = embedding
                        });
                    }
                    else
                    {
                        // Log or handle empty embedding response
                    }
                }
                catch (Exception ex)
                {
                    // Log the error and continue with next chunk
                    Console.WriteLine($"Error embedding chunk: {ex.Message}");
                }
            }

            return _chunks;
        }

        public List<CodeChunk> RetrieveRelevantChunks(float[] queryEmbedding, int topK = 5)
        {
            return _chunks
                .OrderByDescending(chunk => CosineSimilarity(chunk.Embedding, queryEmbedding))
                .Take(topK)
                .ToList();
        }

        private float CosineSimilarity(float[] a, float[] b)
        {
            float dot = 0, magA = 0, magB = 0;
            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                magA += a[i] * a[i];
                magB += b[i] * b[i];
            }
            return dot / (MathF.Sqrt(magA) * MathF.Sqrt(magB));
        }

        public async Task<string> AskQuestionAsync(string question)
        {

            var queryEmbedding = await _client.EmbeddingsEndpoint.CreateEmbeddingAsync(question, "text-embedding-ada-002");
            var queryVector = queryEmbedding.Data[0].Embedding.Select(d => (float)d).ToArray();
            var relevantChunks = RetrieveRelevantChunks(queryVector);


            var prompt = $"You are a C# expert. Based on the following code snippets, answer the question:\n\n" +
                         string.Join("\n\n", relevantChunks.Select(c => c.Text)) +
                         $"\n\nQuestion: {question}";

            var chatRequest = new ChatRequest(
                new[]
                {
                    new Message(Role.System, "You are a helpful assistant for analyzing C# code."),
                    new Message(Role.User, prompt)
                },
                model: "gpt-4"
            );

            var response = await _client.ChatEndpoint.GetCompletionAsync(chatRequest);
            return response.FirstChoice.Message.Content;
        }
    }
}