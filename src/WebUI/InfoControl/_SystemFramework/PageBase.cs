using System;
using System.Linq;
using System.Web.UI;
using InfoControl;
using InfoControl.Data;
using InfoControl.Web.UI;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


namespace Vivina.Erp.SystemFramework
{
    public class PageBase : DataPage
    {
        private Company _company;
        private Deposit _deposit;
        private Employee _employee;
        private Company _hostCompany;

        private string _htmlAlertInfo;
        private Plan _plan;

        private HumanResourcesManager _humanResourcesManager;


        public Company Company
        {
            get
            {
                if (_company == null)
                {
                    _company = User.IsAuthenticated
                                   ? (Company)Session["_company"]
                                   : (Company)Session["_siteCompany"];

                    if (_company != null && !String.IsNullOrEmpty(Request["cid"]) && _company.CompanyId != Convert.ToInt32(Request["cid"]))
                        Session["_company"] = Session["_siteCompany"] = _company = null;

                    if (_company == null)
                    {
                        var manager = new CompanyManager(this);
                        _company = manager.GetCompanyByContext(Context);

                        if (User.IsAuthenticated)
                        {
                            Session["_company"] = _company;
                            Session["_siteCompany"] = null;
                        }
                        else
                        {
                            Session["_company"] = null;
                            Session["_siteCompany"] = _company;
                        }
                    }
                }
                return _company;
            }
        }

        public Plan Plan
        {
            get
            {
                if (_plan == null)
                {
                    if (Session["_plan"] == null)
                    {

                        var planManager = new PlanManager(this);
                        Session["_plan"] = planManager.GetCurrentPlan(Company.CompanyId);
                    }
                    _plan = Session["_plan"] as Plan;
                }
                return _plan;
            }
        }

        public Employee Employee
        {
            get
            {
                if (_employee == null)
                {
                    if (Session["_employee"] == null)
                    {
                        _humanResourcesManager = new HumanResourcesManager(this);
                        Session["_employee"] = _humanResourcesManager.GetEmployeeByUser(User.Identity, Company.CompanyId);
                    }
                    _employee = Session["_employee"] as Employee;
                }
                return _employee;
            }
        }

        public Deposit Deposit
        {
            get
            {
                if (_deposit != null)
                    return _deposit;

                if (Session["_deposit"] != null)
                    return _deposit = (Session["_deposit"] as Deposit);

                if (User.Identity.UserId > 0)
                {
                    var manager = new CompanyManager(this);
                    Session["_deposit"] = manager.GetCurrentDeposit(User.Identity.UserId, Company.CompanyId).Detach();
                }
                else
                {
                    Session["_deposit"] = Company.Deposits.FirstOrDefault().Detach();
                }

                _deposit = Session["_deposit"] as Deposit;

                return _deposit;
            }
            set { Session["_deposit"] = value; }
        }

        public Company HostCompany
        {
            get
            {
                if (_hostCompany == null)
                {
                    if (Session["_hostCompany"] == null)
                    {
                        var manager = new CompanyManager(this);
                        Session["_hostCompany"] = manager.GetHostCompany();
                    }
                    _hostCompany = Session["_hostCompany"] as Company;
                }
                return _hostCompany;
            }
        }

        public override string StyleSheetTheme
        {
            get
            {
                if (User.IsAuthenticated && Company != null && Company.Theme != null)
                    return Company.Theme;

                return base.StyleSheetTheme;
            }
            set { base.StyleSheetTheme = value; }
        }

        #region Refresh

        public void RefreshCompany()
        {
            _company = null;
            Session["_company"] = null;
        }

        public void RefreshPlan()
        {
            _plan = null;
            Session["_plan"] = null;
        }

        public void RefreshDeposit()
        {
            _deposit = null;
            Session["_deposit"] = null;
        }

        public void RefreshCredentials()
        {
            User.RefreshCredentials();
            RefreshCompany();
            RefreshPlan();
            RefreshDeposit();
        }

        #endregion

        #region AlertInfo

        /// <summary>
        /// Register the animation scripts for info
        /// </summary>
        public void RegisterAnimationAlertInfo()
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "RegisterAnimationAlertInfo", @"
            var sequence = new top.AjaxControlToolkit.Animation.SequenceAnimation($get('AlertMsg'), null, null, null, 1);                      
            sequence.add(new top.AjaxControlToolkit.Animation.FadeOutAnimation($get('AlertMsg'), 0.3, null, 1, 0, false));                                
            sequence.add(new top.AjaxControlToolkit.Animation.FadeInAnimation($get('AlertMsg'), 0.3, null, 1, 0, false));  
            
            sequence.add(new top.AjaxControlToolkit.Animation.FadeOutAnimation($get('AlertMsg'), 0.3, null, 1, 0, false));                                
            sequence.add(new top.AjaxControlToolkit.Animation.FadeInAnimation($get('AlertMsg'), 0.3, null, 1, 0, false));            
            
            sequence.add(new top.AjaxControlToolkit.Animation.FadeOutAnimation($get('AlertMsg'), 0.3, null, 1, 0, false));                                
            sequence.add(new top.AjaxControlToolkit.Animation.FadeInAnimation($get('AlertMsg'), 0.3, null, 1, 1, false));            
            sequence.play();", true);
        }

        /// <summary>
        /// Show the AJAX alert in the window
        /// </summary>
        /// <param name="message"></param>
        public void ShowAlert(string message)
        {
            _htmlAlertInfo = "<span id='AlertMsg' class='cAlert11'><label>" + message + "</label><span></span></span>";
            ClientScript.RegisterStartupScript(GetType(), "ShowAlert", _htmlAlertInfo);
            RegisterAnimationAlertInfo();
        }

        /// <summary>
        /// Show the AJAX Error label in the window
        /// </summary>
        /// <param name="message"></param>
        public void ShowError(string message)
        {
            _htmlAlertInfo = "document.write(\"<span id='AlertMsg' class='cError11'><label>" + message + "</label><span></span></span>\");";
            ScriptManager.RegisterStartupScript(Page, GetType(), "ShowError", _htmlAlertInfo, true);
            RegisterAnimationAlertInfo();
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //
            // Set in session to Vivina.Erp.SystemFramework.CustomerCallNotifier get when error occurs
            //
            Session["_lastPageTitle"] = Title;

            //
            // Repair the prior Context RewritePath that changes Form.Action causing validation MAC errors
            //
            //this.Context.RewritePath(Request.RawUrl);
        }
    }

    public class PageBase<T> : PageBase where T : BusinessManager
    {
        private T _manager;

        public T Manager
        {
            get { return _manager ?? (_manager = (T)Activator.CreateInstance(typeof(T), new object[] { this })); }
        }
    }
}