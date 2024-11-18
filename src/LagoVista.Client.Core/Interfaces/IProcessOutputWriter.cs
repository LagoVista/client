using LagoVista.Client.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Client.Core.Interfaces
{
    public interface IProcessOutputWriter
    {
        void AddMessage(LogType type, string message);
        void Flush(Boolean clear = false);

        ObservableCollection<ConsoleOutput> Output { get;  }
    }
}
