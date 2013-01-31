using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ArcadeShmup.Helpers;
using Microsoft.Xna.Framework.Input;

namespace ArcadeShmup.GameScreens
{
    class StarColor
    {
        public Vector2 Pos;
        public Color Colour;
        public StarColor(int x, int y, Color c)
        {
            Pos = new Vector2(x, y);
            Colour = c;
        }
    }

    class InGame : GameScreen
    {

        Texture2D texShip, texIndicator, texBullet, texLife, texPickup;
        SpriteFont MediumFont;

        //Stars
        Texture2D smallStar, mediumStar, largeStar;
        List<StarColor> smallStars = new List<StarColor>(),
                        mediumStars = new List<StarColor>(),
                        largeStars = new List<StarColor>();
        

        //RandomGen
        Random random = new Random();

        public InGame()
        {

        }

        public override void Load()
        {
            texShip = Game.Content.Load<Texture2D>("Sprites/Ship");
            texIndicator = Game.Content.Load<Texture2D>("Sprites/Indicator");
            texBullet = Game.Content.Load<Texture2D>("Sprites/Bullet");
            texLife = Game.Content.Load<Texture2D>("Sprites/Life");
            texPickup = Game.Content.Load<Texture2D>("Sprites/Pickup");

            MediumFont = Game.Content.Load<SpriteFont>("Fonts/SegeoMedium");

            smallStar = Game.Content.Load<Texture2D>("Sprites/Stars/Star1");
            mediumStar = Game.Content.Load<Texture2D>("Sprites/Stars/Star2");
            largeStar = Game.Content.Load<Texture2D>("Sprites/Stars/Star3");

            for (int i = 0; i < 200; i++)
                smallStars.Add(new StarColor(random.Next(-600, 600), random.Next(-600, 600), new Color(random.Next(0, 255), 0, random.Next(0, 255))));
            for (int i = 0; i < 100; i++)
                mediumStars.Add(new StarColor(random.Next(-600, 600), random.Next(-600, 600), new Color(random.Next(180, 220), random.Next(140, 180), random.Next(0, 40))));
            for (int i = 0; i < 50; i++)
                largeStars.Add(new StarColor(random.Next(-600, 600), random.Next(-600, 600), new Color(random.Next(175, 255), random.Next(175, 255), 0)));

            //Game.PushPopUpScreen(new PickupScreen()); //test
        }

