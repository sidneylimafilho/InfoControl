(function($){

    //
    // Constructor
    //
    $.chat = function(serviceUrl){
        this.ServiceUrl = serviceUrl;
        $(this).refresh();
    }
    
    $.extend( $.chat.prototype , {
    
        /* Function refresh status  */
        refresh : function()
        {
            clearInterval(this.intervalID);
            
            //
            // Check messages
            //
            $.ajax({
                async: true,
                type: "POST",
                url: this.ServiceUrl + "/CheckMessages",
                sucess: function(result){
                    $("#chatpane").innerHTML += result;
                }
            });  
            
            //
            // Check new chat invite
            //
            
            //
            // 
            //
            
            this.intervalID = top.setInterval("$.chat.refresh()", 2000); 
        },
        
        /* When the user choose a contact to call*/
        newRoom : function(contactName, contactId){            
            $.jGrowl("<textarea></textarea>", "id", {
                header: contactName                
            });
        },
        
        /* When the user send the message*/
        send : function(roomId, msg)
        {
            //
            // Get variables
            //
            var textarea = $("#chatRoom textarea");
            var msg = textarea.value;          
           
            $.ajax({
                async: true,
                type: "POST",
                url: this.ServiceUrl + "/send",
                data: {
                    message: encodeURIComponent(msg),
                    roomID: this.RoomID
                }
                sucess: function(result){
                    
                }
            });
            
            textarea.value = "";
        }
    });

    
    
    
    
})(jQuery);



function getBufferText()
{
    rnd++;
	url = 'Server.aspx?action=GetMsg&session=' + rnd;
	req = getAjax();
	
	req.onreadystatechange = function(){
	
		if( req.readyState == 4 && req.status == 200 ) {
		
			obj = getElement( "chatbuffer" );
			obj.innerHTML = req.responseText;
			scrollChatPane();
			//FocusWindow();
		}
	}
	
	req.open( 'GET', url , true );
	req.send( null );
}

function postText()
{
	rnd++;
	chatbox = getElement( "mytext" );
	chat = chatbox.value;
	chatbox.value = "";
	
	userid = location.search.substring( 1, location.search.length );
	url = 'Server.aspx?action=PostMsg&u=' + userid + '&t=' + encodeURIComponent(chat) + '&session=' + rnd;
	
	req = getAjax();
	
	req.onreadystatechange = function(){
	
		if( req.readyState == 4 && req.status == 200 ) {
			updateAll();
		}
	
	}
	
	req.open( 'GET', url, true );
	req.send( null );
}



function scrollChatPane()
{
	var obj = $("#chatpane");
	obj.scrollTop = obj.scrollHeight;
}