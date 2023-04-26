using SwiftLauncher.Classes.DatabaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwiftLauncher.Enums;
using SwiftLauncher.Helpers;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Collections.Immutable;

namespace SwiftLauncher.Classes
{
    /// <summary>
    /// The app must already exist in the database!
    /// </summary>
    public class AppCardActionClass : DBInteractor
    {
        //properties
        private string AppID { get; set; }
        private string AppName { get; set; }
        private string AppPath { get; set; }
        private string AppCategory { get; set; }
        private string AppDescription { get; set; }
        private string AppLaunchArguments { get; set; }

        private string AppLaunchAsAdmin { get; set; }
        private string AppShowInQuickLauncher { get; set; }

        public AppCardActionClass(string ID)
        {
            AppID = ID;

            AppName = GetAppPropertyFromDBByID(ID, Enums.DatabaseAppProperty.Name);
            AppPath = GetAppPropertyFromDBByID(ID, DatabaseAppProperty.ExecutablePath);
            AppCategory = GetAppPropertyFromDBByID(ID, DatabaseAppProperty.ApplicationCategory);
            AppDescription = GetAppPropertyFromDBByID(ID, DatabaseAppProperty.ApplicationNotes);
            AppLaunchArguments = GetAppPropertyFromDBByID(ID, DatabaseAppProperty.ApplicationLaunchArguments);
            //AppLaunchAsAdmin = GetAppPropertyFromDBByID(ID, DatabaseAppProperty.ApplicationLaunchAsAdmin);
            //AppShowInQuickLauncher = GetAppPropertyFromDBByID(ID, DatabaseAppProperty.ApplicationShowInQuickLauncher);
        }

