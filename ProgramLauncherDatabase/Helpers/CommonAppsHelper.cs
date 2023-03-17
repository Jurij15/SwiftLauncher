using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ProgramLauncherDatabase.Database;
using System.Runtime.CompilerServices;

namespace ProgramLauncherDatabase.Helpers
{
    public class CommonAppsHelper
    {
        public async static void CheckForCommonAppsAndAddThemToDatabase()
        {
            Config.InitDBConnection();
            string msedgeLoc = @"C:\\\\Program Files (x86)\\\\Microsoft\\\\Edge\\\\Application\\\\msedge.exe\";
            string chromeLoc = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            string firefoxLoc = @"C:\\Program Files\\Mozilla Firefox\\firefox.exe";
            string notepadppLoc = @"C:\\Program Files\\Notepad++\\notepad++.exe";
            string obsLoc = @"C:\\Program Files\\obs-studio\\bin\\64bit\\obs64.exe";
            string qbittorrentLoc = @"C:\\Program Files\\qBittorrent\\qbittorrent.exe";
            if (File.Exists(msedgeLoc))
            {
                DBCreator db = new DBCreator();
                db.CreateApp("Microsoft Edge", msedgeLoc, "Web Browsers", "Microsoft Edge is a web browser developed by Microsoft, first released in 2015. It is designed to be fast, secure, and customizable, with features such as built-in compatibility with Microsoft services and extensions. Edge is available for Windows, macOS, iOS, and Android platforms.", "");
            }
            if (File.Exists(chromeLoc))
            {
                DBCreator db = new DBCreator();
                db.CreateApp("Google Chrome", chromeLoc, "Web Browsers", "Google Chrome is a web browser developed by Google, first released in 2008. It is known for its speed, simplicity, and user-friendly interface. Chrome offers a range of features such as tabbed browsing, extensions, and synchronization across multiple devices. It is available for Windows, macOS, Linux, iOS, and Android platforms.", "");
            }
            if (File.Exists(firefoxLoc))
            {
                DBCreator db = new DBCreator();
                db.CreateApp("Mozilla FireFox", firefoxLoc, "Web Browsers", "Mozilla Firefox is a web browser developed by Mozilla Foundation, first released in 2004. It is known for its fast performance, strong privacy features, and open-source nature. Firefox offers a range of features such as tabbed browsing, add-ons, and customization options. It is available for Windows, macOS, Linux, iOS, and Android platforms.", "");
            }
            if (File.Exists(notepadppLoc))
            {
                DBCreator db = new DBCreator();
                db.CreateApp("Notepad++", notepadppLoc, "Deveopment Tools", "Notepad++ is a free source code editor and text editor, first released in 2003. It is designed to provide a lightweight and efficient environment for coding and editing text. Notepad++ offers a range of features such as syntax highlighting, auto-completion, and macro recording. It is available for Windows platforms.", "");
            }
            if (File.Exists(obsLoc))
            {
                DBCreator db = new DBCreator();
                db.CreateApp("OBS", obsLoc, "Tools", "OBS (Open Broadcaster Software) is a free and open-source software suite for video recording and live streaming, first released in 2012. It is designed for content creators and gamers who want to capture and share their gameplay or other activities on platforms such as Twitch, YouTube, or Facebook. OBS offers a range of features such as scene composition, audio mixing, and customizable transitions. It is available for Windows, macOS, and Linux platforms.", "");
            }
            if (File.Exists(qbittorrentLoc))
            {
                DBCreator db = new DBCreator();
                db.CreateApp("QBittorrent", qbittorrentLoc, "Tools", "qBittorrent is a free and open-source BitTorrent client, first released in 2006. It is designed to provide a lightweight and user-friendly environment for downloading and sharing files via the BitTorrent protocol. qBittorrent offers a range of features such as integrated search engine, bandwidth scheduling, and torrent prioritization. It is available for Windows, macOS, Linux, and FreeBSD platforms.", "");
            }
            Config.DisconnectFromDB();
        }
    }
}
