using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace RapidXNA.Services
{
    public class PhoneInputService : IRapidService
    {
        
#if WINDOWS_PHONE
        List<GestureSample> _GestureSamples = new List<GestureSample>();
#endif
        public List<GestureSample> GestureSamples
        {
#if WINDOWS_PHONE
            get { return _GestureSamples; }
#else
            get { return new List<GestureSample>(); } //Should rethink this
#endif
        }

        public override void Load()
        {
            
        }

        public override void Update()
        {
#if WINDOWS_PHONE
            _GestureSamples.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                var gs = TouchPanel.ReadGesture();
                _GestureSamples.Add(gs);
            }
#endif
        }

        public override void Draw()
        {
            
        }

    }

#if WINDOWS_PHONE
#else
    public class GestureSample
    {
        //Used for when there is no gesturesample class available (it isnt available on windows/xbox?)
        public Vector2 Delta { get { return new Vector2(); } set {} }
        public Vector2 Delta2 { get { return new Vector2(); } set {} }
        public TimeSpan Timestamp { get { return new TimeSpan(); } }
        public Vector2 Position { get { return new Vector2(); } set {} }
        public Vector2 Position2 { get { return new Vector2(); } set {} }
        public GestureType GestureType { get { return GestureType.CustomNone; } }
    }

    public enum GestureType
    {
        DoubleTap,
        DragComplete,
        Flick,
        FreeDrag,
        Hold,
        HorrizontalDrag,
        None,
        Pinch,
        PinchComplete,
        Tap,
        VerticalDrag,
        CustomNone = 9999
    }
#endif
}
