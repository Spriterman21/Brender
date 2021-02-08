using System;
using System.Collections.Generic;
using System.Text;

namespace Brender_0_5
{
    public interface IMenu
    {
        // the bool allows for information about whether something changed to be passed onto the lower layers of recursion
        bool StartOwnMenu();
    }
}
