using OpenRecall.Library.Models;

namespace OpenRecall.Library.Repositories
{
    public interface IActivityRepository
    {
        Task<IEnumerable<Activity>> GetActivities();
        Task<IEnumerable<Activity>> GetActivitiesBetweenDates(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Activity>> GetActivitiesBeforeDate(DateTime date);
        Task<IEnumerable<Activity>> GetActivitiesAfterDate(DateTime date);
    }
}
