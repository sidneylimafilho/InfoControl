using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public class LeafBoxTemplateNamingContainer : Control, INamingContainer
{
}

public partial class App_Shared_LeafBox : System.Web.UI.UserControl
{
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof(LeafBoxTemplateNamingContainer))]
    public ITemplate ItemTemplate { get; set; }

    public string CssClass{ get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Page_Init(object sender, EventArgs e)
    {
        PlaceHolder.Controls.Clear();
        if (ItemTemplate == null)
            PlaceHolder.Controls.Add(new LiteralControl("Nenhum template definido"));
        else
        {
            LeafBoxTemplateNamingContainer c = new LeafBoxTemplateNamingContainer();
            ItemTemplate.InstantiateIn(c);
            PlaceHolder.Controls.Add(c);
        }
    }

}
