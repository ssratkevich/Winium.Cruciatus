extern alias UIAComWrapper;
using System.Collections.Generic;
using Interop.UIAutomationClient;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Search strategies helper class.
    /// </summary>
    public abstract class By
    {
        /// <summary>
        /// Search by AutomationProperty.
        /// </summary>
        /// <param name="property">
        /// Target property.
        /// </param>
        /// <param name="value">
        /// Target property value.
        /// </param>
        /// <returns>
        /// Search strategy.
        /// </returns>
        public static ByProperty AutomationProperty(Automation::AutomationProperty property, object value)
        {
            return AutomationProperty(TreeScope.TreeScope_Subtree, property, value);
        }

        /// <summary>
        /// Search by AutomationProperty within given scope context.
        /// </summary>
        /// <param name="scope">
        /// Search scope.
        /// </param>
        /// <param name="property">
        /// Target property.
        /// </param>
        /// <param name="value">
        /// Target property value.
        /// </param>
        /// <returns>
        /// Search strategy.
        /// </returns> 
        public static ByProperty AutomationProperty(TreeScope scope, Automation::AutomationProperty property, object value)
        {
            return new ByProperty(scope, property, value);
        }

        /// <summary>
        /// Search element by its name.
        /// </summary>
        /// <param name="value">
        /// Element name.
        /// </param>
        /// <returns>
        /// Search strategy.
        /// </returns> 
        public static ByProperty Name(string value)
        {
            return AutomationProperty(Automation::AutomationElement.NameProperty, value);
        }

        /// <summary>
        /// Search element by its name within given scope context.
        /// </summary>
        /// <param name="scope">
        /// Search scope.
        /// </param>
        /// <param name="value">
        /// Element name.
        /// </param>
        /// <returns>
        /// Search strategy.
        /// </returns> 
        public static ByProperty Name(TreeScope scope, string value)
        {
            return AutomationProperty(scope, Automation::AutomationElement.NameProperty, value);
        }

        /// <summary>
        /// Search element by AutomationId.
        /// </summary>
        /// <param name="value">
        /// Element UID.
        /// </param>
        /// <returns>
        /// Search strategy.
        /// </returns> 
        public static ByProperty Uid(string value)
        {
            return AutomationProperty(Automation::AutomationElement.AutomationIdProperty, value);
        }

        /// <summary>
        /// Поиск по AutomationId.
        /// </summary>
        /// <param name="scope">
        /// Search scope.
        /// </param>
        /// <param name="value">
        /// Element UID.
        /// </param>
        /// <returns>
        /// Search strategy.
        /// </returns> 
        public static ByProperty Uid(TreeScope scope, string value)
        {
            return AutomationProperty(scope, Automation::AutomationElement.AutomationIdProperty, value);
        }

        /// <summary>
        /// Search element by XPath.
        /// </summary>
        /// <param name="value">
        /// XPath string.
        /// </param>
        /// <returns>
        /// Search strategy.
        /// </returns> 
        public static ByXPath XPath(string value)
        {
            return new ByXPath(value);
        }

        /// <summary>
        /// Element search strategy string representation.
        /// </summary>
        public abstract override string ToString();

        /// <summary>
        /// Find all elements with this strategy starting with given element.
        /// </summary>
        /// <param name="parent">Starting element.</param>
        /// <param name="timeout">Search time threshold.</param>
        /// <returns>Collection of found elements (may be null).</returns>
        public abstract IEnumerable<Automation::AutomationElement> FindAll(Automation::AutomationElement parent, int timeout);

        /// <summary>
        /// Find first element with this strategy starting with given element.
        /// </summary>
        /// <param name="parent">Starting element.</param>
        /// <param name="timeout">Search time threshold.</param>
        /// <returns>Found element (may be null).</returns>
        public abstract Automation::AutomationElement FindFirst(Automation::AutomationElement parent, int timeout);
    }
}
