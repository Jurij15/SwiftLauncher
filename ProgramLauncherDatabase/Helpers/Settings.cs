using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SulfurLauncher.Helpers
{
    public class Settings
    {
        public static string RecentAppsConfig = @"Config/RecentlyOpenedApps.txt";
        public static bool bIsFirstTimeUse()
        {
            bool RetVal = false;
            if (!File.Exists(RecentAppsConfig))
            {
                RetVal = true;
            }

            return RetVal;
        }

        public static void CreateSettings()
        {
            Directory.CreateDirectory("Config/");
            File.Create(RecentAppsConfig);
        }

        public static void FilterLaunchArguments()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg == "-DoShowQuickLauncherOnly")
                {
                    //Config.bOnlyStartQuickLauncher = true;
                }
            }
        }
    }
}
