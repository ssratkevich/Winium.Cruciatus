using System.Windows.Automation;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Extensions;

namespace Winium.Cruciatus.Elements
{
    /// <summary>
    /// Represents <see cref="Microsoft.Win32.SaveFileDialog"/> window.
    /// </summary>
    public class SaveFileDialog : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="element">Wrapped element.</param>
        public SaveFileDialog(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="parent">Parent element.</param>
        /// <param name="searchStrategy">Search strategy.</param>
        public SaveFileDialog(CruciatusElement parent, By searchStrategy)
            : base(parent, searchStrategy)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Cancel button
        /// </summary>
        public CruciatusElement CancelButton =>
            this.FindElement(By.Uid(TreeScope.Children,
                CruciatusFactory.Settings.SaveFileDialogUid.CancelButton));
            
        /// <summary>
        /// Gets file name combo box.
        /// </summary>
        public ComboBox FileNameComboBox =>
            this.FindElement(By.Uid(TreeScope.Subtree,
                CruciatusFactory.Settings.SaveFileDialogUid.FileNameEditableComboBox))
            .ToComboBox();

        /// <summary>
        /// Gets file type combo box.
        /// </summary>
        public ComboBox FileTypeComboBox =>
            this.FindElement(By.Uid(TreeScope.Subtree,
                CruciatusFactory.Settings.SaveFileDialogUid.FileTypeComboBox))
            .ToComboBox();

        /// <summary>
        /// Gets Save button.
        /// </summary>
        public CruciatusElement SaveButton =>
            this.FindElement(By.Uid(TreeScope.Children,
                CruciatusFactory.Settings.SaveFileDialogUid.SaveButton));

        #endregion
    }
}
