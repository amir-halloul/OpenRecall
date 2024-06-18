using OpenAI.Chat;
using OpenAI.Embeddings;
using OpenRecall.Library.Models;

namespace OpenRecall.Library.Utilities
{
    public class AiUtility
    {
        private readonly ChatClient _chatClient;
        private readonly EmbeddingClient _embeddingClient;
        private readonly ScreenshotUtility _screenshotUtility = new();

        public AiUtility(string apiKey)
        {
            _chatClient = new("gpt-4o", apiKey);
            _embeddingClient = new("text-embedding-3-large", apiKey);
        }

        public async Task<string> SummarizeActivityAsync(Activity activity)
        {
            var activeTabNames = activity.Snapshots.Select(snapshot => snapshot.ActiveWindowTitle).Where(s => !string.IsNullOrEmpty(s)).ToList();
            var imageBytes = activity.Snapshots.Where(snapshot => snapshot.Screenshot is not null).Select(snapshot => BinaryData.FromStream(_screenshotUtility.ImageToStream(_screenshotUtility.Resize(snapshot.Screenshot!, 960, 540)))).Where(data => data is not null).ToList();

            List<ChatMessage> messages =
            [
                ChatMessage.CreateSystemMessage("You are a desktop activity monitoring tool. Given a list of active windows the user had open and screenshots of them, you provide a very brief and accurate description of the user's activity."),
            ];

            var userMessage = new UserChatMessage(ChatMessageContentPart.CreateTextMessageContentPart("Windows I had open:"));

            foreach (var tabName in activeTabNames)
            {
                userMessage.Content.Add(ChatMessageContentPart.CreateTextMessageContentPart($"- {tabName}"));
            }

            foreach (var image in imageBytes)
            {
                userMessage.Content.Add(ChatMessageContentPart.CreateImageMessageContentPart(image, "image/jpeg"));
            }

            messages.Add(userMessage);


            ChatCompletion chatCompletion = await _chatClient.CompleteChatAsync(messages, new ChatCompletionOptions
            {
                MaxTokens = 300,
            });

            return chatCompletion.Content[0].Text;
        }

        public async Task<ReadOnlyMemory<float>> VectorizeStringAsync(string input)
        {
            var result = await _embeddingClient.GenerateEmbeddingAsync(input);
            return result.Value.Vector;
        }
    }
}
