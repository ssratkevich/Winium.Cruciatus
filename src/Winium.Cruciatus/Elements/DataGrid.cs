extern alias UIAComWrapper;
using Interop.UIAutomationClient;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Exceptions;
using Winium.Cruciatus.Extensions;
using Winium.Cruciatus.Helpers;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus.Elements
{
    /// <summary>
    /// Represents Data grid element.
    /// </summary>
    public class DataGrid : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates new instance of data grid.
        /// </summary>
        /// <param name="element">Wrapped element.</param>
        public DataGrid(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Creates new instance of data grid.
        /// </summary>
        /// <param name="parent">Parent element.</param>
        /// <param name="searchStrategy">Search strategy.</param>
        public DataGrid(CruciatusElement parent, By searchStrategy)
            : base(parent, searchStrategy)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Table columns count.
        /// </summary>
        public int ColumnCount =>
            this.GetAutomationPropertyValue<int>(Automation::GridPattern.ColumnCountProperty);

        /// <summary>
        /// Table row count.
        /// </summary>
        public int RowCount =>
            this.GetAutomationPropertyValue<int>(Automation::GridPattern.RowCountProperty);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get first item in a cell.
        /// </summary>
        /// <param name="row">Row number.</param>
        /// <param name="column">Column number.</param>
        /// <returns>First item in a cell.</returns>
        public virtual CruciatusElement Item(int row, int column)
        {
            if (!this.Element.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Scroll failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new Automation::ElementNotEnabledException("NOT GET ITEM");
            }

            // Проверка на дурака
            if (row < 0 || column < 0)
            {
                Logger.Error("Cell index [{1}, {2}] is out of bounds for DataGrid {0}.", this, row, column);
                throw new CruciatusException("NOT GET ITEM");
            }

            // Условие для поиска ячейки [row, column]
            var cellCondition =
                new Automation::AndCondition(
                    new Automation::PropertyCondition(Automation::AutomationElement.IsGridItemPatternAvailableProperty, true), 
                    new Automation::PropertyCondition(Automation::GridItemPattern.RowProperty, row), 
                    new Automation::PropertyCondition(Automation::GridItemPattern.ColumnProperty, column));
            var cell = AutomationElementHelper.FindFirst(this.Element, TreeScope.TreeScope_Subtree, cellCondition);

            // Проверка, что ячейку видно
            if (cell == null || !this.Element.ContainsClickablePoint(cell))
            {
                Logger.Error("Cell [{1}, {2}] is not visible in DataGrid {0}.", this, row, column);
                throw new CruciatusException("NOT GET ITEM");
            }

            // Поиск подходящего элемента в ячейке
            var elem = cell.FindFirst(TreeScope.TreeScope_Subtree, Automation::Condition.TrueCondition);
            if (elem == null)
            {
                Logger.Error("Item not found in cell [{1}, {2}] for DataGrid {0}.", this, row, column);
                throw new CruciatusException("NOT GET ITEM");
            }

            return CruciatusElement.Create(elem, null, null);
        }

        /// <summary>
        /// Scrolls table to cell.
        /// </summary>
        /// <param name="row">Row number.</param>
        /// <param name="column">Column number.</param>
        public virtual void ScrollTo(int row, int column)
        {
            if (!this.Element.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Scroll failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new Automation::ElementNotEnabledException("NOT SCROLL");
            }

            // Проверка на дурака
            if (row < 0 || column < 0)
            {
                var msg = string.Format("Cell index [{1}, {2}] is out of bounds for DataGrid {0}.", this, row, column);
                Logger.Error(msg);
                throw new CruciatusException("NOT SCROLL");
            }

            var scrollPattern = this.Element.GetCurrentPattern(Automation::ScrollPattern.Pattern) as Automation::ScrollPattern;
            if (scrollPattern == null)
            {
                Logger.Error("{0} does not support ScrollPattern.", this.ToString());
                throw new CruciatusException("NOT SCROLL");
            }

            // Условие для вертикального поиска ячейки [row, 0] (через строку)
            var cellCondition =
                new Automation::AndCondition(
                    new Automation::PropertyCondition(Automation::AutomationElement.IsGridItemPatternAvailableProperty, true), 
                    new Automation::PropertyCondition(Automation::GridItemPattern.RowProperty, row));

            // Стартовый поиск ячейки
            var cell = AutomationElementHelper.FindFirst(this.Element, TreeScope.TreeScope_Subtree, cellCondition);

            // Вертикальная прокрутка (при необходимости и возможности)
            if (cell == null && scrollPattern.Current.VerticallyScrollable)
            {
                // Установка самого верхнего положения прокрутки
                while (scrollPattern.Current.VerticalScrollPercent > 0.1)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.ScrollAmount_LargeIncrement);
                }

                // Установка самого левого положения прокрутки (при возможности)
                if (scrollPattern.Current.HorizontallyScrollable)
                {
                    while (scrollPattern.Current.HorizontalScrollPercent > 0.1)
                    {
                        scrollPattern.ScrollHorizontal(ScrollAmount.ScrollAmount_LargeIncrement);
                    }
                }

                // Основная вертикальная прокрутка
                while (cell == null && scrollPattern.Current.VerticalScrollPercent < 99.9)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.ScrollAmount_LargeIncrement);
                    cell = AutomationElementHelper.FindFirst(this.Element, TreeScope.TreeScope_Subtree, cellCondition);
                }
            }

            // Если прокрутив до конца ячейка не найдена, то номер строки не действительный
            if (cell == null)
            {
                Logger.Error("Row index {1} is out of bounds for {0}.", this, row);
                throw new CruciatusException("NOT SCROLL");
            }

            // Если точка клика ячейки [row, 0] под границей таблицы - докручиваем по вертикали вниз
            while (cell.IsClickablePointLower(this.Element, scrollPattern))
            {
                scrollPattern.ScrollVertical(ScrollAmount.ScrollAmount_SmallIncrement);
            }

            // Если точка клика ячейки [row, 0] над границей таблицы - докручиваем по вертикали вверх
            while (cell.IsClickablePointUpper(this.Element))
            {
                scrollPattern.ScrollVertical(ScrollAmount.ScrollAmount_SmallDecrement);
            }

            // Условие для горизонтального поиска ячейки [row, column]
            cellCondition =
                new Automation::AndCondition(
                    new Automation::PropertyCondition(Automation::AutomationElement.IsGridItemPatternAvailableProperty, true), 
                    new Automation::PropertyCondition(Automation::GridItemPattern.RowProperty, row), 
                    new Automation::PropertyCondition(Automation::GridItemPattern.ColumnProperty, column));

            // Стартовый поиск ячейки
            cell = AutomationElementHelper.FindFirst(this.Element, TreeScope.TreeScope_Subtree, cellCondition);

            // Основная горизонтальная прокрутка (при необходимости и возможности)
            if (cell == null && scrollPattern.Current.HorizontallyScrollable)
            {
                while (cell == null && scrollPattern.Current.HorizontalScrollPercent < 99.9)
                {
                    scrollPattern.ScrollHorizontal(ScrollAmount.ScrollAmount_LargeIncrement);
                    cell = AutomationElementHelper.FindFirst(this.Element, TreeScope.TreeScope_Subtree, cellCondition);
                }
            }

            // Если прокрутив до конца ячейка не найдена, то номер колонки не действительный
            if (cell == null)
            {
                Logger.Error("Column index {1} is out of bounds for DataGrid {0}.", this, column);
                throw new CruciatusException("NOT SCROLL");
            }

            // Если точка клика ячейки [row, column] справа от границы таблицы - докручиваем по горизонтали вправо
            while (cell.IsClickablePointOnRight(this.Element, scrollPattern))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.ScrollAmount_SmallIncrement);
            }

            // Если точка клика ячейки [row, column] слева от границы таблицы - докручиваем по горизонтали влево
            while (cell.IsClickablePointOnLeft(this.Element))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.ScrollAmount_SmallDecrement);
            }
        }

        /// <summary>
        /// Select cell.
        /// </summary>
        /// <param name="row">Row number.</param>
        /// <param name="column">Column number.</param>
        public virtual void SelectCell(int row, int column)
        {
            var cell = this.Item(row, column);
            if (cell == null)
            {
                Logger.Error("Cell index [{1}, {2}] is out of bounds for DataGrid {0}.", this, row, column);
                throw new CruciatusException("NOT SELECT CELL");
            }

            cell.Click();
        }

        #endregion
    }
}
