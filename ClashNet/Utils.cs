using SimpleUI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace ClashNet
{
    public static class Utils
    {
        public static string LogPath = System.AppDomain.CurrentDomain.BaseDirectory+ "logs";

        public static void ChangTheme(string value, IThemeApply<App> ThemeApply)
        {
            switch (value)
            {
                case "暗色":
                    ThemeApply.Apply(SimpleUI.Themes.ThemeType.Dark);
                    break;
                case "浅色":
                    ThemeApply.Apply(SimpleUI.Themes.ThemeType.Light);
                    break;
                case "自动":
                    ThemeApply.Apply(SimpleUI.Themes.ThemeType.Default);
                    break;
            }
        }
    }
}
