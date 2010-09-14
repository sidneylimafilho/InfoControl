using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoControl.Web.UI.WebControls.Provider
{
    public abstract class EcfProviderBase
    {
        public List<string> scripts = new List<string>();

        public Control _control;
        public virtual Control Control
        {
            get { return _control; }
            set { _control = value; }
        }        

        #region Metodos de Acesso ao Registry
        public abstract void Registry_Porta(System.String FlagPorta);

        public abstract void Registry_Path(System.String FlagPath);

        public abstract void Registry_Status(System.String FlagStatus);

        public abstract void Registry_StatusFuncao(System.String FlagStatusFuncao);

        public abstract void Registry_Retorno(System.String FlagRetorno);

        public abstract void Registry_ControlePorta(System.String FlagControlePorta);

        public abstract void Registry_ModoGaveta(System.String FlagConfigRede);

        public abstract void Registry_ConfigRede(System.String FlagModoGaveta);

        public abstract void Registry_Log(System.String FlagLog);

        public abstract void Registry_NomeLog(System.String FlagNomeLog);

        public abstract void Registry_Emulador(System.String FlagEmulador);

        public abstract void Registry_Separador(System.String CharSeparador);

        public abstract void Registry_SeparaMsgPromo(System.String FlagSeparaMsgPromo);

        public abstract void Registry_ZAutomatica(System.String FlagZAutomatica);

        public abstract void Registry_XAutomatica(System.String FlagXAutomatica);

        public abstract void Registry_VendeItemUmaLinha(System.String FlagVendeUmaLinha);

        public abstract void Registry_Default();

        public abstract void Registry_ImprimeRegistry(System.String NomeProduto);

        public abstract void Registry_RetornaValor(System.String Produto, System.String Chave, string Valor);

        public abstract void Registry_AlteraRegistry(System.String cValue, System.String cValor);

        public abstract void Registry_TerminalServer(System.String tserver);

        public abstract void Registry_ErroExtendidoOk(System.String eextok);

        public abstract void Registry_AbrirDiaFiscal(System.String abredia);

        public abstract void Registry_VendaAutomatica(System.String vauto);

        public abstract void Registry_IgnorarPoucoPapel(System.String IgnorarPoucoPapel);

        public abstract void Registry_Velocidade(System.String Velocidade);

        public abstract void Registry_LogTamMaxMB(System.String LogTamMaxMB);

        public abstract void Registry_MFD_ArredondaValor(System.String Flag);

        public abstract void Registry_MFD_ArredondaQuantidade(System.String Flag);

        public abstract void Registry_AplMensagem1(System.String AplMsg1);

        public abstract void Registry_AplMensagem2(System.String AplMsg2);

        public abstract void Registry_NumeroSerieNaoFormatado(System.String Flag);

        public abstract void Registry_TEF_NumeroLinhasImpressao(System.String NumeroLinhas);

        public abstract void Registry_MFD_ProgramarSinalSonoro(System.String Flag, System.String FlagNumeroBeeps);
        #endregion
        # region Metodos de Acesso aos Comandos de Cupom Fiscal

        public abstract void AbreCupom(
            string companyName,  
            string addressName,
            string neighborhood,
            string city,
            string stateId,
            string postalCode,
            string ie,
            string cpfCnpj,
            string numeroCupom);

        public abstract void VendeItem(System.String codigo, System.String descricao, System.String aliquota, System.String tipoQuantidade, int quantidade, int casasDecimais, double valorUnitario, System.String tipoDesconto, System.String desconto);

        public abstract void VendeItemDepartamento(System.String codigo, System.String descricao, System.String Aliquota, System.String ValorUnitario, System.String Quantidade, System.String ValorAcrescimo, System.String ValorDesconto, System.String IndiceDepartamento, System.String UnidadeMedida);

        public abstract void VendeItemTresDecimais(string codigo, string descricao, string aliquota, string Quantidade, string Vlr_Unitario, string Acres_ou_Desc, string Percentual_Acresc_Desc);

        public abstract void CancelaItemAnterior();

        public abstract void CancelaItemGenerico(string numeroItem);

        public abstract void IniciaFechamentoCupom(bool acrescimoDesconto, string tipoAcrescimoDesconto, double valorAcrescimoDesconto);

        public abstract void EfetuaFormaPagamento(string formaPagamento, double valorFormaPagamento);

        public abstract void EfetuaFormaPagamentoDescricaoForma(string formaPagamento, string valorFormaPagamento, string textoLivre);

        public abstract void IdentificaConsumidor(string nome, string endereco, string cpfCnpj);

        public abstract void TerminaFechamentoCupom(string mensagem);

        public abstract void FechaCupom(string formaPagamento, string acrescimoDesconto, string tipoAcrescimoDesconto, string valorAcrescimoDesconto, double valorPago, string mensagem);

        public abstract void FechaCupomResumido(string formaPagamento, string mensagem);

        public abstract void EmitirCupomAdicional();

        public abstract void CancelaCupom();

        public abstract void AumentaDescricaoItem(string descricao);

        public abstract void UsaUnidadeMedida(string unidadeMedida);

        public abstract void EstornoFormasPagamento(string formaOrigem, string formaDestino, string valor);
        #endregion
        #region Metodos Nao Fiscal e Vinculados

        public abstract void RecebimentoNaoFiscal(string IndiceTotalizador, string ValorRecebimento, string FormaPagamento);

        public abstract void AbreRecebimentoNaoFiscal(string IndiceTotalizador, string Acrescimo_ou_Desconto, string Tipo_Acrescimo_ou_Desconto, string Valor_Acrescimo_ou_Desconto, string ValorRecebimento, string TextoLivre);

        public abstract void EfetuaFormaPagamentoNaoFiscal(string FormaPagamento, string ValorFormaPagamento, string TextoLivre);

        public abstract void AbreComprovanteNaoFiscalVinculado(string FormaPagamento, string Valor, string NumeroCupom);

        public abstract void UsaComprovanteNaoFiscalVinculado(string Texto);

        public abstract void FechaComprovanteNaoFiscalVinculado();

        public abstract void Sangria(string Valor);

        public abstract void Suprimento(string Valor, string FormaPagamento);

        public abstract void FundoCaixa(string ValorFundoCaixa, string FormaPagamento);

        public abstract void AbreRelatorioGerencial();

        public abstract void EnviarTextoCNF(string texto);
        #endregion
        #region Metodos de Acesso A Leitura da Memoria Fiscal

        public abstract void LeituraMemoriaFiscalData(string DataInicial, string DataFinal);

        public abstract void LeituraMemoriaFiscalReducao(string ReducaoInicial, string ReducaoFinal);

        public abstract void LeituraMemoriaFiscalSerialReducao(string ReducaoInicial, string ReducaoFinal);

        public abstract void LeituraMemoriaFiscalSerialData(string DataInicial, string DataFinal);

        public abstract void FechaRelatorioGerencial();

        public abstract void RelatorioGerencial(string Texto);

        public abstract void ReducaoZ(string cData, string cHora);

        public abstract void ReducaoZAjustaDataHora(string cData, string cHora);

        public abstract void LeituraX();
        #endregion
        #region Metodos de Informacoes da Impressora

        public abstract void NumeroCupom(string VarRetNumeroCupom);

        public abstract void RetornoImpressora(ref int iACK, ref int iST1, ref int iST2);

        public abstract void VerificaEstadoImpressora(ref int iAckNak, ref int iST1, ref int iST2);

        public abstract void RetornoAliquotas(string Aliquotas);

        public abstract void VerificaTotalizadoresParciais(string cTotalizadores);

        public abstract void SubTotal(string SubTotal);

        public abstract void Troco(string Trocro);

        public abstract void SaldoAPagar(string Saldo);

        public abstract void MonitoramentoPapel(string LinhasImpressas);

        public abstract void DadosUltimaReducao(string DadosReducao);

        public abstract void UltimaFormaPagamento(string FormaPagamento, string ValorForma);

        public abstract void TipoUltimoDocumento(string TipoUltimoDoc);

        public abstract void NumeroSerie(string NumeroSerie);

        public abstract void VersaoFirmware(string VersaoFirmware);

        public abstract void CGC_IE(string CGC, string IE);

        public abstract void GrandeTotal(string GrandeTotal);

        public abstract void VendaBruta(string VendaBruta);

        public abstract void VendaBrutaAcumulada(string VendaBrutaAcumulada);

        public abstract void Descontos(string Descontos);

        public abstract void Cancelamentos(string Cancelamentos);

        public abstract void NumeroOperacoesNaoFiscais(string Operacoes);

        public abstract void NumeroCuponsCancelados(string CuponsCancelados);

        public abstract void NumeroReducoes(string Reducoes);

        public abstract void NumeroIntervencoes(string Intervencoes);

        public abstract void NumeroSubstituicoesProprietario(string Substituicoes);

        public abstract void UltimoItemVendido(string UltimoItem);

        public abstract void ClicheProprietario(string ClicheProprietario);

        public abstract void ClicheProprietarioEx(string ClicheProprietario);

        public abstract void NumeroCaixa(string NumeroCaixa);

        public abstract void NumeroLoja(string NumeroLoja);

        public abstract void SimboloMoeda(string SimboloMoeda);

        public abstract void FlagsFiscais(ref int FlagFiscal);

        public abstract void MinutosLigada(string MinutosLigada);

        public abstract void MinutosImprimindo(string MinutosImprimindo);

        public abstract void VerificaModoOperacao(string cModo);

        public abstract void StatusCupomFiscal(string cStatus);

        public abstract void StatusComprovanteNaoFiscalVinculado(string cStatus);

        public abstract void StatusComprovanteNaoFiscalNaoVinculado(string cStatus);

        public abstract void StatusRelatorioGerencial(string cStatus);

        public abstract void VerificaEpromConectada(string FlagEprom);

        public abstract void VerificaZPendente(string FlagZPendente);

        public abstract void VerificaXPendente(string FlagXPendente);

        public abstract void VerificaDiaAberto(string FlagDiaAberto);

        public abstract void VerificaHorarioVerao(string FlagHorarioVerao);

        public abstract void ValorPagoUltimoCupom(string Valor);

        public abstract void DataHoraImpressora(string Data, string Hora);

        public abstract void ContadoresTotalizadoresNaoFiscais(string Contadores);

        public abstract void VerificaTotalizadoresNaoFiscais(string Totalizadores);

        public abstract void VerificaTotalizadoresNaoFiscaisEx(string TotalizadoresEx);

        public abstract void DataHoraReducao(string DataReducao, string HoraReducao);

        public abstract void DataMovimento(string DataMovimento);

        public abstract void VerificaTruncamento(string FlagTruncamento);

        public abstract void VerificaAliquotasIss(string AliquotasIss);

        public abstract void Acrescimos(string ValorAcrescimo);

        public abstract void VerificaFormasPagamento(string cFormas);

        public abstract void VerificaFormasPagamentoEx(string cFormas);

        public abstract void VerificaDescricaoFormasPagamento(string cFormas);

        public abstract void VerificaRecebimentoNaoFiscal(string cRecebimentos);

        public abstract void VerificaTipoImpressora(ref int TipoImpressora);

        public abstract void VerificaModeloECF();

        public abstract void VerificaIndiceAliquotasIss(string cIndices);

        public abstract void ValorFormaPagamento(string cFormaPagamento, string Valor);

        public abstract void ValorTotalizadorNaoFiscal(string Totalizador, string Valor);

        public abstract void VerificaImpressoraLigada();

        public abstract void MapaResumo();

        public abstract void RelatorioTipo60Analitico();

        public abstract void RelatorioTipo60Mestre();

        public abstract void COO(string cCooInicial, string cCooFinal);

        public abstract void RetornaErroExtendido(string cCooInicial);

        public abstract void CRO(string cCRO);

        public abstract void PalavraStatus(string psatus);

        public abstract void PalavraStatusBinario(string pstatusbin);

        public abstract void LerAliquotasComIndice(string aindice);

        #region Funções Exclusivas para MFD

        public abstract void FIMFD_IndicePrimeiroVinculado(string aindice);

        public abstract void FIMFD_CasasDecimaisProgramada(string DecimaisQuantidade, ref string DecimaisValor);

        public abstract void FIMFD_DownloadDaMFD(string COOInicial, string COOFinal);

        public abstract void FIMFD_RetornaInformacao(string Indice, string Valor);

        public abstract void FIMFD_TerminaFechamentoCupomCodigoBarras(string Mensagem, string Tipo, string Codigo, string Largura, string Altura, string Posicao);

        public abstract void FIMFD_ImprimeCodigoBarras(string Tipo, string Codigo, string Largura, string Altura, string Posicao);

        public abstract void FIMFD_StatusCupomFiscal(string cStatus_Cupom);

        public abstract void FIB_AbreBilhetePassagem(string Origem, string Destino, string UF, string Percurso, string Prestadora, string Plataforma, string Poltrona, string Modalidade, string Categoria, string DataEmbarque, string PassRg, string PassNome, string PassEndereco);

        public abstract void FIB_VendeItem(string Descricao, string St, string Valor, string DescontoAcrescimo, string TipoDesconto, string ValorDesconto);

        public abstract void FIMFD_SinalSonoro(string Str_Valor);

        public abstract void FIMFD_EqualizarVelocidade(StringBuilder EqualizarVelocidade);

        public abstract void FIMFD_AcionarGuilhotina();

        public abstract void FIMFD_ProgramaRelatoriosGerenciais(string Str_Valor);

        public abstract void FIMFD_AbreRelatorioGerencial(string Str_Valor);

        public abstract void FIMFD_EmitirCupomAdicional();

        public abstract void FIMFD_RetornaInformacao(string Indice, StringBuilder Valor);

        public abstract void FIMFD_AbreRecebimentoNaoFiscal(string CPF, string Nome, string Endereco);

        public abstract void FIMFD_RecebimentoNaoFiscal(string DescricaoTotalizador, string AcresDesc, string TipoAcresDesc, string ValorAcresDesc, string ValorRecebimento);

        public abstract void FIMFD_IniciaFechamentoNaoFiscal(string AcresDesc, string TipoAcresDesc, string ValorAcresDesc);

        public abstract void FIMFD_EfetuaFormaPagamentoNaoFiscal(string FormaPgto, string Valor, string Observacao);

        public abstract void FIMFD_TerminaFechamentoNaoFiscal(string MsgPromo);

        public abstract void FIMFD_ProgramarGuilhotina(System.String Separacao_Entre_Documentos, System.String Linhas_para_Acionamento_Guilhotina, System.String Status_da_Guilhotina, System.String Impressao_Antecipada_Cliche);

        public abstract void StatusCupomFiscal(StringBuilder cStatus);

        public abstract void GrandeTotal(StringBuilder GrandeTotal);

        public abstract void SubTotal(StringBuilder SubTotal);

        public abstract void Registry_CupomAdicionalDll(System.String CupomAdicional);

        public abstract void Registry_CupomAdicionalDllConfig(System.String CupomAdicional);

        public abstract void Registry_MFD_LeituraMFCompleta(string valor);


        #endregion


        #endregion
        #region Retornos

        public abstract void RetornaGNF(string cGeralNaoFiscal);

        public abstract void RetornaValorComprovanteNaoFiscal(string Indice, string ValorComprNaoFiscal);

        public abstract void RetornaIndiceComprovanteNaoFiscal(string Indice, string IndiceCNF);

        public abstract void RetornaCFCancelados(string cCupomCancelado);

        public abstract void RetornaCNFCancelados(string cCupomNFCancelado);

        public abstract void RetornaCLX(string cLeituraX);

        public abstract void RetornaCRO(string cCRO);

        public abstract void RetornaCRZ(string cReducaoZ);

        public abstract void RetornaCRZRestante(string cReducaoRestante);

        public abstract void RetornaTotalPagamentos(string CtotalFormasPago);

        public abstract void RetornaTroco(string cTroco);

        public abstract void RetornaCNFNV(string cCNaoFiscalNaoVinculado);

        public abstract void RetornaDescricaoCNFV(string cDescricaoNaoFiscalVinculado);

        public abstract void RetornaDescontoNF(string cDescNF);

        public abstract void RetornaAcrescimoNF(string cAcrescimoNF);

        public abstract void RetornaCancelamentoNF(string cCancelamentoNF);

        public abstract void RetornaCNFV(string CContadorNFVinculado);

        public abstract void RetornaTempoLigado(string cTimeLigado);

        public abstract void RetornaTempoImprimindo(string CTimeImprimindo);

        public abstract void RetornaRegistradoresNaoFiscais(string CRgistradoresNF);

        public abstract void RetornaRegistradoresFiscais(string CRgistradoresFiscais);

        #endregion
        #region Configuracaoes do ECF

        public abstract void Cfg(int VariavelDoEcf, string VarConfigEcf);

        public abstract void CfgFechaAutomaticoCupom(string VarConfigEcf);

        public abstract void CfgRedZAutomatico(string VarConfigEcf);

        public abstract void CfgImpEstGavVendas(string VarConfigEcf);

        public abstract void CfgLeituraXAuto(string VarConfigEcf);

        public abstract void CfgCalcArredondamento(string VarConfigEcf);

        public abstract void CfgHorarioVerao(string VarConfigEcf);

        public abstract void CfgSensorAut(string VarConfigEcf);

        public abstract void CfgCupomAdicional(string VarConfigEcf);

        public abstract void CfgEspacamentoCupons(string VarConfigEcf);

        public abstract void CfgHoraMinReducaoZ(string VarConfigEcf);

        public abstract void CfgLimiarNearEnd(string VarConfigEcf);

        public abstract void CfgPermMensPromCNF(string VarConfigEcf);

        public abstract void CfgLegProdutos(string VarConfigEcf);
        #endregion
        #region Outras
        public abstract void AbrePortaSerial();

        public abstract void FechaPortaSerial();

        public abstract void AberturaDoDia(string valor, string formaPagamento);

        public abstract void FechamentoDoDia();

        public abstract void ImprimeConfiguracoesImpressora();

        public abstract void RegistraNumeroSerie();

        public abstract void VerificaNumeroSerie();

        public abstract void RetornaSerialCriptografado(string SerialCriptografado, string NumeroSerial);

        public abstract void ConfiguraHorarioVerao(string DataEntrada, string DataSaida, string Controle);

        public abstract void ImprimeTexto(string texto, string fonte, bool italic, bool underline, bool extended, bool negrito);
        #endregion
        #region Programacao do ECF

        public abstract void AlteraSimboloMoeda(string cMoeda);

        public abstract void ProgramaAliquota(string cAliquota, int ICMS_ou_ISS);

        public abstract void ProgramaHorarioVerao();

        public abstract void NomeiaTotalizadorNaoSujeitoIcms(int iIndice, string cTotalizador);

        public abstract void ProgramaArredondamento();

        public abstract void ProgramaTruncamento();

        public abstract void LinhasEntreCupons(int Linhas);

        public abstract void EspacoEntreLinhas(int Dots);

        public abstract void ForcaImpactoAgulhas(int ForcaImpacto);

        public abstract void ResetaImpressora();

        public abstract void ProgramaFormasPagamento(string FormaPagamento);

        public abstract void ProgFormasPagtoSemVincular(string FormaPagamento);

        public abstract void ProgramaVinculados(string DescricaoVinculado);

        public abstract void EqualizaFormasPgto();

        public abstract void ProgramaOperador(string Operador);
        #endregion
        #region Metodos de Autenticacao e Gaveta

        public abstract void AutenticacaoStr(string TextoStr);

        public abstract void Autenticacao();

        public abstract void VerificaDocAutenticacao();

        public abstract void AcionaGaveta();

        public abstract void VerificaEstadoGaveta(ref int Estado);

        public abstract void VerificaEstadoGavetaStr(string EstadoGaveta);
        #endregion
        #region Metodos de TEF
        public abstract void TEF_EsperarArquivo(string cArquivo, string cTempo, string cTravar);
        public abstract void TEF_ImprimirResposta(string cArquivoResp, string cForma, string cTravar);
        public abstract void TEF_ImprimirRespostaCartao(string cArquivoResp, string cForma, string cTravar, string ValorPago);
        public abstract void TEF_SetFocus(string cWndFocus);
        public abstract void TEF_TravarTeclado(string cTravar);
        public abstract void TEF_FechaRelatorio();
        #endregion

        /// <summary>
        /// Funcao que analiza o retorno da impressora
        /// FrameWork Daruma32.dll
        /// </summary>
        public virtual void Mostrar_Retorno()
        {
            string Str_Erro_Extendido = new string(' ', 4); ;

            int Int_ACK = 0;
            int Int_ST1 = 0;
            int Int_ST2 = 0;

            RetornoImpressora(ref Int_ACK, ref Int_ST1, ref Int_ST2);

            RetornaErroExtendido(Str_Erro_Extendido);


            string message = "Retorno do Metodo = '+ eval(retorno) +'\r\n"
            + "ACK = " + Int_ACK.ToString() + "\r\n"
            + "ST1 = " + Int_ST1.ToString() + "\r\n"
            + "ST2 = " + Int_ST2.ToString() + "\r\n"
            + "Erro Extendido = " + Str_Erro_Extendido.ToString();

            scripts.Add("alert('" + message + "');");
        }        

        public abstract string Render();        
    }
}

