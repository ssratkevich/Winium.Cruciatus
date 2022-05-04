using System;

namespace Winium.Cruciatus.Settings.MessageBoxSettings
{
    /// <summary>
    /// Yes, No buttons description class.
    /// </summary>
    public class YesNoType : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// No button UID.
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// Yes button UID.
        /// </summary>
        public string Yes { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public object Clone() =>
            new YesNoType { Yes = this.Yes, No = this.No };

        #endregion
    }
}
