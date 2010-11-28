using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using InfoControl.Runtime;

using InfoControl.Web.UI;
using InfoControl.Web.Reporting;
using InfoControl.Web.Reporting.RdlSchema;


public partial class ReportViewer : InfoControl.Web.UI.DataPage
{
    StyleType rowsStyle = new StyleType();
    StyleType headerStyle = new StyleType();


    protected void Page_Load(object sender, EventArgs e)
    {
        BlockContextMenu = false;
        DataTable table;
        Report report;

        CreateReportStyles();

        ProcessReportSettings(out table, out  report);

        ConfigureReportPage(report);

        ReportViewer1.Style.Add(HtmlTextWriterStyle.Display, "");
        ReportViewer1.LocalReport.LoadReportDefinition(report.SerializeToXmlInStream());
        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("MyData", table));
    }

    /// <summary>
    /// Create the Report Styles same CSS funcionality
    /// </summary>
    private void CreateReportStyles()
    {
        headerStyle.Color = "#000000";
        headerStyle.BackgroundColor = "#EEEEEE";
        headerStyle.FontWeight = "700";

        rowsStyle.Color = "#444444";

        rowsStyle.FontFamily = headerStyle.FontFamily = "Tahoma";
        rowsStyle.FontSize = headerStyle.FontSize = "8pt";
        rowsStyle.Format = headerStyle.Format = "f0";
        rowsStyle.BorderColor.Default = headerStyle.BorderColor.Default = "#777777";
        rowsStyle.BorderStyle.Default = headerStyle.BorderStyle.Default = "Solid";
        rowsStyle.BorderWidth.Default = headerStyle.BorderWidth.Default = "1pt";
        rowsStyle.TextAlign = "Center";
        headerStyle.TextAlign = "Left";
        headerStyle.PaddingTop = headerStyle.PaddingRight = headerStyle.PaddingBottom = headerStyle.PaddingLeft = "2pt";
    }

    /// <summary>
    /// Get the Report Definition been Dynamic or Static
    /// </summary>
    /// <returns></returns>
    private void ProcessReportSettings(out DataTable table, out Report report)
    {
        //
        // Dynamic
        //
        //if (Page.PreviousPage is ReportGeneratorPage)
        //{
        InfoControl.Web.Reporting.DataClasses.ReportSettings settings = Session["ReportSettings"] as InfoControl.Web.Reporting.DataClasses.ReportSettings;

        //
        // Create Report Generator
        //
        ReportDefinitionBuilder generator = new ReportDefinitionBuilder(settings);



        //
        // Create Table
        //
        if (settings.MatrixRows.Count > 0)
        {
            MatrixBuilder matrixBuilder = new MatrixBuilder(settings);
            matrixBuilder.RowsStyle = rowsStyle;
            matrixBuilder.HeaderStyle = headerStyle;
            generator.ReportItems.Add(matrixBuilder.ToMatrix());
        }
        else
        {
            TableBuilder tableBuilder = new TableBuilder(settings);
            tableBuilder.RowsStyle = rowsStyle;
            tableBuilder.HeaderStyle = headerStyle;
            generator.ReportItems.Add(tableBuilder.ToTable());
        }


        //
        // Create the dataTable 
        //
        table = new ReportsManager(this).ExecuteReport(settings);


        report = generator.Report;
        //}
        //else
        //{
        //    table = new DataTable();
        //    string fileName = Server.MapPath("../Static/") + Request["r"];
        //    report = System.IO.File.ReadAllText(fileName).DeserializeFromXml<Report>();
        //}
    }

    /// <summary>
    /// Set the report margin, width, height, etc
    /// </summary>
    /// <param name="report"></param>
    private void ConfigureReportPage(Report report)
    {
        report.Width = "16cm";
        report.InteractiveHeight = "29.7cm";
        report.InteractiveWidth = "21cm";
        report.RightMargin = "2.5cm";
        report.LeftMargin = "2.5cm";
        report.TopMargin = "2.5cm";
        report.BottomMargin = "2.5cm";
        report.PageWidth = "21cm";
        report.PageHeight = "29.7cm";

        //CreateReportHeader(report);

        //CreateReportFooter(report);
    }

    /// <summary>
    /// Create the Report Header that contains a Company Logo, Title, Execution Date, etc...
    /// </summary>
    /// <returns></returns>
    private void CreateReportHeader(Report report)
    {
        report.PageHeader = new PageHeaderFooterType();

        //
        // Configure report Header
        //
        RectangleType rectangle = new RectangleType();
        rectangle.Left = "0cm";
        rectangle.Top = "0cm";
        rectangle.Height = "5cm";

        report.PageHeader.ReportItems.Add(rectangle);
    }

    /// <summary>
    /// Create the Report Header that contains a Company Logo, Title, Execution Date, etc...
    /// </summary>
    /// <returns></returns>
    private void CreateReportFooter(Report report)
    {
        report.PageFooter = new PageHeaderFooterType();

        //
        // Configure report Footer
        //
        RectangleType rectangle = new RectangleType();
        rectangle.Left = "0cm";
        rectangle.Top = "0cm";
        rectangle.Height = "3cm";

        report.PageFooter.ReportItems.Add(rectangle);
    }
}
