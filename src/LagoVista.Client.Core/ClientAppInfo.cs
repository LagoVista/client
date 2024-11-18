using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core
{
    public interface IClientAppInfo
    {
        Type MainViewModel { get; }
    }
}
