using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SwiftLauncher.Downloader;

namespace SwiftLauncher.Pages
{
    /// <summary>
    /// Interaction logic for DownloadMoreAppsPage.xaml
    /// </summary>
    public partial class DownloadMoreAppsPage : Page
    {
        public DownloadMoreAppsPage()
        {
            InitializeComponent();

            if (DownloaderHelper.bIsWinGetInstalled())
            {
                WinGetInstalledCard.Visibility = Visibility.Visible;
                WinGetNotInstalledCard.Visibility = Visibility.Collapsed;
            }
            else
            {
                WinGetInstalledCard.Visibility=Visibility.Collapsed;
                WinGetNotInstalledCard.Visibility =Visibility.Visible;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SearchBox.Width = this.Width /2;
        }

        private void RootWrapPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        private void WinGetInstallBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClosePopupBtn_Click(object sender, RoutedEventArgs e)
        {
            WinGetInstalledCard.Visibility = Visibility.Collapsed ;
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void InstallPackageBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListInstalledPackages_Click(object sender, RoutedEventArgs e)
        {
            OutputBox.Text = DownloaderHelper.ListAllInstalledPackages();
        }
    }
}
