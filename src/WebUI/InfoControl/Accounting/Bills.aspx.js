$("#filterLegend").mouseover(function(){     
    $("#filterBody").show("slow"); 
    $("#filter").attr({className:"open"});
});

$("#closeFilter").mouseover(function(){     
    $("#filterBody").hide(500, function(){$("#filter").attr({className:"closed"})})
});

                           
function RowMouseOver(sender, eventArgs){$get(eventArgs.get_id()).className += " RadGrid_Items_Hover";}
function RowMouseOut(sender, eventArgs){$get(eventArgs.get_id()).className = "Item"; }
               