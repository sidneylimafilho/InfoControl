//(function($) {

//    CalcularValorDoFrete();
//    rbtConditionParcel_Click();
//    Validate();
//    $("#deliveryPrices").hide();


//})(jQuery);


////
////This function get value of delivery deal with prices supplied of Sedex company
////
//function CalcularValorDoFrete() {

//    $("#ctl00_ContentPlaceHolder_ucPayment_txtPostalCode").blur(

//                function() {
//                    PageMethods.CalcularFrete(
//                    $(this).attr("peso"),
//                    $(this).attr("valor"),
//                    $(this).attr("cepOrigem"),
//                    $("#FreteForm input:text").val(), function(result) {

//                        $("#deliveryPrices").show();

//                        $("#ctl00_ContentPlaceHolder_ucPayment_lblPacPrice").html(result[0]).attr("value", result[0]);
//                        $("#ctl00_ContentPlaceHolder_ucPayment_lblSedexPrice").html(result[2]).attr("value", result[2]);
//                        $("#ctl00_ContentPlaceHolder_ucPayment_lblSedex10Price").html(result[3]).attr("value", result[3]);
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
