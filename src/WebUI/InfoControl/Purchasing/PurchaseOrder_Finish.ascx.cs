using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using InfoControl;
using InfoControl.Web.Mail;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


using Exception = Resources.Exception;

public partial class Company_POS_PurchaseOrder_Finish : Vivina.Erp.SystemFramework.UserControlBase<Company_POS_PurchaseOrder>
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            


        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.SavePurchaseOrder() == null)
            return;
        Server.Transfer("PurchaseOrders.aspx");
    }

    


    #region Ações


    

    

    private void ExportDocumentTemplate(string fileName, DocumentTemplate documentTemplate)
    {
        var purchaseOrderManager = new PurchaseOrderManager(this);

        //
        // Limpa os buffers de resposta, para não enviar os cabeçalhos da página ASPX
        //
        Response.Clear();
        Response.ContentType = "text/doc";
        //
        // Seta o cabeçalho que irá dizer ao navegador que é para fazer download, ao invés de apresentar na tela.
        //
        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".htm");
        //
        // Indica que o formato do arquivo será o mesmo do modelo
        //
        Response.ContentEncoding = Encoding.Default;

        //
        // Busca a localização do modelo no servidor e retorna seu conteúdo
        //
        string contentFile = File.ReadAllText(Server.MapPath(documentTemplate.FileUrl));

        //
        // Aplica as trocas das máscaras no modelo, ou seja, substitui os []'s pelo conteúdo e retorna
        //
        contentFile = purchaseOrderManager.ApplyPurchaseOrderInDocumentTemplate(Page.PurchaseOrder, contentFile);
        //
        // Envia o arquivo para o cliente
        //
        Response.Write(contentFile);

        //
        // Interrompe o processamento da página para que o conteúdo do modelo não se misture
        // ao HTML da página PurchaseOrder.aspx
        //
        Response.End();
    }

    #endregion

    


}