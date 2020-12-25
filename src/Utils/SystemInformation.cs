using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace FancyShare.Utils
{
    static class SystemInformation
    {
        public static IEnumerable<Rectangle> Screens
            => Screen.AllScreens.Select(x => x.WorkingArea);
        public static double dpiScaleX
            => Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth;
        public static double dpiScaleY
            => Screen.PrimaryScreen.Bounds.Height / SystemParameters.PrimaryScreenHeight;
    }
}
