using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;

public partial class InfoControl_Services_CustomerCall : Vivina.Erp.SystemFramework.PageBase
{
    private CustomerManager customerManager;

    public CustomerManager CustomerManager
    {
        get
        {
            if (customerManager == null)
                customerManager = new CustomerManager(this);

            return customerManager;
        }
    }

    CustomerCall original_CustomerCall;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string callNumber = (CustomerManager.GetCustomerCallsCount(Company.CompanyId, null, null, DateTimeInterval.ThisYear, "", 0, Int32.MaxValue) + 1).ToString().PadLeft(5, '0');
            txtCallNumber.Text = callNumber + "-" + DateTime.Now.Year.ToString().Substring(2);

            cboCustomerCallStatus.DataBind();
            cboCustomerCallType.DataBind();

            cboCustomerCallStatus.SelectedValue = CustomerCallStatus.Opened.ToString();

            if (IsRestricted)
                btnGenerateServiceOrder.Visible = txtCallNumber.Enabled = tblAdditionalInformation.Visible = false;

            if (!String.IsNullOrEmpty(Request["CustomerCallId"]))
            {
                Page.ViewState["CustomerCallId"] = Convert.ToInt32(Request["CustomerCallId"].DecryptFromHex());

                ShowCustomerCall();

                if (Convert.ToBoolean(Request["ReadOnly"]))
                {
                    lblDescription.Text = txtDescription.Value;
                    lblDescription.Visible = true;
                    txtDescription.Visible = false;
                    txtSubject.Enabled = false;
                    cboCustomerCallType.Enabled = false;
                    cboTechnicalEmployee.Enabled = false;
                    rtnPriority.ReadOnly = true;
                }
            }

