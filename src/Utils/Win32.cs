using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace FancyShare.Utils
{
    /// <summary>
    /// Wrapper for Win32 API calls
    /// </summary>
    public static class Win32
    {
        /// <summary>
        /// Get WindowHandle of a windowd
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static IntPtr GetHandle(Window window)
        {
            return new WindowInteropHelper(window).Handle;
        }


        /// <summary>
        /// Do not handle any mouse events on a window.
        /// All events will be triggered on underlying application window
        /// </summary>
        public static void IgnoreMouseEvents(IntPtr hwnd)
        {
            const int GWL_EXSTYLE = -20;
            const int WS_EX_TRANSPARENT = 0x00000020;
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }


        /// <summary>
        /// Get screen of a window
        /// </summary>
        public static Screen GetScreen(IntPtr hwnd)
        {
            return Screen.FromHandle(hwnd);
        }


        /// <summary>
        /// Get VirutalDesktop ID of a window
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static Guid GetWindowDesktopId(IntPtr hwnd)
        {
            var manager = new CVirtualDesktopManager() as IVirtualDesktopManager;
            int hr;
            if ((hr = manager.GetWindowDesktopId(hwnd, out Guid result)) != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
            return result;
        }


        /// <summary>
        /// Get Windows Device ID of a display device
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public static string GetDeviceId(string deviceName)
        {
            string deviceId = null;
            DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
            displayDevice.cb = Marshal.SizeOf(displayDevice);
            for (uint id = 0; EnumDisplayDevices(deviceName, id, ref displayDevice, 0x00000001); id++)
            {
                deviceId = displayDevice.DeviceID;
            }
            return deviceId;
        }



        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public int StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);
    }


    [ComImport, Guid("aa509086-5ca9-4c25-8f95-589d3c07b48a")]
    public class CVirtualDesktopManager
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
    [System.Security.SuppressUnmanagedCodeSecurity]
    public interface IVirtualDesktopManager
    {
        [PreserveSig]
        int IsWindowOnCurrentVirtualDesktop(
            [In] IntPtr TopLevelWindow,
            [Out] out int OnCurrentDesktop
            );
        [PreserveSig]
        int GetWindowDesktopId(
            [In] IntPtr TopLevelWindow,
            [Out] out Guid CurrentDesktop
            );

        [PreserveSig]
        int MoveWindowToDesktop(
            [In] IntPtr TopLevelWindow,
            [MarshalAs(UnmanagedType.LPStruct)]
            [In]Guid CurrentDesktop
            );
    }
}