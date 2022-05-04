using System;
using System.Runtime.InteropServices;
using Winium.Cruciatus.Helpers.NativeHelpers;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// A static class for retreiving and updating the device orientation.
    /// </summary>
    public static class RotationManager
    {
        #region Constants

        private const int CURRENT_SETTINGS_MODE = -1;

        #endregion

        #region Methods

        /// <summary>
        /// Gets the current orientation of the primary screen.
        /// </summary>
        /// <returns>The orientation of the primary screen.</returns>
        public static DisplayOrientation GetCurrentOrientation()
        {
            var currentSettings = new DEVMODE();
            currentSettings.dmSize = (ushort)Marshal.SizeOf(currentSettings);

            NativeMethods.EnumDisplaySettings(null, CURRENT_SETTINGS_MODE, ref currentSettings);

            return (DisplayOrientation)currentSettings.dmDisplayOrientation;
        }

        /// <summary>
        /// Sets the orientation of the primary screen.
        /// </summary>
        /// <param name="orientation">The desired orientation.</param>
        /// <returns>
        /// The result of setting the orientation:
        ///   0: Success or no change required
        ///   1: A device restart is required
        ///   -2: The device does not support this orientation
        ///   Any other number: Unknown error 
        /// </returns>
        public static int SetOrientation(DisplayOrientation orientation)
        {
            var currentSettings = new DEVMODE();
            currentSettings.dmSize = (ushort)Marshal.SizeOf(currentSettings);

            NativeMethods.EnumDisplaySettings(null, CURRENT_SETTINGS_MODE, ref currentSettings);

            if (currentSettings.dmDisplayOrientation == (int)orientation)
            {
                return 0;
            }

            var newSettings = currentSettings;

            newSettings.dmDisplayOrientation = (uint)orientation;

            var bigDimension = Math.Max(newSettings.dmPelsHeight, newSettings.dmPelsWidth);
            var smallDimension = Math.Min(newSettings.dmPelsHeight, newSettings.dmPelsWidth);

            if (orientation == DisplayOrientation.LANDSCAPE)
            {
                newSettings.dmPelsHeight = smallDimension;
                newSettings.dmPelsWidth = bigDimension;
            }
            else
            {
                newSettings.dmPelsHeight = bigDimension;
                newSettings.dmPelsWidth = smallDimension;
            }

            return NativeMethods.ChangeDisplaySettings(ref newSettings, 0);
        }

        #endregion
    }
}
