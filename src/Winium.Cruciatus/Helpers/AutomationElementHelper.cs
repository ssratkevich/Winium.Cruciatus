extern alias UIAComWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Xml.XPath;
using Winium.Cruciatus.Helpers.XPath;
using Interop.UIAutomationClient;
using Winium.Cruciatus.Helpers.NativeHelpers;
using Automation = UIAComWrapper::System.Windows.Automation;
using Condition = UIAComWrapper::System.Windows.Automation.Condition;

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
            ScreenScalingFactorX = 1.0f;
            ScreenScalingFactorY = 1.0f;

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
        internal static IEnumerable<Automation::AutomationElement> FindAll(
            Automation::AutomationElement parent,
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
                    return elements.Cast<Automation::AutomationElement>();
                }
            }

            return Enumerable.Empty<Automation::AutomationElement>();
        }

        /// <summary>
        /// Find all elements by xpath condition.
        /// </summary>
        /// <param name="parent">Parent automation element (starts from).</param>
        /// <param name="xpath">Filter condition.</param>
        /// <param name="timeout">Time threshold.</param>
        /// <returns>All found elements.</returns>
        internal static IEnumerable<Automation::AutomationElement> FindAll(Automation::AutomationElement parent, string xpath, int timeout)
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
                    return elementNodes.Select(item => (Automation::AutomationElement)item.TypedValue);
                }
            }

            return Enumerable.Empty<Automation::AutomationElement>();
        }

        /// <summary>
        /// Find first element by automation properties condition.
        /// </summary>
        /// <param name="parent">Parent automation element (starts from).</param>
        /// <param name="scope">Search scope.</param>
        /// <param name="condition">Filter condition.</param>
        /// <returns>Found element or null.</returns>
        internal static Automation::AutomationElement FindFirst(Automation::AutomationElement parent, TreeScope scope, Condition condition)
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
        internal static Automation::AutomationElement FindFirst(
            Automation::AutomationElement parent,
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
        internal static bool TryGetBoundingRectangleCenter(Automation::AutomationElement element, out Point point)
        {
            var rect = element.Current.BoundingRectangle;
            if (rect.IsEmpty)
            {
                point = new Point();
                return false;
            }

            point = rect.Location;
            GetScreenScalingFactor(out var ScreenScalingFactorX, out var ScreenScalingFactorY);
            point.X = (int) (rect.X / ScreenScalingFactorX);
            point.Y = (int) (rect.Y / ScreenScalingFactorY);

            CruciatusFactory.Logger.Debug("BoundingRectangle location before scaling X co-ordinate:" + rect.X + "Y co-ordinate: " + rect.Y);
            CruciatusFactory.Logger.Debug("BoundingRectangle Points after scaling X co-ordinate:" + point.X + "Y co-ordinate: " + point.Y);
            CruciatusFactory.Logger.Debug("BoundingRectangle width :" + rect.Width);
            CruciatusFactory.Logger.Debug("BoundingRectangle Height :" + rect.Height);
            point.Offset((int) (rect.Width / ScreenScalingFactorX * 0.5), (int) (rect.Height / ScreenScalingFactorY * 0.5));
            return true;
        }

        /// <summary>
        /// Try to get element clickable point.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="point">Clickable point.</param>
        /// <returns>Whether operation is successful.</returns>
        internal static bool TryGetClickablePoint(Automation::AutomationElement element, out Point point) =>
            element.TryGetClickablePoint(out point);

        #endregion
    }
}
