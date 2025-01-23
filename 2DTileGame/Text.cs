using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HextoColor = HexToColor.HexConverter;

namespace Objects
{
    public class Text : Object
    {
        private Vector2 position; //This stores the position the tile is in
        private SpriteFont font;
        private string text;
        private Color color;

        public Text(SpriteFont font, String text, int x, int y, String color)
        {
            this.font = font;
            this.text = text;
            this.position = new Vector2(x,y);
            this.color = HextoColor.colorConverter(color); //Sets the tile colour using a hex code
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.font, this.text, this.position, this.color);
        }
    }
}