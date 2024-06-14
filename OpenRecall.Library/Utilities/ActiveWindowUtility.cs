using System.Runtime.InteropServices;
using System.Text;

namespace OpenRecall.Library.Utilities
{
    public class ActiveWindowUtility
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public string GetActiveWindowTitle()
        {
            IntPtr handle = GetForegroundWindow();
            StringBuilder buffer = new StringBuilder(256);
            if (GetWindowText(handle, buffer, buffer.Capacity) > 0)
            {
                return buffer.ToString();
            }
            
            return string.Empty;
        }
    }
}
