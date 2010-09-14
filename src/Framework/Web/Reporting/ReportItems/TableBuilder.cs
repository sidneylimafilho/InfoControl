using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using InfoControl.Data;

namespace InfoControl.Web.Reporting
{
    public class TableBuilder : IReportItemBuilder
    {
        DataClasses.ReportSettings _settings;

        public TableBuilder(DataClasses.ReportSettings settings)
        {
            _settings = settings;
        }

        public RdlSchema.TableType ToTable()
        {
            RdlSchema.TableType table = new RdlSchema.TableType();
            table.Name = "Table1";
            table.TableColumns = CreateTableColumns();
            table.Header = CreateHeader();
            table.Details = CreateDetails();
            table.Width = "7in";
            return table;
        }

        private RdlSchema.TableColumnsType CreateTableColumns()
        {
            RdlSchema.TableColumnsType tableColumns = new RdlSchema.TableColumnsType();
            for (int i = 0; i < _settings.Columns.Count; i++)
            {
                RdlSchema.TableColumnType tableColumn = new RdlSchema.TableColumnType();
                tableColumn.Width = Math.Round(21m / _settings.Columns.Count) + "cm";
                tableColumns.Add(tableColumn);
            }
            return tableColumns;
        }

        private RdlSchema.HeaderType CreateHeader()
        {
            RdlSchema.HeaderType header = new RdlSchema.HeaderType();
            header.FixedHeader = true;
            header.RepeatOnNewPage = true;
            header.TableRows = new RdlSchema.TableRowsType();

            RdlSchema.TableRowType headerTableRow = new RdlSchema.TableRowType();
            headerTableRow.Height = "14pt";
            headerTableRow.TableCells = CreateHeaderTableCells();

            header.TableRows.TableRow.Add(headerTableRow);
            return header;
        }

        private RdlSchema.TableCellsType CreateHeaderTableCells()
        {
            RdlSchema.TableCellsType headerTableCells = new RdlSchema.TableCellsType();
            for (int i = 0; i < _settings.Columns.Count; i++)
            {
                headerTableCells.Add(CreateHeaderTableCell(_settings.Columns[i].Name));
            }
            return headerTableCells;
        }

        private RdlSchema.TableCellType CreateHeaderTableCell(string fieldName)
        {
            RdlSchema.TableCellType headerTableCell = new RdlSchema.TableCellType();
            headerTableCell.ReportItems = new RdlSchema.ReportItemsType();

            RdlSchema.TextboxType headerTableCellTextbox = new RdlSchema.TextboxType();
            headerTableCellTextbox.Name = fieldName.RemoveSpecialChars() + "_Header";
            headerTableCellTextbox.Value = fieldName;
            headerTableCellTextbox.Style = HeaderStyle;
            headerTableCellTextbox.Style.FontWeight = "700";
            headerTableCellTextbox.CanGrow = false;
            headerTableCellTextbox.CanShrink = true;
            headerTableCell.ReportItems.Items.Add(headerTableCellTextbox);
            return headerTableCell;
        }

        private RdlSchema.DetailsType CreateDetails()
        {
            RdlSchema.DetailsType details = new RdlSchema.DetailsType();
            details.TableRows = new RdlSchema.TableRowsType();

            RdlSchema.TableRowType tableRow = new RdlSchema.TableRowType();
            tableRow.TableCells = CreateDetailsTableCells();
            tableRow.Height = "11pt";

            details.TableRows.TableRow.Add(tableRow);
            return details;
        }

        private RdlSchema.TableCellsType CreateDetailsTableCells()
        {
            RdlSchema.TableCellsType tableCells = new RdlSchema.TableCellsType();
            for (int i = 0; i < _settings.Columns.Count; i++)
            {
                tableCells.Add(CreateDetailsTableCell(_settings.Columns[i].Name));
            }
            return tableCells;
        }

        private RdlSchema.TableCellType CreateDetailsTableCell(string fieldName)
        {
            RdlSchema.TableCellType tableCell = new RdlSchema.TableCellType();
            tableCell.ReportItems = new RdlSchema.ReportItemsType();

            RdlSchema.TextboxType textbox = new RdlSchema.TextboxType();
            textbox.Name = fieldName.RemoveSpecialChars();
            textbox.Value = "=Fields(\"" + fieldName.RemoveSpecialChars() + "\").Value";            
            textbox.CanGrow = false;
            textbox.Style = RowsStyle;
            textbox.Style.BackgroundColor = "=iif(RowNumber(Nothing) mod 2, \"AliceBlue\", \"White\")";
            textbox.Style.TextAlign = "Left";                        
            
            tableCell.ReportItems.Items.Add(textbox);

            return tableCell;
        }

    }
}
