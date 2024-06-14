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
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set;  }
    }
}
