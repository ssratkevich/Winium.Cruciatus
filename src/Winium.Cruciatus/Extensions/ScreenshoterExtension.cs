using System;
using System.IO;
using Winium.Cruciatus.Core;

namespace Winium.Cruciatus.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IScreenshoter"/>.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class ScreenshoterExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// Create screenshot if needed (when <see cref="Settings.CruciatusSettings.AutomaticScreenshotCapture"/> is true).
        /// </summary>
        public static void AutomaticScreenshotCaptureIfNeeded(this IScreenshoter screenshoter)
        {
            if (CruciatusFactory.Settings.AutomaticScreenshotCapture)
            {
                screenshoter.TakeScreenshot();
            }
        }

        /// <summary>
        /// Creates and saves screenshot.
        /// </summary>
        public static void TakeScreenshot(this IScreenshoter screenshoter)
        {
            var timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
            var screenshotPath = Path.Combine(CruciatusFactory.Settings.ScreenshotsPath, timeStamp + ".png");
            screenshoter.GetScreenshot().SaveAsFile(screenshotPath);
            CruciatusFactory.Logger.Info("Saved screenshot to '{0}' file.", Path.GetFullPath(screenshotPath));
        }

        #endregion
    }
}
