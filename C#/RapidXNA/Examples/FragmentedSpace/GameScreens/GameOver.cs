using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace ArcadeShmup.GameScreens
{
    class GameOver : GameScreen
    {
        Texture2D smallStar, mediumStar, largeStar;
        List<StarColor> smallStars = new List<StarColor>(),
                        mediumStars = new List<StarColor>(),
                        largeStars = new List<StarColor>();
        SpriteFont MediumFont;

        Rectangle logoPos = new Rectangle(400, 40, 400, 400);
        Texture2D texLogo;

        Vector2 menuHelperPos = new Vector2(100, 190);

        Random random = new Random();

        public int ScoreGained = 0;

        public override void Load()
        {
            MediumFont = Game.Content.Load<SpriteFont>("Fonts/SegeoMedium");

            smallStar = Game.Content.Load<Texture2D>("Sprites/Stars/Star1");
            mediumStar = Game.Content.Load<Texture2D>("Sprites/Stars/Star2");
            largeStar = Game.Content.Load<Texture2D>("Sprites/Stars/Star3");

            texLogo = Game.Content.Load<Texture2D>("Sprites/MenuArt");

            for (int i = 0; i < 40; i++)
                smallStars.Add(new StarColor(random.Next(800), random.Next(480), new Color(random.Next(0, 255), 0, random.Next(0, 255))));
            for (int i = 0; i < 20; i++)
                mediumStars.Add(new StarColor(random.Next(800), random.Next(480), new Color(random.Next(180, 220), random.Next(140, 180), random.Next(0, 40))));
            for (int i = 0; i < 10; i++)
                largeStars.Add(new StarColor(random.Next(800), random.Next(480), new Color(random.Next(175, 255), random.Next(175, 255), 0)));

            LocalDB.Score s = new LocalDB.Score();
            s.When = DateTime.Now;
            s.Score1 = ScoreGained;
            ScoresData.AddScore(s);
        }

        public override void Update()
        {
            foreach (StarColor sc in smallStars)
            {
                sc.Pos.X -= 10;
                if (sc.Pos.X < 0)
                {
                    sc.Pos.X += 800;
                    sc.Pos.Y = random.Next(480);
                }
            }
            foreach (StarColor sc in mediumStars)
            {
                sc.Pos.X -= 10;
                if (sc.Pos.X < 0)
                {
                    sc.Pos.X += 800;
                    sc.Pos.Y = random.Next(480);
                }
            }
            foreach (StarColor sc in largeStars)
            {
                sc.Pos.X -= 10;
                if (sc.Pos.X < 0)
                {
                    sc.Pos.X += 800;
                    sc.Pos.Y = random.Next(480);
                }
            }

            bool closing = false;
            foreach (GestureSample gs in Game.gestureSamples)
            {
                if (gs.GestureType == GestureType.DoubleTap)
                {
                    closing = true;
                    Game.PopGameScreen();
                }
            }

            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) && (!closing))
                Game.PopGameScreen();
        }

        public override void Draw()
        {
            foreach (StarColor sc in smallStars)
            {
                spriteBatch.Draw(smallStar, sc.Pos, sc.Colour);
            }
            foreach (StarColor sc in mediumStars)
            {
                spriteBatch.Draw(mediumStar, sc.Pos, sc.Colour);
            }
            foreach (StarColor sc in largeStars)
            {
                spriteBatch.Draw(largeStar, sc.Pos, sc.Colour);
            }

            spriteBatch.DrawString(MediumFont, "Game Over!\nScore: " + ScoreGained.ToString(), menuHelperPos, Color.White);

            spriteBatch.Draw(texLogo, logoPos, Color.White);

        }
    }
}
