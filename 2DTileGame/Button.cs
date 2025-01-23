using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HextoColor = HexToColor.HexConverter;

namespace Objects
{
    public class Button
    {
        private Vector2 position; //This stores the position the tile is in
        private SpriteFont font;
        private string text;
        private Color color;
        private int width;
        private int height;
        private Color backgroundColor;
        private Texture2D texture;
        private Rectangle border;
        private int textOfsetX;
        private int textOfsetY;

        public Button(Texture2D texture, SpriteFont font, String text, int x, int y, int width, int height, string color, string backgroundColor, int textOfsetX, int textOfsetY)
        {
            this.font = font;
            this.text = text;
            this.width = width;
            this.height = height;
            this.position = new Vector2(x, y);
            this.color = HextoColor.colorConverter(color); //Sets the tile colour using a hex code
            this.backgroundColor = HextoColor.colorConverter(backgroundColor);
            this.texture = texture;
            this.border = new Rectangle(new Point(x,y),new Point(width,height));
            this.textOfsetX = textOfsetX;
            this.textOfsetY = textOfsetY;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.border, this.backgroundColor);
            spriteBatch.DrawString(this.font, this.text, new Vector2(this.position.X + textOfsetX,this.position.Y+textOfsetY), this.color);
        }

        public bool MouseCollision(Point mouse)//A Collider used to check if the mouse is colliding with the tile
        {
            if ((mouse.X > this.position.X & mouse.X < this.position.X + this.width) & (mouse.Y > this.position.Y & mouse.Y < this.position.Y + this.height))
            {
                Debug.WriteLine("Collition");
                return true;
            }
            return false;
        }
    }

}