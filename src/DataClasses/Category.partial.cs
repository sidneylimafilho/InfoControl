using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfoControl;
using System.Web;

namespace Vivina.Erp.DataClasses
{
    public partial class Category
    {
        /// <summary>
        /// URL from e-commerce category
        /// </summary>
        public string Url
        {
            get
            {
                string text = "";// this.CategoryId + ".aspx";
                Category cat = this;
                
                while (cat != null)
                {
                    text = HttpUtility.UrlEncode(cat.Name.RemoveSpecialChars()) + "/" + text;
                    cat = cat.Category1;
                }

                return "~/site/loja/" + text;


            }
        }
    }
}
