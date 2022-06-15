extern alias UIAComWrapper;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Extensions;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus.Elements
{
    /// <summary>
    /// Represents Tab item.
    /// </summary>
    public class TabItem : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates tab item element.
        /// </summary>
        /// <param name="parent">Parent element.</param>
        /// <param name="searchStrategy">Search strategy.</param>
        public TabItem(CruciatusElement parent, By searchStrategy)
            : base(parent, searchStrategy)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Is tab selected.
        /// </summary>
        public bool IsSelection =>
            this.GetAutomationPropertyValue<bool>(Automation::SelectionItemPattern.IsSelectedProperty);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Select tab.
        /// </summary>
        public void Select()
        {
            if (this.IsSelection)
            {
                return;
            }

            this.Click();
        }

        #endregion
    }
}