            if (!String.IsNullOrEmpty(Request["ModalPopUp"]))
            {
                Page.ViewState["ModalPopUp"] = Request["ModalPopUp"];
                cboTechnicalEmployee.Visible = litTechnical.Visible = false;
                cboRepresentant.Visible = litRepresentant.Visible = false;
                litPriority.Visible = rtnPriority.Visible = false;
            }
        }
    }
    public Boolean IsRestricted
    {
        get
        {
            return Convert.ToInt32(Request["ModalPopUp"]) == 1;
        }
    }

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        Page.ViewState["CustomerId"] = e.Customer.CustomerId;
        cboCustomerEquipments.Items.Clear();
        if (e.Customer.CustomerEquipments.Any())
        {
            cboCustomerEquipments.Items.Add(new ListItem("", ""));
            cboCustomerEquipments.DataSource = new CustomerManager(this).GetCustomerEquipments(Company.CompanyId, e.Customer.CustomerId);
            cboCustomerEquipments.DataBind();
        }
    }

    protected void odsTechnicalUsers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }


    protected void odsRepresentant_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.ViewState["CustomerCallId"] == null && CustomerManager.GetCustomerCall(Company.CompanyId, txtCallNumber.Text) != null)
        {
            ShowError("Número de chamado já existente!");
            return;
        }

        SaveCustomerCall();
        Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "CloseModal",
                    "top.$.LightBoxObject.close();" +
                    "top.content.location.href+='?';",
                    true);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (IsRestricted)
        {
            Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "CloseModal",
                    "top.$.LightBoxObject.close();" +
                    "top.content.location.href+='?';",
                    true);
            return;
        }
        Response.Redirect("CustomerCalls.aspx");
    }

    protected void btnSearchEmployee_Click(object sender, EventArgs e)
    {
        if (Convert.ToDateTime(txtInitialTime.Text) > DateTime.MinValue.Sql2005MinValue().Date && Convert.ToDateTime(txtEndTime.Text) > DateTime.MinValue.Sql2005MinValue().Date)
        {
            LoadAvailableTechnicalEmployee();
            cboTechnicalEmployee.Attributes["Style"] = "display:block";
            pnlShowTechnical.Attributes["Style"] = "display:none";

        }
        else
            ShowError(Resources.Exception.InvalidDate);
    }

    protected void btnGenerateServiceOrder_Click(object sender, EventArgs e)
    {
        Page.ViewState["SO"] = "ServiceOrder";
        SaveCustomerCall();
    }

    #region functions

    /// <summary>
    /// this method show data of customerCall
    /// </summary>
    /// <param name="customerCall"></param>
    public void ShowCustomerCall()
    {
        var customerCall = CustomerManager.GetCustomerCall((Int32)Page.ViewState["CustomerCallId"]);

        CommentsCustomerCall.SubjectId = customerCall.CustomerCallId;
        CommentsCustomerCall.Visible = true;

        String userName = "Não Identificado";
        if (customerCall.User != null)
            userName = customerCall.User.Name;

        lblCustomerName.Text = "Usuário: <br/> <b>" + userName + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        litCreateDate.Text = "Data de Abertura: <br> <b>" + customerCall.OpenedDate + "</b>";
               
        cboCustomerEquipments.DataSource = customerCall.Customer.CustomerEquipments;
        cboCustomerEquipments.DataBind();

        if (customerCall.CustomerEquipmentId.HasValue)
            cboCustomerEquipments.SelectedValue = customerCall.CustomerEquipmentId.ToString();
               
        cboCustomerCallType.SelectedValue = customerCall.CustomerCallTypeId.ToString();

        selCustomer.ShowCustomer(customerCall.Customer);

        txtCallNumber.Text = customerCall.CallNumber;
        txtCallNumberAssociated.Text = customerCall.CallNumberAssociated;
        txtSector.Text = customerCall.Sector;

        if (customerCall.EventId.HasValue)
            txtDescription.Value = customerCall.Event.Message;
        else
            txtDescription.Value = customerCall.Description;

        if (customerCall.RepresentantId.HasValue)
            cboRepresentant.SelectedValue = customerCall.RepresentantId.ToString();

        txtSubject.Text = customerCall.Subject;

        if (customerCall.Rating.HasValue)
            rtnPriority.CurrentRating = Convert.ToInt32(customerCall.Rating);

        if (customerCall.CustomerCallStatusId.HasValue)
            cboCustomerCallStatus.SelectedValue = customerCall.CustomerCallStatusId.ToString();

        if (customerCall.TechnicalEmployeeId.HasValue)
        {
            cboTechnicalEmployee.DataBind();
            cboTechnicalEmployee.SelectedValue = customerCall.TechnicalEmployeeId.ToString();
        }
                   
    }

    /// <summary>
    /// this method return the available employee 
    /// </summary>
    private void LoadAvailableTechnicalEmployee()
    {
        var employeeManager = new HumanResourcesManager(this);
        cboTechnicalEmployee.Items.Clear();
        cboTechnicalEmployee.DataSource = employeeManager.GetAvailableTechnicalAsDataTable(Company.CompanyId, Convert.ToDateTime(txtInitialTime.Text), Convert.ToDateTime(txtEndTime.Text));
        cboTechnicalEmployee.Items.Add("");
        cboTechnicalEmployee.DataBind();
    }

    private void SaveCustomerCall()
    {
        CustomerCall customerCall = new CustomerCall();
        CompanyManager companyManager = new CompanyManager(this);
        Comment comment = new Comment();

        Int32 tmp = 0;

        if (this.Page.ViewState["CustomerCallId"] == null)
            customerCall.UserId = User.Identity.UserId;

        if (Int32.TryParse(Convert.ToString(Page.ViewState["CustomerCallId"]), out tmp))
        {
            original_CustomerCall = CustomerManager.GetCustomerCall(Convert.ToInt32(Page.ViewState["CustomerCallId"]));
            customerCall.CopyPropertiesFrom(original_CustomerCall);
        }


        // if this page was opened in PopUp way, save this customerCall as Host's customerCall

        customerCall.CompanyId = Company.CompanyId;

        if (Page.ViewState["ModalPopUp"] == null)
            if (!Int32.TryParse(Convert.ToString(Page.ViewState["CustomerId"]), out tmp))
            {
                ShowError(Resources.Exception.UnselectedCustomer);
                return;
            }

        customerCall.CustomerId = Convert.ToInt32(Page.ViewState["CustomerId"]);

        if (Convert.ToInt32(Page.ViewState["ModalPopUp"]) == 1)
        {
            customerCall.CustomerId = CustomerManager.GetHostCustomerByLegalEntityProfileId(Company.LegalEntityProfileId).CustomerId;
            customerCall.CompanyId = HostCompany.CompanyId;
        }

        customerCall.RepresentantId = null;
        if (!String.IsNullOrEmpty(cboRepresentant.SelectedValue))
            customerCall.RepresentantId = Convert.ToInt32(cboRepresentant.SelectedValue);

        customerCall.Rating = rtnPriority.CurrentRating;
        customerCall.CustomerCallStatusId = Convert.ToInt32(cboCustomerCallStatus.SelectedValue);
        customerCall.CustomerCallTypeId = Convert.ToInt32(cboCustomerCallType.SelectedValue);
        customerCall.CallNumber = txtCallNumber.Text;
        customerCall.CallNumberAssociated = txtCallNumberAssociated.Text;

        customerCall.Sector = txtSector.Text;
        customerCall.Description = txtDescription.Value;
        customerCall.Subject = txtSubject.Text;

        if (!String.IsNullOrEmpty(cboTechnicalEmployee.SelectedValue))
            customerCall.TechnicalEmployeeId = Convert.ToInt32(cboTechnicalEmployee.SelectedValue);

        if (!String.IsNullOrEmpty(cboCustomerEquipments.SelectedValue))
            customerCall.CustomerEquipmentId = Convert.ToInt32(cboCustomerEquipments.SelectedValue);

        if (original_CustomerCall != null)
        {
            customerCall.ModifiedByUser = User.Identity.UserName;
            CustomerManager.UpdateCustomerCall(original_CustomerCall, customerCall);
        }
        else
        {
            customerCall.CreatedByUser = User.Identity.UserName;
            CustomerManager.InsertCustomerCall(customerCall, null, null, null);
            CommentsCustomerCall.SubjectId = customerCall.CustomerCallId;
        }
        Context.Items["CustomerCallId"] = customerCall.CustomerCallId;

        if (Page.ViewState["SO"] != null)
        {
            Server.Transfer("../Services/ServiceOrder.aspx");
            return;
        }

        if (Page.ViewState["ModalPopUp"] == null)
            Response.Redirect("CustomerCalls.aspx");
    }

    #endregion

    protected void odsTechnicalEmployee_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}
