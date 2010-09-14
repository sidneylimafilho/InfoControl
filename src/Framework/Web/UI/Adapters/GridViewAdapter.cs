using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;

namespace InfoControl.Web.UI.Adapters
{
    public class GridViewAdapter : WebControlAdapter
    {
        private GridView gridView;
        private bool isRowSelectable;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            gridView = Control as GridView;

            //
            // Ensure Security
            //          
            gridView.EnsureSecurity();

            //
            // set the grid view effectgs
            //            
            gridView.RowDataBound += GridViewEffects;
            gridView.RowDataBound += GridViewSortEffect;
            gridView.RowDataBound += GridViewRemoveButtons;
            gridView.RowDataBound += GridViewInsertRowSelectScript;
            Page.ClientScript.RegisterClientScriptResource(GetType(),
                                                           "InfoControl.Web.UI.Adapters.GridViewEffects.js");

            string rowSelectableText = gridView.Attributes["RowSelectable"];
            isRowSelectable = String.IsNullOrEmpty(rowSelectableText)
                                  ? true
                                  : Convert.ToBoolean(rowSelectableText);
        }

        protected void GridViewInsertRowSelectScript(object sender, GridViewRowEventArgs e)
        {
            if (isRowSelectable && e.Row.RowType == DataControlRowType.DataRow &&
                ((e.Row.RowState & DataControlRowState.Edit) != DataControlRowState.Edit))
            {
                var postOptions = new PostBackOptions(gridView, "Select$" + e.Row.RowIndex);
                String selectScript = gridView.Page.ClientScript.GetPostBackEventReference(postOptions);

                //
                // This for not uses equal, to not overbound the index
                //
                //for (int a = 0; a < e.Row.Cells.Count-1; a++)
                //{
                //    if (gridView.Columns[a].SortExpression != "Insert")
                //        e.Row.Cells[a].Attributes.Add("onclick", selectScript);
                //}
                e.Row.Attributes.Add("onclick", selectScript);

                //foreach (TableCell cell in e.Row.Cells)
                //{
                //    int i = e.Row.Cells.GetCellIndex(cell);
                //    if (gridView.Columns[i].GetType().Name != "CommandField")
                //        if (e.Row.RowType == DataControlRowType.DataRow)
                //            e.Row.Cells[a].Attributes.Add("onclick", selectScript);
                //}
            }
        }

        protected void GridViewRemoveButtons(object sender, GridViewRowEventArgs e)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                int i = e.Row.Cells.GetCellIndex(cell);

                if (!gridView.Enabled)
                    if (gridView.Columns[i].GetType().Name == "CommandField")
                        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                            cell.Visible = false;

                cell.Visible = gridView.Columns[i].Visible;
                if (cell is DataControlFieldCell)
                {
                    DataControlField field = (cell as DataControlFieldCell).ContainingField;
                    cell.MergeStyle(field.ItemStyle);
                }
            }
        }

        protected void GridViewSortEffect(object sender, GridViewRowEventArgs e)
        {
            var gridView = (GridView) sender;

            if (gridView.SortExpression.Length > 0)
            {
                int cellIndex = -1;
                //  find the column index for the sorresponding sort expression
                foreach (DataControlField field in gridView.Columns)
                    if (field.SortExpression == gridView.SortExpression)
                    {
                        cellIndex = gridView.Columns.IndexOf(field);
                        break;
                    }

                if (cellIndex > -1)
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        //  this is a header row,
                        //  set the sort style
                        e.Row.Cells[cellIndex].CssClass +=
                            (gridView.SortDirection == SortDirection.Ascending
                                 ? " SelectedColumnAsc"
                                 : " SelectedColumnDesc");
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //  this is a data row
                        e.Row.Cells[cellIndex].CssClass +=
                            (e.Row.RowIndex%2 == 0
                                 ? " SelectedColumnAltRow"
                                 : " SelectedColumnRow");
                    }
            }
        }

        protected void GridViewEffects(object sender, GridViewRowEventArgs e)
        {
            //
            //Verify the row type 
            //
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) != DataControlRowState.Edit)
                {
                    //Add atributes for line
                    e.Row.Attributes.Add("onmouseover", "DataRow_MouseOver(this)");
                    e.Row.Attributes.Add("onmouseout", "DataRow_MouseOut(this)");

                    if (isRowSelectable)
                        e.Row.Attributes["onclick"] += "DataRow_Click(this)";
                }

                e.Row.Cells[e.Row.Cells.Count - 1].Attributes["Align"] = "center";

                if (!Control.Enabled)
                    foreach (TableCell cell in e.Row.Cells)
                        cell.Attributes["onclick"] = "";
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Control.Controls.Count > 0)
                foreach (Control row in Control.Controls[0].Controls)
                {
                    row.ApplyStyleSheetSkin(Control.Page);
                    row.RenderControl(writer);
                }
            else
                base.RenderContents(writer);
        }
    }
}