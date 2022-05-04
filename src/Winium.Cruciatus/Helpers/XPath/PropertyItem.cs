using System.Windows.Automation;
using System.Xml.XPath;

namespace Winium.Cruciatus.Helpers.XPath
{
    /// <summary>
    /// Property item for xpath.
    /// </summary>
    public class PropertyItem : XPathItem
    {
        #region Fields

        private readonly ElementItem parent;

        private readonly AutomationProperty property;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Create xpath property wrapper for given element and automation property.
        /// </summary>
        /// <param name="parent">Parent element.</param>
        /// <param name="property">Property.</param>
        public PropertyItem(ElementItem parent, AutomationProperty property)
        {
            this.parent = parent;
            this.property = property;
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override bool IsEmptyElement =>
            false;

        /// <inheritdoc/>
        public override string Name =>
            AutomationPropertyHelper.GetPropertyName(this.property);

        /// <inheritdoc/>
        public override XPathNodeType NodeType =>
            XPathNodeType.Attribute;

        /// <inheritdoc/>
        public override string Value
        {
            get
            {
                var value = this.TypedValue;
                var type = value as ControlType;
                return type != null ? type.ProgrammaticName : value.ToString();
            }
        }

        /// <inheritdoc/>
        public override object TypedValue =>
            this.parent.GetPropertyValue(this.property);

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override bool IsSamePosition(XPathItem item)
        {
            var obj = item as PropertyItem;
            return obj != null && obj.parent == this.parent && obj.property.Equals(this.property);
        }

        /// <inheritdoc/>
        public override XPathItem MoveToParent() =>
            this.parent;

        /// <inheritdoc/>
        public override XPathItem MoveToFirstChild() =>
            null;

        /// <inheritdoc/>
        public override XPathItem MoveToNext() =>
            null;

        /// <inheritdoc/>
        public override XPathItem MoveToPrevious() =>
            null;

        /// <inheritdoc/>
        public override XPathItem MoveToFirstProperty() =>
            null;

        /// <inheritdoc/>
        public override XPathItem MoveToNextProperty()
        {
            var nextProperty = this.parent.GetNextPropertyOrNull(this.property);
            return (nextProperty == null) ? null : new PropertyItem(this.parent, nextProperty);
        }

        #endregion
    }
}
