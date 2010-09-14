using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vivina.Erp.DataClasses
{
    public partial class Alert
    {
        /// <summary>
        /// This method creates a valid Alert
        /// </summary>
        /// <param name="userID">The user who you want send a alert</param>
        /// <param name="description">the alert's description</param>
        public Alert(Int32 userID, String description)
        {
            this.UserId = userID;
            this.Description = description;
        }

    }
}
