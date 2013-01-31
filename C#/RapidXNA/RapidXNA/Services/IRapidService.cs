using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidXNA.Services
{
    public abstract class IRapidService
    {
        public RapidEngine Engine;
        public abstract void Load();
        public abstract void Update();
        public abstract void Draw();
    }
}
