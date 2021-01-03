using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;

namespace FancyShare.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int BorderWidth => 3;
        public int TopbarHeight => 20;

        const uint WM_NCHITTEST = 0x0084;


        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            this.Loaded += async (s, e) => await HideInfoText();
            Topbar.MouseEnter += async (s, e) => await ShowTopbar(true);
            Topbar.MouseLeave += async (s, e) => await ShowTopbar(false);
            Topbar.MouseLeftButtonDown += (s, e) => DragMove();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            hwndSource?.AddHook(WndProc);
        }

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            bool ctrlKeyDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl);

            if (msg == WM_NCHITTEST && !ctrlKeyDown)
            {
                Debug.WriteLine("IGNORE HITTEST");
                handled = true;
                return new IntPtr(-1);
            }

            return IntPtr.Zero;
        }

        bool showTopbar;

        private async Task ShowTopbar(bool isVisible)
        {
            showTopbar = isVisible;
            if (!isVisible) await Task.Delay(400);

            var newHeight = showTopbar ? TopbarHeight : BorderWidth;
            var animation = new DoubleAnimation(newHeight, TimeSpan.FromMilliseconds(75));
            Topbar.BeginAnimation(HeightProperty, animation);
        }


        private async Task HideInfoText()
        {
            await Task.Delay(2000);
            var animation = new DoubleAnimation(0, TimeSpan.FromMilliseconds(500));
            animation.Completed += (s,e) => InfoText.Visibility = Visibility.Hidden;
            InfoText.BeginAnimation(OpacityProperty, animation);
        }
    }
}