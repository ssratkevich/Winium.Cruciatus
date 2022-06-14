extern alias UIAComWrapper;
using System.Collections.Generic;
using System.Linq;
using Winium.Cruciatus.Helpers;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// XPath search strategy.
    /// </summary>
    public class ByXPath : By
    {
        #region Fields

        /// <summary>
        /// XPath search string.
        /// </summary>
        private readonly string xpath;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates new xpath strategy instance.
        /// </summary>
        /// <param name="xpath">XPath search string.</param>
        internal ByXPath(string xpath)
        {
            this.xpath = xpath;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override string ToString() =>
            this.xpath;

        /// <inheritdoc/>
        public override IEnumerable<Automation::AutomationElement> FindAll(Automation::AutomationElement parent, int timeout) =>
            AutomationElementHelper.FindAll(parent, this.xpath, timeout);

        /// <inheritdoc/>
        public override Automation::AutomationElement FindFirst(Automation::AutomationElement parent, int timeout) =>
            AutomationElementHelper.FindAll(parent, this.xpath, timeout).FirstOrDefault();

        #endregion
    }
}
