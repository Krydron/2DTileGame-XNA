using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Objects;


namespace _2DTileGame
{
    enum GameState
    {
        MainMenu,
        Running,
        WinState,
        LoseState,
        Pause,
    }


    public class Game1 : Game
    {
        #region Variables
        private GameState gameState;
        private GraphicsDeviceManager _graphics; //Stores the Graphics Device Manager
        private SpriteBatch _spriteBatch; //Stores the sprite batch
        private int gridWidth;
        private int gridHeight;
        private List<Tile> tiles = new List<Tile>(); //Stores the game tiles
        private Random rnd = new Random();
        private string[] tileColour = { "FF0000", "ff3200", "FF8700", "D3B315", "FFFF00", "90d315", "00FF00", "00d084", "0000FF", "5100d4", "9B00FF", "c2185b", "FF00FF", "d40044", "ff0064" }; //Stores the colours for tiles
        private MouseState mouse;
        private int TILESCOUNT;
        private SpriteFont defaultFont;
        private SpriteFont buttonFont;
        private SpriteFont mainMenuFont;
        private Text title;
        private Button play;
        private Button quit;
        private Text win;
        private Text lose;
        private Button mainMenu;
        private Timer timer;
        private SpriteFont timerFont;
        private float elapsedGameTime = 0;
        private Image goal;
        private Texture2D goalImage;
        #endregion;

        //Constructor
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this); 
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        //Starting code
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            gridWidth = 4;
            gridHeight = 4;
            Texture2D empty = new(GraphicsDevice, 1, 1); //A blank texture for the tiles
            empty.SetData(new Color[] { Color.White });
            for (int x = 0; x <= 15; x++)//Creates the tiles
            {
                if (x != (gridWidth*gridHeight)-1)//If not the last tile
                {
                    tiles.Add(new Tile(tileColour[x % tileColour.Length], empty, gridWidth, gridHeight, x));//Create a tile and set the colour using the tileColoura# array
                }
                else //If the last tile
                {
                    tiles.Add(new Tile("000000", empty, gridWidth, gridHeight, x)); //Create an invisible tile to denote an empty space
                }
            }
            TILESCOUNT = tiles.Count;

