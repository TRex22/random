using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RapidXNA.DataType
{
    public class IGameScreen
    {
        public RapidEngine Engine;
        private bool _IsLoaded = false;
        public bool IsLoaded { get { return _IsLoaded; } }

        /// <summary>
        /// Loading data goes here, it happens in the background
        /// </summary>
        public void BeginLoad()
        {
            ThreadStart ts = new ThreadStart(LoadGameScreenAsync);
            Thread loadThread = new Thread(ts);
            loadThread.Start();
        }
        private void LoadGameScreenAsync()
        {
            this.Load();
            _IsLoaded = true;
        }

        /// <summary>
        /// All functions needed for the GameScreen to function
        /// </summary>
        #region Overrideable Functions
        public virtual void Load()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
        
        }

        /// <summary>
        /// Allows you to do cleaning up when the screen is removed from the stack.
        /// </summary>
        public virtual void OnPop()
        {

        }
        #endregion
    }
}
