using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brender_0_5
{
    [Serializable()]
    public class Component : IMenu
    {
        public bool enabled;
        public string name;

        public Object _object;

        public virtual void Update()
        {

        }

        public virtual Component Copy()
        {
            return null;
        }

        public virtual bool StartOwnMenu()
        {
            return false;
        }
    }
}
