using System.Windows.Automation;
using System.Xml.XPath;

namespace Winium.Cruciatus.Helpers.XPath
{
    /// <summary>
    /// Root element.
    /// </summary>
    public class RootItem : ElementItem
    {
        #region Constructors and Destructors

        /// <summary>
        /// Create xpath element for desktop.
        /// </summary>
        public RootItem()
            : base(AutomationElement.RootElement)
        {
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override bool IsEmptyElement =>
            false;

        /// <inheritdoc/>
        public override string Name =>
            "Desktop Window";

        /// <inheritdoc/>
        public override XPathNodeType NodeType =>
            XPathNodeType.Root;

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override XPathItem MoveToParent() =>
            null;

        /// <inheritdoc/>
        public override XPathItem MoveToNext() =>
            null;

        /// <inheritdoc/>
        public override XPathItem MoveToPrevious() => null;

        #endregion
    }
}
