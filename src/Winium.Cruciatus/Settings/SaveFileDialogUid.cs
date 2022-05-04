using System;

namespace Winium.Cruciatus.Settings
{
    /// <summary>
    /// SaveFileDialog elements descriptor.
    /// </summary>
    public class SaveFileDialogUid : ICloneable
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
        /// File type combobox UID.
        /// </summary>
        public string FileTypeComboBox { get; set; }

        /// <summary>
        /// Save button UID.
        /// </summary>
        public string SaveButton { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public object Clone() =>
            new SaveFileDialogUid
            {
                SaveButton = this.SaveButton, 
                CancelButton = this.CancelButton, 
                FileNameEditableComboBox = this.FileNameEditableComboBox, 
                FileTypeComboBox = this.FileTypeComboBox
            };

        #endregion
    }
}
