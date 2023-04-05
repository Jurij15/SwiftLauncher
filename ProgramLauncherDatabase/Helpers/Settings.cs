using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace SulfurLauncher.Helpers
{
    public class Settings
    {
        public static string RecentAppsConfig = @"Config/RecentlyOpenedApps.txt";
        public static string QuickLaunchIDSConfig = @"Config/QuickLaunchAppIDS.txt";
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
            File.Create(QuickLaunchIDSConfig);
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

        public static string[] GetAllQuickLaunchAppIDS()
        {
            List<string> ids = new List<string>();
            foreach (var item in File.ReadAllLines(QuickLaunchIDSConfig))
            {
                ids.Add(item);  
            }

            return ids.ToArray();
        }

        public static void AddNewQuickLaunchApp(string ID)
        {
            using(StreamWriter sw = File.AppendText(QuickLaunchIDSConfig))
            {
                sw.WriteLine(ID);
                sw.Close();
            }
        }

        public static void RemoveAppFromQuickLaunch(string ID)
        {
            string filePath = QuickLaunchIDSConfig;
            string lineToRemove = ID;

            // Read the contents of the file
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            // Remove the desired line
            lines.Remove(lineToRemove);

            // Write the updated contents back to the file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}
