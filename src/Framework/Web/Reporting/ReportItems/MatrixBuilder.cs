using System;
using System.Collections.Generic;
using System.Text;

using InfoControl.Data;
using InfoControl.Web.Reporting;

namespace InfoControl.Web.Reporting
{
    public class MatrixBuilder : IReportItemBuilder
    {
        #region Members
        private List<string> m_rowFields = new List<string>();
        private List<string> m_columnFields = new List<string>();
        private List<string> m_summarizedFields = new List<string>();
        private string m_rowHeight = "11pt";
        private string m_columnWidth = "2cm";
        private string m_left = "5pt";
        private string m_top = "5pt";

        private int m_groupingCount = 0;
        private int m_textboxCount = 0;
        private string m_matrixName = "Matrix1";

        DataClasses.ReportSettings _settings;


        #endregion

        #region ctor
        public MatrixBuilder(DataClasses.ReportSettings settings)
        {
            _settings = settings;

            foreach (DataClasses.ReportColumn column in settings.MatrixRows)
            {
                RowFields.Add(column.Name.RemoveAccent());
            }


            foreach (DataClasses.ReportColumn column in settings.Columns)
            {
                if (column.ReportDataTypeId != DataClasses.ReportDataType.Number)
                    ColumnFields.Add(column.Name);
                else
                    SummarizedFields.Add(column.Name);

            }

            if (SummarizedFields.Count == 0)
                SummarizedFields.Add("Qtd");

            //SummarizedFields.Add(settings.MatrixRows[0].Name);
        }
        #endregion

        #region Properties
        public List<string> RowFields
        {
            get { return m_rowFields; }
            set { m_rowFields = value; }
        }

        public List<string> ColumnFields
        {
            get { return m_columnFields; }
            set { m_columnFields = value; }
        }

        public List<string> SummarizedFields
        {
            get { return m_summarizedFields; }
            set { m_summarizedFields = value; }
        }

        #endregion

        #region Methods
        public RdlSchema.MatrixType ToMatrix()
        {
            RdlSchema.MatrixType matrix = new RdlSchema.MatrixType();
            matrix.Name = m_matrixName;
            matrix.ColumnGroupings = CreateColumnGroupings();
            matrix.RowGroupings = CreateRowGroupings();
            matrix.MatrixRows = CreateMatrixRows();
            matrix.MatrixColumns = CreateMatrixColumns();
            matrix.Corner = CreateCorner();
            matrix.Left = m_left;
            matrix.Top = m_top;

            return matrix;
        }

        private RdlSchema.CornerType CreateCorner()
        {
            RdlSchema.CornerType corner = new RdlSchema.CornerType();
            corner.ReportItems = CreateReportItems(new object[] { CreateTextbox("", null) });
            return corner;
        }

        private RdlSchema.ColumnGroupingsType CreateColumnGroupings()
        {
            RdlSchema.ColumnGroupingsType columnGroupings = new RdlSchema.ColumnGroupingsType();
            int columnGroupingCount = m_columnFields.Count;

            if (m_summarizedFields.Count > 1)
                columnGroupingCount++;

            columnGroupings.ColumnGrouping = new RdlSchema.ColumnGroupingType[columnGroupingCount];
            for (int i = 0; i < m_columnFields.Count; i++)
                columnGroupings.ColumnGrouping[i] = CreateDynamicColumnGrouping(m_columnFields[i]);

            if (m_summarizedFields.Count > 1)
            {
                int staticColumnsIndex = columnGroupings.ColumnGrouping.Length - 1;
                columnGroupings.ColumnGrouping[staticColumnsIndex] = CreateStaticColumnGrouping();
            }
            return columnGroupings;
        }

        private RdlSchema.ColumnGroupingType CreateStaticColumnGrouping()
        {
            RdlSchema.ColumnGroupingType columnGrouping = new RdlSchema.ColumnGroupingType();
            columnGrouping.Height = m_rowHeight;
            columnGrouping.StaticColumns = CreateStaticColumns();
            return columnGrouping;
        }

        private RdlSchema.StaticColumnsType CreateStaticColumns()
        {
            RdlSchema.StaticColumnsType staticColumns = new RdlSchema.StaticColumnsType();
            staticColumns.StaticColumn = new RdlSchema.StaticColumnType[m_summarizedFields.Count];
            for (int i = 0; i < m_summarizedFields.Count; i++)
            {
                staticColumns.StaticColumn[i] = CreateStaticColumn(m_summarizedFields[i]);
            }
            return staticColumns;
        }

        private RdlSchema.StaticColumnType CreateStaticColumn(string fieldName)
        {
            RdlSchema.StaticColumnType staticColumn = new RdlSchema.StaticColumnType();

            staticColumn.ReportItems = CreateReportItems(new object[] { CreateTextbox(fieldName, HeaderStyle) });
            return staticColumn;
        }

        private RdlSchema.ColumnGroupingType CreateDynamicColumnGrouping(string fieldName)
        {
            RdlSchema.ColumnGroupingType columnGrouping = new RdlSchema.ColumnGroupingType();
            columnGrouping.Height = m_rowHeight;
            columnGrouping.DynamicColumns = CreateDynamicColumnsRows(fieldName);
            return columnGrouping;
        }

