
$(function () {

    var discount = $("#ctl00_ContentPlaceHolder_txtDiscount");

    discount.blur(function () {
        if (isNaN($(this).val().replace("%", "").replace(",", "."))) {
            $(this).attr("value", "0,00");
            return;
        }
    });
});

function CalculateDiscount(control) {
    //
    // Pegar os valores do SubTotal e do desconto, no momento em que é executada a função.
    // Evitando assim, erros nos cálculos matemáticos.
    //
    if (lblSubTotal) var subTotal = lblSubTotal.innerHTML.replace(".", "").replace(",", ".");
    var discount = control.value.replace(".", "").replace(",", ".");

    //
    // Se no texto do campo desconto, houver o caractere (%), o cálculo se dará em percentagem
    //
    if (discount.indexOf("%") >= 0) {
        discount.replace("%", "");
        subTotal = subTotal - (subTotal * (parseFloat("0" + discount) / 100));
    }
    else if (discount.indexOf("-") >= 0) {
        subTotal = subTotal + parseFloat(discount);
    }
    else {
        subTotal = parseFloat("0" + subTotal) - parseFloat("0" + discount);
    }

    lblTotal.innerHTML = subTotal.localeFormat("N");
}

function ApplyPaymentMethod() {
    if (lblTotal) $cookies('total', lblTotal.firstChild.nodeValue, { expires: 999999 });

    top.tb_show('Formas de Pagamento', 'POS/Sale_Parcels.aspx')
}