using System;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Click on element strategies.
    /// </summary>
    [Flags]
    public enum ClickStrategies
    {
        /// <summary>
        /// No strategy (try everything).
        /// </summary>
        None = 0, 

        /// <summary>
        /// Click on automation element ClickablePoint.
        /// </summary>
        ClickablePoint = 1,

        /// <summary>
        /// Click on center of automation element BoundingRectangle.
        /// </summary>
        BoundingRectangleCenter = 2, 

        /// <summary>
        /// Click through automation element InvokePattern.
        /// </summary>
        InvokePattern = 4
    }
}
