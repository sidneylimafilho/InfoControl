
function DataRow_MouseOver(sender)
{ 
    sender.classNameOld = sender.className; 
    sender.className = 'Items_Hover';
}

function DataRow_MouseOut(sender)
{ 
    sender.className = sender.classNameOld;
}

function DataRow_Click(sender)
{ 
    sender.className = (sender.classNameOld == 'Items_Selected') ? 'Items' : 'Items_Selected' ; 
    sender.classNameOld = sender.className;
}


