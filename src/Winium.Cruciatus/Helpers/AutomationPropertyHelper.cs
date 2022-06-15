extern alias UIAComWrapper;
using System.Text.RegularExpressions;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus.Helpers
{
    /// <summary>
    /// Properties helper.
    /// </summary>
    internal static class AutomationPropertyHelper
    {
        #region Methods

        /// <summary>
        /// Get pure name from <see cref="Automation::AutomationIdentifier"/> property identifier.
        /// </summary>
        /// <param name="property">Property identifier.</param>
        /// <returns>Pure property name (without "Property" suffix).</returns>
        internal static string GetPropertyName(Automation::AutomationIdentifier property)
        {
            var pattern = new Regex(@".*\.(?<name>.*)Property");
            return pattern.Match(property.ProgrammaticName).Groups["name"].Value;
        }

        #endregion
    }
}
