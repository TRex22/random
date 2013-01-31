using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RapidXNA.Services
{
    //TODO: fix inconsistencies from keyboard functions that exist in gamepad helpers
    public class GamePadService : IRapidService
    {
        GamePadState[] PreviousState = new GamePadState[4];
        GamePadState[] CurrentState = new GamePadState[4];
        public override void Load()
        {
#if WINDOWS_PHONE
            PreviousState[0] = GamePad.GetState(PlayerIndex.One);
            CurrentState[0] = GamePad.GetState(PlayerIndex.One);
#else
            PreviousState[0] = GamePad.GetState(PlayerIndex.One);
            PreviousState[1] = GamePad.GetState(PlayerIndex.Two);
            PreviousState[2] = GamePad.GetState(PlayerIndex.Three);
            PreviousState[3] = GamePad.GetState(PlayerIndex.Four);

            CurrentState[0] = GamePad.GetState(PlayerIndex.One);
            CurrentState[1] = GamePad.GetState(PlayerIndex.Two);
            CurrentState[2] = GamePad.GetState(PlayerIndex.Three);
            CurrentState[3] = GamePad.GetState(PlayerIndex.Four);
#endif
        }

        public override void Update()
        {
#if WINDOWS_PHONE
            PreviousState[0] = CurrentState[0];

            CurrentState[0] = GamePad.GetState(PlayerIndex.One);
#else
            PreviousState[0] = CurrentState[0];
            PreviousState[1] = CurrentState[1];
            PreviousState[2] = CurrentState[2];
            PreviousState[3] = CurrentState[3];

            CurrentState[0] = GamePad.GetState(PlayerIndex.One);
            CurrentState[1] = GamePad.GetState(PlayerIndex.Two);
            CurrentState[2] = GamePad.GetState(PlayerIndex.Three);
            CurrentState[3] = GamePad.GetState(PlayerIndex.Four);
#endif
        }

        public override void Draw()
        {
            
        }

        /// <summary>
        /// Triggers functions
        /// </summary>

        public float LeftTriggerValue(int ControllerNum)
        {
#if WINDOWS_PHONE
            return 0f;
#else
            return CurrentState[ControllerNum - 1].Triggers.Left;
#endif
        }
        public float RightTrigerValue(int ControllerNum)
        {
#if WINDOWS_PHONE
            return 0f;
#else
            return CurrentState[ControllerNum - 1].Triggers.Right;
#endif
        }
        public bool LeftTriggerPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Triggers.Left == 0.0f) && (CurrentState[ControllerNum - 1].Triggers.Left > 0.0f));
#endif
        }
        public bool RightTriggerPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Triggers.Right == 0.0f) && (CurrentState[ControllerNum - 1].Triggers.Right > 0.0f));
#endif
        }
        public bool LeftTriggerReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Triggers.Left > 0.0f) && (CurrentState[ControllerNum - 1].Triggers.Left == 0.0f));
#endif  
        }
        public bool RightTriggerReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Triggers.Right > 0.0f) && (CurrentState[ControllerNum - 1].Triggers.Right == 0.0f));
#endif
        }

        /// <summary>
        /// Bumpers
        /// </summary>

        public bool LeftBumberPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.LeftShoulder == ButtonState.Released) && (CurrentState[ControllerNum - 1].Buttons.LeftShoulder == ButtonState.Pressed));
#endif
        }
        public bool RightBumberPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.RightShoulder == ButtonState.Released) && (CurrentState[ControllerNum - 1].Buttons.RightShoulder == ButtonState.Pressed));
#endif
        }
        public bool LeftBumberReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.LeftShoulder == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].Buttons.LeftShoulder == ButtonState.Released));
#endif
        }
        public bool RightBumberReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.RightShoulder == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].Buttons.RightShoulder == ButtonState.Released));
#endif
        }
        public bool LeftBumberHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].Buttons.LeftShoulder == ButtonState.Pressed);
#endif
        }
        public bool RightBumberHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].Buttons.RightShoulder == ButtonState.Pressed);
#endif
        }

        /// <summary>
        /// Sticks
        /// </summary>

        public Vector2 LeftStick(int ControllerNum)
        {
#if WINDOWS_PHONE
            return Vector2.Zero;
#else
            return CurrentState[ControllerNum - 1].ThumbSticks.Left;
#endif
        }
        public Vector2 RightStick(int ControllerNum)
        {
#if WINDOWS_PHONE
            return Vector2.Zero;
#else
            return CurrentState[ControllerNum - 1].ThumbSticks.Right;
#endif
        }
        public float LeftStickDirection(int ControllerNum)
        {
#if WINDOWS_PHONE
            return 0f;
#else
            float _x = CurrentState[ControllerNum - 1].ThumbSticks.Left.X, _y = CurrentState[ControllerNum - 1].ThumbSticks.Left.Y;
            return (float)Math.Atan2(_y, _x);
#endif
        }
        public float RightStickDirection(int ControllerNum)
        {
#if WINDOWS_PHONE
            return 0f;
#else
            float _x = CurrentState[ControllerNum - 1].ThumbSticks.Right.X, _y = CurrentState[ControllerNum - 1].ThumbSticks.Right.Y;
            return (float)Math.Atan2(_y, _x);
#endif
        }
        public bool LeftStickPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.LeftStick == ButtonState.Released) && (CurrentState[ControllerNum - 1].Buttons.LeftStick == ButtonState.Pressed));
