using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Linq;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace InfoControl.Web.Reporting.DataClasses
{

    [Serializable]
    public sealed class ReportSettings
    {
        #region Properties
        private DataClasses.Report _report;
        public DataClasses.Report Report
        {
            get
            {
                if (_report == null)
                {
                    _report = new DataClasses.Report();
                }
                return _report;
            }
            set
            {
                _report = value;

                _columns = value.ReportColumns.Where(c => !c.IsMatrix).ToList();
                _filter = value.ReportFilters.ToList();
                _sort = value.ReportSorts.ToList();
                _matrixRows = value.ReportColumns.Where(c => c.IsMatrix).ToList();
            }
        }

        private List<ReportColumn> _matrixRows;
        public List<ReportColumn> MatrixRows
        {
            get
            {
                if (_matrixRows == null)
                {
                    _matrixRows = new List<ReportColumn>();
                }
                return _matrixRows;
            }
            set { _matrixRows = value; }
        }

        private List<ReportColumn> _columns;
        public List<ReportColumn> Columns
        {
            get
            {
                if (_columns == null)
                {
                    _columns = new List<ReportColumn>();
                }
                return _columns;
            }
            set { _columns = value; }
        }

        private List<ReportFilter> _filter;
        public List<ReportFilter> Filters
        {
            get
            {
                if (_filter == null)
                {
                    _filter = new List<ReportFilter>();
                }
                return _filter;
            }
            set { _filter = value; }
        }

        private List<ReportSort> _sort;
        public List<ReportSort> SortedColumns
        {
            get
            {
                if (_sort == null)
                {
                    _sort = new List<ReportSort>();
                }
                return _sort;
            }
            set { _sort = value; }
        }
        #endregion

        public void Clear()
        {
            _report = null;
            _columns = null;
            _filter = null;
            _sort = null;
        }
    }


    public partial class ReportColumn
    {
        private string _name;

        public string Name
        {
            get
            {
                if (this.ReportColumnsSchema != null)
                    return this.ReportColumnsSchema.Name;

                return _name;

            }
            set
            {
                _name = value;
            }
        }

        public string Width { get; set; }
    }
    public partial class ReportFilterType
    {
    }
    public partial class ReportFilter
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
    public partial class ReportSort
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

    }
}