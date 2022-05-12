using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using NLog;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Elements;
using Winium.Cruciatus.Extensions;
using Winium.Cruciatus.Helpers;

namespace Winium.Cruciatus
{
    internal static class CruciatusCommand
    {
        #region Static Fields

        private static readonly Logger Logger = CruciatusFactory.Logger;

        #endregion

        #region Methods

        internal static IEnumerable<CruciatusElement> FindAll(CruciatusElement parent, By strategy)
        {
            return FindAll(parent, strategy, CruciatusFactory.Settings.SearchTimeout);
        }

        internal static IEnumerable<CruciatusElement> FindAll(CruciatusElement parent, By strategy, int timeout)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var result = strategy.FindAll(parent.Element, timeout);
            return result.Select(e => CruciatusElement.Create(e, parent, strategy));
        }

        internal static CruciatusElement FindFirst(CruciatusElement parent, By strategy)
        {
            return FindFirst(parent, strategy, CruciatusFactory.Settings.SearchTimeout);
        }

        internal static CruciatusElement FindFirst(CruciatusElement parent, By strategy, int timeout)
        {
            var element = strategy.FindFirst(parent.Element, timeout);
            if (element == null)
            {
                Logger.Info("Element '{0}' not found", strategy);
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                return null;
            }

            return CruciatusElement.Create(element, parent, strategy);
        }

        internal static bool TryClickOnBoundingRectangleCenter(
            MouseButton button, 
            CruciatusElement element, 
            bool doubleClick)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            Point point;
            if (!AutomationElementHelper.TryGetBoundingRectangleCenter(element.Element, out point))
            {
                Logger.Debug("Element '{0}' have empty BoundingRectangle", element);
                return false;
            }

            if (doubleClick)
            {
                CruciatusFactory.Mouse.DoubleClick(button, point.X, point.Y);
            }
            else
            {
                CruciatusFactory.Mouse.Click(button, point.X, point.Y);
            }

            Logger.Info(
                "{0} on '{1}' element at ({2}, {3}) BoundingRectangle center", 
                doubleClick ? "DoubleClick" : "Click", 
                element, 
                point.X, 
                point.Y);
            return true;
        }

        internal static bool TryClickOnClickablePoint(MouseButton button, CruciatusElement element, bool doubleClick)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var point = element.Properties.ClickablePoint;
            if (!point.HasValue)
            {
                Logger.Debug("Element '{0}' not have ClickablePoint", element);
                return false;
            }

            var x = point.Value.X;
            var y = point.Value.Y;
            if (doubleClick)
            {
                CruciatusFactory.Mouse.DoubleClick(button, x, y);
            }
            else
            {
                CruciatusFactory.Mouse.Click(button, x, y);
            }

            Logger.Info(
                "{0} on '{1}' element at ({2}, {3}) ClickablePoint", 
                doubleClick ? "DoubleClick" : "Click", 
                element, 
                x, 
                y);
            return true;
        }

        internal static bool TryClickUsingInvokePattern(CruciatusElement element, bool doubleClick)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var invokePattern = element.Element.TryGetPattern<InvokePattern>(InvokePattern.Pattern);
            if (invokePattern != null)
            {
                string cmd;
                if (doubleClick)
                {
                    invokePattern.Invoke();
                    invokePattern.Invoke();
                    cmd = "DoubleClick";
                }
                else
                {
                    invokePattern.Invoke();
                    cmd = "Click";
                }

                Logger.Info("{0} emulation on '{1}' element with use invoke pattern", cmd, element);
                return true;
            }

            Logger.Debug("Element '{0}' not support InvokePattern", element);
            return false;
        }

        internal static bool TryGetTextUsingTextPattern(CruciatusElement element, out string text)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var textPattern = element.Element.TryGetPattern<TextPattern>(TextPattern.Pattern);
            if (textPattern != null)
            {
                text = textPattern.DocumentRange.GetText(-1);
                Logger.Info("Element '{0}' return text using TextPattern", element);
                return true;
            }

            Logger.Debug("Element '{0}' not support TextPattern", element);
            text = null;
            return false;
        }

        internal static bool TryGetTextUsingValuePattern(CruciatusElement element, out string text)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var valuePattern = element.Element.TryGetPattern<ValuePattern>(ValuePattern.Pattern);
            if (valuePattern != null)
            {
                Logger.Info("Element '{0}' return text with use ValuePattern", element);
                text = valuePattern.Current.Value;
                return true;
            }

            Logger.Debug("Element '{0}' not support ValuePattern", element);
            text = null;
            return false;
        }

        #endregion
    }
}
