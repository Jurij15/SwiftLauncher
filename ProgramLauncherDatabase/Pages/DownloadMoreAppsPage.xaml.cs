﻿using System;
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
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SearchBox.Width = this.Width /2;
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
