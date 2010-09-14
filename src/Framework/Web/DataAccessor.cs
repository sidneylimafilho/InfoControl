using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using InfoControl.Data;
using InfoControl.Properties;

namespace InfoControl.Web
{
    /// <summary>
    /// Defines the contract that implements a data accessor using a DataManager
    /// </summary>
    public class DataAccessor : IDataAccessor
    {
        private bool _commitRequired = true;
        public DataAccessor() { }

        public DataAccessor(bool commitRequired)
        {
            _commitRequired = commitRequired;
        }

        private DataManager _dataManager;
        /// <summary>
        /// Provides helper functions that implements data access
        /// </summary>
        public DataManager DataManager
        {
            get
            {
                //
                // Check if exists a DataManager in context
                //
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Items[Resources.CurrentDataManagerKey] != null)
                    {
                        _dataManager = HttpContext.Current.Items[Resources.CurrentDataManagerKey] as DataManager;
                    }
                    else
                    {
                        _dataManager = new DataManager(_commitRequired);
                        HttpContext.Current.Items[Resources.CurrentDataManagerKey] = _dataManager;

                    }

                return _dataManager;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
