using System;

namespace Winium.Cruciatus.Settings
{
    /// <summary>
    /// OpenFileDialog elements descriptor.
    /// </summary>
    public class OpenFileDialogUid : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Cancel button UID.
        /// </summary>
        public string CancelButton { get; set; }

        /// <summary>
        /// File name combobox UID.
        /// </summary>
        public string FileNameEditableComboBox { get; set; }

        /// <summary>
        /// Open button UID.
        /// </summary>
        public string OpenButton { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public object Clone() =>
            new OpenFileDialogUid
            {
                OpenButton = this.OpenButton, 
                CancelButton = this.CancelButton, 
                FileNameEditableComboBox = this.FileNameEditableComboBox
            };

        #endregion
    }
}
