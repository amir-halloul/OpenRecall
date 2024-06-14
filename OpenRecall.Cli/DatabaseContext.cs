using Microsoft.EntityFrameworkCore;
using OpenRecall.Library.Models;

namespace OpenRecall.Cli
{
    internal class DatabaseContext: DbContext
    {
        public DbSet<Activity> Activities { get; set; }

        public string DbPath { get; }

        public DatabaseContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "openrecall.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
