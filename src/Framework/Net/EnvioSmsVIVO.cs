using System;
using System.IO;
using System.Text;
using System.Net;
using System.Data;
using System.Configuration;
using System.Collections;


using InfoControl.Net;

public partial class TesteEnvioSMS 
{
    

    public void Send()
    {
        HttpWebClient loHttp = new HttpWebClient();
        string result;
        loHttp.HandleCookies = true;

        
        StringBuilder sb = new StringBuilder();
        sb.Append("{actionForm.statusCadastro}=S&");
        sb.Append("{actionForm.foneArea}=21&");
        sb.Append("{actionForm.foneNumero}=98564467&");
        sb.Append("{actionForm.senha}=3734&");

        //
        // PASSO 1
        //
        string loginPage = "https://servicos.vivo.com.br/VOLWeb/LoginAction.do";
        result = loHttp.GetUrl(loginPage +"?"+ sb.ToString());



        //
        // PASSO 2
        //
        string cookiePage = "https://servicos.vivo.com.br/VOLWeb/servicos/torpedoEmpresas.do?idMenu=494&idSubMenu=1967";
        result = loHttp.GetUrl(cookiePage);

        //
        // PASSO 3
        //
        int beginScriptTag = result.IndexOf("https://");
        int endScriptTag = result.IndexOf("');");
        string nextPage = result.Substring(beginScriptTag, endScriptTag - beginScriptTag);
        result = loHttp.GetUrl(nextPage);

        //
        // PASSO 4
        //
        string crossPage = "https://online.vivo-rjes.com.br/torpedoempresa/RecuperaDadosEnvio?action=RecuperaDadosEnvio";
        result = loHttp.GetUrl(crossPage);

        //
        // PASSO 5
        //
        loHttp.PostMode = PostMode.MultiPart;
        loHttp.AddPostKey("Titulo","Empresa");
        loHttp.AddPostKey("TipoEnvio","0");
        loHttp.AddPostKey("Periodicidade","4");
        loHttp.AddPostKey("ListaEnvio","0");
        loHttp.AddPostKey("Operadora","11");
        loHttp.AddPostKey("MinutoAtual", DateTime.Now.Minute.ToString() );
        loHttp.AddPostKey("HoraAtual", DateTime.Now.Hour.ToString());
        loHttp.AddPostKey("AnoAtual", DateTime.Now.Year.ToString());
        loHttp.AddPostKey("MesAtual", DateTime.Now.Month.ToString());
        loHttp.AddPostKey("DiaAtual", DateTime.Now.Day.ToString());
        loHttp.AddPostKey("NumDiasValidadeAgendamento","30");
        loHttp.AddPostKey("NumDestinatario","0");
        loHttp.AddPostKey("TelefoneCompleto","2181243318");
        loHttp.AddPostKey("action","EnviaShortMessageAnexadaUmaMensagem");
        loHttp.AddPostKey("DestinatariosMensagem","com.tcbnet.webapp.torpedotelefonicanet.envio.LeafScheduleRecipient,2181243318;");
        loHttp.AddPostKey("Mensagem","Teste de Envio " + DateTime.Now.ToString() );
        string sendSmsPage = "https://online.vivo-rjes.com.br/torpedoempresa/EnvioAnexado";
        result = loHttp.PostUrl(sendSmsPage);
        

    }

    
}
