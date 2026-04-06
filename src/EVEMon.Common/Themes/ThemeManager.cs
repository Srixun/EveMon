using System.Collections.Generic;
using System.Drawing;

namespace EVEMon.Common.Themes
{
    public class ThemeInfo
    {
        public string Name { get; set; }
        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
        public Color BackgroundColor { get; set; }
        public Color AccentColor { get; set; }

        public ThemeInfo(string name, string primaryHex, string secondaryHex, string backgroundHex, string accentHex)
        {
            Name = name;
            PrimaryColor = ColorTranslator.FromHtml(primaryHex);
            SecondaryColor = ColorTranslator.FromHtml(secondaryHex);
            BackgroundColor = ColorTranslator.FromHtml(backgroundHex);
            AccentColor = ColorTranslator.FromHtml(accentHex);
        }
    }

    public static class ThemeManager
    {
        public static readonly Dictionary<string, ThemeInfo> Themes = new Dictionary<string, ThemeInfo>
        {
            { "Amarr", new ThemeInfo("Amarr", "#A98965", "#F9E694", "#101112", "#F0924D") },
            { "Caldari", new ThemeInfo("Caldari", "#A2D6DD", "#4DBABC", "#101112", "#F0924D") },
            { "Carbon", new ThemeInfo("Carbon", "#E6E6E6", "#E6E6E6", "#101112", "#F0924D") },
            { "Gallente", new ThemeInfo("Gallente", "#62B587", "#7EA996", "#101112", "#F0924D") },
            { "Minmatar", new ThemeInfo("Minmatar", "#BA4D35", "#8F4331", "#101112", "#F0924D") },
            { "ORE", new ThemeInfo("ORE", "#D9B438", "#769A9D", "#101112", "#D6948A") },
            { "Photon", new ThemeInfo("Photon", "#60A0C2", "#60A0C2", "#101112", "#F0924D") },
            { "Servant Sisters of EVE", new ThemeInfo("Servant Sisters of EVE", "#DE5555", "#A2D6D6", "#101112", "#C2BE48") }
        };

        private static ThemeInfo _currentTheme = Themes["Carbon"];

        public static ThemeInfo CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (value != null)
                {
                    _currentTheme = value;
                    OnThemeChanged();
                }
            }
        }

        public static event System.EventHandler ThemeChanged;

        private static void OnThemeChanged()
        {
            ThemeChanged?.Invoke(null, System.EventArgs.Empty);
        }
    }
}