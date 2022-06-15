using System.Windows;

namespace Winium.Cruciatus.Helpers
{
    internal static class ScreenCoordinatesHelper
    {
        #region Static Fields

        internal static readonly Point VirtualScreenLowerRightCorner = new Point(65535, 65535);

        #endregion

        #region Methods

        internal static Point ScreenPointToVirtualScreenPoint(Point point)
        {
            var sX = point.X;
            var sY = point.Y;

            var virtualScreen = System.Windows.Forms.SystemInformation.VirtualScreen;
            sX -= virtualScreen.Left;
            sY -= virtualScreen.Top;

            var vsX = sX * (VirtualScreenLowerRightCorner.X / virtualScreen.Width);
            var vsY = sY * (VirtualScreenLowerRightCorner.Y / virtualScreen.Height);

            return new Point(vsX, vsY);
        }

        #endregion
    }
}
