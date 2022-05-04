using System;

namespace Winium.Cruciatus.Settings.MessageBoxSettings
{
    /// <summary>
    /// Ok, Cancel buttons descriptor class.
    /// </summary>
    public class OkCancelType : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Cancel button UID.
        /// </summary>
        public string Cancel { get; set; }

        /// <summary>
        /// Ok button UID.
        /// </summary>
        public string Ok { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public object Clone() =>
            new OkCancelType { Ok = this.Ok, Cancel = this.Cancel };

        #endregion
    }
}
