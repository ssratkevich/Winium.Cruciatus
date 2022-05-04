using System;
using System.Runtime.InteropServices;

namespace Winium.Cruciatus.Helpers.NativeHelpers
{
    /// <summary>
    /// Device cap
    /// </summary>
    public enum DeviceCap
    {
        /// <summary>
        /// Logical pixel for X axis.
        /// </summary>
        LOGPIXELSX = 88,
        /// <summary>
        /// Logical pixel for Y axis.
        /// </summary>
        LOGPIXELSY = 90,
    }

    /// <summary>
    /// Point struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        /// <summary>
        /// X coord.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int x;
        /// <summary>
        /// Y coord.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int y;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;

        [MarshalAs(UnmanagedType.U2)]
        public UInt16 dmSpecVersion;

        [MarshalAs(UnmanagedType.U2)]
        public UInt16 dmDriverVersion;

        [MarshalAs(UnmanagedType.U2)]
        public UInt16 dmSize;

        [MarshalAs(UnmanagedType.U2)]
        public UInt16 dmDriverExtra;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmFields;

        public POINT dmPosition;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmDisplayOrientation;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmDisplayFixedOutput;

        [MarshalAs(UnmanagedType.I2)]
        public Int16 dmColor;

        [MarshalAs(UnmanagedType.I2)]
        public Int16 dmDuplex;

        [MarshalAs(UnmanagedType.I2)]
        public Int16 dmYResolution;

        [MarshalAs(UnmanagedType.I2)]
        public Int16 dmTTOption;

        [MarshalAs(UnmanagedType.I2)]
        public Int16 dmCollate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;

        [MarshalAs(UnmanagedType.U2)]
        public UInt16 dmLogPixels;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmBitsPerPel;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmPelsWidth;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmPelsHeight;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmDisplayFlags;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmDisplayFrequency;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmICMMethod;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmICMIntent;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmMediaType;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmDitherType;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmReserved1;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmReserved2;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmPanningWidth;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmPanningHeight;
    }

    /// <summary>
    /// Class for all native methods.
    /// </summary>
    internal static class NativeMethods
    {
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean EnumDisplaySettings(
            [param: MarshalAs(UnmanagedType.LPTStr)]
            string lpszDeviceName,
            [param: MarshalAs(UnmanagedType.U4)]
            int iModeNum,
            [In, Out]
            ref DEVMODE lpDevMode);

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int ChangeDisplaySettings(
            [In, Out]
            ref DEVMODE lpDevMode,
            [param: MarshalAs(UnmanagedType.U4)]
            uint dwflags);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool GetPhysicalCursorPos(
            [In, Out] ref POINT lpPoint);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc,
            [param: MarshalAs(UnmanagedType.U4)]
            int nIndex);
    }
}
