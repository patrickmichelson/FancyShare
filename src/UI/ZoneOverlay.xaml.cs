using FancyShare.Models;
using FancyShare.Utils;
using System;
using System.Drawing;
using System.Windows;

namespace FancyShare.UI
{
    /// <summary>
    /// Interaction logic for ZoneOverlay.xaml
    /// </summary>
    public partial class ZoneOverlay : Window
    {
        public ZoneOverlay(Rectangle screenArea)
        {
            InitializeComponent();

            Left = screenArea.Left / SystemInformation.dpiScaleX;
            Top = screenArea.Top / SystemInformation.dpiScaleY;
            Width = screenArea.Width / SystemInformation.dpiScaleX;
            Height = screenArea.Height / SystemInformation.dpiScaleY;

            scaledView.Width = this.Width;
            scaledView.Height = this.Height;

            ItemControl.Width = screenArea.Width;
            ItemControl.Height = screenArea.Height;
        }


        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            var screen = Monitor.FromWindow(this);

            (DataContext as ZoneOverlayViewModel).UpdateZones(screen);
        }
    }
}
