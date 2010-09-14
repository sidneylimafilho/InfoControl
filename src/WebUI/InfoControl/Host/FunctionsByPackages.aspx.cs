using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;

public partial class InfoControl_Host_PakageByFunctions : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dlbFunctions.DataBind();
        }
    }
    protected void odsRemainingFunctions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["packageId"] = Convert.ToInt32(Request["PackageId"].DecryptFromHex());
    }
    protected void odsFunctionsByPackages_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["packageId"] = Convert.ToInt32(Request["PackageId"].DecryptFromHex());
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Int32 packageId = Convert.ToInt32(Request["PackageId"].DecryptFromHex());

        IEnumerable<Function> functionsByPackage = (Session["FunctionPackage"] as List<Function>).AsEnumerable();

        IEnumerable<Function> selectedFunctions = dlbFunctions.SelectedItems.Cast<ListItem>()
            .Select(li => new Function()
            {
                FunctionId = Convert.ToInt32(li.Value)
            });

        foreach (Function function in from fp in functionsByPackage.AsEnumerable()
                                      where !selectedFunctions.Any(func => func.FunctionId == fp.FunctionId)
                                      select fp)
            using (PackageFunctionManager manager = new PackageFunctionManager(null))
            {
                PackageFunction pFunction = new PackageFunction();
                pFunction.FunctionId = function.FunctionId;
                pFunction.PackageId = packageId;

                manager.Delete(pFunction);
            }

        foreach (Function function in from fp in selectedFunctions
                                      where !functionsByPackage.AsEnumerable().Any(func => func.FunctionId == fp.FunctionId)
                                      select fp)
            using (PackageFunctionManager manager = new PackageFunctionManager(null))
            {
                PackageFunction packageFunction = new PackageFunction();
                packageFunction.PackageId = packageId;
                packageFunction.FunctionId = function.FunctionId;

                manager.Insert(packageFunction);
            }
        dlbFunctions.DataBind();

    }
    protected void odsFunctionsByPackages_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        List<Function> FunctionsPackage = e.ReturnValue as List<Function>;
        Session["FunctionPackage"] = FunctionsPackage;
    }
}
