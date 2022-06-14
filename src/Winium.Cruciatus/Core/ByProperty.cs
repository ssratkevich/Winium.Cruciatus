extern alias UIAComWrapper;
using System.Collections.Generic;
using System.Text;
using Interop.UIAutomationClient;
using Winium.Cruciatus.Exceptions;
using Winium.Cruciatus.Helpers;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Conditions enum.
    /// </summary>
    internal enum ConditionType
    {
        /// <summary>
        /// No conditions
        /// </summary>
        None,
        /// <summary>
        /// Logical Or.
        /// </summary>
        Or,
        /// <summary>
        /// Logical And.
        /// </summary>
        And
    }

    /// <summary>
    /// Search information for property, condition and value.
    /// </summary>
    internal struct Info
    {
        #region Fields

        /// <summary>
        /// Condition type (None is default).
        /// </summary>
        internal ConditionType ConditionType;
        /// <summary>
        /// Target property description.
        /// </summary>
        internal Automation::AutomationProperty Property;
        /// <summary>
        /// Property value.
        /// </summary>
        internal object Value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates property info.
        /// </summary>
        /// <param name="property">Target property.</param>
        /// <param name="value">Target property value.</param>
        /// <param name="conditionType">Condition type.</param>
        internal Info(Automation::AutomationProperty property, object value, ConditionType conditionType)
        {
            this.Property = property;
            this.Value = value;
            this.ConditionType = conditionType;
        }

        #endregion
    }

    /// <summary>
    /// Element search strategy by automation property.
    /// </summary>
    public class ByProperty : By
    {
        #region Fields

        /// <summary>
        /// List of properties and conditions.
        /// </summary>
        private readonly List<Info> infoList;

        /// <summary>
        /// Search scope.
        /// </summary>
        private readonly TreeScope scope;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates simple property search strategy.
        /// </summary>
        /// <param name="scope">Search scope.</param>
        /// <param name="property">Target property.</param>
        /// <param name="value">Target property value.</param>
        internal ByProperty(TreeScope scope, Automation::AutomationProperty property, object value)
        {
            this.scope = scope;
            this.infoList = new List<Info> { new Info(property, value, ConditionType.None) };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add search condition with logical And operator.
        /// </summary>
        /// <param name="property">
        /// Target property.
        /// </param>
        /// <param name="value">
        /// Target property value.
        /// </param>
        /// <returns>
        /// Search strategy (for chaining).
        /// </returns>
        public ByProperty And(Automation::AutomationProperty property, object value)
        {
            this.infoList.Add(new Info(property, value, ConditionType.And));
            return this;
        }

        /// <summary>
        /// Add search condition with logical And operator.
        /// </summary>
        /// <param name="value">
        /// Element type.
        /// </param>
        public ByProperty AndType(Automation::ControlType value)
        {
            this.And(Automation::AutomationElement.ControlTypeProperty, value);
            return this;
        }

        /// <summary>
        /// Add search condition with logical Or operator.
        /// </summary>
        /// <param name="property">
        /// Target property.
        /// </param>
        /// <param name="value">
        /// Target property value.
        /// </param>
        /// <returns>
        /// Search strategy (for chaining).
        /// </returns>
        public ByProperty Or(Automation::AutomationProperty property, object value)
        {
            this.infoList.Add(new Info(property, value, ConditionType.Or));
            return this;
        }

        /// <summary>
        /// Add search condition for element name with logical Or operator.
        /// </summary>
        /// <param name="value">
        /// Element name.
        /// </param>
        /// <returns>
        /// Search strategy (for chaining).
        /// </returns>
        public ByProperty OrName(string value)
        {
            this.Or(Automation::AutomationElement.NameProperty, value);
            return this;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder builder = new();
            var info = this.infoList[0];
            builder.AppendFormat("{0}: {1}", AutomationPropertyHelper.GetPropertyName(info.Property), info.Value);
            for (var i = 1; i < this.infoList.Count; ++i)
            {
                info = this.infoList[i];
                builder.Insert(0, '(');
                builder.Append(')');
                builder.AppendFormat("{0} {1}: {2}",
                    info.ConditionType.ToString().ToLower(),
                    AutomationPropertyHelper.GetPropertyName(info.Property),
                    info.Value);
            }

            return builder.ToString();
        }

        /// <inheritdoc/>
        public override IEnumerable<Automation::AutomationElement> FindAll(Automation::AutomationElement parent, int timeout) =>
            AutomationElementHelper.FindAll(parent, this.scope, this.GetCondition(), timeout);

        /// <inheritdoc/>
        public override Automation::AutomationElement FindFirst(Automation::AutomationElement parent, int timeout) =>
            AutomationElementHelper.FindFirst(parent, this.scope, this.GetCondition(), timeout);

        #endregion

        #region Private methods

        /// <summary>
        /// Constructs element search condition (simple or complex).
        /// </summary>
        /// <returns>Element search condition.</returns>
        /// <exception cref="CruciatusException">Condition error.</exception>
        private Automation::Condition GetCondition()
        {
            var info = this.infoList[0];
            Automation::Condition result = new Automation::PropertyCondition(info.Property, info.Value);
            for (var i = 1; i < this.infoList.Count; ++i)
            {
                info = this.infoList[i];
                var condition = new Automation::PropertyCondition(info.Property, info.Value);
                result = info.ConditionType switch
                {
                    ConditionType.And => new Automation::AndCondition(result, condition),
                    ConditionType.Or => new Automation::OrCondition(result, condition),
                    _ => throw new CruciatusException("ConditionType ERROR"),
                };
            }

            return result;
        }

        #endregion
    }
}
