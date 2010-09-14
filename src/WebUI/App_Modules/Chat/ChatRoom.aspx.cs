using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


/// <summary>
/// Summary description for Chat.
/// </summary>
public class ChatRoom : System.Web.UI.Page
{
    public string RoomId
    {
        get
        {
            if (ViewState["RoomId"] == null)
                ViewState["RoomId"] = Guid.NewGuid().ToString().Replace("-", "");
            return ViewState["RoomId"].ToString();
        }
    }

    private void Page_Load(object sender, System.EventArgs e)
    {

    }

}

