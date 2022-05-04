using System;

namespace Winium.Cruciatus.Settings.MessageBoxSettings
{
    /// <summary>
    /// Yes, No, Cancel buttons description class.
    /// </summary>
    public class YesNoCancelType : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Cancel button UID.
        /// </summary>
        public string Cancel { get; set; }

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
            new YesNoCancelType { Yes = this.Yes, No = this.No, Cancel = this.Cancel };

        #endregion
    }
}
