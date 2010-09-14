using System;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;


[SupportsEventValidation]
[ValidationProperty("SelectedValue")]
[ControlValueProperty("SelectedValue")]
public partial class App_Shared_ComboTreeBox :  InfoControl.Web.UI.DataUserControl
{
    private bool _enabled = true;
    private string _width = null;
    public string MenuID;

    public bool Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    public string SelectedValue { get; set; }

    public string DataSourceID { get; set; }
    public string DataFieldID { get; set; }
    public string DataFieldParentID { get; set; }
    public string DataTextField { get; set; }
    public string DataValueField { get; set; }

    public string Width
    {
        get { return _width ?? "200px"; }
        set { _width = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        tree.DataFieldID = DataFieldID;
        tree.DataFieldParentID = DataFieldParentID;
        tree.DataTextField = DataTextField;
        tree.DataValueField = DataValueField;
        tree.DataSource = GetDataSource(DataSourceID);

        tree.OnClientNodeClicking = tree.ClientID + "nodeClicking";

        MenuID = tree.ClientID + "Menu";

        string script = @"
           
            function " + tree.ClientID + @"nodeClicking(sender, args){     
                var text = '';
                if(args) text = args.get_node().get_text();
                $('#" + cboTreeBox.ClientID + @"').text(text);
                $('#" + MenuID + @"').hide('slow');
                $('#" + ClientID + @"')[0].value=args.get_node().get_value();}";

        Page.ClientScript.RegisterStartupScript(GetType(), tree.ClientID, script, true);

        if (IsPostBack && tree.SelectedNode != null)
            SelectedValue = tree.SelectedNode.Value;
    }

    public IDataSource GetDataSource(string dataSourceId)
    {
        Control namingContainer = NamingContainer;
        IDataSource dataSource = null;
        while ((dataSource == null) && (namingContainer != Page))
        {
            if (namingContainer == null)
            {
                throw new HttpException("DataBoundControlHelper_NoNamingContainer");
            }
            dataSource = (IDataSource) namingContainer.FindControl(dataSourceId);
            namingContainer = namingContainer.NamingContainer;
        }
        return dataSource;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Enabled)
            cboTreeBox.Attributes["onclick"] = "$('#" + MenuID + "').toggle()";

        tree.DataBind();
        tree.Nodes.Insert(0 , new RadTreeNode("Todos", null));

        RadTreeNode node = tree.FindNodeByValue(SelectedValue);
        if (node != null)
        {
            node.Selected = true;
            cboTreeBox.InnerText = node.Text;
        }

        tree.ExpandAllNodes();
    }

    protected void tree_NodeDataBound(object sender, RadTreeNodeEventArgs e)
    {
        //e.Node.Expanded = !e.Node.Text.StartsWith("-");
        e.Node.Text = e.Node.Text.TrimStart('-').Trim();
    }

    /// <summary>
    /// This method returns true if exists an item with the selectedValue equals the parameter. Otherwise false
    /// </summary>
    /// <param name="selectedValue"></param>
    /// <returns></returns>
    public Boolean ExistItem(String selectedValue)
    {
        return tree.Nodes.FindNodeByValue(selectedValue) != null;
    }
}