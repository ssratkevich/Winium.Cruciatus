using System;
using System.Windows;
using System.Windows.Automation;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Properties of CruciatusElement.
    /// </summary>
    public class CruciatusElementProperties
    {
        #region Fields
        /// <summary>
        /// Wrapped automation element.
        /// </summary>
        private readonly AutomationElement element;

        #endregion

        #region Constructors and Destructors

        internal CruciatusElementProperties(AutomationElement element)
        {
            this.element = element;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// BoundingRectangle property.
        /// </summary>
        public Rect BoundingRectangle =>
            this.element.Current.BoundingRectangle;

        /// <summary>
        /// ClickablePoint property.
        /// Warning: Could be null.
        /// </summary>
        public Point? ClickablePoint
        {
            get
            {
                Point? point;
                try
                {
                    this.element.TryGetClickablePoint(out var p);
                    point = p;
                }
                catch (Exception ex)
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

        #endregion
    }
}
