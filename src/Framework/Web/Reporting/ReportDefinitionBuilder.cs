using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using InfoControl.Runtime;

namespace InfoControl.Web.Reporting
{
    public partial class ReportDefinitionBuilder
    {
        #region Properties
        RdlSchema.Report _report;
        public RdlSchema.Report Report
        {
            get { return _report; }
        }


        public RdlSchema.ReportItemsType ReportItems
        {
            get { return _report.Body.ReportItems; }
        }

        DataClasses.ReportSettings _settings;
        public DataClasses.ReportSettings ReportSettings
        {
            get { return _settings; }
        }
        #endregion

        #region ctor
        public ReportDefinitionBuilder(DataClasses.ReportSettings settings)
        {
            _settings = settings;

            _report = new RdlSchema.Report();
            _report.DataSources = CreateDataSources();
            _report.DataSets = CreateDataSets();
            _report.Body = CreateBody();
        }
        #endregion

        #region Methods

        public void Save(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RdlSchema.Report));
                serializer.Serialize(fs, _report);
            }
        }


        private RdlSchema.DataSourcesType CreateDataSources()
        {
            RdlSchema.DataSourcesType dataSources = new RdlSchema.DataSourcesType();
            dataSources.DataSource = new RdlSchema.DataSourceType[] { new RdlSchema.DataSourceType() };
            dataSources.DataSource[0].Name = "DummyDataSource";
            dataSources.DataSource[0].ConnectionProperties = new RdlSchema.ConnectionPropertiesType();
            dataSources.DataSource[0].ConnectionProperties.ConnectString = "";
            dataSources.DataSource[0].ConnectionProperties.DataProvider = "SQL";
            return dataSources;
        }

        private RdlSchema.BodyType CreateBody()
        {
            RdlSchema.BodyType body = new RdlSchema.BodyType();
            body.Height = "29.7cm";
            return body;
        }

        private RdlSchema.DataSetsType CreateDataSets()
        {
            RdlSchema.DataSetsType dataSets = new RdlSchema.DataSetsType();
            dataSets.DataSet = new RdlSchema.DataSetType[] { new RdlSchema.DataSetType() };
            dataSets.DataSet[0].Name = "MyData";
            dataSets.DataSet[0].Query = new RdlSchema.QueryType();
            dataSets.DataSet[0].Query.DataSourceName = "DummyDataSource";
            dataSets.DataSet[0].Query.CommandText = "";

            //
            // Create Fields
            //
            RdlSchema.FieldType field;
            RdlSchema.FieldsType fields = new RdlSchema.FieldsType();
            List<RdlSchema.FieldType> list = new List<RdlSchema.FieldType>();

            //
            // Create the Columns Dataset fields
            // 
            for (int i = 0; i < _settings.Columns.Count; i++)
            {
                field = new RdlSchema.FieldType();
                field.Name = _settings.Columns[i].Name.RemoveSpecialChars();
                field.DataField = _settings.Columns[i].Name;
                list.Add(field);
            }

            //
            // Create the MatrixRows Dataset fields
            //
            for (int i = 0; i < _settings.MatrixRows.Count; i++)
            {
                field = new RdlSchema.FieldType();
                field.Name = _settings.MatrixRows[i].Name.RemoveSpecialChars();
                field.DataField = _settings.MatrixRows[i].Name;
                list.Add(field);
            }

            //
            // Create the Sumarize Dataset fields
            //
            field = new RdlSchema.FieldType();
            field.Name = "Qtd";
            field.DataField = "Qtd";
            list.Add(field);

            fields.Field = list.ToArray();
            dataSets.DataSet[0].Fields = fields;

            return dataSets;
        }

        #endregion
    }
}