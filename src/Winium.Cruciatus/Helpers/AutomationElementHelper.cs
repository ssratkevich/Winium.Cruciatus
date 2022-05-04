using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Xml.XPath;
using Winium.Cruciatus.Helpers.XPath;
using Condition = System.Windows.Automation.Condition;
using Winium.Cruciatus.Helpers.NativeHelpers;

namespace Winium.Cruciatus.Helpers
{
    /// <summary>
    /// Helper for some common operations on Automation element.
    /// </summary>
    internal static class AutomationElementHelper
    {
        #region Fields

        static readonly int DEFAULTDPI = 96;

        #endregion

        #region Methods

        static void GetScreenScalingFactor(out float ScreenScalingFactorX, out float ScreenScalingFactorY)
        {
            var g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
            var desktop = g.GetHdc();
            int Xdpi = NativeMethods.GetDeviceCaps(desktop, (int) DeviceCap.LOGPIXELSX);
            int Ydpi = NativeMethods.GetDeviceCaps(desktop, (int) DeviceCap.LOGPIXELSY);
            ScreenScalingFactorX = (float) Xdpi / (float) DEFAULTDPI;
            ScreenScalingFactorY = (float) Ydpi / (float) DEFAULTDPI;
        }

        /// <summary>
        /// Find all elements by automation properties condition.
        /// </summary>
        /// <param name="parent">Parent automation element (starts from).</param>
        /// <param name="scope">Search scope.</param>
        /// <param name="condition">Filter condition.</param>
        /// <param name="timeout">Time threshold.</param>
        /// <returns>All found elements.</returns>
        internal static IEnumerable<AutomationElement> FindAll(
            AutomationElement parent,
            TreeScope scope,
            Condition condition,
            int timeout)
        {
            var dtn = DateTime.Now.AddMilliseconds(timeout);

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (DateTime.Now <= dtn)
            {
                var elements = parent.FindAll(scope, condition);
                if (elements.Count > 0)
                {
                    return elements.Cast<AutomationElement>();
                }
            }

            return Enumerable.Empty<AutomationElement>();
        }

        /// <summary>
        /// Find all elements by xpath condition.
        /// </summary>
        /// <param name="parent">Parent automation element (starts from).</param>
        /// <param name="xpath">Filter condition.</param>
        /// <param name="timeout">Time threshold.</param>
        /// <returns>All found elements.</returns>
        internal static IEnumerable<AutomationElement> FindAll(AutomationElement parent, string xpath, int timeout)
        {
            var navigator = new DesktopTreeXPathNavigator(parent);
            var dtn = DateTime.Now.AddMilliseconds(timeout);

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (DateTime.Now <= dtn)
            {
                var obj = navigator.Evaluate(xpath);
                var nodeIterator = obj as XPathNodeIterator;
                if (nodeIterator == null)
                {
                    CruciatusFactory.Logger.Warn("XPath expression '{0}' not searching nodes", xpath);
                    break;
                }

                var nodes = nodeIterator.Cast<DesktopTreeXPathNavigator>();
                var elementNodes = nodes.Where(item => item.NodeType == XPathNodeType.Element).ToList();
                if (elementNodes.Any())
                {
                    return elementNodes.Select(item => (AutomationElement)item.TypedValue);
                }
            }

            return Enumerable.Empty<AutomationElement>();
        }

        /// <summary>
        /// Find first element by automation properties condition.
        /// </summary>
        /// <param name="parent">Parent automation element (starts from).</param>
        /// <param name="scope">Search scope.</param>
        /// <param name="condition">Filter condition.</param>
        /// <returns>Found element or null.</returns>
        internal static AutomationElement FindFirst(AutomationElement parent, TreeScope scope, Condition condition)
        {
            return FindFirst(parent, scope, condition, CruciatusFactory.Settings.SearchTimeout);
        }

        /// <summary>
        /// Find first element by automation properties condition.
        /// </summary>
        /// <param name="parent">Parent automation element (starts from).</param>
        /// <param name="scope">Search scope.</param>
        /// <param name="condition">Filter condition.</param>
        /// <param name="timeout">Time threshold.</param>
        /// <returns>Found element or null.</returns>
        internal static AutomationElement FindFirst(
            AutomationElement parent,
            TreeScope scope,
            Condition condition,
            int timeout)
        {
            var dtn = DateTime.Now.AddMilliseconds(timeout);

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (DateTime.Now <= dtn)
            {
                var element = parent.FindFirst(scope, condition);
                if (element != null)
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// Try to get element bounding rect center.
        /// </summary>
        /// <param name="element">Element</param>
        /// <param name="point">Center of bounding rect.</param>
        /// <returns>Whether operation is successful.</returns>
        internal static bool TryGetBoundingRectangleCenter(AutomationElement element, out Point point)
        {
            var rect = element.Current.BoundingRectangle;
            if (rect.IsEmpty)
            {
                point = new Point();
                return false;
            }

            point = rect.Location;
            GetScreenScalingFactor(out var ScreenScalingFactorX, out var ScreenScalingFactorY);
            point.X = rect.X / ScreenScalingFactorX;
            point.Y = rect.Y / ScreenScalingFactorY;

            CruciatusFactory.Logger.Debug("BoundingRectangle location before scaling X co-ordinate:" + rect.X + "Y co-ordinate: " + rect.Y);
            CruciatusFactory.Logger.Debug("BoundingRectangle Points after scaling X co-ordinate:" + point.X + "Y co-ordinate: " + point.Y);
            CruciatusFactory.Logger.Debug("BoundingRectangle width :" + rect.Width);
            CruciatusFactory.Logger.Debug("BoundingRectangle Height :" + rect.Height);
            point.Offset((rect.Width / ScreenScalingFactorX) * 0.5, (rect.Height / ScreenScalingFactorY) * 0.5);
            return true;
        }

        /// <summary>
        /// Try to get element clickable point.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="point">Clickable point.</param>
        /// <returns>Whether operation is successful.</returns>
        internal static bool TryGetClickablePoint(AutomationElement element, out Point point) =>
            element.TryGetClickablePoint(out point);

        #endregion
    }
}
