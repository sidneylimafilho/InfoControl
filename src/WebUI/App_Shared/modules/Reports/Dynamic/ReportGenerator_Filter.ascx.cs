using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using InfoControl.Web.Reporting;
using InfoControl.Web.Reporting.DataClasses;
using InfoControl.Web.UI;

public partial class ReportGenerator_Filter : ReportStepControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.NextStep == ReportSteps.Filter || Page.ActiveStep == ReportSteps.Filter)
        {
            int tableId = Page.Settings.Report.ReportTablesSchemaId ?? 0;
            bool isFirstTime = (ViewState["tableId"] == null && tableId != 0);
            bool isNewTableId = (ViewState["tableId"] != null && !ViewState["tableId"].Equals(tableId));

            if (isFirstTime || isNewTableId)
            {
                ViewState["tableId"] = tableId;
                BindColumnsSchema(tableId);
                HideAllPanels();
                if (cboColumns.Items.Count > 0)
                {
                    ShowFilterPanel(cboColumns.Items[0].Value);
                }
                BindSelectedFilters();

            }
        }
    }


    protected void cboColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
        HideAllPanels();
        ShowFilterPanel(cboColumns.SelectedValue);
    }


    protected void btnAdicionar_Click(object sender, ImageClickEventArgs e)
    {
        ReportFilter filter;

        //
        // Create the filter object that will persist
        //
        int reportColumnsSchemaId = int.Parse(cboColumns.SelectedValue.Split('|')[0]);
        int reportDataTypeId = int.Parse(cboColumns.SelectedValue.Split('|')[1]);

        //
        // If the filter is string, int or date then add, else read each item in chkFilterList
        //
        if ((DataType)reportDataTypeId == DataType.ForeignKey)
        {
            foreach (ListItem item in chkFilterList.Items)
            {
                if (item.Selected)
                {
                    filter = new ReportFilter();
                    filter.ReportTablesSchemaId = Page.Settings.Report.ReportTablesSchemaId.Value;
                    filter.ReportColumnsSchemaId = reportColumnsSchemaId;
                    //filter.ReportDataTypeId = reportDataTypeId;
                    filter.ReportFilterTypeId = Page.ReportManager.RetrieveFilterType("LIST").ReportFilterTypeId;
                    filter.Name = cboColumns.SelectedItem.Text +  Resources.Resource.CanBe + " '" + item.Text + "'";
                    filter.Value = item.Value;
                    Page.Settings.Filters.Add(filter);
                }
            }
        }
        else
        {
            filter = new ReportFilter();
            filter.ReportColumnsSchemaId = reportColumnsSchemaId;
            filter.ReportTablesSchemaId = Page.Settings.Report.ReportTablesSchemaId.Value;
            //filter.ReportDataTypeId = reportDataTypeId;
            filter.ReportFilterTypeId = int.Parse(lstFilters.SelectedValue);
            filter.Value = txtValue.Text;
            filter.Name = cboColumns.SelectedItem.Text + " " + lstFilters.SelectedItem.Text + " '" + txtValue.Text + "'";
            Page.Settings.Filters.Add(filter);
        }
        BindSelectedFilters();
        ClearControls();
    }

    protected void grdSelectedFilters_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Page.Settings.Filters.RemoveAt(e.RowIndex);
        BindSelectedFilters();
    }


    private void ClearControls()
    {
        txtValue.Text = "";
    }


    /// <summary>
    /// Populate all filters available for column data type
    /// </summary>
    /// <param name="dataType">column data type</param>
    private void BindFilters(DataType dataType)
    {
        lstFilters.DataSource = Page.ReportManager.RetrieveFilterTypes((int)dataType);
        lstFilters.DataBind();
    }

    /// <summary>
    /// Populate the CheckBox with the Related table from column foreign key
    /// </summary>
    /// <param name="tableId"></param>
    /// <param name="columnId"></param>
    private void BindFilterListItem(int tableId, int columnId)
    {
        ReportColumnsSchema column = Page.ReportManager.GetColumnSchema(tableId, columnId);
        chkFilterList.DataSource = Page.ReportManager.RetrieveFilterListItems(column);
        chkFilterList.DataTextField = column.PrimaryLabelColumn;
        chkFilterList.DataValueField = column.PrimaryKey;
        chkFilterList.DataBind();
    }


    /// <summary>
    /// Populate all filters user selected
    /// </summary>
    private void BindSelectedFilters()
    {
        grdSelectedFilters.DataSource = Page.Settings.Filters;
        grdSelectedFilters.DataBind();
    }

    /// <summary>
    /// Populate all columns available for table selected in step Columns
    /// </summary>
    /// <param name="tableId"></param>
    private void BindColumnsSchema(int tableId)
    {
        cboColumns.Items.Clear();
        foreach (ReportColumnsSchema column in Page.ReportManager.RetrieveColumnsSchema(tableId))
        {
            ListItem item = new ListItem();
            item.Text = column.Name;

            // Concatenate with ReportDataTypeId for retrieve in postback with out database access
            item.Value = column.ReportColumnsSchemaId + "|" + column.ReportDataTypeId;

            cboColumns.Items.Add(item);
        }
    }

    /// <summary>
    /// Hide All panels
    /// </summary>
    private void HideAllPanels()
    {
        pnlList.Visible = false;
        pnlFilter.Visible = false;
        ClearControls();
    }

    private void ShowFilterPanel(string selectedColumn)
    {
        int reportColumnsSchemaId = int.Parse("0" + selectedColumn.Split('|')[0]);
        DataType dataType = (DataType)int.Parse("0" + selectedColumn.Split('|')[1]);

        switch (dataType)
        {
            case DataType.String:
                validator.ValidationExpression = ".*";
                //txtValue.Mask = "";
                goto default;

            case DataType.Int32:
                validator.ValidationExpression = InfoControl.Text.RegularExpressions.RegexLib.Integer;
                validator.ErrorMessage = Resources.Resource.InvalidNumber;
                //txtValue.Mask = "";
                goto default;

            case DataType.DateTime:
                validator.ValidationExpression = InfoControl.Text.RegularExpressions.RegexLib.Date;
                validator.ErrorMessage = Resources.Resource.InvalidDate;
                goto default;

            case DataType.ForeignKey:
                BindFilterListItem(int.Parse(ViewState["tableId"].ToString()), reportColumnsSchemaId);
                pnlList.Visible = true;
                break;

            default:
                pnlFilter.Visible = true;
                BindFilters(dataType);
                break;
        }
    }

}
