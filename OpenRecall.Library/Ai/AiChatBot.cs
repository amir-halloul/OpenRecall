using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenRecall.Library.Repositories;

#pragma warning disable SKEXP0010

namespace OpenRecall.Library.Ai
{
    public class AiChatBot
    {
        private readonly IActivityRepository _activityRepository;
        private readonly Kernel _kernel;
        private readonly ChatHistory _chatMessages = new ChatHistory();


        public AiChatBot(IActivityRepository activityRepository, string openAiApiKey)
        {
            _activityRepository = activityRepository;
            var builder = Kernel.CreateBuilder();
            builder.Services.AddOpenAIChatCompletion("gpt-4o", openAiApiKey)
                .AddOpenAITextEmbeddingGeneration("text-embedding-3-large", openAiApiKey)
                // .AddLogging(c => c.SetMinimumLevel(LogLevel.Trace).AddConsole())
                .AddSingleton(activityRepository);
            builder.Plugins.AddFromType<ActivityPlugin>();

            _kernel = builder.Build();
            _chatMessages.AddSystemMessage($"You are OpenRecall AI, an assistant embedded in OpenRecall which is a desktop activity monitoring tool. You have access to the user's activity history and you help the user recall his old activities. Current time is {DateTime.Now}");
        }

        public async Task<string> GetResponse(string input)
        {
            _chatMessages.AddUserMessage(input);
            IChatCompletionService chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            };
            var result = chatCompletionService.GetStreamingChatMessageContentsAsync(_chatMessages, executionSettings: openAIPromptExecutionSettings, kernel: _kernel);

            string fullMessage = "";
            await foreach (var content in result)
            {
                fullMessage += content.Content;
            }

            _chatMessages.AddAssistantMessage(fullMessage);
            return fullMessage;
        }
    }
}
