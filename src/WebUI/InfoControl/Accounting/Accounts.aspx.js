$(".delete").click(
    function () {

        var event = arguments[0] || window.event;
        event.cancelBubble = true;
        event.stopPropagation();

        var line = $(this.parentNode.parentNode);        

        var request = PageMethods.DeleteAccount($(this).attr("accountId"), $(this).attr("companyId"),
            function (result, response, context) {
                if (result)
                    line.removeClass().addClass('Items_Selected').hide();
                else
                    alert("O registro não pode ser apagado pois há outros registros associados!");
            }
        );
    }
);

