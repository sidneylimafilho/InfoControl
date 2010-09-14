using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using InfoControl.Web.UI;

namespace InfoControl.Web.UI.WebControls
{
    public enum Banco
    {
        BancoBrasil,
        Caixa,
        Itau,
        Bradesco,
        Hsbc
    }


    /// <summary>
    /// Gera boletos em formato HTML.
    /// <p>Histórico: <br>
    ///     - Autor: Reinaldo M. R. Alves (maio de 2005). Foi utilizado as rotinas criadas por Marlon Sergio da Silva
    ///     em VB.NET. Basicamente foi feito: <br>
    ///     1) Conversão do código para C#; <br>
    ///     2) As rotinas originais não se utilizavam de conceitos de OOP;<br>
    ///     3) As rotinas originais estavam vinculadas a ASP.NET. Esta implementação possibilita a utilização em 
    ///     ASP.NET, para envio de email, e até em aplicações Windows Forms.<br>
    ///     4) Alterado o nome de algumas variáveis (que foram convertidas para propriedades), creio que ficou
    ///     mais sugestivo. <br>
    ///     
    ///		ATENÇÃO: Ainda não realizei testes práticos. O que implica imprimir as boletas e verificar se
    ///		tudo está OK. Isto deve ser feito individualmente para cada banco.
    ///     </p>
    /// </summary>
    [PersistChildren(true)]
    [ParseChildren(false)]
    [ToolboxData("<{0}:HtmlBoleto runat=server></{0}:HtmlBoleto>")]
    [ToolboxBitmap(typeof(HtmlBoleto))]
    [ToolboxItem(true)]
    [Designer(typeof(HtmlBoletoDesigner))]
    public class HtmlBoleto : WebControl
    {
        #region Members
        public const string DATEFORMAT = "dd/MM/yyyy";
        public const string FORMATO_VALOR = "#,0.00";
        protected Banco _banco = Banco.BancoBrasil;
        private StringBuilder _Buffer = new StringBuilder();
        private bool _Encerrado = false;
     
        private Type generatorType;
        private BoletoGenerator generator = new BoletoBancoBrasil();
        #endregion

        #region Properties
        [Browsable(true)]
        [DefaultValue(typeof(Banco), "BancoBrasil")]
        public Banco Banco
        {
            get { return _banco; }
            set
            {
                generatorType = typeof(HtmlBoleto).Assembly.GetType("InfoControl.Web.UI.WebControls.Boleto" + value.ToString());
                generator = Activator.CreateInstance(generatorType) as BoletoGenerator;

                _banco = value;
            }
        }

        protected string _BancoLogoTipo;
        /// <summary>
        /// Nome do arquivo com o logotipo do banco.
        /// </summary>
        public string BancoLogoTipo
        {
            get { return generator.BancoLogoTipo; }
        }

        protected int _BancoCodigo;
        /// <summary>
        /// Número do banco na câmara de compensação Ex: Banco Brasil = 001
        /// </summary>
        public int BancoCodigo
        {
            get { return generator.BancoCodigo; }
        }

        protected char _BancoCodigoDV;
        /// <summary>
        /// Dígito verificador do número do banco na câmara de compensação
        /// </summary>
        public char BancoCodigoDV
        {
            get { return generator.BancoCodigoDV; }
        }

        protected int _Contrato;
        /// <summary>
        /// Sequencial de cobrança do boleto usado para gerar o "nosso número"
        /// </summary>
        public int Contrato
        {
            get { return _Contrato; }
            set { _Contrato = value; }
        }

        protected int _nossoNumero;
        /// <summary>
        /// Número sequencial utilizado para montar o nosso número. Cada boleta deve receber um valor
        /// único neste campo.
        /// </summary>
        public int NossoNumero
        {
            get { return _nossoNumero; }
            set { _nossoNumero = value; }
        }

        protected string _CedenteNome;
        /// <summary>
        /// Nome do cedente
        /// </summary>
        public string CedenteNome
        {
            get { return _CedenteNome; }
            set { _CedenteNome = value; }
        }

