using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Desktop screenshoter.
    /// </summary>
    public class Screenshoter : IScreenshoter
    {
        /// <inheritdoc/>
        public Screenshot GetScreenshot()
        {
            byte[] imageBytes;
            var rect = new Rectangle(
                (int)System.Windows.SystemParameters.VirtualScreenLeft, 
                (int)System.Windows.SystemParameters.VirtualScreenTop, 
                (int)System.Windows.SystemParameters.VirtualScreenWidth, 
                (int)System.Windows.SystemParameters.VirtualScreenHeight);
            using (var bitmap = new Bitmap(rect.Width, rect.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(new Point(rect.Left, rect.Top), Point.Empty, rect.Size);
                }

                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    imageBytes = stream.ToArray();
                }
            }

            return new Screenshot(imageBytes);
        }
    }
}
