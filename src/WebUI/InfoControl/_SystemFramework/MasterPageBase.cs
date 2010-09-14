using InfoControl.Web.UI;

namespace Vivina.Erp.SystemFramework
{
    public class MasterPageBase : DataMasterPage
    {
        public PageBase PageBase
        {
            get { return Page as PageBase; }
        }
    }
}