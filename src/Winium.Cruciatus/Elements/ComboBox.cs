extern alias UIAComWrapper;
using System.Linq;
using System.Threading;
using Interop.UIAutomationClient;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Exceptions;
using Winium.Cruciatus.Extensions;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus.Elements
{
    /// <summary>
    /// Represents Combo box element.
    /// </summary>
    public class ComboBox : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates new instance of combo box.
        /// </summary>
        /// <param name="element">Wrapped element.</param>
        public ComboBox(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Creates new instance of combo box.
        /// </summary>
        /// <param name="parent">Parent element.</param>
        /// <param name="searchStrategy">Search strategy.</param>
        public ComboBox(CruciatusElement parent, By searchStrategy)
            : base(parent, searchStrategy)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// ComboBox is in expanded state.
        /// </summary>
        public bool IsExpanded =>
            this.ExpandCollapseState == ExpandCollapseState.ExpandCollapseState_Expanded;

        #endregion

        #region Properties

        /// <summary>
        /// ComboBox expanded state.
        /// </summary>
        internal ExpandCollapseState ExpandCollapseState =>
            this.GetAutomationPropertyValue<ExpandCollapseState>(
                Automation::ExpandCollapsePattern.ExpandCollapseStateProperty);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Collapse by click.
        /// </summary>
        public void Collapse() =>
            this.Collapse(ExpandStrategy.Click);

        /// <summary>
        /// Collapse by strategy.
        /// </summary>
        /// <param name="strategy">Strategy.</param>
        public void Collapse(ExpandStrategy strategy)
        {
            if (this.ExpandCollapseState == ExpandCollapseState.ExpandCollapseState_Collapsed)
            {
                return;
            }

            switch (strategy)
            {
                case ExpandStrategy.Click:
                    this.Click();
                    break;
                case ExpandStrategy.ExpandCollapsePattern:
                    this.Element.GetPattern<Automation::ExpandCollapsePattern>(Automation::ExpandCollapsePattern.Pattern).Collapse();
                    break;
                default:
                    Logger.Error("{0} is not valid or implemented collapse strategy.", strategy);
                    throw new CruciatusException("NOT COLLAPSE");
            }

            Thread.Sleep(250);
        }

        /// <summary>
        /// Expand by click.
        /// </summary>
        public void Expand() =>
            this.Expand(ExpandStrategy.Click);

        /// <summary>
        /// Expand by strategy.
        /// </summary>
        /// <param name="strategy">Strategy.</param>
        public void Expand(ExpandStrategy strategy)
        {
            if (this.ExpandCollapseState == ExpandCollapseState.ExpandCollapseState_Expanded)
            {
                return;
            }

            switch (strategy)
            {
                case ExpandStrategy.Click:
                    this.Click();
                    break;
                case ExpandStrategy.ExpandCollapsePattern:
                    this.Element.GetPattern<Automation::ExpandCollapsePattern>(Automation::ExpandCollapsePattern.Pattern).Expand();
                    break;
                default:
                    Logger.Error("{0} is not valid or implemented expand strategy.", strategy);
                    throw new CruciatusException("NOT EXPAND");
            }

            Thread.Sleep(250);
        }

        /// <summary>
        /// Scrolls list to needed item.
        /// </summary>
        /// <param name="getStrategy">Search strategy.</param>
        /// <returns>Item or null.</returns>
        public CruciatusElement ScrollTo(By getStrategy)
        {
            if (!this.Element.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Scroll failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new Automation::ElementNotEnabledException("NOT SCROLL");
            }

            // Проверка, что выпадающий список раскрыт
            if (this.ExpandCollapseState != ExpandCollapseState.ExpandCollapseState_Expanded)
            {
                Logger.Error("Element {0} is not opened.", this);
                throw new CruciatusException("NOT SCROLL");
            }

            var scrollPattern = this.Element.GetCurrentPattern(Automation::ScrollPattern.Pattern) as Automation::ScrollPattern;
            if (scrollPattern == null)
            {
                Logger.Error("{0} does not support ScrollPattern.", this);
                throw new CruciatusException("NOT SCROLL");
            }

            // Стартовый поиск элемента
            var element = CruciatusCommand.FindFirst(this, getStrategy, 1000);

            // Вертикальная прокрутка (при необходимости и возможности)
            if (element == null && scrollPattern.Current.VerticallyScrollable)
            {
                // Установка самого верхнего положения прокрутки
                while (scrollPattern.Current.VerticalScrollPercent > 0.1)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.ScrollAmount_LargeDecrement);
                }

                // Установка самого левого положения прокрутки (при возможности)
                if (scrollPattern.Current.HorizontallyScrollable)
                {
                    while (scrollPattern.Current.HorizontalScrollPercent > 0.1)
                    {
                        scrollPattern.ScrollHorizontal(ScrollAmount.ScrollAmount_LargeDecrement);
                    }
                }

                // Основная вертикальная прокрутка
                while (element == null && scrollPattern.Current.VerticalScrollPercent < 99.9)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.ScrollAmount_LargeIncrement);
                    element = CruciatusCommand.FindFirst(this, getStrategy, 1000);
                }
            }

            // Если прокрутив до конца элемент не найден, то его нет (кэп)
            if (element == null)
            {
                Logger.Debug("No elements matching {1} were found in {0}.", this, getStrategy);
                return null;
            }

            var strategy =
                By.AutomationProperty(TreeScope.TreeScope_Subtree, Automation::AutomationElement.ClassNameProperty, "Popup")
                    .And(Automation::AutomationElement.ProcessIdProperty, this.Element.Current.ProcessId);
            var popupWindow = CruciatusFactory.Root.FindElement(strategy);
            if (popupWindow == null)
            {
                Logger.Error("Popup window of drop-down list was not found.");
                throw new CruciatusException("NOT SCROLL");
            }

            // Если точка клика элемента под границей списка - докручиваем по вертикали вниз
            var popupWindowInstance = popupWindow.Element;
            while (element.Element.IsClickablePointLower(popupWindowInstance, scrollPattern))
            {
                scrollPattern.ScrollVertical(ScrollAmount.ScrollAmount_LargeIncrement);
            }

            // Если точка клика элемента над границей списка - докручиваем по вертикали вверх
            while (element.Element.IsClickablePointUpper(popupWindowInstance))
            {
                scrollPattern.ScrollVertical(ScrollAmount.ScrollAmount_SmallDecrement);
            }

            // Если точка клика элемента справа от границы списка - докручиваем по горизонтали вправо
            while (element.Element.IsClickablePointOnRight(popupWindowInstance, scrollPattern))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.ScrollAmount_LargeIncrement);
            }

            // Если точка клика элемента слева от границы списка - докручиваем по горизонтали влево
            while (element.Element.IsClickablePointOnLeft(popupWindowInstance))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.ScrollAmount_SmallDecrement);
            }

            return element;
        }

        /// <summary>
        /// Gets selected item.
        /// </summary>
        /// <returns>Selected item.</returns>
        public CruciatusElement SelectedItem()
        {
            if (this.IsExpanded)
            {
                return this.FindElement(
                    By.AutomationProperty(TreeScope.TreeScope_Subtree, Automation::SelectionItemPattern.IsSelectedProperty, true));
            }
            var pattern = this.GetPattern<Automation::SelectionPattern>(Automation::SelectionPattern.Pattern);
            var element = pattern.Current.GetSelection().FirstOrDefault();
            return element != null ? Create(element, this, null) : null;
        }

        #endregion
    }
}
