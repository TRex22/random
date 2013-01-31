using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArcadeShmup.Helpers;
using ArcadeShmup.SamplesCode;
using QuadTreeTest;

namespace ArcadeShmup
{
    public class Pickup
    {
        public Vector2 Position;
        public int WhatAmI;
    }

    public class Bullet : IQuadItem
    {
        public Vector2 Position;
        public Vector2 Movement;
        public float Direction;
        public float Speed = 10;
        public Color Colour = Color.Red;
        public int Damage = 1;
        //public bool Friendly; //handled via the colour
    }

    public class Enemy
    {
        public Vector2 Position;
        public float Direction;
        public Color Colour;
        public int FireRate = 1000;
        public int nextFire = 1000;

        public int Speed;
        
        private bool Evade = false;
        private int EvadeCount = 500;
        private int EvadeLength = 500;

        //BOIDS
        public Vector2 loc { get {return Position; } }
        public Vector2 vel = new Vector2();
        public Vector2 acc = new Vector2();
        float r;
        float maxSpeed;
        float maxForce;

        public Enemy(int x, int y, int speed, float mf)
        {
            Position = new Vector2(x, y);
            Speed = speed;

            r = 2.0f;
            maxSpeed = speed;
            maxForce = mf;
        }

        public void Run(List<Enemy> enemies)
        {
            Flock(enemies);

            //ChaseEvade(enemies);

            nextFire -= GameState.gameTime.ElapsedGameTime.Milliseconds;
            if (nextFire <= 0)
            {
                nextFire += FireRate;
                Bullet b = new Bullet();
                b.Damage = 1;
                b.Direction = Direction;
                b.Speed = 2 * Speed;
                b.Colour = Colour;
                b.Position = new Vector2(Position.X, Position.Y);
                GameState.Bullets.Add(b);
            }

            Update();
        }

        private void ChaseEvade(List<Enemy> enemies)
        {
            foreach (Enemy e in enemies)
            {
                Vector2 closest = new Vector2();
                double d = 9999999;
                if (e.Colour != Colour)
                {
                    double d2 = MHelper.Vector2Distance(Position, e.Position);
                    if ((d2 < d) && (d2 > 25))
                    {
                        d = d2;
                        closest = e.Position;
                    }
                }

                if (d < 50)
                {
                    Evade = true;
                }

                double d3 = MHelper.Vector2Distance(e.Position, GameState.PlayerPosition);
                if ((d < d3) && (d3 > 75) && (!Evade))
                {
                    Direction = ChaseAndEvade.TurnToFace((float)Math.Atan2(closest.Y - Position.Y, closest.X - Position.X), Direction);
                }
                else if (Evade)
                {
                    Direction = ChaseAndEvade.TurnToFace((float)Math.Atan2(GameState.PlayerPosition.Y - Position.Y, GameState.PlayerPosition.X - Position.X), -Direction);
                    EvadeCount -= GameState.gameTime.ElapsedGameTime.Milliseconds;
                    if (EvadeCount <= 0)
                    {
                        EvadeCount += EvadeLength;
                        Evade = false;
                    }
                }
            }
        }

        private void Flock(List<Enemy> enemies)
        {
            Vector2 sep = separate(enemies);
            Vector2 ali = align(enemies);
            Vector2 coh = cohese(enemies);
            Vector2 tar = target();

            //weight forces
            sep *= 4.0f;
            ali *= 1.0f;
            coh *= 0.1f;
            tar *= 1.5f;

            acc = sep + ali + coh - tar;

        }

        private Vector2 target()
        {
            Vector2 chase = new Vector2();
            float d = (float)MHelper.Vector2Distance(Position, GameState.PlayerPosition);
            if (d > 150)
            {
                chase = Position - GameState.PlayerPosition;
                chase.Normalize();
                chase *= maxSpeed;
                if (chase.Length() > maxForce)
                {
                    chase.Normalize();
                    chase *= maxForce;
                }
            }
            return chase;
        }

        private Vector2 cohese(List<Enemy> enemies)
        {
            float neighbourDistance = 1000f;
            Vector2 sum = new Vector2();
            int count = 0;

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy other = enemies[i];
                float d = (float)MHelper.Vector2Distance(loc,other.loc);
                if ((d < neighbourDistance) && (other.Colour == Colour) && (d > 0))
                {
                    sum += other.loc;
                    count++;
                }
            }

            if (count > 0)
            {
                sum /= count;
                return steer(sum, false);
            }
            return sum;
        }

        private Vector2 align(List<Enemy> enemies)
        {
            float neighbourDist = 1000f;
            Vector2 steer = new Vector2();
            int count = 0;

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy other = enemies[i];

                float d = (float)MHelper.Vector2Distance(loc, other.loc);
                if ((d < neighbourDist) && (other.Colour == Colour) && (d > 0))
                {
                    steer += other.vel;
                    count++;
                }
            }

            if (count > 0)
                steer /= count;

            if (steer.Length() > 0)
            {
                steer.Normalize();
                steer *= maxSpeed;
                steer -= vel;
                if (steer.Length() > maxForce)
                {
                    steer.Normalize();
                    steer *= maxForce;
                }
            }

            return steer;
        }

        private Vector2 separate(List<Enemy> enemies)
        {
            float desiredSeparation = 100f;
            Vector2 steer = new Vector2();
            int count = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy other = enemies[i];
                float d = (float)MHelper.Vector2Distance(loc, other.loc);
                if ((d < desiredSeparation) && (other.Colour == Colour) && (d > 0)) //colours flock together
                {
                    Vector2 diff = loc - other.loc;
                    diff.Normalize();
                    diff /= d;
                    steer += diff;
                    count++;
                }
            }

            if (count > 0)
                steer /= count;

            if (steer.Length() > 0)
            {
                steer.Normalize();
                steer *= maxSpeed;
                steer /= vel;
                if (steer.Length() > maxForce)
                {
                    steer.Normalize();
                    steer *= maxForce;
                }
            }

            return steer;
        }

        public void seek(Vector2 target)
        {
            acc += steer(target, false);
        }

        private void arrive(Vector2 target)
        {
            acc += steer(target, true);
        }

        private Vector2 steer(Vector2 target, bool slowDown) //steer towards a target
        {
            Vector2 steer = new Vector2();
            Vector2 desired = target - loc;
            float d = desired.Length();

            if (d > 0)
            {
                desired.Normalize();
                if ((slowDown) && (d < 100.0f))
                {
                    desired *= maxSpeed * (d / 100.0f);
                }
                else
                {
                    desired *= maxSpeed;
                }
                steer = desired - vel;
                if (steer.Length() > maxSpeed)
                {
                    steer.Normalize();
                    steer *= maxSpeed;
                }
            }

            return steer;
        }

        private void Update()
        {
            vel += acc;
            if (vel.Length() > maxSpeed)
            {
                vel.Normalize();
                vel *= maxSpeed;
            }
            Position += vel;
            acc *= 0;

            Direction = (float)Math.Atan2(vel.Y, vel.X);
        }
    }
}
