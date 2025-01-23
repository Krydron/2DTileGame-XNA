using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HextoColor = HexToColor.HexConverter;

namespace Objects
{
    public class Timer
    {
        private Vector2 position; //This stores the position the tile is in
        private SpriteFont font;
        private string text;
        private int minutes;
        private int seconds;
        private Color color;

        public Timer(SpriteFont font, int minutes, int seconds, int x, int y, String color)
        {
            this.seconds = seconds;
            this.minutes = minutes;
            this.font = font;
            this.text = (minutes+":"+seconds);
            this.position = new Vector2(x, y);
            this.color = HextoColor.colorConverter(color); //Sets the tile colour using a hex code
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.font, this.text, this.position, this.color);
        }

        public void DecrementTimer(int seconds)
        {
            //Counts the timer down
            string secondText;
            string minuteText;
            if (seconds > this.seconds)//When seconds goes below zero then decrement the minute value
            {
                this.minutes-=1;
                this.seconds += 60;//Resets seconds
                this.seconds -= seconds;//Decrements seconds by seconds passed
            }
            else { this.seconds -= seconds; }
            if (this.seconds < 10) { secondText = "0" + this.seconds; }
            else { secondText = this.seconds.ToString(); };
            if (this.minutes < 10) { minuteText = "0" + this.minutes; }
            else { minuteText = this.minutes.ToString(); };
            this.text = (minuteText + ":" + secondText);
        }

        public bool TimerOver()
        {
            if (this.seconds == 0 & this.minutes == 0) { return true; }
            return false;
        }

        public void SetTimer(int minutes , int seconds)
        {
            this.minutes = minutes;
            this.seconds = seconds;
        }
    }
}
