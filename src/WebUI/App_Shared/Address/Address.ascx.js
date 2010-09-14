if(!top.InfoControl)top.InfoControl = {};
top.InfoControl.Address = {};
top.InfoControl.Address.ShowMap=function (id)
{
    var txtPostalCode = document.getElementById(id);      
    if(txtPostalCode.value != "" && top.$.modal.IsClosed)
        top.tb_show('Visualização no Mapa', '../App_Shared/Address_Maps.htm?cep=' + txtPostalCode.value.substring(0, 5) + '-' + txtPostalCode.value.substring(5, 8));
}    




