using System.Windows.Automation;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Extensions;

namespace Winium.Cruciatus.Elements
{
    /// <summary>
    /// Represents list box element.
    /// </summary>
    public class ListBox : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates new instance of list box.
        /// </summary>
        /// <param name="element">Wrapped element.</param>
        public ListBox(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Creates new instance of list box.
        /// </summary>
        /// <param name="parent">Parent element.</param>
        /// <param name="searchStrategy">Search strategy.</param>
        public ListBox(CruciatusElement parent, By searchStrategy)
            : base(parent, searchStrategy)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Scrolls to element given by search strategy.
        /// </summary>
        /// <param name="getStrategy">Search strategy.</param>
        /// <returns>Found element.</returns>
        public CruciatusElement ScrollTo(By getStrategy)
        {
            if (!this.Element.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Scroll failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new ElementNotEnabledException("NOT SCROLL");
            }

            var scrollPattern = this.Element.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern == null)
            {
                Logger.Debug("{0} does not support ScrollPattern.", this);
                throw new ElementNotEnabledException("NOT SCROLL");
            }

            // Стартовый поиск элемента
            var element = CruciatusCommand.FindFirst(this, getStrategy, 1000);

            // Вертикальная прокрутка (при необходимости и возможности)
            if (element == null && scrollPattern.Current.VerticallyScrollable)
            {
                // Установка самого верхнего положения прокрутки
                while (scrollPattern.Current.VerticalScrollPercent > 0.1)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.LargeDecrement);
                }

                // Установка самого левого положения прокрутки (при возможности)
                if (scrollPattern.Current.HorizontallyScrollable)
                {
                    while (scrollPattern.Current.HorizontalScrollPercent > 0.1)
                    {
                        scrollPattern.ScrollHorizontal(ScrollAmount.LargeDecrement);
                    }
                }

                // Основная вертикальная прокрутка
                while (element == null && scrollPattern.Current.VerticalScrollPercent < 99.9)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);
                    element = CruciatusCommand.FindFirst(this, getStrategy, 1000);
                }
            }

            if (element == null)
            {
                Logger.Debug("No elements matching {1} were found in {0}.", this, getStrategy);
                return null;
            }

            // Если точка клика элемента под границей списка - докручиваем по вертикали вниз
            while (element.Element.IsClickablePointLower(this.Element, scrollPattern))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
            }

            // Если точка клика элемента над границей списка - докручиваем по вертикали вверх
            while (element.Element.IsClickablePointUpper(this.Element))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallDecrement);
            }

            // Если точка клика элемента справа от границы списка - докручиваем по горизонтали вправо
            while (element.Element.IsClickablePointOnRight(this.Element, scrollPattern))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallIncrement);
            }

            // Если точка клика элемента слева от границы списка - докручиваем по горизонтали влево
            while (element.Element.IsClickablePointOnLeft(this.Element))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallDecrement);
            }

            return element;
        }

        #endregion
    }
}