        public override void Update()
        {
            UpdateStars();
            GameState.gameTime = gameTime;
            GameState.Update();

            if (GameState.PlayerHealth <= 0)
            { 
                //Game over
                Game1 g = Game;
                Game.PopGameScreen();
                GameOver go = new GameOver();
                go.ScoreGained = GameState.PlayerScore;
                g.PushGameScreen(go);
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Game.PopGameScreen(); //Note: do something fancy to preserve game state
        }

        Rectangle PlayerAreaRect = new Rectangle(-600, -600, 1200, 1200);
        private void UpdateStars()
        {
            PlayerAreaRect.X = (int)(GameState.PlayerPosition.X - 600);
            PlayerAreaRect.Y = (int)(GameState.PlayerPosition.Y - 600);
            foreach (StarColor sc in smallStars)
            {
                if (!PlayerAreaRect.Contains((int)sc.Pos.X, (int)sc.Pos.Y))
                {
                    double angle = Math.Atan2(GameState.PlayerPosition.Y - sc.Pos.Y, GameState.PlayerPosition.X - sc.Pos.X);
                    double length = MHelper.Vector2Distance(GameState.PlayerPosition, sc.Pos) - 100;
                    sc.Pos.X = GameState.PlayerPosition.X + (float)(length * Math.Cos(angle));
                    sc.Pos.Y = GameState.PlayerPosition.Y + (float)(length * Math.Sin(angle));
                }
            }
            foreach (StarColor sc in mediumStars)
            {
                if (!PlayerAreaRect.Contains((int)sc.Pos.X, (int)sc.Pos.Y))
                {
                    double angle = Math.Atan2(GameState.PlayerPosition.Y - sc.Pos.Y, GameState.PlayerPosition.X - sc.Pos.X);
                    double length = MHelper.Vector2Distance(GameState.PlayerPosition, sc.Pos) - 100;
                    sc.Pos.X = GameState.PlayerPosition.X + (float)(length * Math.Cos(angle));
                    sc.Pos.Y = GameState.PlayerPosition.Y + (float)(length * Math.Sin(angle));
                }
            }
            foreach (StarColor sc in largeStars)
            {
                if (!PlayerAreaRect.Contains((int)sc.Pos.X, (int)sc.Pos.Y))
                {
                    double angle = Math.Atan2(GameState.PlayerPosition.Y - sc.Pos.Y, GameState.PlayerPosition.X - sc.Pos.X);
                    double length = MHelper.Vector2Distance(GameState.PlayerPosition, sc.Pos) - 100;
                    sc.Pos.X = GameState.PlayerPosition.X + (float)(length * Math.Cos(angle));
                    sc.Pos.Y = GameState.PlayerPosition.Y + (float)(length * Math.Sin(angle));
                }
            }
        }

        Vector2 v2_helper = new Vector2();
        Vector2 v2_bulletOrigin = new Vector2(10, 10),
                v2_pickupOrigin = new Vector2(25, 25);
        public override void Draw()
        {
            //Draw all stars
            foreach (StarColor sc in smallStars)
            {
                v2_helper.X = sc.Pos.X - GameState.PlayerPosition.X + 400;
                v2_helper.Y = sc.Pos.Y - GameState.PlayerPosition.Y + 240;
                spriteBatch.Draw(smallStar, v2_helper, sc.Colour);
            }
            foreach (StarColor sc in mediumStars)
            {
                v2_helper.X = sc.Pos.X - GameState.PlayerPosition.X + 400;
                v2_helper.Y = sc.Pos.Y - GameState.PlayerPosition.Y + 240;
                spriteBatch.Draw(mediumStar, v2_helper, sc.Colour);
            }
            foreach (StarColor sc in largeStars)
            {
                v2_helper.X = sc.Pos.X - GameState.PlayerPosition.X + 400;
                v2_helper.Y = sc.Pos.Y - GameState.PlayerPosition.Y + 240;
                spriteBatch.Draw(largeStar, v2_helper, sc.Colour);
            }
            
            //draw all enemies
            foreach (Enemy e in GameState.Enemies)
            {
                v2_helper.X = e.Position.X - GameState.PlayerPosition.X + 400;
                v2_helper.Y = e.Position.Y - GameState.PlayerPosition.Y + 240;
                spriteBatch.Draw(texShip, v2_helper, null, e.Colour, e.Direction, v2_bulletOrigin, 1.0f, SpriteEffects.None, 0.1f);
            }

            //Draw all bullets
            foreach (Bullet b in GameState.Bullets)
            {
                v2_helper.X = b.Position.X - GameState.PlayerPosition.X + 400;
                v2_helper.Y = b.Position.Y - GameState.PlayerPosition.Y + 240;
                spriteBatch.Draw(texBullet, v2_helper, null, b.Colour, 0.0f, v2_bulletOrigin, 1.0f, SpriteEffects.None, 0.1f);
            }

            //Draw Pickup markers
            foreach (Pickup p in GameState.Pickups)
            {
                double direction = Math.Atan2(p.Position.Y - GameState.PlayerPosition.Y, p.Position.X - GameState.PlayerPosition.X);
                v2_helper.X = 400 + (float)(200 * Math.Cos(direction));
                v2_helper.Y = 240 + (float)(200 * Math.Sin(direction));
                spriteBatch.Draw(texIndicator, v2_helper, null, Color.White, (float)direction, v2_bulletOrigin, 1.0f, SpriteEffects.None, 0.05f);

                v2_helper.X = -GameState.PlayerPosition.X + p.Position.X + 400;
                v2_helper.Y = -GameState.PlayerPosition.Y + p.Position.Y + 240;
                spriteBatch.Draw(texPickup, v2_helper, null, Color.White, 0.0f, v2_pickupOrigin, 1.0f, SpriteEffects.None, 0.05f);
            }

            //Draw player
            spriteBatch.Draw(texShip, new Vector2(400, 240), null, Color.LimeGreen, GameState.PlayerDirection, new Vector2(20, 20), 1.0f, SpriteEffects.None, 0);

            //draw health
            v2_helper.Y = 12;
            for (int i = 0; i < GameState.PlayerHealth; i++)
            {
                v2_helper.X = 400 - GameState.PlayerHealth * 10 + i * 20;
                spriteBatch.Draw(texLife, v2_helper, Color.White);
            }

            //draw score
            spriteBatch.DrawString(MediumFont, "Score: " + GameState.PlayerScore.ToString(), Vector2.Zero, Color.White);
        }

    }
}