        protected string _CedenteAgencia;
        /// <summary>
        /// Agência do cedente sem DV
        /// </summary>
        public string CedenteAgencia
        {
            get { return _CedenteAgencia; }
            set { _CedenteAgencia = value; }
        }

        /// <summary>
        /// Monta a conta completa do cliente contendo Agência/Conta-DV.
        /// </summary>
        public virtual string AgenciaCedente()
        {
            return _CedenteAgencia + "/" + _CedenteConta + "-" + _CedenteContaDV;
        }

        protected string _CedenteConta;
        /// <summary>
        /// Conta corrente do cedente sem DV
        /// </summary>
        public string CedenteConta
        {
            get { return _CedenteConta; }
            set { _CedenteConta = value; }
        }

        protected string _CedenteContaDV;
        /// <summary>
        /// DV da conta corrente do cedente
        /// </summary>
        public string CedenteContaDV
        {
            get { return _CedenteContaDV; }
            set { _CedenteContaDV = value; }
        }

        protected string _SacadoNome;
        /// <summary>
        /// Nome do sacado
        /// </summary>
        public string SacadoNome
        {
            get { return _SacadoNome; }
            set { _SacadoNome = value; }
        }

        protected string _LocalPagamento = "ATÉ O VENCIMENTO PAGÁVEL EM QUALQUER BANCO";
        public string LocalPagamento
        {
            get { return _LocalPagamento; }
            set { _LocalPagamento = value; }
        }

        protected DateTime _dataVencimento;
        /// <summary>
        /// Data de vencimento do título
        /// </summary>
        public DateTime DataVencimento
        {
            get { return _dataVencimento; }
            set { _dataVencimento = value; }
        }

        protected float _Valor;
        /// <summary>
        /// Valor do título
        /// </summary>
        public float Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }

        protected DateTime _dataEmissao;
        /// <summary>
        /// Data de emissão do título
        /// </summary>
        public DateTime DataEmissao
        {
            get { return _dataEmissao; }
            set { _dataEmissao = value; }
        }

        protected string _Documento;
        /// <summary>
        /// Número do título
        /// </summary>
        public string Documento
        {
            get { return _Documento; }
            set { _Documento = value; }
        }

        protected DateTime _dataDocumento;
        /// <summary>
        /// Data do título
        /// </summary>
        public DateTime DataDocumento
        {
            get { return _dataDocumento; }
            set { _dataDocumento = value; }
        }

        protected bool _Aceite;
        /// <summary>
        /// Aceite
        /// </summary>
        public bool Aceite
        {
            get { return _Aceite; }
            set { _Aceite = value; }
        }

        protected string _SequNossNume;
        /// <summary>
        /// Sequencial lNossoNumero obs: Esse sequencial quem fornece é o Itaú
        /// </summary>
        public string SequNossNume
        {
            get { return _SequNossNume; }
            set { _SequNossNume = value; }
        }

        protected DateTime _dataProcessamento;
        /// <summary>
        /// Data de processamento do título
        /// </summary>
        public DateTime DataProcessamento
        {
            get { return _dataProcessamento; }
            set { _dataProcessamento = value; }
        }

        protected int _Carteira;
        /// <summary>
        /// Carteira
        /// </summary>
        public int Carteira
        {
            get { return _Carteira; }
            set { _Carteira = value; }
        }

        protected int _Quantidade = 1;
        /// <summary>
        /// Quantidade
        /// </summary>
        public int Quantidade
        {
            get { return _Quantidade; }
            set { _Quantidade = value; }
        }

        protected string _instrucao;
        /// <summary>
        /// Primeira linha da instrução
        /// </summary>
        public string Instrucao
        {
            get { return _instrucao; }
            set { _instrucao = value; }
        }

               

        protected string _SacadoCPF_CNPJ;
        /// <summary>
        /// CPF ou CNPJ do sacado
        /// </summary>
        public string SacadoCPF_CNPJ
        {
            get { return _SacadoCPF_CNPJ; }
            set { _SacadoCPF_CNPJ = value; }
        }

