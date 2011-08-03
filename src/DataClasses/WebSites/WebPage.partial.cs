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

                var parent = this.WebPage1;
                while (parent != null && parent.WebPage1 != null)
                {
                    url = parent.Name.Trim().RemoveSpecialChars() + "/" + url;
                    parent = parent.WebPage1;                    
                }

                return "~/" + url + Name.RemoveSpecialChars() + "," + WebPageId + ".aspx";
            }
        }
    }
}
