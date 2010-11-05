
$(function() {

    var discount = $("#ctl00_ContentPlaceHolder_txtDiscount");

    window.lblTotal = $('[id*=lblTotal]');
    window.lblSubTotal = $('[id*=lblSubtotal]');
    window.txtDiscount = $('[id*=txtDiscount]');

    discount.blur(function() {
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
    if (lblSubTotal) var subTotal = lblSubTotal.html().replace(".", "").replace(",", ".");
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

    lblTotal.html($.format(subTotal, "N"));
}

function ApplyPaymentMethod() {    
    top.$.lightbox('POS/Sale_Parcels.aspx?lightbox[iframe]=true&total=' + lblTotal.html())
}


