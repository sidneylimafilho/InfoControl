using System;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Microsoft.Reporting.WebForms;

namespace InfoControl.Web
{
    public static class ReportMethodExtensions
    {
        public static void ToExcel(this LocalReport report)
        {
            //String format = "PDF"; "PDF", "Excel", "IMAGE", "RGDI", "HTML4.0"
            //String deviceInfo = String.Empty;
            //String mimeType = String.Empty;
            //String encoding = String.Empty;
            //String fileNameExtension = String.Empty;
            //String[] streams;
            //Microsoft.Reporting.WebForms.Warning[] warnings;
            //byte[] result = ReportViewer1.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //using (System.IO.FileStream stream = System.IO.File.Create(@"c:\\teste.pdf"))
            //{
            //    stream.Write(result, 0, result.Length - 1);
            //}


            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            byte[] bytes = report.Render(
                "Excel", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);

            using (FileStream fs = new FileStream("output.xls", FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }


        }

    }
}
