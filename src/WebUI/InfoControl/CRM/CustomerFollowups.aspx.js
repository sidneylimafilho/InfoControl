$(".delete").click(function()
{
    var event = arguments[0] || window.event;
    event.cancelBubble = true;
    event.stopPropagation();
    
    PageMethods.DeleteCustomerFollowup($(this).attr("id")); 
    $(this.parentNode.parentNode).removeClass().addClass('Items_Selected').hide();
});