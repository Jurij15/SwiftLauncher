using SulfurLauncher.Database;
using SulfurLauncher.Helpers;
using SulfurLauncher.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
using System.Windows.Threading;

namespace SulfurLauncher
{
    /// <summary>
    /// Interaction logic for QuickLauncherWindow.xaml
    /// </summary>
    public partial class QuickLauncherWindow : Window, INotifyPropertyChanged
    {
        private double totalWidth;
        public double TotalWidth
        {
            get { return totalWidth; }
            set { totalWidth = value; NotifyPropertyChanged(); }
        }

        string QuickLauncherPosition = string.Empty;
        public List<string> AllIds = new List<string>();
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

            tb.Visibility = Visibility.Collapsed;

            cardHeaderPanel.Children.Add(img);
            cardHeaderPanel.Children.Add(tb);

            NewCard.Content = cardHeaderPanel;

            NewCard.Click += CardClicked_Handler;

            NewCard.Margin = new Thickness(2,2,2,2);
            NewCard.IsChevronVisible = false;

            AppsPanel.Children.Add(NewCard);
        }

        void CreateCardForTaskManager(string AccountName, string AppPath)
        {
            Wpf.Ui.Controls.CardAction NewCard = new Wpf.Ui.Controls.CardAction();
            StackPanel cardHeaderPanel = new StackPanel();
            TextBlock tb = new TextBlock();

            System.Drawing.Icon icn = null;
            Image img = new Image();
            Ellipse AppOpenBadge = new Ellipse();

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

            bool bDetected = false;
            foreach (Process exe in Process.GetProcesses())
            {
                if (exe.ProcessName == System.IO.Path.GetFileNameWithoutExtension(AppPath).ToString())
                {
                    bDetected = true; break;
                }
            }

            AppOpenBadge.Margin = new Thickness(0,8,0,0);
            AppOpenBadge.Visibility = Visibility.Visible;
            AppOpenBadge.Width = 5;
            AppOpenBadge.Height = 5;


            if (bDetected)
            {
                AppOpenBadge.Visibility = Visibility.Visible;
                AppOpenBadge.Fill = new SolidColorBrush(Colors.Green);
                AppOpenBadge.Name = "ProcessRunning";
            }
            else
            {
                AppOpenBadge.Visibility = Visibility.Hidden;
                AppOpenBadge.Name = "ProcessStopped";
            }

            tb.Text = AccountName;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Name = "AppNameBox";

            tb.Visibility = Visibility.Collapsed;

            cardHeaderPanel.Children.Add(img);
            cardHeaderPanel.Children.Add(tb);
            cardHeaderPanel.Children.Add(AppOpenBadge);

            NewCard.Content = cardHeaderPanel;

            NewCard.Click += TaskManagementCardClicked_Handler;
            NewCard.MouseRightButtonDown += TaskManagementRightCardClicked_Handler;

            NewCard.Margin = new Thickness(2, 2, 2, 2);
            NewCard.IsChevronVisible = false;

            AppsTaskManagerPanel.Children.Add(NewCard);
        }

        private void NewCard_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
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

            QuickLauncherPosition = Position;

            QuickTitleBar.MinimizeToTray = true;
            CreateAppsForEveryApp();

            this.SizeChanged += MainWindow_SizeChanged;

            double totalWidth = 0;
            foreach (UIElement item in AppsPanel.Children)
            {
                totalWidth += ((FrameworkElement)item).ActualWidth;
            }

            // Adjust the window width based on the total width of the items
            double padding = 20; // Add some padding to the window width
            totalWidth = totalWidth + padding;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Set the window position to the bottom center of the screen when the window size changes
            if (QuickLauncherPosition == "TOP")
            {
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;

                this.Top = 10;
            }
            else if (QuickLauncherPosition == "BOTTOM")
            {
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;

                this.Top = SystemParameters.PrimaryScreenHeight - this.Height - 60;
            }
            else
            {
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;

                this.Top = 10;
            }

            NotifyPropertyChanged();
        }

        void StartTimer()
        {
            DispatcherTimer t = new DispatcherTimer();
            t.Interval = TimeSpan.FromMilliseconds(500);
            t.Tick += timer_tick;
            t.Start();
        }

        void timer_tick(object sender, EventArgs e)
        {
            CreateTaskManagementCardsForEveryApp();
        }

        void CreateTaskManagementCardsForEveryApp()
        {
            AppsPanel.Visibility = Visibility.Collapsed;
            AppsTaskManagerPanel.Visibility = Visibility.Visible;
            AppsTaskManagerPanel.Children.Clear();
            DBReader Reader = new DBReader();
            foreach (var app in Settings.GetAllQuickLaunchAppIDS())
            {
                if (string.IsNullOrWhiteSpace(app)) { continue; }
                CreateCardForTaskManager(Reader.GetAppNameByID(app), Reader.GetAppExecutablePathByID(app));
            }
        }

        void CreateAppsForEveryApp()
        {
            DBReader Reader = new DBReader();
            foreach (var app in Settings.GetAllQuickLaunchAppIDS())
            {
                if (string.IsNullOrWhiteSpace(app)) {  continue; }
                CreateCard(Reader.GetAppNameByID(app), Reader.GetAppExecutablePathByID(app));
            }

            NotifyPropertyChanged();

        }

        private void TaskBarIcon_LeftClick(Wpf.Ui.Controls.NotifyIcon sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLicked");
            e.Handled = true;
            return;
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void ReturnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Config.MainWindow.Show();
            Config.bIsQuickLauncherVisible = false;
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

        bool FigureOutIfAppIsRunning(Wpf.Ui.Controls.CardAction UiElement)
        {
            bool RetVal = false;
            StackPanel content = (StackPanel)UiElement.Content;
            foreach (var item in content.Children)
            {
                if (item.GetType() == typeof(Ellipse))
                {
                    Ellipse badge = (Ellipse)item;
                    if (badge.Name == "ProcessRunning")
                    {
                        RetVal = true;
                    }
                    else
                    {
                        RetVal = false;
                    }
                }
            }
            return RetVal;
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

        private void TaskManagementCardClicked_Handler(object sender, RoutedEventArgs e)
        {
            bool IsAppRunning = FigureOutIfAppIsRunning((Wpf.Ui.Controls.CardAction)sender);
            if (IsAppRunning)
            {
                DBReader reader = new DBReader();
                string aName = FigureOutAppName((Wpf.Ui.Controls.CardAction)sender);
                string AppID = reader.GetAppIDByName(aName);
                string AppName = System.IO.Path.GetFileNameWithoutExtension(reader.GetAppExecutablePathByID(AppID));

                [DllImport("user32.dll")]
                static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
                [DllImport("user32.dll")]
                static extern bool IsIconic(IntPtr hWnd);

                IntPtr hWnd = Process.GetProcessesByName(AppName)[0].MainWindowHandle;
                bool isMinimized = IsIconic(hWnd);

                if (!isMinimized)
                {
                    ShowWindow(hWnd, 6);
                }
                else
                {
                    ShowWindow(hWnd, 9);
                }
            }
            else
            {
                CardClicked_Handler(sender, e);
            }
        }

        private void TaskManagementRightCardClicked_Handler(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
            //MessageBox.Show(AppsPanel.ActualWidth.ToString(), this.ActualWidth.ToString());
            this.Width = AppsPanel.ActualWidth + 50;
            this.Height = AppsPanel.ActualHeight;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            EnableBlur();
        }

        private void ExpandBtn_Click(object sender, RoutedEventArgs e)
        {
            StartTimer();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
