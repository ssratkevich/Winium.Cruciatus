using System;

namespace Winium.Cruciatus.Settings.MessageBoxSettings
{
    /// <summary>
    /// Ok button description class.
    /// </summary>
    public class OkType : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Ok button UID.
        /// </summary>
        public string Ok { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public object Clone() =>
            new OkType { Ok = this.Ok };

        #endregion
    }
}
