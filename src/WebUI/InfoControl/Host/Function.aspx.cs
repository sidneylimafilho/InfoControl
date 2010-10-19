using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using InfoControl;
using InfoControl.Data;
using InfoControl;

using Vivina.Erp.SystemFramework;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;



namespace Vivina.Erp.WebUI.Host
{
    public partial class Function : Vivina.Erp.SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Vivina.Erp.DataClasses.Function originalFunction;

            FunctionManager functionManager;


            if (!IsPostBack)
            {
                if (Context.Items["FunctionId"] != null)
                {

                    Page.ViewState["FunctionId"] = Context.Items["FunctionId"];
                    functionManager = new FunctionManager(this);
                    //originalFunction = functionManager.GetFunction(Convert.ToInt32(Page.ViewState["FunctionId"]));
                    originalFunction = functionManager.GetFunction(Convert.ToInt32(Page.ViewState["FunctionId"]));


                    txtName.Text = originalFunction.Name;
                    txtCode.Text = originalFunction.CodeName;
                    txtDescription.Value = originalFunction.Description ?? "";

                }


            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            Vivina.Erp.DataClasses.Function originalFunction = new Vivina.Erp.DataClasses.Function();
            FunctionManager functionManager = new FunctionManager(this);
            Vivina.Erp.DataClasses.Function function = new Vivina.Erp.DataClasses.Function();


            if (Page.ViewState["FunctionId"] != null)
            {

                originalFunction = functionManager.GetFunction(Convert.ToInt32(Page.ViewState["FunctionId"]));
                function.CopyPropertiesFrom(originalFunction);
            }

            function.Name = txtName.Text;
            function.CodeName = txtCode.Text;
            function.Description = txtDescription.Value;

            SiteMapNode node = SiteMap.RootNode.GetAllNodes().Cast<SiteMapNode>().Where(n => n.ResourceKey == function.FunctionId.ToString()).FirstOrDefault();
            if (node != null)
                node.Description = function.Description;


            if (Page.ViewState["FunctionId"] != null)
                //functionManager.update(originalFunction, function);
                functionManager.Update(originalFunction, function);
            else
                functionManager.InsertFunction(function);

            Server.Transfer("Functions.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("Functions.aspx");
        }
    }
}
