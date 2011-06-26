using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jabber.Net.Server
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}
