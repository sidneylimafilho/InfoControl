
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace InfoControl.Web
{
    public class TableHelper : Table
    {
        public void AddCell(TableRow row, string cellText)
        {
            TableCell cell1 = new TableCell();
            cell1.Text = cellText;
            row.Cells.Add(cell1);
        }

        public void AddCell(TableRow row, string cellText, string hyperlink)
        {
            TableCell cell1 = new TableCell();
            HtmlAnchor anchor1 = new HtmlAnchor();
            anchor1.HRef = hyperlink;
            anchor1.InnerText = cellText;
            cell1.Controls.Add(anchor1);
            row.Cells.Add(cell1);
        }

        public void AddHeaderCell(TableHeaderRow row, string cellText)
        {
            TableHeaderCell cell1 = new TableHeaderCell();
            cell1.Text = cellText;
            row.Cells.Add(cell1);
        }

        public TableHeaderRow CreateHeaderRow()
        {
            return new TableHeaderRow();
        }

        public TableRow CreateRow()
        {
            return new TableRow();
        }

        public string GetTableHtml()
        {
            StringWriter writer1 = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer1);
            this.RenderControl(writer2);
            return writer1.ToString();
        }

    }
}

