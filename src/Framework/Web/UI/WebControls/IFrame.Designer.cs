using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace InfoControl.Web.UI.WebControls
{
    internal class IFrameDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            string design = null;

            try
            {
                design = base.GetDesignTimeHtml();
            }
            catch (Exception e1)
            {
                design = GetErrorDesignTimeHtml(e1);
            }

            if ((design == null) || (design.Length == 0))
                design = GetEmptyDesignTimeHtml();

            return design;
        }



        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            return CreatePlaceHolderDesignTimeHtml(e.Message);
        }



        protected override string GetEmptyDesignTimeHtml()
        {
            return CreatePlaceHolderDesignTimeHtml("no src");
        }





    }
}
