using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InfoControl.Web.UI.WebControls
{

    public enum PrinterModel
    {
        Bematech,
        BematechNaoFiscal,
        Daruma
    }

    /// <summary>
    /// 
    /// </summary>    
    [ToolboxData("<{0}:Ecf runat=server></{0}:Ecf>")]
    [ToolboxBitmap(typeof(Ecf))]
    public class Ecf : Control
    {
        Dictionary<PrinterModel, Type> providerList = new Dictionary<PrinterModel, Type>();

        public Ecf()
            : base()
        {
            providerList.Add(PrinterModel.Bematech, typeof(Provider.Bematech));
            providerList.Add(PrinterModel.BematechNaoFiscal, typeof(Provider.BematechNaoFiscal));
            Model = PrinterModel.BematechNaoFiscal;                        
        }                


        #region Properties
        [Browsable(true)]
        public PrinterModel Model
        {
            get { return _model; }
            set
            {
                _provider = Activator.CreateInstance(providerList[value]) as Provider.EcfProviderBase;
                _provider.Control = this;
                _providerTypeName = providerList[value].FullName;
                _model = value;
            }
        }
        private PrinterModel _model;

        [Browsable(true)]
        [DefaultValue(typeof(string), "")]
        public string ProviderName
        {
            get { return _providerTypeName; }
            set { _providerTypeName = value; }
        }
        private string _providerTypeName;


        public Provider.EcfProviderBase Provider
        {
            get
            {
                if (_provider == null)
                {
                    if (String.IsNullOrEmpty(_providerTypeName))
                    {
                        _provider = Activator.CreateInstance(providerList[_model]) as Provider.EcfProviderBase;
                    }
                    else
                    {
                        _provider = Activator.CreateInstance(Type.GetType(_providerTypeName)) as Provider.EcfProviderBase;
                    }
                    _provider.Control = this;
                }
                return _provider;
            }
        }
        Provider.EcfProviderBase _provider;
        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.Write(Provider.Render());            
        }        
    }
}



