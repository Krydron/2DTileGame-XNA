using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HextoColor = HexToColor.HexConverter;

namespace Objects
{
    public class Tile : Object
    {
        private string name; //This will be the identifier for the tile
        private Vector2 position; //This stores the position the tile is in
        private int width;
        private int height;
        private Color color;
        private Texture2D texture;
        private Rectangle border; //Used to determine how much space the tile takes up in the screen
        private int tileBuffer; //Creates a space between tiles
        private int tileSpaceIncrement; //Used to position the tiles during instantiation
        private int gridWidth; //Used to create the max border for the grid
        private int gridHeight;
        private int gridXOffset;//Moves the entire grid along the x axis
        private int gridYOffset;//Moves the entire grid along the y axis
        private int gridPosition;

        private int animationFrames = 24;
        GraphicsDeviceManager graphicsDeviceManager;
        SpriteBatch spriteBatch;

        public Tile(string color, Texture2D texture, int gridWidth, int gridHeight, int gridValue)
        {
            if (gridValue != 15) { this.name = gridValue.ToString(); }//If the tile is not the last tile then set its name as the position
            else { this.name = "empty"; }//Set the last tile to empty
            
            this.width = 125;
            this.height = 125;
            this.tileBuffer = 15;

            this.gridWidth = gridWidth; //Default grid layout is 4x4 but can be expanded or reduced
            this.gridHeight = gridHeight;
            this.gridXOffset = 80;
            this.gridYOffset = 100;
            this.gridPosition = gridValue;

            this.tileSpaceIncrement = this.width+tileBuffer; //The ammount you move the tile forward and down is equal to the tile width plus the needed buffer
            this.position = new Vector2((gridValue * this.tileSpaceIncrement % (this.gridWidth * this.tileSpaceIncrement))+gridXOffset, (((gridValue / this.gridHeight) * this.tileSpaceIncrement) % (this.gridHeight * this.tileSpaceIncrement)) + gridYOffset);
            this.color = HextoColor.colorConverter(color); //Sets the tile colour using a hex code
            this.texture = texture; //blank texture
            this.border = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(width,height)); //Uses position and width to work out the border of the tile
        }

        public void Draw(SpriteBatch _spriteBatch)//Drawss tile
        {
            _spriteBatch.Draw(this.texture, this.border,this.color); 
        }

        public bool MouseCollision(Point mouse)//A Collider used to check if the mouse is colliding with the tile
        {
            if ((mouse.X > this.position.X & mouse.X < this.position.X+this.width) & (mouse.Y > this.position.Y & mouse.Y < this.position.Y + this.height))
            {
                return true;
            }
            return false;
        }

        public void MoveTile(int newPos, int seconds)//Code that moves the tile to the new position using linear interpolation
        {
            Debug.WriteLine("Moving " + this.name + " to position " + newPos);
            this.gridPosition = newPos;
            //this.position = new Vector2((newPos * this.tileSpaceIncrement % (this.gridWidth * this.tileSpaceIncrement)) + gridXOffset, (((newPos / this.gridHeight) * this.tileSpaceIncrement) % (this.gridHeight * this.tileSpaceIncrement)) + gridYOffset);
            //this.border = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(width, height));

            Vector2 startPosition = this.position;
            Vector2 endPosition = new Vector2((newPos * this.tileSpaceIncrement % (this.gridWidth * this.tileSpaceIncrement)) + gridXOffset, (((newPos / this.gridHeight) * this.tileSpaceIncrement) % (this.gridHeight * this.tileSpaceIncrement)) + gridYOffset);
            Debug.WriteLine("This position: " + this.position + "Target: " + endPosition);
            double lerpIncrement = 1.0  /(seconds*animationFrames);
            for (int i = 0; i != (seconds*animationFrames)+1 ; i++)
            {
                this.position = Vector2.Lerp(startPosition, endPosition, (float)(lerpIncrement*i));
                Debug.WriteLine("Start: " + startPosition + "End: " + endPosition + "Current Lerp Position: " + this.position + "Lerp Increment" + (float)0.02 + " " + (float)lerpIncrement);
                this.border = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(width, height));
                
                //Task.Delay(TimeSpan.FromSeconds(10000));
            }
            
        }

        public string GetName()
        {
            return this.name;
        }

        public Vector2 GetPosition()
        {
            return this.position;
        }

        public int GetGridPos()
        {
            return this.gridPosition;
        }

        public int GetTileSpaceIncrement()
        {
            return this.tileSpaceIncrement;
        }
    }
}