        protected string _SacadoEndereco;
        /// <summary>
        /// Endereço do sacado
        /// </summary>
        public string SacadoEndereco
        {
            get { return _SacadoEndereco; }
            set { _SacadoEndereco = value; }
        }

        protected string _SacadoBairro;
        /// <summary>
        /// Bairro do sacado
        /// </summary>
        public string SacadoBairro
        {
            get { return _SacadoBairro; }
            set { _SacadoBairro = value; }
        }

        protected string _SacadoCidade;
        /// <summary>
        /// Cidade do sacado
        /// </summary>
        public string SacadoCidade
        {
            get { return _SacadoCidade; }
            set { _SacadoCidade = value; }
        }

        protected string _SacadoUF;
        /// <summary>
        /// Estado do sacado
        /// </summary>
        public string SacadoUF
        {
            get { return _SacadoUF; }
            set { _SacadoUF = value; }
        }

        protected string _SacadoCEP;
        /// <summary>
        /// CEP do sacado
        /// </summary>
        public string SacadoCEP
        {
            get { return _SacadoCEP; }
            set { _SacadoCEP = value; }
        }

        protected string _Especie = "R$";
        /// <summary>
        /// Espécie
        /// </summary>
        public string Especie
        {
            get { return _Especie; }
            set { _Especie = value; }
        }

        protected string _pEspecieDoc = "DP";
        public string pEspecieDoc
        {
            get { return _pEspecieDoc; }
            set { _pEspecieDoc = value; }
        }


        private float _desconto;
        /// <summary>
        /// Desconto
        /// </summary>
        public float Desconto
        {
            get { return _desconto; }
            set { _desconto = value; }
        }


        private float _multa;
        /// <summary>
        /// Multa
        /// </summary>
        public float Multa
        {
            get { return _multa; }
            set { _multa = value; }
        }


        private float _outrasDeducoes;
        /// <summary>
        /// Outras Deduções
        /// </summary>
        public float OutrasDeducoes
        {
            get { return _outrasDeducoes; }
            set { _outrasDeducoes = value; }
        }


        private float _outrasAcrescimos;
        /// <summary>
        /// Outros Acrescimos
        /// </summary>
        public float OutrasAcrescimos
        {
            get { return _outrasAcrescimos; }
            set { _outrasAcrescimos = value; }
        }

     //   private float _valorCobrado;
        public float ValorCobrado
        {
            get
            {
                return Valor - Multa - Desconto - OutrasDeducoes + OutrasAcrescimos;
            }
        }


        #endregion

        #region Methods
        public void SaveToFile(string lFileName)
        {
            byte[] lBuffer = System.Text.Encoding.Default.GetBytes(ToString());
            FileStream fs = new FileStream(lFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            fs.Write(lBuffer, 0, lBuffer.Length);
            fs.Close();
        }
        protected string SacadoEnderecoLinha1()
        {
            return SacadoEndereco + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + SacadoBairro;
        }

        protected string SacadoEnderecoLinha2()
        {
            return SacadoCEP + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + SacadoCidade + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + SacadoUF;
        }
        /// <summary>
        /// Monta o código de barras.
        /// </summary>
        /// <param name="Valor"></param>
        /// <returns></returns>
        private string CodigoBarras25I(string Valor)
        {
            int f, f1, f2, i;
            string s;
            string texto;
            int fino = 1;
            int largo = 3;
            int altura = 50;
            string[] BarCodes = new string[100];
            StringBuilder Codbarras = new System.Text.StringBuilder();

            BarCodes[0] = "00110";
            BarCodes[1] = "10001";
            BarCodes[2] = "01001";
            BarCodes[3] = "11000";
            BarCodes[4] = "00101";
            BarCodes[5] = "10100";
            BarCodes[6] = "01100";
            BarCodes[7] = "00011";
            BarCodes[8] = "10010";
            BarCodes[9] = "01010";
            for (f1 = 9; f1 >= 0; f1--)
                for (f2 = 9; f2 >= 0; f2--)
                {
                    f = f1 * 10 + f2;
                    texto = "";
                    for (i = 0; i <= 4; i++)
                        texto += BarCodes[f1].Substring(i, 1) + BarCodes[f2].Substring(i, 1);
                    BarCodes[f] = texto;
                }

            texto = Valor;
            if ((texto.Length % 2) != 0)
                texto = "0" + texto;

            string pUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "InfoControl.Web.UI.WebControls.Boleto.Resources.p.gif");
            string bUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "InfoControl.Web.UI.WebControls.Boleto.Resources.b.gif");

