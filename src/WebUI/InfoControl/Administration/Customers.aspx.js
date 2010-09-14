
$("#accountSearchBody").hide();
$("#importDataBody").hide();

$("#accountSearchLegend").mouseover(function () {
    $("#importDataBody").hide("slow");
    $("#accountSearchBody").show("slow");
    $("#filter").attr({ className: "open" });
});

$("#importDataLegend").mouseover(function () {
    $("#accountSearchBody").hide("slow");
    $("#importDataBody").show("slow");
    $("#filter").attr({ className: "open" });
});

$("#closeFilter").mouseover(function () {
    $("#importDataBody").hide("slow");
    $("#accountSearchBody").hide(500, function () {
        $("#filter").attr({ className: "closed" })
    })
});

var line;

$(".delete").click(
    function () {
        var event = arguments[0] || window.event;
        event.cancelBubble = true;
        event.stopPropagation();

        line = $(this.parentNode.parentNode);

        var request = PageMethods.DeleteCustomer(
            $(this).attr("customerid"),
            $(this).attr("companyid"),
            function (result, response, context) {
                if (result) {
                    line.removeClass().addClass('Items_Selected').hide();
                } else {
                    alert("O registro não pode ser apagado pois há outros registros associados!");
                }
            }
        );
    }
);