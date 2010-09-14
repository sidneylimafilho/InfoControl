using System.IO;
using System.Text;
using System.Web;

namespace Vivina.Erp.DataClasses
{
    public partial class DocumentTemplate
    {
        public string Content
        {
            get { return File.ReadAllText(HttpContext.Current.Server.MapPath(FileUrl), Encoding.Default); }
        }
    }
}