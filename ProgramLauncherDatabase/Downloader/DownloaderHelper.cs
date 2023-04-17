using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Shell.Interop;
using System.Windows;
using System.Text.RegularExpressions;

namespace SwiftLauncher.Downloader
{
    public class DownloaderHelper
    {
        public static bool bDoesAppExistInWinGet()
        {
            bool RetVal = false;

            return RetVal;
        }

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

        public static string SearchForAppIDs(string Query)
        {
            string RetVal = null;

            return RetVal;
        }

        public static string ListAllInstalledPackages()
        {
            bool RetVal = false;

            ProcessStartInfo processInfo = new ProcessStartInfo("winget", "list")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process process = new Process { StartInfo = processInfo };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
}
