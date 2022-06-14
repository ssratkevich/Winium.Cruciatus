extern alias UIAComWrapper;
using System;
using System.Drawing;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Properties of CruciatusElement.
    /// </summary>
    public class CruciatusElementProperties
    {
        /// <summary>
        /// Wrapped automation element.
        /// </summary>
        private readonly Automation::AutomationElement element;

        internal CruciatusElementProperties(Automation::AutomationElement element)
        {
            this.element = element;
        }

        /// <summary>
        /// BoundingRectangle property.
        /// </summary>
        public Rectangle BoundingRectangle =>
            this.element.Current.BoundingRectangle;

        /// <summary>
        /// ClickablePoint property.
        /// Warning: Could be null.
        /// </summary>
        public Point? ClickablePoint
        {
            get
            {
                Point? point = null;
                try
                {
                    if (this.element.TryGetClickablePoint(out var p))
                    {
                        point = p;
                    }
                }
                catch (Exception)
                {
                    point = null;
                }
                return point;
            }
        }

        /// <summary>
        /// IsEnabled property.
        /// </summary>
        public bool IsEnabled =>
            this.element.Current.IsEnabled;

        /// <summary>
        /// IsOffscreen property.
        /// </summary>
        public bool IsOffscreen =>
            this.element.Current.IsOffscreen;

        /// <summary>
        /// Name property.
        /// </summary>
        public string Name =>
            this.element.Current.Name;

        /// <summary>
        /// RuntimeId property.
        /// </summary>
        public string RuntimeId =>
            string.Join(" ", this.element.GetRuntimeId());
    }
}
