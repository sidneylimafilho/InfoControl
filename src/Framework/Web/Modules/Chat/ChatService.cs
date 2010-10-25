using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace InfoControl.Web.Modules.Chat
{
    // NOTE: If you change the class name "TooltipService" here, you must also update the reference to "TooltipService" in Web.config.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceContract(Namespace = "$")]
    public class ChatService : InfoControl.Web.Services.DataService
    {
        [OperationContract]        
        public List<ChatMessage> CheckMessages()
        {
            return ChatEngine.Engine.GetMessages(User.Identity.UserId).ToList();
        }

        [OperationContract]
        public void SendMessage(string roomId, string message)
        {
            ChatEngine.Engine.AddMessage(roomId, User.Identity.UserId, User.Identity.Profile.FirstName, message);
        }

        [OperationContract]
        public void InviteUser(string roomId, int userId)
        {
            //From
            ChatEngine.Engine.InviteUser(roomId, User.Identity.UserId);
            //To
            ChatEngine.Engine.InviteUser(roomId, userId);
        }
    }
}
