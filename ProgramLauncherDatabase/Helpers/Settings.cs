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
        public static string QuickLauncherPositioningConfig = @"Config/QuickLauncherPositioning";
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
            File.Create(QuickLauncherPositioningConfig);
        }

        public static void FilterLaunchArguments()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg == "-DoShowQuickLauncherOnly")
                {
                   Config.bOnlyStartQuickLauncher = true;
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
            foreach (var app in File.ReadAllLines(QuickLaunchIDSConfig))
            {
                if (app == ID)
                {
                    return;
                    break;
                }
            }
            using(StreamWriter sw = File.AppendText(QuickLaunchIDSConfig))
            {
                sw.WriteLine(ID);
                sw.Close();
            }
        }

        public static void RemoveAppFromQuickLaunch(string ID)
        {
            string filePath = QuickLaunchIDSConfig;
            string wordToDelete = ID;

            // Read the file into memory
            string[] lines = File.ReadAllLines(filePath);

            // Remove the word to delete from each line
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace(wordToDelete, string.Empty);
            }

            // Overwrite the file with the updated content
            File.WriteAllLines(filePath, lines);
        }

        public static void ChangePosition(string newPos)
        {
            File.Delete(QuickLauncherPositioningConfig);

            using (StreamWriter sw = File.CreateText(QuickLauncherPositioningConfig))
            {
                sw.Write(newPos);
                sw.Close();
            }
        }

        public static string GetPosition() //TODO: MAKE THIS BE AN ENUM!
        {
            string RetVal = string.Empty;
            RetVal = File.ReadAllText(QuickLauncherPositioningConfig);
            
            return RetVal;
        }
    }
}
