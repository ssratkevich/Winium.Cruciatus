extern alias UIAComWrapper;
using System;
using System.Drawing;
using Interop.UIAutomationClient;
using Winium.Cruciatus.Exceptions;
using Winium.Cruciatus.Helpers;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Automation::AutomationElement"/>.
    /// </summary>
    internal static class AutomationElementExtension
    {
        #region Constants

        private const string OperationCanceledExceptionText = "Could not determine location of item relative to point\n";

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether first element is on left side of second one.
        /// </summary>
        /// <param name="currentElement">Element.</param>
        /// <param name="rectElement">Container.</param>
        /// <returns>True if element is on left of a container.</returns>
        /// <exception cref="OperationCanceledException"></exception>
        internal static bool IsClickablePointOnLeft(
            this Automation::AutomationElement currentElement,
            Automation::AutomationElement rectElement)
        {
            try
            {
                if (!currentElement.TryGetElementCenterPoint(out var point))
                {
                    throw new OperationCanceledException(OperationCanceledExceptionText);
                }

                var rect = rectElement.GetPropertyValue<Rectangle>(Automation::AutomationElement.BoundingRectangleProperty);

                return point.X < rect.Left;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(OperationCanceledExceptionText, exc);
            }
        }

        internal static bool IsClickablePointUpper(
            this Automation::AutomationElement currentElement,
            Automation::AutomationElement rectElement)
        {
            try
            {
                if (!currentElement.TryGetElementCenterPoint(out var point))
                {
                    throw new OperationCanceledException(OperationCanceledExceptionText);
                }

                var rect = rectElement.GetPropertyValue<Rectangle>(Automation::AutomationElement.BoundingRectangleProperty);

                return point.Y < rect.Top;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(OperationCanceledExceptionText, exc);
            }
        }

        internal static bool IsClickablePointOnRight(
            this Automation::AutomationElement currentElement,
            Automation::AutomationElement rectElement,
            Automation::ScrollPattern scrollPattern)
        {
            try
            {
                if (!currentElement.TryGetElementCenterPoint(out var point))
                {
                    throw new OperationCanceledException(OperationCanceledExceptionText);
                }

                var rect = rectElement.GetPropertyValue<Rectangle>(Automation::AutomationElement.BoundingRectangleProperty);

                if (scrollPattern == null || scrollPattern.Current.HorizontalScrollPercent < 0)
                {
                    return point.X > rect.Right;
                }

                return point.X > rect.Right - CruciatusFactory.Settings.ScrollBarWidth;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(OperationCanceledExceptionText, exc);
            }
        }

        internal static bool IsClickablePointLower(
            this Automation::AutomationElement currentElement,
            Automation::AutomationElement rectElement,
            Automation::ScrollPattern scrollPattern)
        {
            try
            {
                if (!currentElement.TryGetElementCenterPoint(out var point))
                {
                    throw new OperationCanceledException(OperationCanceledExceptionText);
                }

                var rect = rectElement.GetPropertyValue<Rectangle>(Automation::AutomationElement.BoundingRectangleProperty);

                if (scrollPattern == null || scrollPattern.Current.HorizontalScrollPercent < 0)
                {
                    return point.Y > rect.Bottom;
                }

                return point.Y > rect.Bottom - CruciatusFactory.Settings.ScrollBarHeight;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(OperationCanceledExceptionText, exc);
            }
        }

        internal static bool ContainsClickablePoint(
            this Automation::AutomationElement containerElement,
            Automation::AutomationElement internalElement)
        {
            try
            {
                if (!internalElement.TryGetElementCenterPoint(out var point))
                {
                    throw new OperationCanceledException(OperationCanceledExceptionText);
                }

                var externalRect = containerElement.GetPropertyValue<Rectangle>(Automation::AutomationElement.BoundingRectangleProperty);

                return externalRect.Contains(point);
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(
                    "Could not determine if element is contained by another element\n", 
                    exc);
            }
        }

        /// <summary>
        /// Try go get clicable point by all available strategies.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="point">Clicable point.</param>
        /// <returns>True if operation is success.</returns>
        internal static bool TryGetElementCenterPoint(this Automation::AutomationElement element, out Point point) =>
            AutomationElementHelper.TryGetClickablePoint(element, out point) ||
            AutomationElementHelper.TryGetBoundingRectangleCenter(element, out point);

        internal static void ScrollIntoView(this Automation::AutomationElement element, Automation::AutomationElement scrollableParent = null)
        {
            var scrollItemPattern = element.TryGetPattern<Automation::ScrollItemPattern>(Automation::ScrollItemPattern.Pattern);
            if (scrollItemPattern != null)
            {
                scrollItemPattern.ScrollIntoView();
                return;
            }
            if (scrollableParent == null)
            {
                throw new CruciatusException("No scrollable parent given.");
            }
            var scrollPattern = scrollableParent.TryGetPattern<Automation::ScrollPattern>(Automation::ScrollPattern.Pattern);
            if (scrollPattern == null)
            {
                throw new CruciatusException("Given element doesn't support scrollable pattern.");
            }

            if (scrollPattern.Current.VerticallyScrollable)
            {
                // Если точка клика элемента под границей списка - докручиваем по вертикали вниз
                while (element.IsClickablePointLower(scrollableParent, scrollPattern))
                {
                    scrollPattern.ScrollVertical(ScrollAmount.ScrollAmount_SmallIncrement);
                }

                // Если точка клика элемента над границей списка - докручиваем по вертикали вверх
                while (element.IsClickablePointUpper(scrollableParent))
                {
                    scrollPattern.ScrollVertical(ScrollAmount.ScrollAmount_SmallDecrement);
                }
            }

            if (scrollPattern.Current.HorizontallyScrollable)
            {
                // Если точка клика элемента справа от границы списка - докручиваем по горизонтали вправо
                while (element.IsClickablePointOnRight(scrollableParent, scrollPattern))
                {
                    scrollPattern.ScrollHorizontal(ScrollAmount.ScrollAmount_SmallIncrement);
                }

                // Если точка клика элемента слева от границы списка - докручиваем по горизонтали влево
                while (element.IsClickablePointOnLeft(scrollableParent))
                {
                    scrollPattern.ScrollHorizontal(ScrollAmount.ScrollAmount_SmallDecrement);
                }
            }

            if (element.Current.IsOffscreen)
            {
                throw new CruciatusException("Couldn't scroll element into view");
            }
        }

        internal static T GetPattern<T>(this Automation::AutomationElement element, Automation::AutomationPattern pattern) where T : class
        {
            if (element.TryGetCurrentPattern(pattern, out var foundPattern))
            {
                return (T)foundPattern;
            }

            var msg = string.Format("Element does not support {0}", typeof(T).Name);
            throw new CruciatusException(msg);
        }

        internal static T TryGetPattern<T>(this Automation::AutomationElement element, Automation::AutomationPattern pattern) where T: class =>
            element.TryGetCurrentPattern(pattern, out var foundPattern) ? (T) foundPattern : null;
        
        internal static TOut GetPropertyValue<TOut>(this Automation::AutomationElement element, Automation::AutomationProperty property)
        {
            var obj = element.GetCurrentPropertyValue(property, true);
            if (obj == Automation::AutomationElement.NotSupported)
            {
                throw new NotSupportedException();
            }

            if (obj is TOut)
            {
                return (TOut)obj;
            }

            throw new InvalidCastException(obj.GetType().ToString());
        }

        #endregion
    }
}
