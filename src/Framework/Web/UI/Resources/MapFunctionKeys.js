document.onkeydown=function(e)
{
    e = document.all ? event : e;
    if(e.keyCode > 111 && e.keyCode < 124)
    {
        var key = "F" + (e.keyCode - 111);
        var list = document.getElementsByTagName("INPUT");
        for(i=0; i<list.length; i++)
        {            
            if(list[i].getAttribute("accesskey") == key)
            {      
                list[i].click();         
                if(document.all) e.keyCode = 0;
                return false;
            }
        }
    }
    return true;
}