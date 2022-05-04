using System;
using System.Windows.Automation;
using System.Xml;
using System.Xml.XPath;

namespace Winium.Cruciatus.Helpers.XPath
{
    /// <summary>
    /// Navigator for automation tree.
    /// </summary>
    public class DesktopTreeXPathNavigator : XPathNavigator
    {
        #region Fields

        private XPathItem item;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Create new navigator instance (for root element).
        /// </summary>
        public DesktopTreeXPathNavigator()
            : this(AutomationElement.RootElement)
        {
        }

        /// <summary>
        /// Create navigator instance for given automation element.
        /// </summary>
        /// <param name="element">Automation element.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DesktopTreeXPathNavigator(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.item = ElementItem.Create(element);
        }

        /// <summary>
        /// Create navigator instance for given xpath element.
        /// </summary>
        /// <param name="item">XPath element.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DesktopTreeXPathNavigator(XPathItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.item = item;
        }

        #endregion

        #region Public Properties

        /// <inheritdoc/>
        public override string BaseURI =>
            string.Empty;

        /// <inheritdoc/>
        public override bool IsEmptyElement =>
            this.item.IsEmptyElement;

        /// <inheritdoc/>
        public override string LocalName =>
            this.Name;

        /// <inheritdoc/>
        public override string Name =>
            this.item.Name;

        /// <inheritdoc/>
        public override XmlNameTable NameTable =>
            null;

        /// <inheritdoc/>
        public override string NamespaceURI =>
            string.Empty;

        /// <inheritdoc/>
        public override XPathNodeType NodeType =>
            this.item.NodeType;

        /// <inheritdoc/>
        public override string Prefix =>
            string.Empty;

        /// <inheritdoc/>
        public override object TypedValue =>
            this.item.TypedValue;

        /// <inheritdoc/>
        public override string Value =>
            this.item.Value;

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override XPathNavigator Clone() =>
            new DesktopTreeXPathNavigator(this.item);

        /// <inheritdoc/>
        public override bool IsSamePosition(XPathNavigator other)
        {
            var obj = other as DesktopTreeXPathNavigator;
            return obj != null && obj.item.IsSamePosition(this.item);
        }

        /// <inheritdoc/>
        public override bool MoveTo(XPathNavigator other)
        {
            var obj = other as DesktopTreeXPathNavigator;
            if (obj == null)
            {
                return false;
            }

            this.item = obj.item;
            return true;
        }

        /// <inheritdoc/>
        public override bool MoveToFirstAttribute() =>
            this.MoveToItem(this.item.MoveToFirstProperty());

        /// <inheritdoc/>
        public override bool MoveToFirstChild() =>
            this.MoveToItem(this.item.MoveToFirstChild());

        /// <inheritdoc/>
        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope) =>
            false;

        /// <inheritdoc/>
        public override bool MoveToId(string id) =>
            false;

        /// <inheritdoc/>
        public override bool MoveToNext() =>
            this.MoveToItem(this.item.MoveToNext());

        /// <inheritdoc/>
        public override bool MoveToNextAttribute() =>
            this.MoveToItem(this.item.MoveToNextProperty());

        /// <inheritdoc/>
        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope) =>
            false;

        /// <inheritdoc/>
        public override bool MoveToParent() =>
            this.MoveToItem(this.item.MoveToParent());

        /// <inheritdoc/>
        public override bool MoveToPrevious() =>
            this.MoveToItem(this.item.MoveToPrevious());

        #endregion

        #region Methods

        private bool MoveToItem(XPathItem newItem)
        {
            if (newItem == null)
            {
                return false;
            }

            this.item = newItem;
            return true;
        }

        #endregion
    }
}
