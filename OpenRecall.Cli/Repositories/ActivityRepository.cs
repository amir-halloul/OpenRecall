using Microsoft.EntityFrameworkCore;
using OpenRecall.Library.Models;
using OpenRecall.Library.Repositories;

namespace OpenRecall.Cli.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        public async Task<IEnumerable<Activity>> GetActivities()
        {
            using var dbContext = new DatabaseContext();
            return await dbContext.Activities.ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetActivitiesAfterDate(DateTime date)
        {
            using var dbContext = new DatabaseContext();
            return await dbContext.Activities.Where(a => a.StartTime > date).ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetActivitiesBeforeDate(DateTime date)
        {
            using var dbContext = new DatabaseContext();
            return await dbContext.Activities.Where(a => a.StartTime < date).ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetActivitiesBetweenDates(DateTime startDate, DateTime endDate)
        {
            using var dbContext = new DatabaseContext();
            return await dbContext.Activities.Where(a => a.StartTime > startDate && a.StartTime < endDate).ToListAsync();
        }
    }
}
