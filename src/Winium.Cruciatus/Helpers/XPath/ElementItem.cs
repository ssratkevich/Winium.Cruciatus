using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using System.Xml.XPath;
using Winium.Cruciatus.Elements;
using Winium.Cruciatus.Exceptions;

namespace Winium.Cruciatus.Helpers.XPath
{
    /// <summary>
    /// Element class.
    /// </summary>
    public class ElementItem : XPathItem
    {
        #region Fields

        private readonly AutomationElement element;

        private readonly TreeWalker treeWalker = TreeWalker.ControlViewWalker;

        private List<AutomationProperty> properties;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Create new xpath element from automation element.
        /// </summary>
        /// <param name="element">Automation element.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ElementItem(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override bool IsEmptyElement
        {
            get
            {
                var hasChild = this.MoveToFirstChild() != null;
                if (hasChild)
                {
                    return false;
                }

                try
                {
                    CruciatusElement.Create(this.element, null, null).Text();
                    return false;
                }
                catch (CruciatusException)
                {
                    return true;
                }
            }
        }

        /// <inheritdoc/>
        public override string Name =>
            this.element.Current.Name;

        /// <inheritdoc/>
        public override XPathNodeType NodeType =>
            XPathNodeType.Element;

        /// <inheritdoc/>
        public override object TypedValue =>
            this.element;

        /// <summary>
        /// Supported properties.
        /// </summary>
        public List<AutomationProperty> SupportedProperties =>
            this.properties ??= this.element.GetSupportedProperties().ToList();
            
        #endregion

        #region Methods

        /// <summary>
        /// Create xpath element from automation one.
        /// </summary>
        /// <param name="instance">Automation element.</param>
        /// <returns></returns>
        public static XPathItem Create(AutomationElement instance) =>
            instance.Equals(AutomationElement.RootElement)
            ? new RootItem()
            : new ElementItem(instance);

        /// <summary>
        /// Get next property if it exists.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Next property if it exists.</returns>
        public AutomationProperty GetNextPropertyOrNull(AutomationProperty property)
        {
            var index = this.SupportedProperties.IndexOf(property);
            return this.SupportedProperties.ElementAtOrDefault(index + 1);
        }

        /// <summary>
        /// Get value of the property.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Value of the property.</returns>
        public object GetPropertyValue(AutomationProperty property) =>
            this.element.GetCurrentPropertyValue(property);

        /// <inheritdoc/>
        public override bool IsSamePosition(XPathItem item)
        {
            var obj = item as ElementItem;
            return obj != null && obj.element.Equals(this.element);
        }

        /// <inheritdoc/>
        public override XPathItem MoveToParent()
        {
            var parent = this.treeWalker.GetParent(this.element);
            return (parent == null) ? null : Create(parent);
        }

        /// <inheritdoc/>
        public override XPathItem MoveToFirstChild()
        {
            var firstChild = this.treeWalker.GetFirstChild(this.element);
            return (firstChild == null) ? null : Create(firstChild);
        }

        /// <inheritdoc/>
        public override XPathItem MoveToNext()
        {
            var next = this.treeWalker.GetNextSibling(this.element);
            return (next == null) ? null : Create(next);
        }

        /// <inheritdoc/>
        public override XPathItem MoveToPrevious()
        {
            var previous = this.treeWalker.GetPreviousSibling(this.element);
            return (previous == null) ? null : Create(previous);
        }

        /// <inheritdoc/>
        public override XPathItem MoveToFirstProperty() =>
            this.SupportedProperties.Any()
            ? new PropertyItem(this, this.SupportedProperties[0])
            : null;

        /// <inheritdoc/>
        public override XPathItem MoveToNextProperty() =>
            null;

        #endregion
    }
}
