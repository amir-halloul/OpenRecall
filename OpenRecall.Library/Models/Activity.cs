using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenRecall.Library.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public IList<ActivitySnapshot> Snapshots { get; set; } = new List<ActivitySnapshot>();
        public string Description { get; set; } = string.Empty;
        public ReadOnlyMemory<float> DescriptionVector { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set;  }

        public override string ToString()
        {
            return $"{StartTime.ToString("dddd, dd MMMM yyyy HH:mm:ss")} - {EndTime.ToString("dddd, dd MMMM yyyy HH:mm:ss")}: {Description}";
        }
    }
}
