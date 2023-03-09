using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProgramLauncherDatabase.Pages
{
    /// <summary>
    /// Interaction logic for AppDetailsPage.xaml
    /// </summary>
    public partial class AppDetailsPage : Wpf.Ui.Controls.UiPage
    {
        System.Drawing.Icon icn = null;

        public AppDetailsPage()
        {
            InitializeComponent();

            SetAppDetails();

            DispatcherTimer timer = new DispatcherTimer();  
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer_tick;
            timer.Start();
        }

        void SetAppIcon()
        {
            if (!string.IsNullOrEmpty(CurrentAppDefinitions.AppExecutablePath)) { icn = System.Drawing.Icon.ExtractAssociatedIcon(CurrentAppDefinitions.AppExecutablePath);}
            if (icn != null)
            {
                AppImage.Source = Imaging.CreateBitmapSourceFromHIcon(icn.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else { return;}
        }

        void SetAppDetails()
        {
            AppNameBlock.Text = CurrentAppDefinitions.AppName;
            AppPathBlock.Text = CurrentAppDefinitions.AppExecutablePath;
            AppNotesBox.Text = CurrentAppDefinitions.AppNotes;
            AppCategoryBadge.Content = CurrentAppDefinitions.AppCategory;

            SetAppIcon();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            //this will refresh things
            SetAppDetails();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Config.GlobalFrame.Navigate(new AllAppsPage());
        }
    }

    public static class CurrentAppDefinitions
    {
        public static string AppName { get; set; }
        public static string AppExecutablePath { get; set;}
        public static string AppCategory { get; set;}
        public static string AppNotes { get; set;}
        public static string AppLaunchArguents { get; set;}
    }
}
