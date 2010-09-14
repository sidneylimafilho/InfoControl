using System;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InfoControl.Web.UI.WebControls
{
    /// <summary>Represents a business object that provides data to data-bound controls in multi-tier Web application architectures.</summary>

    [ToolboxData("<{0}:BusinessManagerDataSource runat=server></{0}:BusinessManagerDataSource>")]
    public class BusinessManagerDataSource : ObjectDataSource
    {
        public BusinessManagerDataSource()
            : base()
        {
            this.ObjectCreating += new ObjectDataSourceObjectEventHandler(CreateBusinessManagerObject);
            
        }

        

        protected void CreateBusinessManagerObject(object sender, ObjectDataSourceEventArgs e)
        {            
            Type type = BuildManager.GetType(TypeName, false, true);
            e.ObjectInstance = Activator.CreateInstance(type, Page);
        }

        

        
    }    
}
