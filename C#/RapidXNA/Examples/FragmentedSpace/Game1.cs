using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using ArcadeShmup.GameScreens;
using LocalDB;

namespace ArcadeShmup
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Input
        public List<GestureSample> gestureSamples = new List<GestureSample>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            TargetElapsedTime = TimeSpan.FromTicks(333333);

            InactiveSleepTime = TimeSpan.FromSeconds(1);

            ScoresData.CreateDatabase();
        }

 
        protected override void Initialize()
        {
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.DragComplete | GestureType.Flick | GestureType.DoubleTap;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GameState.SetGame(this);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {

            gestureSamples.Clear();

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                gestureSamples.Add(gs);
            }

            if (gameScreens.Count == 0)
                PushGameScreen(new Menu());

            if (popUpScreens.Count > 0)
            {
                popUpScreens[popUpScreens.Count - 1].gameTime = gameTime;
                popUpScreens[popUpScreens.Count - 1].Update();
            }
            else
            {
                if (gameScreens.Count > 0)
                {
                    gameScreens[gameScreens.Count - 1].gameTime = gameTime;
                    gameScreens[gameScreens.Count - 1].Update();
                }
            }


            base.Update(gameTime);
        }

        private List<GameScreen>  gameScreens = new List<GameScreen>()
                                , popUpScreens = new List<GameScreen>();
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (gameScreens.Count > 0)
            {
                gameScreens[gameScreens.Count - 1].gameTime = gameTime;
                gameScreens[gameScreens.Count - 1].Draw();
            }
            foreach (GameScreen popUp in popUpScreens)
            {
                popUp.gameTime = gameTime;
                popUp.Draw();
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void PushGameScreen(GameScreen gs)
        {
            gs.Game = this;
            gs.spriteBatch = spriteBatch;
            gs.Load();
            gameScreens.Add(gs);
        }

        public void PopGameScreen()
        {
            if (gameScreens.Count > 0)
            {
                gameScreens[gameScreens.Count - 1].gameTime = null;
                gameScreens[gameScreens.Count - 1].Game = null;
                gameScreens[gameScreens.Count - 1].spriteBatch = null;
                gameScreens.RemoveAt(gameScreens.Count - 1);
            }
        }

        public void PushPopUpScreen(GameScreen gs)
        {
            gs.Game = this;
            gs.spriteBatch = spriteBatch;
            gs.Load();
            popUpScreens.Add(gs);
        }

        public void PopPopUpScreen()
        {
            if (popUpScreens.Count > 0)
            {
                popUpScreens[popUpScreens.Count - 1].gameTime = null;
                popUpScreens[popUpScreens.Count - 1].Game = null;
                popUpScreens[popUpScreens.Count - 1].spriteBatch = null;
                popUpScreens.RemoveAt(popUpScreens.Count - 1);
            }
        }
    }
}