            gameState = GameState.MainMenu;
            defaultFont = Content.Load<SpriteFont>("Fonts/DefaultFont");
            buttonFont = Content.Load<SpriteFont>("Fonts/ButtonFont");
            mainMenuFont = Content.Load<SpriteFont>("Fonts/ButtonFont2");
            title = new Text(defaultFont, "15 Tiles Game", 225, 100, "FFFFFF");
            play = new Button(empty, buttonFont, "Play", 450, 300, 300, 100, "000000", "AFAFAF", 50, 0);
            quit = new Button(empty, buttonFont, "Quit", 450, 450, 300, 100, "000000", "AFAFAF", 50, 0);
            win = new Text(defaultFont, "You Win!", 350, 100, "FFFFFF");
            lose = new Text(defaultFont, "You Lose", 350, 100, "FFFFFF");
            mainMenu = new Button(empty, mainMenuFont, "Main Menu", 450, 300, 300, 100, "000000", "AFAFAF", 10, 20);
            timerFont = Content.Load<SpriteFont>("Fonts/TimerFont");
            timer = new Timer(timerFont, 10, 0, 80, 50, "FAFAFA");
            goalImage = Content.Load<Texture2D>("Images/TileGoal");
            goal = new Image(goalImage, 700, 100, 200, 200);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit(); }

            // TODO: Add your update logic here 
            mouse = Mouse.GetState();

            switch (gameState)
            {
                case GameState.MainMenu://Main Menu logic
                    if (mouse.LeftButton != ButtonState.Pressed) { break; }
                    if (play.MouseCollision(mouse.Position))
                    {
                        ShuffleTiles();
                        gameState++;
                    }
                    else if (quit.MouseCollision(mouse.Position)) { Exit(); }
                    break;
                case GameState.Running://Game running logic
                    if (mouse.LeftButton == ButtonState.Pressed)//When the left mouse button is clicked
                    {
                        for (int x = 0; x < TILESCOUNT; x++)//Check all the tiles in the list 
                        {
                            if (tiles[x].GetName() == "empty") { continue; } //(Except for empty)
                            else if (tiles[x].MouseCollision(mouse.Position) != true) { continue; }//For a collition if a colition is detected then
                                                                                                   //The Clicked tile has been identified and will now be swapped with the empty tile
                            int tileDifference = Convert.ToInt32((tiles[TILESCOUNT - 1].GetPosition().X - tiles[x].GetPosition().X) + (tiles[TILESCOUNT - 1].GetPosition().Y - tiles[x].GetPosition().Y));
                            int tileSpaceIncrement = tiles[x].GetTileSpaceIncrement();
                            int tileSteps = (tileDifference / tileSpaceIncrement);
                            if (Math.Abs(tileSteps) > 1) { break; }
                            Debug.WriteLine("Moving Tiles " + tileSteps + " " + tileDifference + " " + tileSpaceIncrement);
                            SwapTiles(tiles[x], tiles[TILESCOUNT - 1]);
                            if (PuzzleComplete() == true) { gameState = GameState.WinState; }
                            break;

                            /* Failed attempt to move the tiles by row and by column
                             * 
                            //Filter the possibilities of which case has been met and which movements corrospond to that case
                            int tileSpaceIncrement = tiles[x].GetTileSpaceIncrement();//140*Tile Steps Away

                            if (tiles[x].GetPosition().Y == tiles[TILESCOUNT - 1].GetPosition().Y)//If the tiles are on the same column
                            {
                                int tileDifference = Convert.ToInt32(tiles[TILESCOUNT - 1].GetPosition().X - tiles[x].GetPosition().X);
                                int tileSteps = (tileDifference / tileSpaceIncrement);
                                Debug.WriteLine("Tile Difference: " + tileDifference + " Tile Steps: " + tileSteps);

                                //Swaps the clicked tile with the empty tile
                                SwapTiles(tiles[x], tiles[TILESCOUNT - 1]);
                                break;
                            }
                            else if (tiles[x].GetPosition().X == tiles[TILESCOUNT - 1].GetPosition().X)
                            {
                                int tileDifference = Convert.ToInt32(tiles[TILESCOUNT - 1].GetPosition().Y - tiles[x].GetPosition().Y);
                                int tileSteps = (tileDifference / tileSpaceIncrement);
                                for (int y = 0; y < Math.Abs(tileSteps); y++)
                                {
                                    if (tileSteps > 0)
                                    {
                                        Debug.WriteLine("TileStep Pattern " + (Convert.ToInt32(tiles[TILESCOUNT-1].GetGridPos())-4));
                                        //SwapTiles(tiles[Convert.ToInt32(tiles[Convert.ToInt32(tiles[TILESCOUNT - 1].GetGridPos()) - 4].GetName())], tiles[TILESCOUNT - 1]);
                                    }
                                    else if (tileSteps < 0)
                                    {
                                        Debug.WriteLine("TileStep Pattern " + ((Convert.ToInt32(tiles[TILESCOUNT - 1].GetGridPos()) + 4)));
                                        //SwapTiles(tiles[(Convert.ToInt32(tiles[TILESCOUNT - 1].GetGridPos()) + 4)], tiles[TILESCOUNT - 1]);
                                        SwapTiles(tiles[x], tiles[TILESCOUNT - 1]);
                                    }
                                }
                            }*/
                        }
                    }
                    //timer.DecrementTimer(((int)(gameTime.TotalGameTime.TotalSeconds % 1)));
                    elapsedGameTime += ((float)gameTime.ElapsedGameTime.TotalSeconds);
                    Debug.WriteLine("Time " + elapsedGameTime);
                    if (elapsedGameTime > 1)
                    {
                        timer.DecrementTimer(1);
                        elapsedGameTime = 0;
                        if (timer.TimerOver()) { gameState = GameState.LoseState; }
                    }
                    break;
                case GameState.WinState: //Win game state
                case GameState.LoseState://Lose game state
                    if (mouse.LeftButton != ButtonState.Pressed) { break; }
                    if (mainMenu.MouseCollision(mouse.Position)) { gameState = GameState.MainMenu; }
                    else { Exit(); }
                    break;
                default:
                    break;
            }

            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Set background to black
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            switch (gameState)
            {
                case GameState.MainMenu://Main Menu logic
                    _spriteBatch.Begin();
                    title.Draw(_spriteBatch);
                    play.Draw(_spriteBatch);
                    quit.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;
                case GameState.Running://Game running logic
                    _spriteBatch.Begin();// Begin layer
                    tiles[TILESCOUNT - 1].Draw(_spriteBatch);//Draw empty first so other tiles aren't effected by it
                    for (int x = 0; x < TILESCOUNT; x++) //Draw tiles
                    {
                        if (tiles[x].GetName() != "empty") { tiles[x].Draw(_spriteBatch); }

                    }
                    timer.Draw(_spriteBatch); //Draws timer
                    goal.Draw(_spriteBatch);
                    _spriteBatch.End(); //End layer
                    break;
                case GameState.WinState: //Win game state
                    _spriteBatch.Begin();
                    win.Draw(_spriteBatch);
                    mainMenu.Draw(_spriteBatch);
                    quit.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;
                case GameState.LoseState://Lose game state
                    _spriteBatch.Begin();
                    lose.Draw(_spriteBatch);
                    mainMenu.Draw(_spriteBatch);
                    quit.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;
                default:
                    break;
            }


            
            base.Draw(gameTime);
        }

        public int FindTile(string name)//Looks for the tile in the tile list
        {
            for (int x = 0;x < TILESCOUNT;x++)
            {
                if (tiles[x].GetName() == name) { return tiles[x].GetGridPos(); }//If a tile is found with the same name as the tile that is being looked for its grid position is returned
            }
            return -1;
        }

        public string FindTileAtPos(int gridPos)//Looks for the tile in the tile list
        {
            for (int x = 0; x < TILESCOUNT; x++)
            {
                if (tiles[x].GetGridPos() == gridPos) { return tiles[x].GetName(); }//If a tile is found with the same name as the tile that is being looked for its grid position is returned
            }
            return "null";
        }

        public void SwapTiles(Tile tile1, Tile tile2)//Swapping the grid position of the tiles
        {
            int tile1Pos = FindTile(tile1.GetName());
            tile1.MoveTile(FindTile(tile2.GetName()), 10);
            tile2.MoveTile(tile1Pos, 2);
        }

        public void ShuffleTiles()
        {
            for (int x = 0; x < TILESCOUNT ; x++)
            {
                SwapTiles(tiles[x], tiles[rnd.Next(TILESCOUNT)]);
            }
        }

        public bool PuzzleComplete()
        {
            for (int x = 0; x < TILESCOUNT ; x++)
            {
                if (tiles[x].GetGridPos() != x) { return false; }
            }
            return true;
        }
    }
}