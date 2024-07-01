using OpenRecall.Cli.Repositories;
using OpenRecall.Library;
using OpenRecall.Library.Ai;
using OpenRecall.Library.Utilities;

namespace OpenRecall.Cli
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var configuration = Configuration.Load();
            var aiUtility = new AiUtility(configuration.OpenAiApiKey);
            var activityManager = new ActivityManager(aiUtility, configuration.SnapshotInterval, configuration.ActivitySnapshotThreashold);
            var chatBot = new AiChatBot(new ActivityRepository(), configuration.OpenAiApiKey);

            activityManager.ActivityCreated += ActivityManager_ActivityCreated;

            activityManager.Start();

            while (true)
            {
                Console.Write("You: ");
                var input = Console.ReadLine();

                if (input == null)
                {
                    continue;
                }

                if (input.Trim().ToLower() == "quit")
                {
                    activityManager.Stop();
                    break;
                } else
                {
                    // Get response from AI asynchronously on a separate thread
                    var response = chatBot.GetResponse(input).Result;
                    Console.WriteLine($"OpenRecall AI: {response}");
                }
            }

        }

        private static async void ActivityManager_ActivityCreated(object? sender, ActivityEventArgs e)
        {
            using var dbContext = new DatabaseContext();
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                dbContext.Activities.Add(e.Activity);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await transaction.RollbackAsync();
            }
        }
    }
}
