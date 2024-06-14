using OpenRecall.Library;
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

            activityManager.ActivityCreated += ActivityManager_ActivityCreated;

            activityManager.Start();

            while (true)
            {
                var input = Console.ReadLine();

                if (input == "quit")
                {
                    activityManager.Stop();
                    break;
                }

                if (input == "ping")
                {
                    Console.WriteLine("Pong");
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
