using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgramLauncherDatabase.Helpers
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
    }
}
