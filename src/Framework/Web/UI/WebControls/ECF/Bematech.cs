using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace InfoControl.Web.UI.WebControls.Provider
{
    public class Bematech : EcfProviderBase
    {
        public Bematech() { }

        #region Metodos de Acesso ao Registry

        public override void Registry_Porta(string FlagPorta) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_Path(System.String FlagPath) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_Status(System.String FlagStatus) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_StatusFuncao(System.String FlagStatusFuncao) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_Retorno(System.String FlagRetorno) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_ControlePorta(System.String FlagControlePorta) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_ModoGaveta(System.String FlagConfigRede) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_ConfigRede(System.String FlagModoGaveta) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_Log(System.String FlagLog) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_NomeLog(System.String FlagNomeLog) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_Emulador(System.String FlagEmulador) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_Separador(System.String CharSeparador) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_SeparaMsgPromo(System.String FlagSeparaMsgPromo) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_ZAutomatica(System.String FlagZAutomatica) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_XAutomatica(System.String FlagXAutomatica) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_VendeItemUmaLinha(System.String FlagVendeUmaLinha) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_Default() { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_ImprimeRegistry(System.String NomeProduto) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_RetornaValor(System.String Produto, System.String Chave, string Valor) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_AlteraRegistry(System.String cValue, System.String cValor) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_TerminalServer(System.String tserver) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_ErroExtendidoOk(System.String eextok) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_AbrirDiaFiscal(System.String abredia) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_VendaAutomatica(System.String vauto) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_IgnorarPoucoPapel(System.String IgnorarPoucoPapel) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_Velocidade(System.String Velocidade) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_LogTamMaxMB(System.String LogTamMaxMB) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_MFD_ArredondaValor(System.String Flag) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_MFD_ArredondaQuantidade(System.String Flag) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_AplMensagem1(System.String AplMsg1) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_AplMensagem2(System.String AplMsg2) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_NumeroSerieNaoFormatado(System.String Flag) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_TEF_NumeroLinhasImpressao(System.String NumeroLinhas) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_MFD_ProgramarSinalSonoro(System.String Flag, System.String FlagNumeroBeeps) { throw new Exception("The method or operation is not implemented."); }
        #endregion
        # region Metodos de Acesso aos Comandos de Cupom Fiscal

        public override void AbreCupom(
            string companyName,  
            string addressName,
            string neighborhood,
            string city,
            string stateId,
            string postalCode,
            string ie,
            string cpfCnpj,
            string numeroCupom)
        {
            scripts.Add("Ecf.AbreCupom('" + cpfCnpj + "');");
        }

        public override void VendeItem(System.String codigo, System.String descricao, System.String aliquota, System.String tipoQuantidade, int quantidade, int casasDecimais, double valorUnitario, System.String tipoDesconto, System.String desconto)
        {
            //string script = "Ecf.VendeItem('" + +"', '" + +"', '" + +"', '" + +"', '" + +"', '" + +"', '" + +"', '" + +"', '" + +"')";
            scripts.Add("Ecf.VendeItem('" + codigo + "', '" + descricao + "', '" + aliquota + "', '" + tipoQuantidade + "', '" + quantidade + "', '" + casasDecimais + "', '" + valorUnitario + "', '" + tipoDesconto + "', '" + desconto + "');");
        }

        public override void VendeItemDepartamento(
            System.String codigo,
            System.String descricao,
            System.String aliquota,
            System.String valorUnitario,
            System.String quantidade,
            System.String valorAcrescimo,
            System.String valorDesconto,
            System.String indiceDepartamento,
            System.String unidadeMedida)
        {
            scripts.Add("Ecf.VendeItemDepartamento('" + codigo + "', '" + descricao + "', '" + aliquota + "', '" + valorUnitario + "', '" + quantidade + "', '" + valorAcrescimo + "', '" + valorDesconto + "', '" + indiceDepartamento + "', '" + unidadeMedida + "');");
        }

        public override void VendeItemTresDecimais(string Codigo, string Descricao, string Aliquota, string Quantidade, string Vlr_Unitario, string Acres_ou_Desc, string Percentual_Acresc_Desc) { throw new Exception("The method or operation is not implemented."); }

        public override void CancelaItemAnterior()
        {
            scripts.Add("Ecf.CancelaItemAnterior();");
        }

        public override void CancelaItemGenerico(string numeroItem)
        {
            scripts.Add("Ecf.CancelaItemGenerico('" + numeroItem + "');");
        }

        public override void IniciaFechamentoCupom(
            bool acrescimoDesconto,
            string tipoAcrescimoDesconto,
            double valorAcrescimoDesconto)
        {
            scripts.Add("Ecf.IniciaFechamentoCupom('" + acrescimoDesconto + "', '" + tipoAcrescimoDesconto + "', '" + valorAcrescimoDesconto + "');");
        }

        public override void EfetuaFormaPagamento(string formaPagamento, double valorFormaPagamento)
        {
            scripts.Add("Ecf.EfetuaFormaPagamento('" + formaPagamento + "', '" + valorFormaPagamento + "');");
        }

        public override void EfetuaFormaPagamentoDescricaoForma(string formaPagamento, string valorFormaPagamento, string textoLivre)
        {
            scripts.Add("Ecf.EfetuaFormaPagamentoDescricaoForma('" + formaPagamento + "', '" + valorFormaPagamento + "', '" + textoLivre + "');");
        }

        public override void IdentificaConsumidor(string cNome, string cEndereco, string cCpf_ou_Cnpj) { throw new Exception("The method or operation is not implemented."); }

        public override void TerminaFechamentoCupom(string mensagem)
        {
            scripts.Add("Ecf.TerminaFechamentoCupom('" + mensagem + "');");
        }

        public override void FechaCupom(
            string formaPagamento,
            string acrescimoDesconto,
            string tipoAcrescimoDesconto,
            string valorAcrescimoDesconto,
            double valorPago,
            string mensagem)
        {
            scripts.Add("Ecf.FechaCupom('" + formaPagamento + "', '" + acrescimoDesconto + "', '" + tipoAcrescimoDesconto + "', '" + valorAcrescimoDesconto + "', '" + valorPago + "', '" + mensagem + "');");
        }

        public override void FechaCupomResumido(string formaPagamento, string mensagem)
        {
            scripts.Add("Ecf.FechaCupomResumido('" + formaPagamento + "', '" + mensagem + "');");
        }

        public override void EmitirCupomAdicional() { throw new Exception("The method or operation is not implemented."); }

        public override void CancelaCupom()
        {
            scripts.Add("Ecf.CancelaCupom();");
        }

        public override void AumentaDescricaoItem(string descricao)
        {
            scripts.Add("Ecf.AumentaDescricaoItem('" + descricao + "');");
        }

        public override void UsaUnidadeMedida(string unidadeMedida)
        {
            scripts.Add("Ecf.UsaUnidadeMedida('" + unidadeMedida + "');");
        }

        public override void EstornoFormasPagamento(string formaOrigem, string formaDestino, string valor)
        {
            scripts.Add("Ecf.EstornoFormasPagamento('" + formaOrigem + "', '" + formaDestino + "', '" + valor + "');");
        }
        #endregion
        #region Metodos Nao Fiscal e Vinculados

        public override void RecebimentoNaoFiscal(string indiceTotalizador, string valorRecebimento, string formaPagamento)
        {
            scripts.Add("Ecf.RecebimentoNaoFiscal('" + indiceTotalizador + "', '" + valorRecebimento + "', '" + formaPagamento + "');");
        }

        public override void AbreRecebimentoNaoFiscal(string IndiceTotalizador, string Acrescimo_ou_Desconto, string Tipo_Acrescimo_ou_Desconto, string Valor_Acrescimo_ou_Desconto, string ValorRecebimento, string TextoLivre) { throw new Exception("The method or operation is not implemented."); }

        public override void EfetuaFormaPagamentoNaoFiscal(string FormaPagamento, string ValorFormaPagamento, string TextoLivre) { throw new Exception("The method or operation is not implemented."); }

        public override void AbreComprovanteNaoFiscalVinculado(string formaPagamento, string valor, string numeroCupom)
        {
            scripts.Add("Ecf.AbreComprovanteNaoFiscalVinculado('" + formaPagamento + "', '" + valor + "', '" + numeroCupom + "');");
        }

        public override void UsaComprovanteNaoFiscalVinculado(string texto)
        {
            scripts.Add("Ecf.UsaComprovanteNaoFiscalVinculado('" + texto + "');");
        }

        public override void FechaComprovanteNaoFiscalVinculado() { throw new Exception("The method or operation is not implemented."); }

        public override void Sangria(string valor)
        {
            scripts.Add("Ecf.Sangria('" + valor + "');");
        }

        public override void Suprimento(string valor, string formaPagamento)
        {
            scripts.Add("Ecf.Suprimento('" + valor + "', '" + formaPagamento + "');");
        }

        public override void FundoCaixa(string ValorFundoCaixa, string FormaPagamento) { throw new Exception("The method or operation is not implemented."); }

        public override void AbreRelatorioGerencial() { throw new Exception("The method or operation is not implemented."); }

        public override void EnviarTextoCNF(string texto) { throw new Exception("The method or operation is not implemented."); }
        #endregion
        #region Metodos de Acesso A Leitura da Memoria Fiscal

        public override void LeituraMemoriaFiscalData(string dataInicial, string dataFinal)
        {
            scripts.Add("Ecf.LeituraMemoriaFiscalData('" + dataInicial + "', '" + dataFinal + "');");
        }

        public override void LeituraMemoriaFiscalReducao(string reducaoInicial, string reducaoFinal)
        {
            scripts.Add("Ecf.LeituraMemoriaFiscalReducao('" + reducaoInicial + "', '" + reducaoFinal + "');");
        }

        public override void LeituraMemoriaFiscalSerialReducao(string ReducaoInicial, string ReducaoFinal) { throw new Exception("The method or operation is not implemented."); }

        public override void LeituraMemoriaFiscalSerialData(string DataInicial, string DataFinal) { throw new Exception("The method or operation is not implemented."); }

        public override void FechaRelatorioGerencial()
        {
            scripts.Add("Ecf.FechaRelatorioGerencial();");
        }

        public override void RelatorioGerencial(string texto)
        {
            scripts.Add("Ecf.RelatorioGerencial('" + texto + "');");
        }

        public override void ReducaoZ(string data, string hora)
        {
            scripts.Add("Ecf.ReducaoZ('" + data + "', '" + hora + "');");
        }

        public override void ReducaoZAjustaDataHora(string cData, string cHora) { throw new Exception("The method or operation is not implemented."); }

        public override void LeituraX()
        {
            scripts.Add("Ecf.LeituraX();");
        }
        #endregion
        #region Metodos de Informacoes da Impressora

        public override void NumeroCupom(string VarRetNumeroCupom) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornoImpressora(ref int iACK, ref int iST1, ref int iST2) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaEstadoImpressora(ref int iAckNak, ref int iST1, ref int iST2) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornoAliquotas(string Aliquotas) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaTotalizadoresParciais(string cTotalizadores) { throw new Exception("The method or operation is not implemented."); }

        public override void SubTotal(string SubTotal) { throw new Exception("The method or operation is not implemented."); }

        public override void Troco(string Trocro) { throw new Exception("The method or operation is not implemented."); }

        public override void SaldoAPagar(string Saldo) { throw new Exception("The method or operation is not implemented."); }

        public override void MonitoramentoPapel(string LinhasImpressas) { throw new Exception("The method or operation is not implemented."); }

        public override void DadosUltimaReducao(string DadosReducao) { throw new Exception("The method or operation is not implemented."); }

        public override void UltimaFormaPagamento(string FormaPagamento, string ValorForma) { throw new Exception("The method or operation is not implemented."); }

        public override void TipoUltimoDocumento(string TipoUltimoDoc) { throw new Exception("The method or operation is not implemented."); }

        public override void NumeroSerie(string NumeroSerie) { throw new Exception("The method or operation is not implemented."); }

        public override void VersaoFirmware(string VersaoFirmware) { throw new Exception("The method or operation is not implemented."); }

        public override void CGC_IE(string CGC, string IE) { throw new Exception("The method or operation is not implemented."); }

        public override void GrandeTotal(string GrandeTotal) { throw new Exception("The method or operation is not implemented."); }

        public override void VendaBruta(string VendaBruta) { throw new Exception("The method or operation is not implemented."); }

        public override void VendaBrutaAcumulada(string VendaBrutaAcumulada) { throw new Exception("The method or operation is not implemented."); }

        public override void Descontos(string Descontos) { throw new Exception("The method or operation is not implemented."); }

        public override void Cancelamentos(string Cancelamentos) { throw new Exception("The method or operation is not implemented."); }

        public override void NumeroOperacoesNaoFiscais(string Operacoes) { throw new Exception("The method or operation is not implemented."); }

        public override void NumeroCuponsCancelados(string CuponsCancelados) { throw new Exception("The method or operation is not implemented."); }

        public override void NumeroReducoes(string Reducoes) { throw new Exception("The method or operation is not implemented."); }

        public override void NumeroIntervencoes(string Intervencoes) { throw new Exception("The method or operation is not implemented."); }

        public override void NumeroSubstituicoesProprietario(string Substituicoes) { throw new Exception("The method or operation is not implemented."); }

        public override void UltimoItemVendido(string UltimoItem) { throw new Exception("The method or operation is not implemented."); }

        public override void ClicheProprietario(string ClicheProprietario) { throw new Exception("The method or operation is not implemented."); }

        public override void ClicheProprietarioEx(string ClicheProprietario) { throw new Exception("The method or operation is not implemented."); }

        public override void NumeroCaixa(string NumeroCaixa) { throw new Exception("The method or operation is not implemented."); }

        public override void NumeroLoja(string NumeroLoja) { throw new Exception("The method or operation is not implemented."); }

        public override void SimboloMoeda(string SimboloMoeda) { throw new Exception("The method or operation is not implemented."); }

        public override void FlagsFiscais(ref int FlagFiscal) { throw new Exception("The method or operation is not implemented."); }

        public override void MinutosLigada(string MinutosLigada) { throw new Exception("The method or operation is not implemented."); }

        public override void MinutosImprimindo(string MinutosImprimindo) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaModoOperacao(string cModo) { throw new Exception("The method or operation is not implemented."); }

        public override void StatusCupomFiscal(string cStatus) { throw new Exception("The method or operation is not implemented."); }

        public override void StatusComprovanteNaoFiscalVinculado(string cStatus) { throw new Exception("The method or operation is not implemented."); }

        public override void StatusComprovanteNaoFiscalNaoVinculado(string cStatus) { throw new Exception("The method or operation is not implemented."); }

        public override void StatusRelatorioGerencial(string cStatus) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaEpromConectada(string FlagEprom) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaZPendente(string FlagZPendente) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaXPendente(string FlagXPendente) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaDiaAberto(string FlagDiaAberto) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaHorarioVerao(string FlagHorarioVerao) { throw new Exception("The method or operation is not implemented."); }

        public override void ValorPagoUltimoCupom(string Valor) { throw new Exception("The method or operation is not implemented."); }

        public override void DataHoraImpressora(string Data, string Hora) { throw new Exception("The method or operation is not implemented."); }

        public override void ContadoresTotalizadoresNaoFiscais(string Contadores) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaTotalizadoresNaoFiscais(string Totalizadores) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaTotalizadoresNaoFiscaisEx(string TotalizadoresEx) { throw new Exception("The method or operation is not implemented."); }

        public override void DataHoraReducao(string DataReducao, string HoraReducao) { throw new Exception("The method or operation is not implemented."); }

        public override void DataMovimento(string DataMovimento) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaTruncamento(string FlagTruncamento) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaAliquotasIss(string AliquotasIss) { throw new Exception("The method or operation is not implemented."); }

        public override void Acrescimos(string ValorAcrescimo) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaFormasPagamento(string cFormas) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaFormasPagamentoEx(string cFormas) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaDescricaoFormasPagamento(string cFormas) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaRecebimentoNaoFiscal(string cRecebimentos) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaTipoImpressora(ref int TipoImpressora) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaModeloECF() { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaIndiceAliquotasIss(string cIndices) { throw new Exception("The method or operation is not implemented."); }

        public override void ValorFormaPagamento(string cFormaPagamento, string Valor) { throw new Exception("The method or operation is not implemented."); }

        public override void ValorTotalizadorNaoFiscal(string Totalizador, string Valor) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaImpressoraLigada()
        {
            scripts.Add("alert('A Impressora se encontra ' + (Printer.VerificaImpressoraLigada()==-6)?'DESLIGADA.':'LIGADA.');");
        }

        public override void MapaResumo()
        {
            scripts.Add("Ecf.MapaResumo();");
        }

        public override void RelatorioTipo60Analitico()
        {
            scripts.Add("Ecf.RelatorioTipo60Analitico();");
        }

        public override void RelatorioTipo60Mestre()
        {
            scripts.Add("Ecf.RelatorioTipo60Mestre();");
        }

        public override void COO(string cCooInicial, string cCooFinal) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaErroExtendido(string cCooInicial) { throw new Exception("The method or operation is not implemented."); }

        public override void CRO(string cCRO) { throw new Exception("The method or operation is not implemented."); }

        public override void PalavraStatus(string psatus) { throw new Exception("The method or operation is not implemented."); }

        public override void PalavraStatusBinario(string pstatusbin) { throw new Exception("The method or operation is not implemented."); }

        public override void LerAliquotasComIndice(string aindice) { throw new Exception("The method or operation is not implemented."); }

        #region Funções Exclusivas para MFD

        public override void FIMFD_IndicePrimeiroVinculado(string aindice) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_CasasDecimaisProgramada(string DecimaisQuantidade, ref string DecimaisValor) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_DownloadDaMFD(string COOInicial, string COOFinal) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_RetornaInformacao(string Indice, string Valor) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_TerminaFechamentoCupomCodigoBarras(string Mensagem, string Tipo, string Codigo, string Largura, string Altura, string Posicao) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_ImprimeCodigoBarras(string Tipo, string Codigo, string Largura, string Altura, string Posicao) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_StatusCupomFiscal(string cStatus_Cupom) { throw new Exception("The method or operation is not implemented."); }

        public override void FIB_AbreBilhetePassagem(string Origem, string Destino, string UF, string Percurso, string Prestadora, string Plataforma, string Poltrona, string Modalidade, string Categoria, string DataEmbarque, string PassRg, string PassNome, string PassEndereco) { throw new Exception("The method or operation is not implemented."); }

        public override void FIB_VendeItem(string Descricao, string St, string Valor, string DescontoAcrescimo, string TipoDesconto, string ValorDesconto) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_SinalSonoro(string Str_Valor) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_EqualizarVelocidade(StringBuilder EqualizarVelocidade) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_AcionarGuilhotina() { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_ProgramaRelatoriosGerenciais(string Str_Valor) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_AbreRelatorioGerencial(string Str_Valor) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_EmitirCupomAdicional() { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_RetornaInformacao(string Indice, StringBuilder Valor) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_AbreRecebimentoNaoFiscal(string CPF, string Nome, string Endereco) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_RecebimentoNaoFiscal(string DescricaoTotalizador, string AcresDesc, string TipoAcresDesc, string ValorAcresDesc, string ValorRecebimento) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_IniciaFechamentoNaoFiscal(string AcresDesc, string TipoAcresDesc, string ValorAcresDesc) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_EfetuaFormaPagamentoNaoFiscal(string FormaPgto, string Valor, string Observacao) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_TerminaFechamentoNaoFiscal(string MsgPromo) { throw new Exception("The method or operation is not implemented."); }

        public override void FIMFD_ProgramarGuilhotina(System.String Separacao_Entre_Documentos, System.String Linhas_para_Acionamento_Guilhotina, System.String Status_da_Guilhotina, System.String Impressao_Antecipada_Cliche) { throw new Exception("The method or operation is not implemented."); }

        public override void StatusCupomFiscal(StringBuilder cStatus) { throw new Exception("The method or operation is not implemented."); }

        public override void GrandeTotal(StringBuilder GrandeTotal) { throw new Exception("The method or operation is not implemented."); }

        public override void SubTotal(StringBuilder SubTotal) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_CupomAdicionalDll(System.String CupomAdicional) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_CupomAdicionalDllConfig(System.String CupomAdicional) { throw new Exception("The method or operation is not implemented."); }

        public override void Registry_MFD_LeituraMFCompleta(string valor) { throw new Exception("The method or operation is not implemented."); }


        #endregion


        #endregion
        #region Retornos

        public override void RetornaGNF(string cGeralNaoFiscal) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaValorComprovanteNaoFiscal(string Indice, string ValorComprNaoFiscal) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaIndiceComprovanteNaoFiscal(string Indice, string IndiceCNF) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaCFCancelados(string cCupomCancelado) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaCNFCancelados(string cCupomNFCancelado) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaCLX(string cLeituraX) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaCRO(string cCRO) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaCRZ(string cReducaoZ) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaCRZRestante(string cReducaoRestante) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaTotalPagamentos(string CtotalFormasPago) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaTroco(string cTroco) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaCNFNV(string cCNaoFiscalNaoVinculado) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaDescricaoCNFV(string cDescricaoNaoFiscalVinculado) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaDescontoNF(string cDescNF) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaAcrescimoNF(string cAcrescimoNF) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaCancelamentoNF(string cCancelamentoNF) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaCNFV(string CContadorNFVinculado) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaTempoLigado(string cTimeLigado) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaTempoImprimindo(string CTimeImprimindo) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaRegistradoresNaoFiscais(string CRgistradoresNF) { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaRegistradoresFiscais(string CRgistradoresFiscais) { throw new Exception("The method or operation is not implemented."); }

        #endregion
        #region Configuracaoes do ECF

        public override void Cfg(int VariavelDoEcf, string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgFechaAutomaticoCupom(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgRedZAutomatico(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgImpEstGavVendas(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgLeituraXAuto(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgCalcArredondamento(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgHorarioVerao(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgSensorAut(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgCupomAdicional(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgEspacamentoCupons(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgHoraMinReducaoZ(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgLimiarNearEnd(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgPermMensPromCNF(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }

        public override void CfgLegProdutos(string VarConfigEcf) { throw new Exception("The method or operation is not implemented."); }
        #endregion
        #region Outras
        public override void AbrePortaSerial() { throw new Exception("The method or operation is not implemented."); }

        public override void FechaPortaSerial() { throw new Exception("The method or operation is not implemented."); }

        public override void AberturaDoDia(string valor, string formaPagamento)
        {
            scripts.Add("Ecf.AberturaDoDia('" + valor + "','" + formaPagamento + "');");
        }

        public override void FechamentoDoDia()
        {
            scripts.Add("Ecf.FechamentoDoDia();");
        }

        public override void ImprimeConfiguracoesImpressora()
        {
            scripts.Add("Ecf.ImprimeConfiguracoesImpressora();");
        }

        public override void RegistraNumeroSerie() { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaNumeroSerie() { throw new Exception("The method or operation is not implemented."); }

        public override void RetornaSerialCriptografado(string SerialCriptografado, string NumeroSerial) { throw new Exception("The method or operation is not implemented."); }

        public override void ConfiguraHorarioVerao(string DataEntrada, string DataSaida, string Controle) { throw new Exception("The method or operation is not implemented."); }
        #endregion
        #region Programacao do ECF

        public override void AlteraSimboloMoeda(string simbolo)
        {
            scripts.Add("Ecf.AlteraSimboloMoeda('" + simbolo + "');");
        }

        public override void ProgramaAliquota(string aliquota, int icmsIss)
        {
            scripts.Add("Ecf.ProgramaAliquota('" + aliquota + "', '" + icmsIss + "');");
        }

        public override void ProgramaHorarioVerao()
        {
            scripts.Add("Ecf.ProgramaHorarioVerao();");
        }

        public override void NomeiaTotalizadorNaoSujeitoIcms(int indice, string totalizador)
        {
            scripts.Add("Ecf.NomeiaTotalizadorNaoSujeitoIcms('" + indice + "', '" + totalizador + "');");
        }

        public override void ProgramaArredondamento()
        {
            scripts.Add("Ecf.ProgramaArredondamento();");
        }

        public override void ProgramaTruncamento()
        {
            scripts.Add("Ecf.ProgramaTruncamento();");
        }

        public override void LinhasEntreCupons(int linhas)
        {
            scripts.Add("Ecf.LinhasEntreCupons('" + linhas + "');");
        }

        public override void EspacoEntreLinhas(int pontos)
        {
            scripts.Add("Ecf.EspacoEntreLinhas('" + pontos + "');");
        }

        public override void ForcaImpactoAgulhas(int forcaImpacto)
        {
            scripts.Add("Ecf.ForcaImpactoAgulhas('" + forcaImpacto + "');");
        }

        public override void ResetaImpressora()
        {
            scripts.Add("Ecf.ResetaImpressora();");
        }

        public override void ProgramaFormasPagamento(string FormaPagamento) { throw new Exception("The method or operation is not implemented."); }

        public override void ProgFormasPagtoSemVincular(string FormaPagamento) { throw new Exception("The method or operation is not implemented."); }

        public override void ProgramaVinculados(string DescricaoVinculado) { throw new Exception("The method or operation is not implemented."); }

        public override void EqualizaFormasPgto() { throw new Exception("The method or operation is not implemented."); }

        public override void ProgramaOperador(string Operador) { throw new Exception("The method or operation is not implemented."); }
        #endregion
        #region Metodos de Autenticacao e Gaveta

        public override void AutenticacaoStr(string TextoStr) { throw new Exception("The method or operation is not implemented."); }

        public override void Autenticacao()
        {
            scripts.Add("Ecf.Autenticacao();");
        }

        public override void VerificaDocAutenticacao() { throw new Exception("The method or operation is not implemented."); }

        public override void AcionaGaveta()
        {
            scripts.Add("Ecf.AcionaGaveta();");
        }

        public override void VerificaEstadoGaveta(ref int Estado) { throw new Exception("The method or operation is not implemented."); }

        public override void VerificaEstadoGavetaStr(string EstadoGaveta) { throw new Exception("The method or operation is not implemented."); }
        #endregion
        #region Metodos de TEF
        public override void TEF_EsperarArquivo(string cArquivo, string cTempo, string cTravar) { throw new Exception("The method or operation is not implemented."); }
        public override void TEF_ImprimirResposta(string cArquivoResp, string cForma, string cTravar) { throw new Exception("The method or operation is not implemented."); }
        public override void TEF_ImprimirRespostaCartao(string cArquivoResp, string cForma, string cTravar, string ValorPago) { throw new Exception("The method or operation is not implemented."); }
        public override void TEF_SetFocus(string cWndFocus) { throw new Exception("The method or operation is not implemented."); }
        public override void TEF_TravarTeclado(string cTravar) { throw new Exception("The method or operation is not implemented."); }
        public override void TEF_FechaRelatorio() { throw new Exception("The method or operation is not implemented."); }
        #endregion

        public override void ImprimeTexto(string texto, string fonte, bool italic, bool underline, bool extended, bool negrito) { throw new Exception("The method or operation is not implemented."); }
        
        public override string Render()
        {
            return "<object classid=\"clsid:2F9082AF-E2A7-4F20-9D6B-3855D79A3D86\" id=\"Ecf\" width=\"0\" height=\"0\"></object>" +
                    "<script type=\"text/javascript\">var Ecf = document.getElementById('Ecf');</script>";
        }


    }
}

