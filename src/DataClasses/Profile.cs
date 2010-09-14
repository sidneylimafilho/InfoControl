using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vivina.Erp.DataClasses
{
    public partial class Profile
    {
        public string FirstName { get { return Name.Split(' ')[0]; } }
        public string LastName { get { string[] names = Name.Split(' '); return names[names.Length - 1]; } }
        public string AbreviatedName { get { return FirstName + " " + LastName; } }
    }
}
