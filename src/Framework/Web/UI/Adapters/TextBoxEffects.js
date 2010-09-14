if (!window.InfoControl) window.InfoControl = { };
if (!window.InfoControl.TextBoxAdapter) window.InfoControl.TextBoxAdapter = { };

InfoControl.TextBoxAdapter.AttachEvents = function(elementId)
{
    var obj = document.getElementById(elementId);

    obj.parentNode.onmouseover = GlassTextBox_MouseOver;
    obj.parentNode.onmouseout = GlassTextBox_MouseOut;
    obj.parentNode.onclick = GlassTextBox_Click; 
    obj.keypress = GlassTextBox_KeyPress;   
    
}

function GlassTextBox_Click(e)
{
    var obj = document.all ? event.srcElement : e.target;
 
    if(obj.tagName == "SPAN"){
        if(obj.childNodes.length>0)
           obj.childNodes[0].focus();
    }else{
        obj.focus();
    }
}

function GlassTextBox_MouseOver(e)
{   
    var obj = document.all ? event.srcElement : e.target;
    if(obj.parentNode.isFocused || obj.tagName != "INPUT") return;    
    obj.parentNode.className += "_hover"; 
    
}

function GlassTextBox_MouseOut(e)
{  
    var obj = document.all ? event.srcElement : e.target;
    if(obj.parentNode.isFocused) return;
    obj.parentNode.className = obj.parentNode.className.replace("_hover", ""); 
}

function GlassTextBox_Focus(obj)
{    
    obj.parentNode.isFocused = true;  
    obj.parentNode.className = obj.parentNode.className.replace("_hover", "") + "_focus"; 
}

function GlassTextBox_Blur(obj)
{       
    obj.parentNode.isFocused = false;
    obj.parentNode.className = obj.parentNode.className.replace("_hover", "").replace("_focus", ""); 
}

function GlassTextBox_KeyPress(e)
{
    alert(e.KeyCode);
}