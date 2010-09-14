using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoControl.Web.Security
{
    [PermissionRequired("Service")]
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class PermissionRequiredAttribute : Attribute
    {
        private string _permissionName;

        public string PermissionName
        {
            get { return _permissionName; }
            set { _permissionName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionName">Permission Name</param>
        public PermissionRequiredAttribute(string permissionName)
        {
            this._permissionName = permissionName;
        }
        
    }

}
