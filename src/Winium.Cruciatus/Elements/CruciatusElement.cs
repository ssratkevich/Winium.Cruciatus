extern alias UIAComWrapper;
using System;
using System.Collections.Generic;
using Interop.UIAutomationClient;
using NLog;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Exceptions;
using Winium.Cruciatus.Extensions;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus.Elements
{
    /// <summary>
    /// Базовый элемент управления.
    /// </summary>
    public class CruciatusElement : IEquatable<CruciatusElement>
    {
        #region Static Fields

        /// <summary>
        /// Logger instance.
        /// </summary>
        protected static readonly Logger Logger = CruciatusFactory.Logger;

        #endregion

        #region Fields

        private Automation::AutomationElement element;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Copies element instance.
        /// </summary>
        /// <param name="element">
        /// Base element.
        /// </param>
        public CruciatusElement(CruciatusElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.Element = element.Element;
            this.Parent = element;
            this.SearchStrategy = element.SearchStrategy;
        }

        /// <summary>
        /// Create element instance. Do delayed search (if required).
        /// </summary>
        /// <param name="parent">
        /// Parent element.
        /// </param>
        /// <param name="searchStrategy">
        /// Element search strategy.
        /// </param>
        public CruciatusElement(CruciatusElement parent, By searchStrategy)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            this.Parent = parent;
            this.SearchStrategy = searchStrategy;
        }

        /// <summary>
        /// Creates element with given parent, wrapped automation peer and search strategy.
        /// </summary>
        /// <param name="element">Wrapped automation element.</param>
        /// <param name="parent">Parent element.</param>
        /// <param name="searchStrategy">Element search strategy.</param>
        private CruciatusElement(Automation::AutomationElement element, CruciatusElement parent, By searchStrategy)
        {
            if (element == null && (parent == null || searchStrategy == null))
            {
                throw new InvalidOperationException($"Invalid operation. Either {nameof(element)} or {nameof(parent)} and {nameof(searchStrategy)} must be provided.");
            }
            this.Element = element;
            this.Parent = parent;
            this.SearchStrategy = searchStrategy;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Parent element (if exists).
        /// </summary>
        public CruciatusElement Parent { get; internal set; }

        /// <summary>
        /// Element search strategy.
        /// </summary>
        public By SearchStrategy { get; internal set; }

        /// <summary>
        /// Automation element.
        /// </summary>
        public Automation::AutomationElement Element
        {
            get
            {
                if (this.element == null
                    && this.Parent != null
                    && this.SearchStrategy != null)
                {
                    var element = this.Parent.FindElement(this.SearchStrategy);
                    this.element = element != null ? element.Element : null;
                }

                if (this.element == null)
                {
                    throw new NoSuchElementException("ELEMENT NOT FOUND");
                }

                return this.element;
            }

            internal set
            {
                this.element = value;
            }
        }

        /// <summary>
        /// Checks whether element exists.
        /// </summary>
        public bool IsStale
        {
            get
            {
                try
                {
                    this.Element.GetCurrentPropertyValue(Automation::AutomationElement.AutomationIdProperty);
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (Automation::ElementNotAvailableException)
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Element properties.
        /// </summary>
        public CruciatusElementProperties Properties =>
            new CruciatusElementProperties(this.Element);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates element with given parent, wrapped automation peer and search strategy.
        /// </summary>
        /// <param name="element">Wrapped automation element.</param>
        /// <param name="parent">Parent element (may be null if element is not null).</param>
        /// <param name="searchStrategy">Element search strategy (may be null if element is not null).</param>
        /// <returns>
        /// Created element.
        /// </returns>
        public static CruciatusElement Create(Automation::AutomationElement element, CruciatusElement parent, By searchStrategy) =>
            new CruciatusElement(element, parent, searchStrategy);

        /// <summary>
        /// Does click by element.
        /// </summary>
        public void Click() =>
            this.Click(CruciatusFactory.Settings.ClickButton);

        /// <summary>
        /// Does click by element.
        /// </summary>
        /// <param name="button">
        /// Mouse button.
        /// </param>
        public void Click(MouseButton button) =>
            this.Click(button, ClickStrategies.None, false);

        /// <summary>
        /// Does click by element.
        /// </summary>
        /// <param name="button">
        /// Mouse button.
        /// </param>
        /// <param name="strategy">
        /// Click strategy.
        /// </param>
        public void Click(MouseButton button, ClickStrategies strategy) =>
            this.Click(button, strategy, false);

        /// <summary>
        /// Does click by element.
        /// </summary>
        /// <param name="button">
        /// Mouse button.
        /// </param>
        /// <param name="strategy">
        /// Click strategy.
        /// </param>
        /// <param name="doubleClick">
        /// Is double click.
        /// </param>
        public void Click(MouseButton button, ClickStrategies strategy, bool doubleClick)
        {
            if (!this.Element.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Click failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new Automation::ElementNotEnabledException("NOT CLICK");
            }

            if (strategy == ClickStrategies.None)
            {
                strategy = ~strategy;
            }

            if (strategy.HasFlag(ClickStrategies.ClickablePoint))
            {
                if (CruciatusCommand.TryClickOnClickablePoint(button, this, doubleClick))
                {
                    return;
                }
            }

            if (strategy.HasFlag(ClickStrategies.BoundingRectangleCenter))
            {
                if (CruciatusCommand.TryClickOnBoundingRectangleCenter(button, this, doubleClick))
                {
                    return;
                }
            }

            if (strategy.HasFlag(ClickStrategies.InvokePattern))
            {
                if (CruciatusCommand.TryClickUsingInvokePattern(this, doubleClick))
                {
                    return;
                }
            }

            Logger.Error("Click on '{0}' element failed", this.ToString());
            throw new CruciatusException("NOT CLICK");
        }

        /// <summary>
        /// Does double click by element.
        /// </summary>
        public void DoubleClick() =>
            this.DoubleClick(CruciatusFactory.Settings.ClickButton);

        /// <summary>
        /// Does double click by element.
        /// </summary>
        /// <param name="button">
        /// Mouse button.
        /// </param>
        public void DoubleClick(MouseButton button) =>
            this.DoubleClick(button, ClickStrategies.None);

        /// <summary>
        /// Does double click by element.
        /// </summary>
        /// <param name="button">
        /// Mouse button.
        /// </param>
        /// <param name="strategy">
        /// Click strategy.
        /// </param>
        public void DoubleClick(MouseButton button, ClickStrategies strategy) =>
            this.Click(button, strategy, true);

        /// <inheritdoc/>
        public bool Equals(CruciatusElement other)
        {
            return other != null && this.Element.Equals(other.Element);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var cruciatusElement = obj as CruciatusElement;
            return cruciatusElement != null && this.Equals(cruciatusElement);
        }

        /// <summary>
        /// Does element search.
        /// </summary>
        /// <param name="strategy">
        /// Search strategy.
        /// </param>
        /// <returns>
        /// Returns found element or null if nothing found.
        /// </returns>
        public virtual CruciatusElement FindElement(By strategy) =>
            CruciatusCommand.FindFirst(this, strategy);


        /// <summary>
        /// Search element by name.
        /// </summary>
        /// <param name="value">
        /// Element name.
        /// </param>
        /// <returns>
        /// Returns found element or null if nothing found.
        /// </returns>
        public virtual CruciatusElement FindElementByName(string value) =>
            this.FindElement(By.Name(value));

        /// <summary>
        /// Search element by AutomationId.
        /// </summary>
        /// <param name="value">
        /// Element UID.
        /// </param>
        /// <returns>
        /// Returns found element or null if nothing found.
        /// </returns>
        public virtual CruciatusElement FindElementByUid(string value) =>
            this.FindElement(By.Uid(value));

        /// <summary>
        /// Does elements search.
        /// </summary>
        /// <param name="strategy">
        /// Search strategy.
        /// </param>
        /// <returns>
        /// Returns founded elements or null if nothing found.
        /// </returns>
        public IEnumerable<CruciatusElement> FindElements(By strategy) =>
            CruciatusCommand.FindAll(this, strategy);

        /// <inheritdoc/>
        public override int GetHashCode() =>
            this.Element.GetHashCode();

        /// <summary>
        /// Sets the focus on element.
        /// If element is collapsed window then expand it.
        /// </summary>
        public void SetFocus()
        {
            if (!this.Element.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Set focus failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new Automation::ElementNotEnabledException("NOT SET FOCUS");
            }

            if (this.Element.Current.ControlType.Equals(Automation::ControlType.Window))
            {
                object windowPatternObject;
                if (this.Element.TryGetCurrentPattern(Automation::WindowPattern.Pattern, out windowPatternObject))
                {
                    ((Automation::WindowPattern)windowPatternObject).SetWindowVisualState(WindowVisualState.WindowVisualState_Normal);
                    return;
                }
            }

            try
            {
                this.Element.SetFocus();
            }
            catch (InvalidOperationException exception)
            {
                Logger.Error("Set focus on element '{0}' failed.", this.ToString());
                Logger.Debug(exception);
                throw new CruciatusException("NOT SET FOCUS");
            }
        }

        /// <summary>
        /// Set element text.
        /// </summary>
        /// <param name="text">
        /// Text to set.
        /// </param>
        public void SetText(string text)
        {
            if (!this.Element.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Set text failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new Automation::ElementNotEnabledException("NOT SET TEXT");
            }

            this.Click(MouseButton.Left, ClickStrategies.ClickablePoint | ClickStrategies.BoundingRectangleCenter);

            CruciatusFactory.Keyboard.SendCtrlA().SendBackspace().SendText(text);
        }

        /// <summary>
        /// Get element text.
        /// </summary>
        public string Text() =>
            this.Text(GetTextStrategies.None);

        /// <summary>
        /// Get element text with given strategy.
        /// </summary>
        /// <param name="strategy">
        /// Element text getting strategy.
        /// </param>
        public string Text(GetTextStrategies strategy)
        {
            if (strategy == GetTextStrategies.None)
            {
                strategy = ~strategy;
            }

            string text;
            if (strategy.HasFlag(GetTextStrategies.TextPattern))
            {
                if (CruciatusCommand.TryGetTextUsingTextPattern(this, out text))
                {
                    return text;
                }
            }

            if (strategy.HasFlag(GetTextStrategies.ValuePattern))
            {
                if (CruciatusCommand.TryGetTextUsingValuePattern(this, out text))
                {
                    return text;
                }
            }

            Logger.Error("Get text from '{0}' element failed.", this.ToString());
            CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
            throw new CruciatusException("NO GET TEXT");
        }

        /// <summary>
        /// Gets element string representation.
        /// </summary>
        public override string ToString()
        {
            var typeName = this.Element.Current.ControlType.ProgrammaticName;
            var uid = this.Element.Current.AutomationId;
            var name = this.Element.Current.Name;
            var str = string.Format(
                "{0}{1}{2}", 
                "type: " + typeName, 
                string.IsNullOrEmpty(uid) ? string.Empty : ", uid: " + uid, 
                string.IsNullOrEmpty(name) ? string.Empty : ", name: " + name);
            return str;
        }

        #endregion
    }
}
