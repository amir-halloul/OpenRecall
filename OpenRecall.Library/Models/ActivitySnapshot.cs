using System.Drawing;

namespace OpenRecall.Library.Models
{
    public class ActivitySnapshot
    {
        public string ActiveWindowTitle { get; set; } = string.Empty;
        public Bitmap? Screenshot { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
