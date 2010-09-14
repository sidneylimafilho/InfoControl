var line;

$(".delete").click(
    function()
    {
        var event = arguments[0] || window.event;
        event.cancelBubble = true;
        event.stopPropagation();
        
        line = $(this.parentNode.parentNode);
        
        var request = PageMethods.DeleteReceipt($(this).attr("id"),$(this).attr("companyid"),
            function(result, response, context)
            {
                if(result)
                    line.removeClass().addClass('Items_Selected').hide();
                else
                    alert("O registro não pode ser apagado pois há outros registros associados!");
            }
        );
    }
);