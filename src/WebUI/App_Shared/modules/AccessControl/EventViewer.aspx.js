$(".delete").click(function()
{
    var event = arguments[0] || window.event;
    event.cancelBubble = true;
    event.stopPropagation();
    
    PageMethods.DeleteEvent($(this).attr("id"), $(this).attr("userId")); 
    $(this.parentNode.parentNode).removeClass().addClass('Items_Selected').hide();
});

