using Azure;
using Azure.AI.OpenAI;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Mvc;
using OpenAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Yash.BusinessLogicExtractor;


namespace Yash.CustomTool.API.Ritesh.Model
{
    public static class AIHelper
    {
        #region Prompt        
        //public static string OPEN_AI_Prompt_GenerateBRD = @"Based on the uploaded file,Summarize the following ASP.NET Web Forms code into a business-oriented description suitable for non-technical stakeholders.";

        public static string OPEN_AI_Prompt_GetControls = @"This is a Web Forms application. Could you please analyze this .aspx page and provide a count of all UI controls present I need the output in JSON format, showing each control type and its count — something I can bind to a grid. The JSON should include the following columns 
          ControlName: Name of Control 
          Count: Count of control " + Environment.NewLine + "expecting JSON Format only no other information";


        public static string OPEN_AI_Prompt_GetCodeImprovementSummary = @"Analyze my code and provide suggestions to improve code quality, performance, and maintainability.
             I want the output in JSON format so I can bind it to a grid in Angular.
             The JSON should include the following columns:
             methodName: Name of the method
             severity: Severity level of the issue (e.g., Low, Medium, High)
             improvement: Area of improvement (e.g., readability, performance, structure)
             suggestion: Specific suggestion for improvement" + Environment.NewLine + "expecting JSON Format only no other information";


        public static string OPEN_AI_Prompt_GetMethodImprovementDetail = @"Please analyze my code and provide detailed suggestions to improve its quality. Also, generate an auto-corrected version of the method with best practices applied.";

        public static string OPEN_AI_Prompt_DesignArch = "Generate a high-level conceptual architecture for an ASP.NET Web Forms application." + Environment.NewLine + "The output should be structured and easy to read, using boxes or separators to represent layers.";

        public static string OPEN_AI_Prompt_PageLevelFeature = @"Analyze the provided ASP.NET Web Forms page (.aspx and .cs files) and create a concise one-page            feature  summary. The summary should include:
                Page Name and Purpose – Explain the business goal or functionality of the page in 2–3 sentences.
                Key Features – List major functionalities
                Security or Performance Notes – Highlight any role-based access, input validation, or caching implemented.";

        public static string OPEN_AI_Prompt_GenerateBRD  = @"Create a comprehensive Business Requirements Document (BRD), with following feature summary. The BRD should include these sections: Document Overview, Business Objectives, Scope, Stakeholders, Functional Requirements, Non-Functional Requirements, Assumptions & Constraints, Dependencies, UI/UX Requirements, Data Requirements, Workflow/Use Cases, and Acceptance Criteria. Apply professional formatting with headings, bullet points, and tables where appropriate. Include placeholders for diagrams and ensure the document is suitable for stakeholder review.";

        #endregion

        //public static string OPEN_Gemini_Key = @"AIzaSyD7QnlTuyTJ1RWmK2DA8okmGyBLSiJvgDo";


        //public static string OPEN_AI_AssistantId = @"asst_ANrq46eDqR5zLIMf5op6EiUO";

        //public static string AssistantName = "";
        //public static string OPEN_AI_Key = @"7OBI4px4KXZ1Awhltacicl63IA701Q5krKgwjuPUPKo9MTKapUcjJQQJ99BKACYeBjFXJ3w3AAABACOGtZI6";


        //public static string AzureOpenAI_Endpoint = "https://poc5-openai.openai.azure.com/";
        //public static string AzureOpenAI_ApiKey = "7OBI4px4KXZ1Awhltacicl63IA701Q5krKgwjuPUPKo9MTKapUcjJQQJ99BKACYeBjFXJ3w3AAABACOGtZI6";
        //public static string AzureOpenAI_DeploymentName = "gpt-4o";

           



    }
}


