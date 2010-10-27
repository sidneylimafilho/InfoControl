using System;
using System.ComponentModel;
using System.Web;

using InfoControl;


namespace Vivina.Erp.DataClasses
{
    public partial class WebPage : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public string Url
        {
            get
            {
                if (!String.IsNullOrEmpty(RedirectUrl))
                    return RedirectUrl;

                string url = "";

                var webPage = this;                
                while ((webPage = webPage.WebPage1) != null)
                    url = webPage.Name.Trim().RemoveSpecialChars() + "/" + url;

                return "~/site/" + url + Name.RemoveSpecialChars() + "," + WebPageId + ".aspx";                
            }
        }               
    }
}
