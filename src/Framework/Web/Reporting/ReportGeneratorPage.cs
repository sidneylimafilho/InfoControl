using System;
using System.Web.UI.WebControls;
using InfoControl.Web.Reporting.DataClasses;
using InfoControl.Web.UI;

namespace InfoControl.Web.Reporting
{
    public enum ReportSteps
    {
        Start = 0,
        MatrixRows,
        Columns,
        Filter,
        Sort,
        Finish
    }

    public class ReportGeneratorPage : DataPage
    {
        #region Properties

        private ReportsManager manager;
        private ReportSettings settings;
        private Wizard wizard;

        public Wizard Wizard
        {
            get { return wizard; }
            set { wizard = value; }
        }

        /// <summary> Contains the user _report settings, before of save
        /// </summary>
        public ReportSettings Settings
        {
            get
            {
                if (settings == null)
                    settings = Session["ReportSettings"] as ReportSettings ?? new ReportSettings();
                return settings;
            }
            set { settings = value; }
        }

        /// <summary> Manages the configuration, persistence, etc.
        /// </summary>
        public ReportsManager ReportManager
        {
            get
            {
                if (manager == null) manager = new ReportsManager(this);
                return manager;
            }
            set { manager = value; }
        }

        /// <summary> Get the active step
        /// </summary>
        public ReportSteps ActiveStep
        {
            get { return (ReportSteps)wizard.ActiveStepIndex; }
        }

        /// <summary> Get the next step
        /// </summary>
        public ReportSteps NextStep
        {
            get { return (ReportSteps)wizard.ActiveStepIndex + 1; }
        }

        /// <summary>
        /// Get the prior step
        /// </summary>
        public ReportSteps PriorStep
        {
            get { return (ReportSteps)wizard.ActiveStepIndex - 1; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ReportSettings"] = null;
                settings = null;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Session["ReportSettings"] = Settings;
        }

        public void LoadSettings(int reportId)
        {
            Settings.Report = ReportManager.GetReport(reportId);
        }

        #region Save Statements

        public void SaveReportSettings()
        {
            SaveReportTable();
            SaveReportColumns();
            SaveReportFilter();
            SaveReportSort();
        }

        private void SaveReportTable()
        {
            ReportManager.Insert(Settings.Report);
        }

        private void SaveReportColumns()
        {
            foreach (ReportColumn column in Settings.Columns)
            {
                column.ReportId = Settings.Report.ReportId;
                column.IsMatrix = false;
                ReportManager.Insert(column);
            }

            foreach (ReportColumn column in Settings.MatrixRows)
            {
                column.ReportId = Settings.Report.ReportId;
                column.IsMatrix = true;
                ReportManager.Insert(column);
            }
        }

        private void SaveReportFilter()
        {
            foreach (ReportFilter filterColumn in Settings.Filters)
            {
                filterColumn.ReportId = Settings.Report.ReportId;
                ReportManager.Insert(filterColumn);
            }
        }

        private void SaveReportSort()
        {
            foreach (ReportSort sortColumn in Settings.SortedColumns)
            {
                sortColumn.ReportId = Settings.Report.ReportId;
                ReportManager.Insert(sortColumn);
            }
        }

        #endregion
    }
}