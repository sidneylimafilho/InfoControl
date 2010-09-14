using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoControl.Web.Security
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class RoleRequiredAttribute : Attribute
    {
        private string _roleName;

        public string RoleName
        {
            get { return _roleName; }
            set { _roleName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName">Role Name</param>
        public RoleRequiredAttribute(string roleName)
        {
            this._roleName = roleName;
        }
        
    }

}
