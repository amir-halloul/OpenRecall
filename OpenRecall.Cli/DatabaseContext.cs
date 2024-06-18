using Microsoft.EntityFrameworkCore;
using OpenRecall.Library.Models;
using System.Text.Json;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var jsonSerliazerOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
            };

            modelBuilder.Entity<Activity>()
                .Property(a => a.DescriptionVector)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonSerliazerOptions),
                    v => JsonSerializer.Deserialize<ReadOnlyMemory<float>>(v, jsonSerliazerOptions)
                );
        }
    }
}
