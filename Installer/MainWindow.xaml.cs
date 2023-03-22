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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.UiWindow
    {
        void CheckInstalledDotNetVersion()
        {
            string command = "dotnet --info"; // CMD command to execute
            Process process = new Process();

            // Set the process information
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + command; // "/c" indicates that we want to run a command
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            // Start the process
            process.Start();

            // Read the output from the process
            string output = process.StandardOutput.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();

            // Print the output
            if (!output.Contains("Microsoft.WindowsDesktop.App 7.0.3"))
            {
                InstallBtn.IsEnabled = false;
                InstallLocBox.IsEnabled = false;
                DotNetUnsupportedInfo.IsOpen = true;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            CheckInstalledDotNetVersion();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
