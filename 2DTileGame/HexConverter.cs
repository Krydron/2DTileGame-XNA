using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexToColor
{
    public class HexConverter
    {
        public HexConverter()
        {

        }

        
        public static Color colorConverter(string hex)
        {
            int hexValue = Convert.ToInt32(hex, 16);
            int r = (hexValue >> 16) & 0xFF;  // Extract the RR byte
            int g = (hexValue >> 8) & 0xFF;   // Extract the GG byte
            int b = (hexValue) & 0xFF;        // Extract the BB byte
            return new Color(r, g, b);
        }
    }
}

