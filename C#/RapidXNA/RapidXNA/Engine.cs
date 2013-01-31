using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RapidXNA.Services;
using RapidXNA.DataType;

namespace RapidXNA
{
    public class RapidEngine
    {
        public RapidEngine(Game game1, GraphicsDevice graphicsDevice, ContentManager contentManager, IGameScreen initalScreen)
        {
            AddService(new ScreenService(), false);

            _GraphicsDevice = graphicsDevice;
            _ContentManager = contentManager;
            _SpriteBatch = new SpriteBatch(_GraphicsDevice);

            ServiceOf<ScreenService>().PushScreen(initalScreen);
            _Game = game1;

            _RenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// Allows exiting of the game outside of Game1.cs
        /// </summary>
        Game _Game;
        public void Exit()
        {
            //Todo: Engine cleanup here
            _Game.Exit();
        }

        /// <summary>
        /// Services Implementations
        ///  - ServiceOf<T>() gives back a service so you can get direct access to the services
        ///  - AddService(service,replace) adds a new instance of a service into the services list, if replace is false and a service of that type already exists
        ///    it wont get added.
        /// </summary>
        private List<IRapidService> _Services = new List<IRapidService>();
        public T ServiceOf<T>()
        {
            for (int i = 0; i < _Services.Count; i++)
            {
                if (_Services[i].GetType() == typeof(T))
                {
                    return (T)Convert.ChangeType(_Services[i],typeof(T),null);
                }
            }
            return default(T);
        }
        public void AddService(IRapidService service, bool replace)
        {
            for (int i = _Services.Count - 1; i > -1; i--)
            {
                if (service.GetType() == _Services[i].GetType())
                    if (replace)
                    {
                        _Services[i].Engine = null;
                        _Services.RemoveAt(i);
                    }
                    else
                    {
                        return;
                    }
            }
            service.Engine = this;
            service.Load();
            _Services.Add(service);
        }

        /// <summary>
        /// Game Time snapshots from update and draw
        /// TODO: decide if I really need both
        /// </summary>
        private GameTime _GameTimeUpdate, _GameTimeDraw;
        public GameTime GameTime { get { return _GameTimeUpdate; } }

        /// <summary>
        /// Graphics Device from XNA Game
        /// </summary>
        private GraphicsDevice _GraphicsDevice;
        public GraphicsDevice GraphicsDevice { get { return _GraphicsDevice;  } }

        /// <summary>
        /// Content Manager from XNA Game
        /// </summary>
        private ContentManager _ContentManager;
        public ContentManager Content { get { return _ContentManager;  } }

        /// <summary>
        /// SpriteBatch created inside the engine
        /// </summary>
        private SpriteBatch _SpriteBatch;
        public SpriteBatch SpriteBatch { get { return _SpriteBatch; } }

        /// <summary>
        /// Handles all the core updating for the game engine
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _GameTimeUpdate = gameTime;
            foreach (IRapidService rs in _Services)
            {
                rs.Update();
            }
        }

        /// <summary>
        /// Allows for screenshots and post processing
        /// </summary>
        RenderTarget2D _RenderTarget;

        /// <summary>
        /// Handles all the core drawing for the game engine
        /// </summary>
        /// <param name="gameTime"></param>
        Color _ClearColor = new Color(255, 255, 255, 255);
        public void Draw(GameTime gameTime, Color clearColor)
        {
            _GameTimeDraw = gameTime;

            GraphicsDevice.SetRenderTarget(_RenderTarget);

            if (clearColor != null)
            {
                GraphicsDevice.Clear(clearColor);
            }
            else
            {
                GraphicsDevice.Clear(_ClearColor);
            }

            foreach (IRapidService rs in _Services)
            {
                rs.Draw();
            }

            GraphicsDevice.SetRenderTarget(null);

            if (OnFinalDraw != null)
            {
                OnFinalDraw(_RenderTarget);
            }

            SpriteBatch.Begin();
            SpriteBatch.Draw(_RenderTarget, _RenderTarget.Bounds, Color.White);
            SpriteBatch.End();
        }

        /// <summary>
        /// Event that triggers before the final draw is done, where you can do screenshots, etc
        /// </summary>
        public event OnFinalDrawEventHandler OnFinalDraw;

        /// <summary>
        /// IGameComponent implementation
        /// </summary>
        public void Initialize()
        {
            
        }
    }

    /// <summary>
    /// Delegate used to add post processing to the engine
    /// </summary>
    /// <param name="renderTarget">The render target that will be passed to the functions</param>
    public delegate void OnFinalDrawEventHandler(RenderTarget2D renderTarget);
}
