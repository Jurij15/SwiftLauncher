using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Shell.Interop;
using System.Windows;
using System.Text.RegularExpressions;
using Windows.Services.Store;

namespace SwiftLauncher.Downloader
{
    public class DownloaderHelper
    {

        public static bool bIsWinGetInstalled()
        {
            bool RetVal = false;

            ProcessStartInfo processInfo = new ProcessStartInfo("winget", "--info")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process process = new Process { StartInfo = processInfo };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (!output.Contains("Windows Package Manager"))
            {
                RetVal = false;
            }
            else
            {
                RetVal = true;
            }

            return RetVal;
        }
    }
}
