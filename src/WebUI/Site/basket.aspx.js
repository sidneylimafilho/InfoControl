//var isDisplayed;


//(function($) {

//    CalcularValorDoFrete();
//    rdbConditionParcels_Click();
//    Validate();

//})(jQuery);

////
////This function get value of delivery deal with prices supplied of Sedex company
////
//function CalcularValorDoFrete() {

//    $("#ctl00_ContentPlaceHolder_ucPayment_txtPostalCode").blur(

//                function() {
//                    PageMethods.CalcularFrete(
//                    $("#peso").attr("value"),
//                    $("#valor").attr("value"),
//                    $("#cepOrigem").attr("value"),
//                    $(this).val(), function(result) {

//                        $("#deliveryPrices").show();
//                        $("#paymenMethodFieldSet").show();

//                        $("#ctl00_ContentPlaceHolder_ucPayment_lblPacPrice").html("R$ " + result[0]);
//                        $("#ctl00_ContentPlaceHolder_ucPayment_lblSedexPrice").html("R$ " + result[3]);
//                        $("#ctl00_ContentPlaceHolder_ucPayment_lblSedex10Price").html("R$ " + result[2]);
//                    });
//                }
//    );
//}

////
////This function validate if the paymentMethod and address delivery are filled
////
//function Validate() {
//    $("#linkNext").click(function() {

//        if ($(this).attr("href") == "CreateCustomer.aspx") {
//            alert("Escolha uma forma de pagamento!");
//            return false;
//        }

//        if ($("#ctl00_ContentPlaceHolder_txtPostalCode").attr("value") == "") {
//            alert("Informe o cep de entrega!");
//            $("#ctl00_ContentPlaceHolder_txtPostalCode").focus();
//            return false;
//        }

//    });
//}

//function rdbConditionParcels_Click() {

//    $("input[type='radio']").attr("class", "finacierCondition");

//    $(".finacierCondition").click(function() {

//        $("#linkNext").attr("href", "CreateCustomer.aspx?fcId=" + $(this).attr("value") + "&amount=" + $("#valueByParcel").html());

//    });

//}
