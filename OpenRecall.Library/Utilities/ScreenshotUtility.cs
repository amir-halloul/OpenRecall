using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Management;
using System.Runtime.InteropServices;

namespace OpenRecall.Library.Utilities
{
    public class ScreenshotUtility
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        private Size GetScreenSize()
        {
            ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\cimv2");
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_VideoController " + "Where DeviceID=\"VideoController1\"");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();

            foreach (ManagementObject m in queryCollection)
            {
                return new Size(int.Parse(m["CurrentHorizontalResolution"].ToString() ?? "0"), int.Parse(m["CurrentVerticalResolution"].ToString() ?? "0"));
            }
            return new Size(0, 0);
        }

        public Bitmap? TakeScreenshot()
        {
            Size screenSize = GetScreenSize();
            Bitmap bmp = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, screenSize);
                return bmp;
            }
        }

        public MemoryStream ImageToStream(Image image)
        {
            var stream = new MemoryStream();
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter encoderQualityParameter = new EncoderParameter(Encoder.Quality, 75L);

            encoderParameters.Param[0] = encoderQualityParameter;
            image.Save(stream, jpgEncoder, encoderParameters);
            stream.Position = 0;
            return stream;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public Image Resize(Image originalImage, int w, int h)
        {
            //Original Image attributes
            int originalWidth = originalImage.Width;
            int originalHeight = originalImage.Height;

            // Figure out the ratio
            double ratioX = (double)w / (double)originalWidth;
            double ratioY = (double)h / (double)originalHeight;
            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(originalHeight * ratio);
            int newWidth = Convert.ToInt32(originalWidth * ratio);

            Image thumbnail = new Bitmap(newWidth, newHeight);
            Graphics graphic = Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            graphic.Clear(Color.Transparent);
            graphic.DrawImage(originalImage, 0, 0, newWidth, newHeight);

            return thumbnail;
        }
    }
}
