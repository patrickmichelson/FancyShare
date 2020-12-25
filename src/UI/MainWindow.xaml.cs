using FancyShare.Utils;
using System;
using System.Windows;

namespace FancyShare.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            Win32.IgnoreMouseEvents(Win32.GetHandle(this));
        }
    }
}