using System;
using System.Linq;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Exceptions;
using Winium.Cruciatus.Extensions;

namespace Winium.Cruciatus.Elements
{
    /// <summary>
    /// Represents menu.
    /// </summary>
    public class Menu : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates menu instance.
        /// </summary>
        /// <param name="element">Wrapped element.</param>
        public Menu(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Creates menu instance.
        /// </summary>
        /// <param name="parent">Parent element.</param>
        /// <param name="searchStrategy">Search strategy.</param>
        public Menu(CruciatusElement parent, By searchStrategy)
            : base(parent, searchStrategy)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get the menu item.
        /// </summary>
        /// <param name="headersPath">Headers path with '$' separator (ex:control$view$zoom).</param>
        /// <returns>Menu item.</returns>
        public CruciatusElement GetItem(string headersPath)
        {
            if (string.IsNullOrEmpty(headersPath))
            {
                throw new ArgumentNullException("headersPath");
            }

            var item = (CruciatusElement)this;
            var headers = headersPath.Split('$');
            for (var i = 0; i < headers.Length - 1; ++i)
            {
                var name = headers[i];
                item = item.FindElement(By.Name(name));
                if (item == null)
                {
                    Logger.Error("Item '{0}' not found. Find item failed.", name);
                    throw new CruciatusException("NOT GET ITEM");
                }

                item.Click();
            }

            return item.FindElement(By.Name(headers.Last()));
        }

        /// <summary>
        /// Select menu item.
        /// </summary>
        /// <param name="headersPath">Headers path with '$' separator (ex:control$view$zoom).</param>
        /// <returns>Menu item.</returns>
        public virtual void SelectItem(string headersPath)
        {
            if (!this.Element.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Select item failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new CruciatusException("NOT SELECT ITEM");
            }

            var item = this.GetItem(headersPath);
            if (item == null)
            {
                Logger.Error("Item '{0}' not found. Select item failed.", headersPath);
                throw new CruciatusException("NOT SELECT ITEM");
            }

            item.Click();
        }

        #endregion
    }
}
