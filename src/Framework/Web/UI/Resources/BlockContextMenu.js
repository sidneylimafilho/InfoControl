document.oncontextmenu = function (e) 
{
    var ctrl = document.all ? event.srcElement : e.target;
    var tagName = ctrl.tagName.toUpperCase();

    return (tagName == "INPUT" && ctrl.type.toUpperCase()=="TEXT" ) || tagName == "SELECT" || tagName == "TEXTAREA" ;
}