        public Wpf.Ui.Controls.CardAction CreateStandardCard(CardDisplayMode DisplayMode, RoutedEventHandler DirectLaunch_Click, RoutedEventHandler OptionsBtn_Click = null, RoutedEventHandler CardClicked_Handler = null)
        {
            Wpf.Ui.Controls.CardAction NewCard = new Wpf.Ui.Controls.CardAction();
            StackPanel cardHeaderPanel = new StackPanel();
            Wpf.Ui.Controls.Button DirectLaunchBtn = new Wpf.Ui.Controls.Button();
            Wpf.Ui.Controls.Button OptionsBtn = new Wpf.Ui.Controls.Button();

            Wpf.Ui.Controls.SymbolIcon icon = new Wpf.Ui.Controls.SymbolIcon();
            TextBlock tb = new TextBlock();
            TextBlock CategoryBox = new TextBlock();
            System.Drawing.Icon icn = null;
            Image img = new Image();

            bool bShouldShowIcon = false;

            try
            {
                if (!string.IsNullOrEmpty(AppPath)) { icn = System.Drawing.Icon.ExtractAssociatedIcon(AppPath); bShouldShowIcon = true; }
            }
            catch (Exception ex)
            {
                //an exception occured
                bShouldShowIcon = false;
            }

            if (bShouldShowIcon)
            {
                icon.Visibility = Visibility.Collapsed;
                img.Visibility = Visibility.Visible;
                img.Source = Imaging.CreateBitmapSourceFromHIcon(icn.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                img.Height = 50;
                img.Width = 70;

                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.Fant);
            }
            else
            {
                icon.Symbol = Wpf.Ui.Common.SymbolRegular.Apps24;
                icon.Visibility = Visibility.Visible;
                img.Visibility = Visibility.Collapsed;
            }

            tb.Text = AppName;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Name = "AppNameBox";
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            //tb.MaxWidth = 130;
            tb.TextTrimming = TextTrimming.CharacterEllipsis;
            tb.TextWrapping = TextWrapping.NoWrap;

            CategoryBox.Foreground = Brushes.LightGray;
            CategoryBox.Text = AppCategory;
            CategoryBox.FontSize = 12;
            CategoryBox.HorizontalAlignment = HorizontalAlignment.Center;

            DirectLaunchBtn.Appearance = Wpf.Ui.Controls.ControlAppearance.Primary;
            DirectLaunchBtn.Content = "Launch";
            DirectLaunchBtn.Name = StringsHelper.ReplaceDisallowedChars(AppName);
            DirectLaunchBtn.VerticalAlignment = VerticalAlignment.Bottom;
            DirectLaunchBtn.HorizontalAlignment = HorizontalAlignment.Center;
            DirectLaunchBtn.Click += DirectLaunch_Click;
            DirectLaunchBtn.Margin = new Thickness(2, 4, 2, 0);
            DirectLaunchBtn.ToolTip = "Launch " + AppName;

            OptionsBtn.Appearance = Wpf.Ui.Controls.ControlAppearance.Secondary;
            OptionsBtn.Icon = Wpf.Ui.Common.SymbolRegular.MoreVertical16;
            OptionsBtn.VerticalAlignment = VerticalAlignment.Top;
            OptionsBtn.HorizontalAlignment = HorizontalAlignment.Right;
            OptionsBtn.Height = 40;
            OptionsBtn.Width = 30;
            OptionsBtn.Margin = new Thickness(-15, -20, -12, -10);
            OptionsBtn.Click += OptionsBtn_Click;

            cardHeaderPanel.Children.Add(OptionsBtn);
            cardHeaderPanel.Children.Add(img);
            cardHeaderPanel.Children.Add(icon);
            cardHeaderPanel.Children.Add(tb);
            cardHeaderPanel.Children.Add(CategoryBox);
            cardHeaderPanel.Children.Add(DirectLaunchBtn);

            NewCard.Content = cardHeaderPanel;

            NewCard.Click += CardClicked_Handler;

            //NewCard.MaxHeight = 250;
            //NewCard.MaxWidth = 230;

            NewCard.Height = 170;
            NewCard.Width = 150;

            NewCard.ToolTip = "View details about " + AppName;

            NewCard.Margin = new Thickness(4, 4, 4, 4);
            NewCard.IsChevronVisible = false;

            //this is going to be a little stupid, but the above code is already as bad as it is and i do not want to repair or break it any further

            switch (DisplayMode)
            {
                case CardDisplayMode.Small:
                    cardHeaderPanel.Children.Clear();
                    cardHeaderPanel.Children.Add(icon);
                    cardHeaderPanel.Children.Add(tb);
                    cardHeaderPanel.Children.Add(CategoryBox);
                    //cardHeaderPanel.Children.Add(DirectLaunchBtn);
                    NewCard.Height = 140;
                    NewCard.Width = 120;
                    break;

                case CardDisplayMode.Normal:
                    break;

                case CardDisplayMode.Large:
                    NewCard.Height = 250;
                    NewCard.Width = 230;
                    break;

                case CardDisplayMode.Expanded:
                    cardHeaderPanel.Children.Clear();
                    Grid headerGrid = new Grid();

                    StackPanel NameAndIconPanel = new StackPanel();
                    NameAndIconPanel.Orientation = Orientation.Horizontal;

                    StackPanel DirectLaunchAndOptionsPanel = new StackPanel();
                    DirectLaunchAndOptionsPanel.Orientation = Orientation.Horizontal;
                    DirectLaunchAndOptionsPanel.HorizontalAlignment = HorizontalAlignment.Right;

                    img.HorizontalAlignment = HorizontalAlignment.Left;
                    img.VerticalAlignment = VerticalAlignment.Center;

                    CategoryBox.VerticalAlignment = VerticalAlignment.Center;
                    CategoryBox.HorizontalAlignment = HorizontalAlignment.Center;

                    DirectLaunchBtn.VerticalAlignment = VerticalAlignment.Center;
                    DirectLaunchBtn.HorizontalAlignment = HorizontalAlignment.Right;

                    OptionsBtn.HorizontalAlignment = HorizontalAlignment.Right;
                    OptionsBtn.Margin = new Thickness(0, 0, 0, 0);
                    OptionsBtn.VerticalAlignment = VerticalAlignment.Center;

                    tb.HorizontalAlignment = HorizontalAlignment.Left;
                    tb.VerticalAlignment = VerticalAlignment.Center;

                    NameAndIconPanel.Children.Add(img);
                    NameAndIconPanel.Children.Add(tb);
                    headerGrid.Children.Add(NameAndIconPanel);

                    headerGrid.Children.Add(CategoryBox);

                    DirectLaunchAndOptionsPanel.Children.Add(DirectLaunchBtn);
                    DirectLaunchAndOptionsPanel.Children.Add(OptionsBtn);
                    headerGrid.Children.Add(DirectLaunchAndOptionsPanel);

                    cardHeaderPanel.Children.Add(headerGrid);

                    NewCard.Width = Config.MainWindow.Width - 100;
                    NewCard.Height = 90;
                    break;
            }

            //NewCard.Height = 120;
            //NewCard.Width = 120;

            //RootWrapPanel.Children.Add(NewCard);
            return NewCard;
        }
    }
}
