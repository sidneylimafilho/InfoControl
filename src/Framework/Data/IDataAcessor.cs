using System;
using System.Collections.Generic;
using System.Text;

namespace InfoControl.Data
{
    /// <summary>
    /// Defines the contract that implements a data accessor using a DataManager
    /// </summary>
    public interface IDataAccessor : IDisposable
    {
        DataManager DataManager { get; }        
    }
}