#endif
        }
        public bool RightStickPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.RightStick == ButtonState.Released) && (CurrentState[ControllerNum - 1].Buttons.RightStick == ButtonState.Pressed));
#endif
        }
        public bool LeftStickReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.LeftStick == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].Buttons.LeftStick == ButtonState.Released));
#endif
        }
        public bool RightStickReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.RightStick == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].Buttons.RightStick == ButtonState.Released));
#endif
        }
        public bool LeftStickHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].Buttons.LeftStick == ButtonState.Pressed);
#endif
        }
        public bool RightStickHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].Buttons.RightStick == ButtonState.Pressed);
#endif
        }

        /// <summary>
        /// DPAD
        /// </summary>


        public bool LeftPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].DPad.Left == ButtonState.Released) && (CurrentState[ControllerNum - 1].DPad.Left == ButtonState.Pressed));
#endif
        }
        public bool LeftReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].DPad.Left == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].DPad.Left == ButtonState.Released));
#endif
        }
        public bool LeftHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].DPad.Left == ButtonState.Pressed);
#endif
        }
        public bool RightPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].DPad.Right == ButtonState.Released) && (CurrentState[ControllerNum - 1].DPad.Right == ButtonState.Pressed));
#endif
        }
        public bool RightReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].DPad.Right == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].DPad.Right == ButtonState.Released));
#endif
        }
        public bool RightHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].DPad.Right == ButtonState.Pressed);
#endif
        }
        public bool UpPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].DPad.Up == ButtonState.Released) && (CurrentState[ControllerNum - 1].DPad.Up == ButtonState.Pressed));
#endif
        }
        public bool UpReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].DPad.Up == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].DPad.Up == ButtonState.Released));
#endif
        }
        public bool UpHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].DPad.Up == ButtonState.Pressed);
#endif
        }
        public bool DownPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].DPad.Down == ButtonState.Released) && (CurrentState[ControllerNum - 1].DPad.Down == ButtonState.Pressed));
#endif
        }
        public bool DownReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].DPad.Down == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].DPad.Down == ButtonState.Released));
#endif
        }
        public bool DownHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].DPad.Down == ButtonState.Pressed);
#endif
        }

        public bool XPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.X == ButtonState.Released) && (CurrentState[ControllerNum - 1].Buttons.X == ButtonState.Pressed));
#endif
        }
        public bool XReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.X == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].Buttons.X == ButtonState.Released));
#endif
        }
        public bool XHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].Buttons.X == ButtonState.Pressed);
#endif
        }
        public bool YPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.Y == ButtonState.Released) && (CurrentState[ControllerNum - 1].Buttons.Y == ButtonState.Pressed));
#endif
        }
        public bool YReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.Y == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].Buttons.Y == ButtonState.Released));
#endif
        }
        public bool YHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].Buttons.Y == ButtonState.Pressed);
#endif
        }
        public bool APressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.A == ButtonState.Released) && (CurrentState[ControllerNum - 1].Buttons.A == ButtonState.Pressed));
#endif
        }
        public bool AReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.A == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].Buttons.A == ButtonState.Released));
#endif
        }
        public bool AHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].Buttons.A == ButtonState.Pressed);
#endif
        }
        public bool BPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.B == ButtonState.Released) && (CurrentState[ControllerNum - 1].Buttons.B == ButtonState.Pressed));
#endif
        }
        public bool BReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.B == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].Buttons.B == ButtonState.Released));
#endif
        }
        public bool BHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].Buttons.B == ButtonState.Pressed);
#endif
        }

        public bool StartPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.Start == ButtonState.Released) && (CurrentState[ControllerNum - 1].Buttons.Start == ButtonState.Pressed));
#endif
        }
        public bool StartReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return ((PreviousState[ControllerNum - 1].Buttons.Start == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].Buttons.Start == ButtonState.Released));
#endif
        }
        public bool StartHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return false;
#else
            return (CurrentState[ControllerNum - 1].Buttons.Start == ButtonState.Pressed);
#endif
        }
        public bool BackPressed(int ControllerNum)
        {
#if WINDOWS_PHONE
            return ((PreviousState[0].Buttons.Back == ButtonState.Released) && (CurrentState[0].Buttons.Back == ButtonState.Pressed));
#else
            return ((PreviousState[ControllerNum - 1].Buttons.Back == ButtonState.Released) && (CurrentState[ControllerNum - 1].Buttons.Back == ButtonState.Pressed));
#endif
        }
        public bool BackReleased(int ControllerNum)
        {
#if WINDOWS_PHONE
            return ((PreviousState[0].Buttons.Back == ButtonState.Pressed) && (CurrentState[0].Buttons.Back == ButtonState.Released));
#else
            return ((PreviousState[ControllerNum - 1].Buttons.Back == ButtonState.Pressed) && (CurrentState[ControllerNum - 1].Buttons.Back == ButtonState.Released));
#endif
        }
        public bool BackHeld(int ControllerNum)
        {
#if WINDOWS_PHONE
            return (CurrentState[0].Buttons.Back == ButtonState.Pressed);
#else
            return (CurrentState[ControllerNum - 1].Buttons.Back == ButtonState.Pressed);
#endif
        }
    }
}