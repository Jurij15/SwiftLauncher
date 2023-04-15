using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SwiftLauncher.Native
{
    public class Interop
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);

        private const int SPI_GETDESKWALLPAPER = 0x73;
        private const int MAX_PATH = 260;

        public static string GetDesktopWallpaperPath()
        {
            string RetValue = "";
            StringBuilder wallpaperPath = new StringBuilder(MAX_PATH);
            SystemParametersInfo(SPI_GETDESKWALLPAPER, MAX_PATH, wallpaperPath, 0);

            RetValue = wallpaperPath.ToString();

            return RetValue;
        }
    }
}
