using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using WindowsInput;
using Winium.Cruciatus.Helpers;

namespace Winium.Cruciatus.Core
{

    /// <summary>
    /// Mouse simulator (wrapper on WindowsInput.MouseSimulator).
    /// </summary>
    public class MouseSimulatorExt
    {
        #region Fields

        private readonly IMouseSimulator mouseSimulator;

        #endregion

        #region Constructors and Destructors

        internal MouseSimulatorExt(IMouseSimulator mouseSimulator)
        {
            this.mouseSimulator = mouseSimulator;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Cursor current position.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed.")]
        public Point CurrentCursorPos
        {
            get
            {
                var currentPoint = Cursor.Position;
                return new Point(currentPoint.X, currentPoint.Y);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Emulates mouse click in current position.
        /// </summary>
        /// <param name="button">
        /// Button key code.
        /// </param>
        public void Click(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    this.LeftButtonClick();
                    break;
                case MouseButton.Right:
                    this.RightButtonClick();
                    break;
            }
        }

        /// <summary>
        /// Emulates click on given coords.
        /// </summary>
        /// <param name="button">
        /// Mouse button key.
        /// </param>
        /// <param name="x">
        /// X coord.
        /// </param>
        /// <param name="y">
        /// Y coord.
        /// </param>
        public void Click(MouseButton button, double x, double y)
        {
            this.SetCursorPos(x, y);
            this.Click(button);
        }

        /// <summary>
        /// Emulates mouse double click in current position.
        /// </summary>
        /// <param name="button">
        /// Mouse button key.
        /// </param>
        public void DoubleClick(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    this.LeftButtonDoubleClick();
                    break;
                case MouseButton.Right:
                    this.RightButtonDoubleClick();
                    break;
            }
        }

        /// <summary>
        /// Emulates mouse double click in given position.
        /// </summary>
        /// <param name="button">
        /// Mouse button key.
        /// </param>
        /// <param name="x">
        /// X coord.
        /// </param>
        /// <param name="y">
        /// Y coord.
        /// </param>
        public void DoubleClick(MouseButton button, double x, double y)
        {
            this.SetCursorPos(x, y);
            this.DoubleClick(button);
        }

        /// <summary>
        /// Emulates left mouse button click in current cursor position.
        /// </summary>
        public void LeftButtonClick()
        {
            this.mouseSimulator.LeftButtonClick();
            Thread.Sleep(250);
        }

        /// <summary>
        /// Emulates left mouse button double click in current cursor position.
        /// </summary>
        public void LeftButtonDoubleClick()
        {
            this.mouseSimulator.LeftButtonDoubleClick();
            Thread.Sleep(250);
        }

        /// <summary>
        /// Emulates right mouse button click in current cursor position.
        /// </summary>
        public void RightButtonClick()
        {
            this.mouseSimulator.RightButtonClick();
            Thread.Sleep(250);
        }

        /// <summary>
        /// Emulates right mouse button click in current cursor position.
        /// </summary>
        public void RightButtonDoubleClick()
        {
            this.mouseSimulator.RightButtonDoubleClick();
            Thread.Sleep(250);
        }

        /// <summary>
        /// Sets cursor position to given point.
        /// </summary>
        /// <param name="x">
        /// X coord.
        /// </param>
        /// <param name="y">
        /// Y coord.
        /// </param>
        public void SetCursorPos(double x, double y)
        {
            var virtualScreenPoint = ScreenCoordinatesHelper.ScreenPointToVirtualScreenPoint(new Point(x, y));
            this.mouseSimulator.MoveMouseToPositionOnVirtualDesktop(virtualScreenPoint.X, virtualScreenPoint.Y);
            Thread.Sleep(250);
        }

        /// <summary>
        /// Moves the cursor for a given distance from its current position.
        /// </summary>
        /// <param name="x">
        /// X coord (in pixels).
        /// </param>
        /// <param name="y">
        /// Y coord (in pixels).
        /// </param>
        public void MoveCursorPos(double x, double y)
        {
            var currentPoint = this.CurrentCursorPos;
            this.SetCursorPos(currentPoint.X + x, currentPoint.Y + y);
        }

        /// <summary>
        /// Emulates vertical scroll.
        /// </summary>
        /// <param name="amountOfClicks">
        /// The amount to scroll in clicks.
        /// A positive value indicates that the wheel was rotated forward,
        /// away from the user;
        /// a negative value indicates that the wheel was rotated backward,
        /// toward the user.
        /// </param>
        public void VerticalScroll(int amountOfClicks)
        {
            this.mouseSimulator.VerticalScroll(amountOfClicks);
            Thread.Sleep(250);
        }

        /// <summary>
        /// Emulates horizontal scroll.
        /// </summary>
        /// <param name="amountOfClicks">
        /// The amount to scroll in clicks.
        /// A positive value indicates that the wheel was rotated to the right;
        /// a negative value indicates that the wheel was rotated to the left.
        /// </param>
        public void HorizontalScroll(int amountOfClicks)
        {
            this.mouseSimulator.HorizontalScroll(amountOfClicks);
            Thread.Sleep(250);
        }

        #endregion
    }
}