            //draw da guarda inicial
            Codbarras.Append("<img src=\"" + pUrl + "\" width=\"" + fino.ToString() + "\" height=\"" + altura.ToString() + "\" border=0>");
            Codbarras.Append("<img src=\"" + bUrl + "\" width=\"" + fino.ToString() + "\" height=\"" + altura.ToString() + "\" border=0>");
            Codbarras.Append("<img src=\"" + pUrl + "\" width=\"" + fino.ToString() + "\" height=\"" + altura.ToString() + "\" border=0>");
            Codbarras.Append("<img src=\"" + bUrl + "\" width=\"" + fino.ToString() + "\" height=\"" + altura.ToString() + "\" border=0>");

            // Draw dos dados
            while (texto.Length > 0)
            {
                i = Convert.ToInt32(texto.Substring(0, 2));
                texto = texto.Remove(0, 2);

                s = BarCodes[i];

                for (i = 0; i <= 9; i += 2)
                {
                    if (s[i] == '0') f1 = fino;
                    else f1 = largo;

                    Codbarras.Append("<img src=\"" + pUrl + "\" width=\"" + f1.ToString() + "\" height=\"" + altura.ToString() + "\" border=0>");

                    if (s[i + 1] == '0') f2 = fino;
                    else f2 = largo;

                    Codbarras.Append("<img src=\"" + bUrl + "\" width=\"" + f2.ToString() + "\" height=\"" + altura.ToString() + "\" border=0>");
                }
            }

            // draw da guarda final
            Codbarras.Append("<img src=\"" + pUrl + "\" width=\"" + largo.ToString() + "\" height=\"" + altura.ToString() + "\" border=0>");
            Codbarras.Append("<img src=\"" + bUrl + "\" width=\"" + fino.ToString() + "\" height=\"" + altura.ToString() + "\" border=0>");
            Codbarras.Append("<img src=\"" + pUrl + "\" width=\"" + fino.ToString() + "\" height=\"" + altura.ToString() + "\" border=0>");

