using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LocalDB;

namespace ArcadeShmup.GameScreens
{
    class Menu : GameScreen
    {

        List<string> menuOptions = new List<string>(),
                    menuHelpers = new List<string>();
        Vector2 menuHelperPos = new Vector2(100,440);
        int selectedItem = 0;

        SpriteFont MediumFont, SmallFont, LargeFont;

        Rectangle logoPos = new Rectangle(400, 40, 400, 400);
        Texture2D texLogo;

        Texture2D smallStar, mediumStar, largeStar;
        List<StarColor> smallStars = new List<StarColor>(),
                        mediumStars = new List<StarColor>(),
                        largeStars = new List<StarColor>();

        public Menu()
        {
            if (GameState.HasLoadState())
            {
                menuOptions.Add("Continue");
                menuHelpers.Add("Continue the game from where you were. [Double tap to select]");
            }
            menuOptions.Add("New Game");
            menuHelpers.Add("Start a new game. [Double tap to select]");

            menuOptions.Add("Local Scores");
            menuHelpers.Add("");

            menuOptions.Add("Quit");
            menuHelpers.Add("Exit the game. [Double tap to select]");


        }


        Random random = new Random();
        IList<Score> myScores;
        public override void Load()
        {
            SmallFont = Game.Content.Load<SpriteFont>("Fonts/SegeoSmall");
            MediumFont = Game.Content.Load<SpriteFont>("Fonts/SegeoMedium");
            LargeFont = Game.Content.Load<SpriteFont>("Fonts/SegeoLarge");

            texLogo = Game.Content.Load<Texture2D>("Sprites/MenuArt");

            smallStar = Game.Content.Load<Texture2D>("Sprites/Stars/Star1");
            mediumStar = Game.Content.Load<Texture2D>("Sprites/Stars/Star2");
            largeStar = Game.Content.Load<Texture2D>("Sprites/Stars/Star3");

            for (int i = 0; i < 40; i++)
                smallStars.Add(new StarColor(random.Next(800), random.Next(480), new Color(random.Next(0, 255), 0, random.Next(0, 255))));
            for (int i = 0; i < 20; i++)
                mediumStars.Add(new StarColor(random.Next(800), random.Next(480), new Color(random.Next(180, 220), random.Next(140, 180), random.Next(0, 40))));
            for (int i = 0; i < 10; i++)
                largeStars.Add(new StarColor(random.Next(800), random.Next(480), new Color(random.Next(175, 255), random.Next(175, 255), 0)));
        }

        public override void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Game.Exit();

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

            if (OffSet == 0)
            {
                foreach (var gs in Game.gestureSamples)
                {
                    if (gs.GestureType == Microsoft.Xna.Framework.Input.Touch.GestureType.Flick)
                    {
                        if (gs.Delta2.Y < 0) //downwards
                        {
                            selectedItem = (selectedItem + 1) % menuOptions.Count;
                            if (menuOptions[selectedItem] == "Local Scores")
                            {
                                myScores = ScoresData.GetScores();
                            }
                            OffSet = 8;
                        }
                        else //upwards
                        {
                            selectedItem = selectedItem - 1;
                            if (selectedItem < 0)
                                selectedItem += menuOptions.Count;
                            if (menuOptions[selectedItem] == "Local Scores")
                            {
                                myScores = ScoresData.GetScores();
                            }
                            OffSet = -8;
                        }
                    }
                    else if (gs.GestureType == Microsoft.Xna.Framework.Input.Touch.GestureType.DoubleTap)
                    {
                        if (menuOptions[selectedItem] == "Quit")
                        {
                            Game.Exit();
                        }
                        else if (menuOptions[selectedItem] == "New Game")
                        {
                            GameState.Clear();
                            Game.PushGameScreen(new InGame());
                        }
                        else if (menuOptions[selectedItem] == "Continue")
                        {

                            Game.PushGameScreen(new InGame());
                        }
                    }
                }
            }
            else if (OffSet < 0)
            {
                OffSet += 1;
            }
            else
            {
                OffSet -= 1;
            }

        }


        Vector2 pos = new Vector2(), pos2 = new Vector2(100,0), v2_helper = new Vector2();
        Color smallColor = new Color(0, 75, 0),
              mediumColor = new Color(0, 150, 0),
              largeColor = new Color(0, 255, 0);
        int OffSet = 0;
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

            pos.Y = 170;
            for (int i = selectedItem - 2; i < selectedItem + 3; i++)
            {
                pos.Y += 14;
                pos2.Y = pos.Y + OffSet;
                if (Math.Abs(selectedItem - i) > 1)
                {
                    spriteBatch.DrawString(SmallFont, menuOptions[(i + menuOptions.Count) % menuOptions.Count], pos2, smallColor);
                }
                else if (Math.Abs(selectedItem - i) == 1)
                {
                    spriteBatch.DrawString(MediumFont, menuOptions[(i + menuOptions.Count) % menuOptions.Count], pos2, mediumColor);
                    pos.Y += 4;
                }
                else
                {
                    spriteBatch.DrawString(LargeFont, menuOptions[(i + menuOptions.Count) % menuOptions.Count], pos2, largeColor);
                    pos.Y += 8;
                }
            }

            spriteBatch.DrawString(MediumFont, menuHelpers[selectedItem], menuHelperPos, mediumColor);

            if (menuOptions[selectedItem] != "Local Scores")
            {
                spriteBatch.Draw(texLogo, logoPos, Color.White);
            }
            else
            {
                v2_helper.X = 400;
                v2_helper.Y = 40;
                if (myScores != null)
                {
                    for (int i = 0; (i < myScores.Count) && (i < 5); i++)
                    {
                        v2_helper.Y += 20;
                        spriteBatch.DrawString(MediumFont, myScores[i].When.ToShortDateString() + " - " + myScores[i].Score1.ToString(), v2_helper, Color.White);
                    }
                }
            }
        }

    }
}
