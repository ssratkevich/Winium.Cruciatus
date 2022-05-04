using System.Xml.XPath;

namespace Winium.Cruciatus.Helpers.XPath
{
    /// <summary>
    /// XPath item base class.
    /// </summary>
    public abstract class XPathItem
    {
        #region Properties

        /// <summary>
        /// Checks element is empty.
        /// </summary>
        public abstract bool IsEmptyElement { get; }

        /// <summary>
        /// Name of element.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Type of node.
        /// </summary>
        public abstract XPathNodeType NodeType { get; }

        /// <summary>
        /// Node value string representation.
        /// </summary>
        public virtual string Value =>
            string.Empty;

        /// <summary>
        /// Node value.
        /// </summary>
        public abstract object TypedValue { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether items are equals.
        /// </summary>
        /// <param name="item">Target item.</param>
        /// <returns></returns>
        public abstract bool IsSamePosition(XPathItem item);


        /// <summary>
        /// Get parent element.
        /// </summary>
        /// <returns>Parent element.</returns>
        public abstract XPathItem MoveToParent();

        /// <summary>
        /// Get first child item.
        /// </summary>
        /// <returns>First child item or null.</returns>
        public abstract XPathItem MoveToFirstChild();

        /// <summary>
        /// Get next sibling element.
        /// </summary>
        /// <returns>Next sibling element</returns>
        public abstract XPathItem MoveToNext();

        /// <summary>
        /// Get previous sibling element.
        /// </summary>
        /// <returns>Previous sibling element.</returns>
        public abstract XPathItem MoveToPrevious();

        /// <summary>
        /// Get first property of item.
        /// </summary>
        /// <returns>First property of item.</returns>
        public abstract XPathItem MoveToFirstProperty();

        /// <summary>
        /// Get next sibling property.
        /// </summary>
        /// <returns>Next sibling propert</returns>
        public abstract XPathItem MoveToNextProperty();

        #endregion
    }
}
