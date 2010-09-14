$("#filterBody").hide();
$("#exportBody").hide();

$("#filterLegend").mouseover(function () {
    $("#exportBody").hide("slow");
    $("#filterBody").show("slow");
    $("#filter").attr({ className: "open" });
});
$("#exportLegend").mouseover(function () {
    $("#filterBody").hide("slow");
    $("#exportBody").show("slow");
    $("#filter").attr({ className: "open" });
});

$("#closeFilter").mouseover(function () {
    $("#exportBody").hide("slow");
    $("#filterBody").hide(500, function () {
        $("#filter").attr({ className: "closed" })
    })
});


function RowMouseOver(sender, eventArgs) { $get(eventArgs.get_id()).className += " RadGrid_Items_Hover"; }
function RowMouseOut(sender, eventArgs) { $get(eventArgs.get_id()).className = "Item"; }