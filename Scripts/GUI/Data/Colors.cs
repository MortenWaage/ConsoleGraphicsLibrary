using System.Collections.Generic;
using System.Drawing;

namespace GUILib
{
    public static class Colors
    {
        public static int GetColor(Color col)
        {
            return ColorId.TryGetValue(col, out var value) ? value : ColorId[Color.Black];
            //var hasKey = ColorId.TryGetValue(col, out int value);
            //return hasKey ? value : ColorId[Color.Black];
        }

        //-- ALL COLOR VALUES
        //-- https://i.stack.imgur.com/q0rog.jpg

        public static readonly Dictionary<Color, int> ColorId = new Dictionary<Color, int>()
        {
            {Color.Black, 16},
            {Color.Red, 1},
            {Color.Green, 2},
            {Color.Yellow, 3},
            {Color.Blue, 4},
            {Color.DarkBlue, 21},
            {Color.Purple, 180},
            {Color.CornflowerBlue, 6},
            {Color.Wheat, 7},
            {Color.Gray, 8},
            {Color.IndianRed, 9},
            {Color.ForestGreen, 10},
            {Color.Peru, 130},
            {Color.Goldenrod, 142},
            {Color.HotPink, 206},
            {Color.DarkOrange, 202},
            {Color.DarkRed, 124},
            {Color.DarkSlateGray, 23},
            {Color.LightSlateGray, 251},
            {Color.DarkSlateBlue, 25},
            {Color.DarkGreen, 22},
            {Color.White, 255},
        };
    }
}
