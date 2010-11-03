using System;
using System.Web.UI;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

using Vivina.Erp.SystemFramework;

[SupportsEventValidation]
[ValidationProperty("EmployeeId")]
[ControlValueProperty("EmployeeId")]
public partial class App_Shared_SelectEmployee : Vivina.Erp.SystemFramework.UserControlBase
{
    #region Fields

    private Employee _employee;
    private HumanResourcesManager _humanResourcesManager;

    #endregion

    #region Events

    public event EventHandler<SelectingEmployeeEventArgs> SelectingEmployee;
    public event EventHandler<SelectedEmployeeEventArgs> SelectedEmployee;

    protected void Page_Load(object sender, EventArgs e)
    {
        

        txtEmployee.Enabled = Enabled;
        _humanResourcesManager = new HumanResourcesManager(this);
    }

    protected void txtEmployee_TextChanged(object sender, EventArgs e)
    {
        onSelectingEmployee(this, new SelectingEmployeeEventArgs
                                  {
                                      EmployeeName = txtEmployee.Text
                                  });

        if (txtEmployee.Text.Contains("|"))
        {
            string identification = txtEmployee.Text.Split('|')[0].Trim();

            _employee = _humanResourcesManager.RetrieveEmployeeByCpf(Page.Company.CompanyId, identification);
            ShowEmployee(_employee);
        }
    }

    protected void onSelectingEmployee(object sender, SelectingEmployeeEventArgs e)
    {
        if (SelectingEmployee != null)
            SelectingEmployee(sender, e);
    }

    protected void onSelectedEmployee(object sender, SelectedEmployeeEventArgs e)
    {
        if (SelectedEmployee != null)
            SelectedEmployee(sender, e);
    }

    #endregion

    #region Properties

    public Int32? EmployeeId
    {
        get { return (Int32?)ViewState["EmployeeId"]; }
        set
        {
            ViewState["EmployeeId"] = value;
            if (value.HasValue)
            {
                var employeeManager = new HumanResourcesManager(this);
                _employee = employeeManager.GetEmployee(Page.Company.CompanyId, value.Value);
                ShowEmployee(_employee);
            }
        }
    }

    public string Name
    {
        get { return txtEmployee.Text; }
        set { txtEmployee.Text = value; }
    }

    private bool _enabled = true;
    public Boolean Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    #endregion

    #region Methods

    public void ShowEmployee(Employee employee)
    {
        if (employee != null)
        {
            ViewState["EmployeeId"] = employee.EmployeeId;
            if (employee.Profile.Address != null)
            {
                lblEmployeeAddress.Text = employee.Profile.Address.Name
                                          + ", " + employee.Profile.AddressNumber
                                          + ", " + employee.Profile.AddressComp;

                lblEmployeeLocalization.Text = employee.Profile.Address.City
                                               + " - " + employee.Profile.Address.Neighborhood
                                               + ", " + employee.Profile.Address.StateId;
            }
            lblPostalCode.Text = employee.Profile.PostalCode;
            lnkEmployeeName.Text = employee.Profile.Name;
            lnkEmployeeName.Text += " / ";
            lblEmployeePhone.Text = "Tel: " + employee.Profile.Phone.Replace("(__)____-____", "");
            lblCPF.Text = employee.Profile.CPF;

            lnkEmployeeName.OnClientClick = "top.$.lightbox('RH/Employee.aspx?EmployeeId=" + employee.EmployeeId.EncryptToHex() + "&lightbox[iframe]=true');return;";
            pnlEmployee.Visible = true;
        }

        txtEmployee.Text = String.Empty;
        onSelectedEmployee(this, new SelectedEmployeeEventArgs
                                 {
                                     Employee = employee
                                 });
    }

    #endregion

}
public class SelectingEmployeeEventArgs : EventArgs
{
    public String EmployeeName { get; set; }
}

public class SelectedEmployeeEventArgs : EventArgs
{
    public Employee Employee { get; set; }
}