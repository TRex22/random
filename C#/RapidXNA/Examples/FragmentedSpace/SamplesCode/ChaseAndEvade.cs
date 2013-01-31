using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArcadeShmup.SamplesCode
{
    static class ChaseAndEvade
    {
        /// <summary>  
        /// Calculates the angle that an object should face, given its position, its  
        /// target's position, its current angle, and its maximum turning speed. 
        /// NOTE: adjusted down to angles only
        /// </summary>  
        public static float TurnToFace(float desiredAngle, float currentAngle)
        {
            float turnSpeed = MathHelper.Pi / 32;


            // first, figure out how much we want to turn, using WrapAngle to get our  
            // result from -Pi to Pi ( -180 degrees to 180 degrees )  
            float difference = WrapAngle(desiredAngle - currentAngle);

            // clamp that between -turnSpeed and turnSpeed.  
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            // so, the closest we can get to our target is currentAngle + difference.  
            // return that, using WrapAngle again.  
            return WrapAngle(currentAngle + difference);
        }

        /// <summary>  
        /// Returns the angle expressed in radians between -Pi and Pi.  
        /// <param name="radians">the angle to wrap, in radians.</param>  
        /// <returns>the input value expressed in radians from -Pi to Pi.</returns>  
        /// </summary>  
        public static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }  

    }
}
