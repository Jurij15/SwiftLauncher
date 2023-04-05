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
using System.Windows.Shapes;

namespace SulfurLauncher
{
    /// <summary>
    /// Interaction logic for QuickLauncherWindow.xaml
    /// </summary>
    public partial class QuickLauncherWindow : Wpf.Ui.Controls.UiWindow
    {
        public QuickLauncherWindow()
        {
            InitializeComponent();
            //QuickTitleBar.MinimizeToTray = true;
        }

        private void TaskBarIcon_LeftClick(Wpf.Ui.Controls.NotifyIcon sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLicked");
            e.Handled = true;
            return;
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            Config.MainWindow.Show();
        }
    }
}
