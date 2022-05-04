using System;

namespace Winium.Cruciatus.Settings.MessageBoxSettings
{
    /// <summary>
    /// MessageBox dialog window buttons description class.
    /// </summary>
    public class MessageBoxButtonUid : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Cancel button UID.
        /// </summary>
        public string CloseButton { get; set; }

        /// <summary>
        /// Two Ok, Cancel buttons set.
        /// </summary>
        public OkCancelType OkCancelType { get; set; }

        /// <summary>
        /// One Ok button set.
        /// </summary>
        public OkType OkType { get; set; }

        /// <summary>
        /// Three Yes, No, Cancel buttons set.
        /// </summary>
        public YesNoCancelType YesNoCancelType { get; set; }

        /// <summary>
        /// Two Yes, No buttons set.
        /// </summary>
        public YesNoType YesNoType { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public object Clone() =>
            new MessageBoxButtonUid
            {
                CloseButton = this.CloseButton, 
                OkType = (OkType)this.OkType.Clone(), 
                OkCancelType = (OkCancelType)this.OkCancelType.Clone(), 
                YesNoType = (YesNoType)this.YesNoType.Clone(), 
                YesNoCancelType = (YesNoCancelType)this.YesNoCancelType.Clone()
            };

        #endregion
    }
}
