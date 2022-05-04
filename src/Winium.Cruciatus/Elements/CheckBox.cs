using System.Windows.Automation;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Extensions;

namespace Winium.Cruciatus.Elements
{
    /// <summary>
    /// Represents Check box element.
    /// </summary>
    public class CheckBox : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates new instance of CheckBox.
        /// </summary>
        /// <param name="element">Wrapped element.</param>
        public CheckBox(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Creates new instance of CheckBox.
        /// </summary>
        /// <param name="parent">Parent element.</param>
        /// <param name="searchStrategy">Search strategy.</param>
        public CheckBox(CruciatusElement parent, By searchStrategy)
            : base(parent, searchStrategy)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Is checked property.
        /// Actually must be bool? (null for intermediate state).
        /// </summary>
        public bool IsToggleOn =>
            this.ToggleState == ToggleState.On;

        #endregion

        #region Properties

        /// <summary>
        /// Toggle state.
        /// </summary>
        internal ToggleState ToggleState =>
            this.GetAutomationPropertyValue<ToggleState>(TogglePattern.ToggleStateProperty);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Set to checked.
        /// </summary>
        public void Check()
        {
            if (this.IsToggleOn)
            {
                return;
            }
            // TODO: Use TogglePattern, because intermediate state is totally ignored.

            this.Click();
        }

        /// <summary>
        /// Set to unchecked.
        /// </summary>
        public void Uncheck()
        {
            if (!this.IsToggleOn)
            {
                return;
            }
            // TODO: Use TogglePattern, because intermediate state is totally ignored.

            this.Click();
        }

        #endregion
    }
}
