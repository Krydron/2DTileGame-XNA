using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    public class Image
    {
        private Vector2 position; //This stores the position the tile is in
        private Color color = Color.White;
        private Texture2D image; //Stores the image to be rendered
        private Rectangle border; //Used to determine how much space the tile takes up in the screen

        public Image(Texture2D image, int x, int y, int height, int width)
        {
            this.image = image;
            position = new Vector2(x, y);
            this.border = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(width, height));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.image, this.border, this.color);
        }
    }
}
