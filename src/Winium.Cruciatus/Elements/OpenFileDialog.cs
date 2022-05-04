using System.Windows.Automation;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Extensions;

namespace Winium.Cruciatus.Elements
{
    /// <summary>
    /// Represents <see cref="Microsoft.Win32.OpenFileDialog"/> window.
    /// </summary>
    public class OpenFileDialog : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="element">Wrapped element.</param>
        public OpenFileDialog(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="parent">Parent element.</param>
        /// <param name="searchStrategy">Search strategy.</param>
        public OpenFileDialog(CruciatusElement parent, By searchStrategy)
            : base(parent, searchStrategy)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Cancel button.
        /// </summary>
        public CruciatusElement CancelButton =>
            this.FindElement(By.Uid(TreeScope.Children,
                CruciatusFactory.Settings.OpenFileDialogUid.CancelButton));

        /// <summary>
        /// Gets file name combobox.
        /// </summary>
        public ComboBox FileNameComboBox =>
            this.FindElement(By.Uid(TreeScope.Children,
                CruciatusFactory.Settings.OpenFileDialogUid.FileNameEditableComboBox))
            .ToComboBox();

        /// <summary>
        /// Gets Open button.
        /// </summary>
        public CruciatusElement OpenButton =>
            this.FindElement(By.Uid(TreeScope.Children,
                CruciatusFactory.Settings.OpenFileDialogUid.OpenButton));

        #endregion
    }
}
