namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Screenshoter interface.
    /// </summary>
    public interface IScreenshoter
    {
        /// <summary>
        /// Desktop screenshot.
        /// </summary>
        /// <returns>
        /// Desktop screenshot.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        Screenshot GetScreenshot();
    }
}
