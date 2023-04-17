using Microsoft.VisualBasic.ApplicationServices;
using SulfurLauncher;
using SulfurLauncher.Database;
using SulfurLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SwiftLauncher.Helpers
{
    public class StartMenuApps
    {
        public static async Task AddAllStartMenuApps()
        { 
            string startMenuPath = string.Empty;
            if (Directory.Exists(@"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs"))
            {
                startMenuPath = @"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs";
            }
            else
            {
                startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            }

            startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            //CommonAppsHelper.CheckForCommonAppsAndAddThemToDatabase();
            Config.InitDBConnection();
            string[] files = Directory.GetFiles(startMenuPath, "*", SearchOption.AllDirectories);
            //MessageBox.Show(files.Length.ToString());

            foreach (string app in files)
            {
                if (!app.Contains("lnk"))
                {
                    continue;
                }
                string AppName = string.Empty;
                string AppExePath = string.Empty;
                string AppCategory = string.Empty;
                string AppDesc = string.Empty;

                AppExePath = StringsHelper.GetLnkTarget(app);
                AppName = Path.GetFileNameWithoutExtension(app);

                if (!AppExePath.Contains("exe"))
                {
                    continue;
                }
                if (AppName.Contains("uninstall", StringComparison.OrdinalIgnoreCase) || AppName.Contains("install", StringComparison.OrdinalIgnoreCase) || AppName.Contains("installer", StringComparison.OrdinalIgnoreCase) || AppName.Contains("updater", StringComparison.OrdinalIgnoreCase) || AppName.Contains("error", StringComparison.OrdinalIgnoreCase) || AppName.Contains("command prompt", StringComparison.OrdinalIgnoreCase) || AppName.Contains("speech recognition", StringComparison.OrdinalIgnoreCase))
                {
                    continue; //ignore common installers and uninstallers, updaters and what not
                }
                if (AppExePath.Contains("system32") || AppExePath.Contains("powershell"))
                {
                    continue; //remove some default windows apps
                }
                //add the app category and desc to any known app

                DBCreator dBCreator = new DBCreator();
                await dBCreator.CreateAppAsync(AppName, AppExePath, AppCategory, AppDesc, "");
            }

            Config.DisconnectFromDB();
        }

        public static string[] GetAllApps()
        {
            List<string> RetVal = new List<string>();
            List<string> ToRemoveApps = new List<string>();
            string startMenuPath = @"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs";

            foreach (var appPath in Directory.GetFiles(startMenuPath, "*", SearchOption.AllDirectories))
            {
                if (!appPath.Contains("lnk")) continue;
                RetVal.Add(appPath);
            }

            foreach (var app in RetVal)
            {
                if (!StringsHelper.GetLnkTarget(app).Contains("exe"))
                {
                    ToRemoveApps.Add(app);
                }
            }

            foreach (var item in ToRemoveApps)
            {
                RetVal.Remove(item);
            }

            ToRemoveApps.Clear();

            return RetVal.ToArray();
        }

        public static async Task AddAppToDBByName(string Name)
        {
            List<string> Apps = GetAllApps().ToList();

            foreach (var item in Apps)
            {
                if (Path.GetFileNameWithoutExtension(item) == Name)
                {
                    string AppPatn = StringsHelper.GetLnkTarget(item);
                    string AppName = Path.GetFileNameWithoutExtension(item);

                    DBCreator dBCreator = new DBCreator();
                    await dBCreator.CreateAppAsync(AppName, AppPatn);
                }
            }
        }
    }
}
