using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace GUILib
{
    public enum BorderStyle
    {
        None,
        Default,
        Heart,
        Simple,
    }
    public enum Border
    {
        None,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Vertical,
        Horizontal,
    }

    public static class ScreenData
    {
        public static readonly Dictionary<Border, char> BorderDefault = new Dictionary<Border, char>
        {
            {Border.None,          ' '},
            {Border.TopLeft,       '╔'},
            {Border.TopRight,      '╗'},
            {Border.BottomLeft,    '╚'},
            {Border.BottomRight,   '╝'},
            {Border.Vertical,      '║'},
            {Border.Horizontal,    '═'},
        };

        public static readonly Dictionary<Border, char> BorderSimple = new Dictionary<Border, char>
        {
            {Border.None,          ' '},
            {Border.TopLeft,       '╔'},
            {Border.TopRight,      '╗'},
            {Border.BottomLeft,    '╚'},
            {Border.BottomRight,   '╝'},
            {Border.Vertical,      ' '},
            {Border.Horizontal,    ' '},
        };
        public static readonly Dictionary<Border, char> BorderHeart = new Dictionary<Border, char>
        {
            {Border.None,          ' '},
            {Border.TopLeft,       '╔'},
            {Border.TopRight,      '╗'},
            {Border.BottomLeft,    '╚'},
            {Border.BottomRight,   '╝'},
            {Border.Vertical,      (char)3},
            {Border.Horizontal,    (char)3},
        };

        public static int ScreenWidth  = 160;
        public static int ScreenHeight = 48;

        public static Vector2 InputPosition = new Vector2(5, ScreenHeight - 2);

        private const int HeaderPadding = 16;

        public static Vector2 BorderWidth = new Vector2(1, 1);
        public static Vector2 TextPadding = new Vector2(1, 1);

        public static int InputAreaHeight = 5;

        public static string Dithering1 = "░";
        public static string Dithering2 = "▒";
        public static string Dithering3 = "▓";
            	
        public static string CellOccupiedSymbol = "~";

        public static Color        DefaultBackgroundColor2       = Color.ForestGreen;
        public static Color        DefaultBackgroundColor        = Color.DarkBlue;
        public static Color        DefaultElementColor           = Color.White;
        public static Color        DefaultBorderColor            = Color.LightSlateGray;
        public static Color        DefaultTextBackgroundColor    = Color.Wheat;
        public static ConsoleColor DefaultTextColor              = ConsoleColor.Black;

        public static Color        GameAreaBackground            = Color.Purple;
        public static Color        GameAreaBorder                = Color.Goldenrod;
        public static ConsoleColor GameAreaTextColor             = ConsoleColor.Black;

        public static Color        InfoBackground                = Color.LightSlateGray;
        public static Color        InfoBorder                    = Color.Goldenrod;
        public static ConsoleColor InfoTextColor                 = ConsoleColor.Black;

        public static Color        InputBackground               = Color.Black;
        public static Color        InputBorder                   = Color.DarkSlateGray;
        public static ConsoleColor InputTextColor                = ConsoleColor.Black;

        public static Color        InventoryBackground           = Color.LightSlateGray;
        public static Color        InventoryBorder               = Color.DarkSlateGray;
        public static ConsoleColor InventoryTextColor            = ConsoleColor.Black;

        public static Color        CombatLogLeft                 = Color.LightSlateGray;
        public static Color        CombatLogLeftBorder           = Color.DarkSlateGray;
        public static ConsoleColor CombatLogLeftTextColor        = ConsoleColor.Black;

        public static Color        CombatLogRight                = Color.LightSlateGray;
        public static Color        CombatLogRightBorder          = Color.DarkSlateGray;
        public static ConsoleColor CombatLogRightTextColor       = ConsoleColor.Black;

        public static Color        HeaderBackground              = Color.Black;
        public static Color        HeaderBorder                  = Color.DarkSlateGray;
        public static ConsoleColor HeaderTextColor               = ConsoleColor.DarkYellow;
    }
}
