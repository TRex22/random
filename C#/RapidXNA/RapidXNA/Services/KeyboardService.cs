using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace RapidXNA.Services
{
    public class KeyboardService : IRapidService
    {
#if WINDOWS
        private KeyboardState CurrentState, PreviousState;
        private Dictionary<Keys, float>
            PressLengths = new Dictionary<Keys, float>(),
            ReleasedLength = new Dictionary<Keys, float>();
        //keyboardHandler.add(Keys.A);
        private List<Keys> LengthCheckedKeys = new List<Keys>();
#endif


        public override void Load()
        {
#if WINDOWS
            CurrentState = Keyboard.GetState();
            PreviousState = Keyboard.GetState();
#endif
        }

        public override void Update()
        {
#if WINDOWS
            PreviousState = CurrentState;
            CurrentState = Keyboard.GetState();

            for (int i = 0; i < LengthCheckedKeys.Count; i++)
            {
                if (this.KeyHeld(LengthCheckedKeys[i]))
                {
                    PressLengths[LengthCheckedKeys[i]] += Engine.GameTime.ElapsedGameTime.Milliseconds;
                }
                else if (this.KeyLeft(LengthCheckedKeys[i]))
                {
                    PressLengths[LengthCheckedKeys[i]] += Engine.GameTime.ElapsedGameTime.Milliseconds;
                    ReleasedLength[LengthCheckedKeys[i]] = PressLengths[LengthCheckedKeys[i]];
                }
                else
                {
                    PressLengths[LengthCheckedKeys[i]] = 0.0f;
                }
            }
#endif
        }

        public override void Draw()
        {
            
        }

        /// <summary>
        /// Check if a key was pressed, this will trigger exclusively over KeyHeld
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns></returns>
        public bool KeyPress(Keys k)
        {
#if WINDOWS
            return ((CurrentState.IsKeyDown(k)) && (!PreviousState.IsKeyDown(k)));
#else
            return false;
#endif
        }

        /// <summary>
        /// Check if a key is currently being held down, this wont trigger at initial press
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns></returns>
        public bool KeyHeld(Keys k)
        {
#if WINDOWS
            return ((CurrentState.IsKeyDown(k)) && (PreviousState.IsKeyDown(k)));
#else
            return false;
#endif
        }

        /// <summary>
        /// Check if a key was released
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns></returns>
        public bool KeyLeft(Keys k)
        {
#if WINDOWS
            return ((!CurrentState.IsKeyDown(k)) && (PreviousState.IsKeyDown(k)));
#else
            return false;
#endif
        }

        /// <summary>
        /// Add a key to the list of keys you want the pressed length of time for
        /// </summary>
        /// <param name="k">The key to check</param>
        public void AddKey(Keys k)
        {
#if WINDOWS
            if (!LengthCheckedKeys.Contains(k))
            {
                PressLengths.Add(k, 0.0f);
                ReleasedLength.Add(k, 0.0f);
                LengthCheckedKeys.Add(k);
            }
#endif
        }

        /// <summary>
        /// Checks how long a key was pressed for so far in millisecond
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns></returns>
        public float HeldFor(Keys k)
        {
#if WINDOWS
            if (PressLengths.ContainsKey(k))
            {
                return PressLengths[k];
            }
            else
#endif
                return 0f;
        }

        /// <summary>
        /// Check for how long a key was pressed for after release
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns></returns>
        public float ReleasedTime(Keys k)
        {
#if WINDOWS
            if (ReleasedLength.ContainsKey(k))
            {
                return ReleasedLength[k];
            }
            else
#endif
                return 0f;
        }

    }
}