            return Codbarras.ToString();
        }

        #endregion

        #region Events
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //
            // Stylesheet
            //
            (Page as Page).ImportStylesheet(Page.ClientScript.GetWebResourceUrl(this.GetType(), "InfoControl.Web.UI.WebControls.Boleto.Resources.Stylesheet.css"));

        }
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (Visible)
            {
                base.RenderControl(writer);

                writer.Write(GetLayoutHtml());
            }
        }

        internal string GetDesignTimeHtml()
        {
            return GetLayoutHtml();
        }

        internal string GetLayoutHtml()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing=\"0\" cellpadding=\"0\" width=\"700\" border=\"0\"><tr><td>");

            sb.Append(Boleto.Resources.ComprovanteEntrega);
            sb.Append(Boleto.Resources.LinhaCorte);

            sb.Append(Boleto.Resources.ReciboSacado);
            sb.Append(Boleto.Resources.Corpo);
            sb.Append(Boleto.Resources.ReciboSacadoFim);
            sb.Append(Boleto.Resources.LinhaCorte);

            sb.Append(Boleto.Resources.ParteBanco);
            sb.Append(Boleto.Resources.Corpo);
            sb.Append(Boleto.Resources.ParteBancoFim);
            sb.Append(Boleto.Resources.LinhaCorte);

            sb.Append("</td></tr></table>");


            string sImgBarra = Page.ClientScript.GetWebResourceUrl(this.GetType(), "InfoControl.Web.UI.WebControls.Boleto.Resources.barra.gif");
            string sImgCorte = Page.ClientScript.GetWebResourceUrl(this.GetType(), "InfoControl.Web.UI.WebControls.Boleto.Resources.corte.gif");
            string sImgBancoLogoTipo = Page.ClientScript.GetWebResourceUrl(this.GetType(), "InfoControl.Web.UI.WebControls.Boleto.Resources." + BancoLogoTipo);

            //
            // Gera o numero que identifica o boleto no banco
            //
            string lNossoNumero, lLinhaDigitavel, lCodigoBarras;
            generator.MontaCodigos(this, out lNossoNumero, out lLinhaDigitavel, out lCodigoBarras);


            //
            // Sacado
            // 
            sb = sb.Replace("[=SacadoNome=]", SacadoNome);
            sb = sb.Replace("[=SacadoEnderecoLinha2=]", SacadoEnderecoLinha2());
            sb = sb.Replace("[=SacadoEnderecoLinha1=]", SacadoEnderecoLinha1());
            sb = sb.Replace("[=SacadoCPF_CNPJ=]", SacadoCPF_CNPJ);

            //
            // Cedente
            //
            sb = sb.Replace("[=CodigoBarras=]", CodigoBarras25I(lCodigoBarras));
            sb = sb.Replace("[=sImgCorte=]", sImgCorte);
            sb = sb.Replace("[=lNossoNumero=]", lNossoNumero);

            sb = sb.Replace("[=Instrucao=]", Instrucao);           

            sb = sb.Replace("[=CedenteNome=]", CedenteNome);
            sb = sb.Replace("[=DataProcessamento=]", DataProcessamento.ToString(DATEFORMAT));
            sb = sb.Replace("[=DataEmissao=]", DataEmissao.ToString(DATEFORMAT));
            sb = sb.Replace("[=DataDocumento=]", DataDocumento.ToString(DATEFORMAT));
            sb = sb.Replace("[=Documento=]", Documento);
            sb = sb.Replace("[=DataVencimento=]", DataVencimento.ToString(DATEFORMAT));

            sb = sb.Replace("[=pEspecieDoc=]", pEspecieDoc);
            sb = sb.Replace("[=Especie=]", Especie);
            sb = sb.Replace("[=Carteira=]", Carteira.ToString());
            sb = sb.Replace("[=Quantidade=]", Quantidade.ToString());

            sb = sb.Replace("[=LocalPagamento=]", LocalPagamento);
            sb = sb.Replace("[=Aceite=]", Aceite.ToString());
            sb = sb.Replace("[=AgenciaCedente=]", AgenciaCedente());
            sb = sb.Replace("[=BancoCodigoCompleto=]", generator.BancoCodigoCompleto());

            sb = sb.Replace("[=lLinhaDigitavel=]", lLinhaDigitavel.Replace(" ", "&nbsp;&nbsp;"));

            //
            // Valores
            //
            sb = sb.Replace("[=ValorCobrado=]", ValorCobrado.ToString(FORMATO_VALOR));
            sb = sb.Replace("[=Valor=]", Valor.ToString(FORMATO_VALOR));
            sb = sb.Replace("[=Multa=]", Multa.ToString(FORMATO_VALOR));
            sb = sb.Replace("[=Desconto=]", Desconto.ToString(FORMATO_VALOR));
            sb = sb.Replace("[=OutrasAcrescimos=]", OutrasAcrescimos.ToString(FORMATO_VALOR));
            sb = sb.Replace("[=OutrasDeducoes=]", OutrasDeducoes.ToString(FORMATO_VALOR));

            //
            // Images
            //
            sb = sb.Replace("[=sImgBarra=]", sImgBarra);
            sb = sb.Replace("[=sImgBancoLogoTipo=]", sImgBancoLogoTipo);


            return sb.ToString();
        }
        #endregion

















    }
}
