using System;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Element text getting strategies.
    /// </summary>
    [Flags]
    public enum GetTextStrategies
    {
        /// <summary>
        /// No stategy (try everything).
        /// </summary>
        None = 0, 

        /// <summary>
        /// Get text through automation element TextPattern.
        /// </summary>
        TextPattern = 1,

        /// <summary>
        /// Get text through automation element ValuePattern.
        /// </summary>
        ValuePattern = 2
    }
}
