using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RapidXNA.Services
{
    public class MouseService : IRapidService
    {
#if XBOX
#else
        MouseState previousState;
        MouseState currentState;
#endif

        public override void Load()
        {
#if XBOX
#else
            currentState = Mouse.GetState();
            previousState = Mouse.GetState();
#endif
        }

        public override void Update()
        {
#if XBOX
#else
            previousState = currentState;
            currentState = Mouse.GetState();
#endif
        }

        public override void Draw()
        {
          
        }

        public Vector2 Position()
        {
#if XBOX
            return Vector2.Zero;
#else
            return new Vector2(currentState.X, currentState.Y);
#endif
        }

        public bool LeftPressed()
        {
#if XBOX
            return false;
#else
            return ((currentState.LeftButton == ButtonState.Pressed) && (previousState.LeftButton == ButtonState.Released));
#endif
        }
        public bool LeftReleased()
        {
#if XBOX
            return false;
#else
            return ((previousState.LeftButton == ButtonState.Pressed) && (currentState.LeftButton == ButtonState.Released));
#endif
        }
        public bool LeftHeld()
        {
#if XBOX
            return false;
#else
            return ((previousState.LeftButton == ButtonState.Pressed) && (currentState.LeftButton == ButtonState.Pressed));
#endif
        }

        public bool MiddlePressed()
        {
#if XBOX
            return false;
#else
            return ((currentState.MiddleButton == ButtonState.Pressed) && (previousState.MiddleButton == ButtonState.Released));
#endif
        }
        public bool MiddleReleased()
        {
#if XBOX
            return false;
#else
            return ((previousState.MiddleButton == ButtonState.Pressed) && (currentState.MiddleButton == ButtonState.Released));
#endif
        }
        public bool MiddleHeld()
        {
#if XBOX
            return false;
#else
            return ((previousState.MiddleButton == ButtonState.Pressed) && (currentState.MiddleButton == ButtonState.Pressed));
#endif
        }

        public bool RightPressed()
        {
#if XBOX
            return false;
#else
            return ((currentState.RightButton == ButtonState.Pressed) && (previousState.RightButton == ButtonState.Released));
#endif
        }
        public bool RightReleased()
        {
#if XBOX
            return false;
#else
            return ((previousState.RightButton == ButtonState.Pressed) && (currentState.RightButton == ButtonState.Released));
#endif
        }
        public bool RightHeld()
        {
#if XBOX
            return false;
#else
            return ((previousState.RightButton == ButtonState.Pressed) && (currentState.RightButton == ButtonState.Pressed));
#endif
        }

        public int ScrollWheel { 
#if XBOX
            get { return 0; }
#else
            get { return currentState.ScrollWheelValue; }
#endif
        }

    }
}
