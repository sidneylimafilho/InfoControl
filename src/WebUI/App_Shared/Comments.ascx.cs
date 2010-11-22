using System;

using System.Linq;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl.Web.UI;
using Vivina.Erp.BusinessRules.Comments;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;
using InfoControl.Web;

public partial class App_Shared_Comments : Vivina.Erp.SystemFramework.UserControlBase
{
    public string queryString;

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public int SubjectId
    {
        get { return Convert.ToInt32(ViewState["SubjectId"]); }
        set { ViewState["SubjectId"] = value; }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string PageName
    {
        get;
        set;
    }

    public string HomePath
    {
        get;
        set;
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public bool ShowStatistics { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public bool ShowButtons { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageName = PageName ?? Request["pageName"] ?? Path.GetFileName(Page.Request.PhysicalPath);

        if (String.IsNullOrEmpty(HomePath))
            HomePath = Page.Company.GetFilesDirectory();

        buttonsPanel.Visible = ShowButtons;

        commentIframeholder.Visible = ShowStatistics;
        commentForm.Visible = !ShowStatistics;
        dtlComments.Visible = !ShowStatistics;

        namePanel.Visible = !Page.User.IsAuthenticated;
        fupComments.Visible = lblSelectFile.Visible = Page.User.IsAuthenticated;


        if (!ShowStatistics && IsPostBack && Visible)
            btnInsert_Click(null, e);

        if (commentIframeholder.Visible)
            commentIframeholder.InnerHtml = GetCommentsStatistics();
    }

    protected void odsComments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (SubjectId != 0)
        {
            e.InputParameters["PageName"] = PageName;
            e.InputParameters["SubjectId"] = SubjectId;
        }
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {

        if (!String.IsNullOrEmpty(txtDescription.Text.Trim()) && txtMail.Text.IsValidMail())
        {

            var comment = new Comment
                          {
                              Website = txtSite.Text,
                              Email = Page.User.Identity.Email ?? txtMail.Text,
                              Description = txtDescription.Text.Replace("\n", "<br/>"),
                              PageName = PageName,
                              SubjectId = SubjectId,
                              FileName = fupComments.FileName,
                              FileUrl = HomePath + fupComments.FileName,
                              UserName = txtName.Text
                          };

            comment.CompanyId = Page.Company.CompanyId;

            if (String.IsNullOrEmpty(comment.UserName))
                comment.UserName = Page.User.Identity.Profile.AbreviatedName;

            if (fupComments.HasFile)
                fupComments.SaveAs(Server.MapPath(comment.FileUrl));

            new CommentsManager(this).Save(comment);

            odsComments.Select();
            dtlComments.DataBind();

            txtName.Text = txtMail.Text = txtSite.Text = txtDescription.Text = "";
        }


    }

    public string GetCommentsStatistics()
    {
        var manager = new CommentsManager(this);

        var query = manager.GetComments(SubjectId, PageName).OrderByDescending(c => c.CreatedDate).ToList();
        int amount = query.Count;

        if (amount > 0)
            return "Comentários (" + amount + "), último por " + query.First().UserName;
        else
            return "Seja o primeiro a comentar!";

    }
}