        private RdlSchema.DynamicColumnsRowsType CreateDynamicColumnsRows(string fieldName)
        {
            RdlSchema.DynamicColumnsRowsType dynamicColumnsRows = new RdlSchema.DynamicColumnsRowsType();
            dynamicColumnsRows.Grouping = CreateGrouping(fieldName);
            dynamicColumnsRows.ReportItems = CreateReportItems(new object[] { CreateTextbox("=Fields(\"" + fieldName.RemoveSpecialChars() + "\").Value", HeaderStyle) });

            return dynamicColumnsRows;
        }



        private RdlSchema.BorderColorStyleWidthType CreateBorderColorStyleWidth(string s)
        {
            RdlSchema.BorderColorStyleWidthType b = new RdlSchema.BorderColorStyleWidthType();
            b.Default = s;
            return b;
        }

        private RdlSchema.GroupingType CreateGrouping(string fieldName)
        {
            RdlSchema.GroupingType grouping = new RdlSchema.GroupingType();
            grouping.Name = m_matrixName + "_Group" + (++m_groupingCount);
            grouping.GroupExpressions = CreateGroupExpressions(new string[] { fieldName });
            return grouping;
        }

        private RdlSchema.GroupExpressionsType CreateGroupExpressions(string[] fieldNames)
        {
            RdlSchema.GroupExpressionsType groupExpressions = new RdlSchema.GroupExpressionsType();
            groupExpressions.GroupExpression = new string[fieldNames.Length];
            for (int i = 0; i < fieldNames.Length; i++)
                groupExpressions.GroupExpression[i] = "=Fields(\"" + fieldNames[i].RemoveSpecialChars() + "\").Value";
            return groupExpressions;
        }

        private RdlSchema.ReportItemsType CreateReportItems(object[] reportItemArray)
        {
            RdlSchema.ReportItemsType reportItems = new RdlSchema.ReportItemsType();
            reportItems.Items = new System.Collections.ArrayList(reportItemArray);
            return reportItems;
        }

        private RdlSchema.TextboxType CreateTextbox(string expression, RdlSchema.StyleType style)
        {
            RdlSchema.TextboxType textbox = new RdlSchema.TextboxType();
            textbox.Name = "Textbox" + (++m_textboxCount);
            textbox.Value = expression;
            textbox.CanGrow = true;
            textbox.Style = style;
            textbox.CanShrink = true;
            return textbox;
        }

        private RdlSchema.RowGroupingsType CreateRowGroupings()
        {
            RdlSchema.RowGroupingsType rowGroupings = new RdlSchema.RowGroupingsType();
            rowGroupings.RowGrouping = new RdlSchema.RowGroupingType[m_rowFields.Count];
            for (int i = 0; i < m_rowFields.Count; i++)
                rowGroupings.RowGrouping[i] = CreateRowGrouping(m_rowFields[i]);
            return rowGroupings;
        }

        private RdlSchema.RowGroupingType CreateRowGrouping(string fieldName)
        {
            RdlSchema.RowGroupingType rowGrouping = new RdlSchema.RowGroupingType();
            rowGrouping.DynamicRows = CreateDynamicColumnsRows(fieldName);
            rowGrouping.Width = m_columnWidth;
            return rowGrouping;
        }

        private RdlSchema.MatrixRowsType CreateMatrixRows()
        {
            RdlSchema.MatrixRowsType matrixRows = new RdlSchema.MatrixRowsType();
            matrixRows.MatrixRow = CreateMatrixRow();
            return matrixRows;
        }

        private RdlSchema.MatrixRowType[] CreateMatrixRow()
        {
            RdlSchema.MatrixRowType[] matrixRow = new RdlSchema.MatrixRowType[1];
            matrixRow[0] = new RdlSchema.MatrixRowType();
            matrixRow[0].Height = m_rowHeight;
            matrixRow[0].MatrixCells = CreateMatrixCells();
            return matrixRow;
        }

        private RdlSchema.MatrixCellsType CreateMatrixCells()
        {
            RdlSchema.MatrixCellsType matrixCells = new RdlSchema.MatrixCellsType();
            matrixCells.MatrixCell = new RdlSchema.MatrixCellType[m_summarizedFields.Count];
            for (int i = 0; i < matrixCells.MatrixCell.Length; i++)
            {
                matrixCells.MatrixCell[i] = CreateMatrixCell(m_summarizedFields[i]);
            }
            return matrixCells;
        }

        private RdlSchema.MatrixCellType CreateMatrixCell(string fieldName)
        {
            RdlSchema.MatrixCellType matrixCell = new RdlSchema.MatrixCellType();
            string expression = "=Sum(CDbl(Fields!" + fieldName.RemoveSpecialChars() + ".Value))";
            matrixCell.ReportItems = CreateReportItems(new object[] { CreateTextbox(expression, RowsStyle) });
            return matrixCell;
        }



        private RdlSchema.MatrixColumnsType CreateMatrixColumns()
        {
            RdlSchema.MatrixColumnsType matrixColumns = new RdlSchema.MatrixColumnsType();
            matrixColumns.MatrixColumn = CreateMatrixColumn();
            return matrixColumns;
        }

        private RdlSchema.MatrixColumnType[] CreateMatrixColumn()
        {
            RdlSchema.MatrixColumnType[] matrixColumn = new RdlSchema.MatrixColumnType[m_summarizedFields.Count];
            for (int i = 0; i < matrixColumn.Length; i++)
            {
                matrixColumn[i] = new RdlSchema.MatrixColumnType();
                matrixColumn[i].Width = m_columnWidth;
            }
            return matrixColumn;
        }

        #endregion
    }
}
