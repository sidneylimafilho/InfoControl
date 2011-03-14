using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using InfoControl.IO;

using InfoControl.Web;
using InfoControl.Web.UI;
using InfoControl.Web.Configuration;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


using InfoControl.Runtime;

public partial class _Default : DataPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //ZipFile zip = new ZipFile(Server.MapPath("App_Shared/modules.zip"));
        //zip.Extract("c:\\teste");


        if (Request.Url.OriginalString.ToLower().Contains("pervasiva.com.br"))
        {
            Response.StatusCode = 301;
            Response.StatusDescription = "Moved Permanently";
            Response.RedirectLocation = "http://www.vivina.com.br/pervasiva/start,452.aspx";
            Response.End();
        }

        Server.Execute("~/site/default.aspx");




        //DataTable table = new DataTable();
        //table.Columns.Add("Regiao");
        //table.Columns.Add("Estado");
        //table.Columns.Add("Sexo");
        //table.Columns.Add("Qtd");
        //table.Rows.Add("SE", "RJ", "F", "1");
        //table.Rows.Add("SE", "RJ", "M", "2");
        //table.Rows.Add("SE", "SP", "F", "3");
        //table.Rows.Add("SE", "SP", "M", "4");
        //table.Rows.Add("NE", "CE", "F", "5");
        //table.Rows.Add("NE", "CE", "M", "6");
        //table.Rows.Add("NE", "PB", "F", "7");
        //table.Rows.Add("NE", "PB", "M", "8");
        //table.Rows.Add("SO", "RS", "F", "9");
        //table.Rows.Add("SO", "RS", "M", "10");


        //DataTable matrix = new DataTable();
        //for (int idx = 1; idx < table.Columns.Count - 1; idx++)
        //    matrix.Columns.Add(table.Columns[idx].ColumnName);

        //table.DefaultView.Sort = table.Columns[0].ColumnName;
        //string value = "";
        //int matrixRow = -1;
        //int matrixColumn = matrix.Columns.Count - 1;
        //double totalizer = 0;
        //double totalizerPrior = 0;
        //int totalizerColumn = -1;
        //for (int idx = 0; idx < table.Rows.Count; idx++)
        //{
        //    DataRowView row = table.DefaultView[idx];

        //    if (value != row[0].ToString())
        //    {
        //        //
        //        // Add Total Row
        //        //
        //        //if (!String.IsNullOrEmpty(value))
        //        //{
        //        //    matrix.Rows.Add(matrix.NewRow());
        //        //    matrix.Rows[matrixRow + 1][matrixColumn] = totalizer;
        //        //}               

        //        value = row[0].ToString();
        //        matrix.Columns.Add(value);
        //        //matrix.Columns.Add(value + " %");
        //        matrixRow = -1;
        //        matrixColumn ++;

        //        totalizer = 0;
        //    }

        //    matrixRow++;
        //    if (matrix.Rows.Count - 1 < matrixRow)
        //        matrix.Rows.Add(matrix.NewRow());

        //    matrix.Rows[matrixRow][matrixColumn] = row[table.Columns.Count - 1].ToString();

        //    //if (totalizerColumn < matrixColumn-2)
        //    //{
        //    //totalizer += Convert.ToDouble(row[table.Columns.Count - 1]);
        //    //    matrix.Rows[matrixRow][totalizerColumn] = totalizerPrior;
        //    //}

        //    for (int j = 1; j < table.Columns.Count - 1; j++)
        //    {
        //        matrix.Rows[matrixRow][j - 1] = row[j];
        //    }
        //}        
    }
}
