(function () {
    $(".avancar").click(function () {
        var radioButton = $(":radio:checked");
        var parcelCount = radioButton.val();
        var operationId = radioButton.parents(".financierConditions").attr("FinancierOperationId");

        if ($("form").valid())
            if (parcelCount != null) {
                var options = "toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=1,screenX=0,screenY=0,left=0,top=0,width=765,height=440";
                options += (navigator.appName.indexOf("Netscape") != -1) ? ",resizable=0" : ",resizable=0";
                //mpg_popup = window.open("Checkout_PaymentProcess.aspx?methodId=" + val, "mpg_popup", options);

                $("#pagamento").fadeOut("slow")
                .next().fadeIn("slow")
                .attr("src", "Checkout_Summary.aspx?operationId=" + operationId + "&" + $("FORM").serialize());

            } else {
                alert("Escolha uma forma de pagamento!");
            }

        return false;
    });

    $(":radio").click(function () {
        $(".formAvancar").fadeOut("slow");
        $(this).parents(".financierConditions").eq(0).find(".formAvancar").fadeIn("slow");
    });

    $(".paymentMethod").click(function () {
        $('.lstParcels, .formAvancar').fadeOut('slow');
        $(this).parents('.financierConditions').find('.lstParcels').fadeIn("slow")
    });
})();