function CalculateDiscount() {
    //
    // Pegar os valores do SubTotal e do desconto, no momento em que é executada a função.
    // Evitando assim, erros nos cálculos matemáticos.
    //

    var discount = txtDiscount.value.replace(".", "").replace(",", ".");
    var addtionalCost = txtAdditionalCost.value.replace(".", "").replace(",", ".");
    var total;

    //
    // Se no texto do campo desconto, houver o caractere (%), o cálculo se dará em percentagem
    //  

    if (discount.indexOf("%") >= 0) {
        discount.replace("%", "");
        total = subTotal - (subTotal * (parseFloat("0" + discount) / 100));
    }
    else {
        if (discount.indexOf("-") >= 0) {
            discount.replace("-", "");
            total = subTotal + parseFloat(discount);
        }
        else
            total = parseFloat("0" + subTotal) - parseFloat("0" + discount);
    }

    total = total + parseFloat("0" + addtionalCost);

    lblTotal.innerHTML = total.localeFormat("N");
}

