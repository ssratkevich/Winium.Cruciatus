using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Automation;
using WindowsInput;
using Winium.Cruciatus.Elements;
using Winium.Cruciatus.Exceptions;

namespace Winium.Cruciatus.Extensions
{
    /// <summary>
    /// Extensions for <see cref="CruciatusElement"/>.
    /// </summary>
    public static class CruciatusElementExtension
    {
        /// <summary>
        /// Click to element with Ctrl key.
        /// </summary>
        public static void ClickWithPressedCtrl(this CruciatusElement element) =>
            ClickWithPressedKeys(element, new List<VirtualKeyCode> { VirtualKeyCode.CONTROL });

        /// <summary>
        /// Click to element with pressed buttons.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="keys">Pressed keys list.</param>
        public static void ClickWithPressedKeys(this CruciatusElement element, List<VirtualKeyCode> keys)
        {
            keys.ForEach(key => CruciatusFactory.Keyboard.KeyDown(key));
            element.Click();
            keys.ForEach(key => CruciatusFactory.Keyboard.KeyUp(key));
        }

        /// <summary>
        /// Get element property value.
        /// </summary>
        /// <param name="cruciatusElement">Element.</param>
        /// <param name="property">Target property.</param>
        /// <returns>Property value.</returns>
        public static TOut GetAutomationPropertyValue<TOut>(
            this CruciatusElement cruciatusElement, 
            AutomationProperty property)
        {
            try
            {
                return cruciatusElement.Element.GetPropertyValue<TOut>(property);
            }
            catch (NotSupportedException)
            {
                var msg = string.Format("Element '{0}' not support '{1}'", cruciatusElement, property.ProgrammaticName);
                CruciatusFactory.Logger.Error(msg);

                throw new CruciatusException("GET PROPERTY VALUE FAILED");
            }
            catch (InvalidCastException invalidCastException)
            {
                var msg = string.Format("Invalid cast from '{0}' to '{1}'.", invalidCastException.Message, typeof(TOut));
                CruciatusFactory.Logger.Error(msg);

                throw new CruciatusException("GET PROPERTY VALUE FAILED");
            }
        }

        /// <summary>
        /// Gets required automation pattern.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="pattern">Required automation pattern (ex: ExpandCollapsePattern.Pattern).</param>
        /// <typeparam name="T">Pattern type.</typeparam>
        /// <returns>Automation pattern.</returns>
        public static T GetPattern<T>(this CruciatusElement element, AutomationPattern pattern) where T : class =>
            element.Element.GetPattern<T>(pattern);

        /// <summary>
        /// Try to get required automation pattern.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="pattern">Required automation pattern (ex: ExpandCollapsePattern.Pattern).</param>
        /// <typeparam name="T">Pattern type.</typeparam>
        /// <returns>Automation pattern or null, if element not support pattern.</returns>
        public static T TryGetPattern<T>(this CruciatusElement element, AutomationPattern pattern) where T : class =>
            element.Element.TryGetPattern<T>(pattern);

        /// <summary>
        /// Scroll given item into view.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="scrollableParent">Scrollable parent (optional, but recommended).</param>
        public static void ScrollIntoView(this CruciatusElement element, CruciatusElement scrollableParent = null) =>
            element.Element.ScrollIntoView(scrollableParent?.Element);

        /// <summary>
        /// Convert to <see cref="CheckBox"/>.
        /// </summary>
        /// <returns><see cref="CheckBox"/></returns>
        public static CheckBox ToCheckBox(this CruciatusElement element) =>
            new CheckBox(element);

        /// <summary>
        /// Convert to <see cref="ComboBox"/>.
        /// </summary>
        /// <returns><see cref="ComboBox"/></returns>
        public static ComboBox ToComboBox(this CruciatusElement element) =>
            new ComboBox(element);

        /// <summary>
        /// Convert to <see cref="DataGrid"/>.
        /// </summary>
        /// <returns><see cref="DataGrid"/></returns>
        public static DataGrid ToDataGrid(this CruciatusElement element) =>
            new DataGrid(element);

        /// <summary>
        /// Convert to <see cref="ListBox"/>.
        /// </summary>
        /// <returns><see cref="ListBox"/></returns>
        public static ListBox ToListBox(this CruciatusElement element) =>
            new ListBox(element);

        /// <summary>
        /// Convert to <see cref="Menu"/>.
        /// </summary>
        /// <returns><see cref="Menu"/></returns>
        public static Menu ToMenu(this CruciatusElement element) =>
            new Menu(element);
    }
}
