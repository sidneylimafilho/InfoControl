if (!window.InfoControl) window.InfoControl = { };
if (!window.InfoControl.ButtonAdapter) window.InfoControl.ButtonAdapter = { };

InfoControl.ButtonAdapter.AttachEvents = function(elementId)
{
    var obj = document.getElementById(elementId);
    obj.onmouseover = GlassButton_MouseOver;
    obj.onmouseout =  GlassButton_MouseOut;    
}
    
function GlassButton_MouseOver(e)
{     
    var span = document.all ? event.srcElement.parentNode : e.target.parentNode;           
    span.className += "_hover"; 
}


function GlassButton_MouseOut(e)
{ 
    var span = document.all ? event.srcElement.parentNode : e.target.parentNode;
    span.className = span.className.replace("_hover", ""); 
}

