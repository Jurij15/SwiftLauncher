using SulfurLauncher.Database;
using SulfurLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SulfurLauncher.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        #region Cards and related stuff that should be changed so its not duplicated
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

                img.Height = 40;
                img.Width = 60;

                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.Fant);
            }

            tb.Text = AccountName;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Name = "AppNameBox";

            //img.Visibility = Visibility.Collapsed;
            tb.Visibility = Visibility.Visible;

            cardHeaderPanel.Children.Add(img);
            cardHeaderPanel.Children.Add(tb);

            NewCard.Content = cardHeaderPanel;

            NewCard.Click += CardClicked_Handler;

            NewCard.Margin = new Thickness(2, 6, 2, 0);
            NewCard.IsChevronVisible = false;

            //NewCard.Height = 120;
            //NewCard.Width = 120;

            //RootWrapPanel.Children.Add(NewCard);
            AppsPanel.Children.Add(NewCard);
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
        #endregion
        public HomePage()
        {
            InitializeComponent();
            CreateAppsForEveryApp();
        }

        void CreateAppsForEveryApp()
        {
            DBReader Reader = new DBReader();
            foreach (var app in Settings.GetAllQuickLaunchAppIDS())
            {
                if (string.IsNullOrWhiteSpace(app)) { continue; }
                CreateCard(Reader.GetAppNameByID(app), Reader.GetAppExecutablePathByID(app));
            }
        }

        private void TemplateCard1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddAppCard_Click(object sender, RoutedEventArgs e)
        {
            Config.GlobalNavigation.Navigate(typeof(AddAppPage));
        }

        private void ViewAllAppsCard_Click(object sender, RoutedEventArgs e)
        {
            Config.GlobalNavigation.Navigate(typeof(AllAppsPage));
        }
    }
}
