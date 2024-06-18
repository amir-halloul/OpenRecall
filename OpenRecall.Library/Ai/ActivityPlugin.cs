using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using OpenRecall.Library.Collections;
using OpenRecall.Library.Models;
using OpenRecall.Library.Repositories;
using System.ComponentModel;
using System.Numerics.Tensors;

// Embedding functionality is experimental
#pragma warning disable SKEXP0001

namespace OpenRecall.Library.Ai
{
    internal class ActivityPlugin
    {
        [KernelFunction]
        [Description("Gets a list of all activities")]
        public async Task<IEnumerable<string>> GetAllActivities(Kernel kernel)
        {
            var activityRepository = kernel.GetRequiredService<IActivityRepository>();
            var activities = await activityRepository.GetActivities();
            return activities.Select(a => a.ToString());
        }

        [KernelFunction]
        [Description("Gets a list of all activities the user performed between two datetimes")]
        public async Task<IEnumerable<string>> GetAllActivitiesBetweenDates(Kernel kernel, 
            [Description("Start datetime")] DateTime startDateTime,
            [Description("End datetime")] DateTime endDateTime)
        {
            var activityRepository = kernel.GetRequiredService<IActivityRepository>();
            var activities = await activityRepository.GetActivitiesBetweenDates(startDateTime, endDateTime);
            return activities.Select(a => a.ToString());
        }

        [KernelFunction]
        [Description("Gets a list of all activities the user performed before a given datetime")]
        public async Task<IEnumerable<string>> GetAllActivitiesBefore(Kernel kernel, DateTime dateTime)
        {
            var activityRepository = kernel.GetRequiredService<IActivityRepository>();
            var activities = await activityRepository.GetActivitiesBeforeDate(dateTime);
            return activities.Select(a => a.ToString());
        }

        [KernelFunction]
        [Description("Gets a list of all activities the user performed after a given datetime until now")]
        public async Task<IEnumerable<string>> GetAllActivitiesAfter(Kernel kernel, DateTime dateTime)
        {
            var activityRepository = kernel.GetRequiredService<IActivityRepository>();
            var activities = await activityRepository.GetActivitiesAfterDate(dateTime);
            return activities.Select(a => a.ToString());
        }

        [KernelFunction]
        [Description("Gets a list of all activities the user performed that match a given description")]
        public async Task<IEnumerable<string>> GetActivitiesByDescription(Kernel kernel, 
            [Description("A brief description of the task to search for using semantic search")]string description,
            [Description("The maximum number of activities to return.")] int limit)
        {
            var activityRepository = kernel.GetRequiredService<IActivityRepository>();
            var activities = await activityRepository.GetActivities();

            var textEmbeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

            var descriptionVector = await textEmbeddingService.GenerateEmbeddingAsync(description);

            TopNCollection<Activity> topActivities = new(limit);

            foreach (var activity in activities)
            {
                double similarity = TensorPrimitives.CosineSimilarity(descriptionVector.Span, activity.DescriptionVector.Span);

                if (similarity >= 0.1)
                {
                    topActivities.Add(new(activity, similarity));
                }
            }

            topActivities.SortByScore();
            return topActivities.Select(x => x.Value.ToString());
        }
    }
}
