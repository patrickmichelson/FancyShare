using System;
using System.Windows;
using FancyShare.Utils;

namespace FancyShare.Models
{
    class Monitor
    {
        public static Monitor FromWindow(Window window)
        {
            var hwnd = Win32.GetHandle(window);
            var screen = Win32.GetScreen(hwnd);

            return new Monitor
            {
                VirtualDesktopId = Win32.GetWindowDesktopId(hwnd),
                DeviceId = Win32.GetDeviceId(screen.DeviceName),
                Bounds = new Rect(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height),
                WorkingArea = new Rect(screen.WorkingArea.X, screen.WorkingArea.Y, screen.WorkingArea.Width, screen.WorkingArea.Height)
            };
        }

        public string DeviceId { get; protected set; }
        public Guid VirtualDesktopId { get; protected set; }
        public Rect Bounds { get; protected set; }
        public Rect WorkingArea { get; protected set; }

    }
}
