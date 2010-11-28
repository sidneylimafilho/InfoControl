function RefreshChatSessions()
{ 
    /* Check messages */
    ChatService.CheckMessages(
        function(result){ 
            for(i=0; i< result.length; i++)
            {   
                InitializeChatRoom(result[i]);
            }
            ChatService.timeoutId = setTimeout("RefreshChatSessions()", 5000);   
        }
    );
    clearTimeout(ChatService.timeoutId); 
};

function InitializeChatRoom (chatMessage){   
    
    var roomId = chatMessage.RoomId;
    
    if(ChatService[roomId] == undefined)
        ChatService[roomId] = { lastMessageId : -1, history:""};
      
    
    if(ChatService[roomId].lastMessageId < chatMessage.ID && chatMessage.Description != "")
    {
        var room = OpenChat(0, "", chatMessage.RoomId);
        
        ChatService[roomId].lastMessageId = chatMessage.ID;
        
        if(room.find(".chatpane").html()=="")
            room.find(".chatpane").html("<span class='history'>" + ChatService[roomId].history + "</span>");
            
        FormatMessage(room.find(".chatpane"), chatMessage.UserName, chatMessage.Description);
    
    }
}


function SendMessage(room)
{
    var chatPane = room.find(".chatpane");
    var textArea = room.find("textarea");
   
    if(textArea[0].value != "")
    {
        FormatMessage(chatPane, "Eu", textArea[0].value);        
        ChatService.SendMessage(room.data("id"), textArea[0].value);    
        textArea[0].value='';        
        textArea[0].focus();
    }
    
}

function FormatMessage(chatPane, userName, text)
{
    if(text != ""){
        chatPane.html(chatPane.html() + "<span class='user'>"+ userName +":</span><p>"+ text + "</p><br />");
        chatPane[0].scrollTop = chatPane[0].scrollHeight;
        
        //
        // Blink the status bar
        //
        top.window.focus();
    }
}

function OpenChat(userId, header, roomId)
{
    if(roomId == undefined)
        roomId = (new Date()).getMilliseconds();
       
    if(header == undefined)        
        header = "";
        
    var room = $("#room_" + roomId);         
    if(room.size()==0) 
    {       
        $.jGrowl("<div class='chatpane'></div><textarea></textarea>", {
             id: "room_" + roomId, 
             header: header,
             beforeClose : function(){
                if(ChatService[roomId])
                    ChatService[roomId].history = this.find(".chatpane").html();
             }                 
        });
    }
    
    room = $("#room_" + roomId);
    room.data("id", roomId);    
    room.draggable({opacity: 0.66});  
        
    //
    // Attach onKeyDown for enter key fire SendMessage
    //
    room.find("textarea").keyup(function(e){        
        if(e.keyCode == 13 || e.which == 13)
            SendMessage($(this).parent().parent());
    });             
    
    
    if(userId > 0) 
        ChatService.InviteUser(roomId, userId); 
        
    //
    // Blink the status bar
    //
    top.window.focus();
        
    return room;
}