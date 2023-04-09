using SulfurLauncher.Database;
using SulfurLauncher.Helpers;
using SulfurLauncher.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SulfurLauncher
{
    /// <summary>
    /// Interaction logic for QuickLauncherWindow.xaml
    /// </summary>
    public partial class QuickLauncherWindow : Window
    {
        #region WindowBlur And Runded Corners
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private uint _blurOpacity;
        public double BlurOpacity
        {
            get { return _blurOpacity; }
            set { _blurOpacity = (uint)value; EnableBlur(); }
        }

        private uint _blurBackgroundColor = 0x990000; /* BGR color format */
        internal enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
            ACCENT_INVALID_STATE = 5
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public uint AccentFlags;
            public uint GradientColor;
            public uint AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        {
            // ...
            WCA_ACCENT_POLICY = 19
            // ...
        }
        internal void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
            accent.GradientColor = (_blurOpacity << 24) | (_blurBackgroundColor & 0xFFFFFF);

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        public enum DWMWINDOWATTRIBUTE

        {

            DWMWA_WINDOW_CORNER_PREFERENCE = 33

        }



        public enum DWM_WINDOW_CORNER_PREFERENCE

        {

            DWMWCP_DEFAULT = 1,

            DWMWCP_DONOTROUND = 1,

            DWMWCP_ROUND = 2,

            DWMWCP_ROUNDSMALL = 1

        }



        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]

        private static extern long DwmSetWindowAttribute(IntPtr hwnd,

                                                         DWMWINDOWATTRIBUTE attribute,

                                                         ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,

                                                         uint cbAttribute);
        #endregion
        #region Card
        void CreateCard(string AccountName, string AppPath)
        {
            Wpf.Ui.Controls.CardAction NewCard = new Wpf.Ui.Controls.CardAction();
            StackPanel cardHeaderPanel = new StackPanel();

            TextBlock tb = new TextBlock();
            System.Drawing.Icon icn = null;
            Image img = new Image();

            bool bShouldShowIcon = false;

            if (!string.IsNullOrEmpty(AppPath)) { icn = System.Drawing.Icon.ExtractAssociatedIcon(AppPath); bShouldShowIcon = true; }

            if (bShouldShowIcon)
            {
                img.Visibility = Visibility.Visible;
                img.Source = Imaging.CreateBitmapSourceFromHIcon(icn.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                img.Height = 30;
                img.Width = 50;

                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.Fant);
            }

            tb.Text = AccountName;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Name = "AppNameBox";

            //img.Visibility = Visibility.Collapsed;
            tb.Visibility = Visibility.Collapsed;

            cardHeaderPanel.Children.Add(img);
            cardHeaderPanel.Children.Add(tb);

            NewCard.Content = cardHeaderPanel;

            NewCard.Click += CardClicked_Handler;

            NewCard.Margin = new Thickness(4,4,4,4);
            NewCard.IsChevronVisible = false;

            //NewCard.Height = 120;
            //NewCard.Width = 120;

            //RootWrapPanel.Children.Add(NewCard);
            AppsPanel.Children.Add(NewCard);
        }
        #endregion
        public QuickLauncherWindow(string Position)
        {
            InitializeComponent();

            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();

            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;

            var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;

            DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint)); //rounded corners

            if (Position == "TOP")
            {
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;

                this.Top = 10;
            }
            else if (Position == "BOTTOM")
            {
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;

                this.Top = SystemParameters.PrimaryScreenHeight - this.Height - 60;
            }
            else
            {
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;

                this.Top = 10;
            }

            QuickTitleBar.MinimizeToTray = true;
            CreateAppsForEveryApp();
        }

        void CreateAppsForEveryApp()
        {
            DBReader Reader = new DBReader();
            foreach (var app in Settings.GetAllQuickLaunchAppIDS())
            {
                if (string.IsNullOrWhiteSpace(app)) {  continue; }
                CreateCard(Reader.GetAppNameByID(app), Reader.GetAppExecutablePathByID(app));
            }
        }

        private void TaskBarIcon_LeftClick(Wpf.Ui.Controls.NotifyIcon sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLicked");
            e.Handled = true;
            return;
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*
            this.Hide();
            Config.MainWindow.Show();
            Config.MainWindow.WindowState = WindowState.Normal;
            */
        }

        private void ReturnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Config.MainWindow.Show();
            Config.MainWindow.WindowState = WindowState.Normal;
        }

        string FigureOutAppName(Wpf.Ui.Controls.CardAction UiElement)
        {
            string RetValue = string.Empty;
            StackPanel content = (StackPanel)UiElement.Content;
            foreach (var item in content.Children)
            {
                if (item.GetType() == typeof(TextBlock))
                {
                    TextBlock textBlock = (TextBlock)item;
                    if (textBlock.Name == "AppNameBox")
                    {
                        RetValue = textBlock.Text;
                    }
                }
            }
            return RetValue;
        }

        private void ExitCompletely_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CardClicked_Handler(object sender, RoutedEventArgs e)
        {
            DBReader reader = new DBReader();
            string AppName = FigureOutAppName((Wpf.Ui.Controls.CardAction)sender);
            string AppID = reader.GetAppIDByName(AppName);
            string AppPath = reader.GetAppExecutablePathByID(AppID);
            string AppLaunchArguments = reader.GetAppLaunchArgumentsByID(AppID);

            if (!string.IsNullOrEmpty(AppLaunchArguments))
            {
                Process.Start(AppPath, AppLaunchArguments);
            }
            else
            {
                Process.Start(AppPath);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            EnableBlur();
        }
    }
}
