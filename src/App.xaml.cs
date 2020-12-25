using System;
using System.Linq;
using System.Windows;
using FancyShare.FancyZones;
using FancyShare.Models;
using FancyShare.UI;
using FancyShare.Utils;

namespace FancyShare
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var zoneConfig = new ZoneConfiguration();

            foreach (var screen in SystemInformation.Screens)
            {
                var viewModel = new ZoneOverlayViewModel(zoneConfig);
                viewModel.ZoneSelected += OnZoneSelected;
                var window = new ZoneOverlay(screen) { DataContext = viewModel };
                window.Show();
            }
        }

        private void OnZoneSelected(object sender, Zone zone)
        {
            CloseOverlayWindows();
            var viewModel = sender as ZoneOverlayViewModel;
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            ShowFrameWindow(zone, viewModel);
        }

        private void CloseOverlayWindows()
        {
            foreach (var window in Windows.OfType<ZoneOverlay>())
            {
                window.Close();
            }
        }

        private void ShowFrameWindow(Zone zone, ZoneOverlayViewModel viewModel)
        {
            var scaleX = viewModel.ZoneSet.ReferenceBounds.Width / viewModel.Monitor.WorkingArea.Width * SystemInformation.dpiScaleX;
            var scaleY = viewModel.ZoneSet.ReferenceBounds.Height / viewModel.Monitor.WorkingArea.Height * SystemInformation.dpiScaleY;

            this.MainWindow = new MainWindow
            {
                Height = Math.Round(zone.Bounds.Height / scaleY),
                Width = Math.Round(zone.Bounds.Width / scaleX),
                Top = Math.Round((zone.Bounds.Y + viewModel.ZoneSet.ScreenOffset.Y) / scaleY),
                Left = Math.Round((zone.Bounds.X + viewModel.ZoneSet.ScreenOffset.X) / scaleX)
            };
            this.MainWindow.Show();
        }
    }
}
