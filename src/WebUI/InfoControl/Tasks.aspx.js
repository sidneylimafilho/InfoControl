function CompleteTask(object)
{
    var line = object;        
    while(line.tagName.toUpperCase() != "LI")
         line = line.parentNode;
         
    line.className+=' deleted';        
    
    var request = PageMethods.CompleteTask(            
        $(object).attr("companyId"),
        $(object).attr("taskId"), 
        $(object).attr("userId")
    );
}