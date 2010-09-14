using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace InfoControl.Web.Modules.Chat
{
    public sealed class ChatEngine
    {
        #region Members
        const string messageFormat = "<li class=\"{0}\">{1}</li>\r\n";
        //const string userlistfmt = "<li>{0}</li>\r\n";

        //const string serverstyle = "servermsg";
        //const string userstyle = "usermsg";
        //const string actionstyle = "actionmsg";

        //const string timeoutfmt = "User {0} timed out";
        //const string textlimitfmt = "{0}, your message was {1} characters long.  The limit is {2} characters.";
        //const string nickinusefmt = "{0} a user with this nick already exists";
        //const string joinedfmt = "User {0} has joined";
        //const string nickfmt = "{0} is now known as {1}";
        //const string killfmt = "<strong>User {0} has been terminated!</strong>";
        //const string clearfmt = "<strong>User {0} has cleared all chat log!</strong>";
        //const string unknowcmdfmt = "<strong>User {0} : Unknow command</strong>";
        //const string mefmt = "{0} {1}";
        //const string txtfmt = "{0}: {1}";

        //const long timerdelay = 300000;
        //const int maxbuffer = 20;


        Timer timer;

        Hashtable users;
        StringCollection chat;
        StringCollection pings;

        static object syncRoot = new object();

        #endregion

        private static ChatEngine _engine;
        public static ChatEngine Engine
        {
            get
            {
                if (_engine == null)
                    lock (syncRoot)
                        if (_engine == null)
                        {
                            _engine = new ChatEngine();
                        }

                return _engine;
            }
        }

        private Dictionary<string, string> usersByRoom;
        private List<ChatMessage> messages;


        /// <summary>
        /// Default constructor
        /// </summary>
        private ChatEngine()
        {
            messages = new List<ChatMessage>();
            usersByRoom = new Dictionary<string, string>();
        }



        ///// <summary>
        ///// Check for pinged out users
        ///// </summary>
        //private void TimerTick(object state)
        //{
        //    lock (syncRoot)
        //    {
        //        string[] current = new string[users.Keys.Count];
        //        users.Keys.CopyTo(current, 0);

        //        foreach (string guid in current)
        //        {
        //            if (!pings.Contains(guid))
        //            {
        //                chat.Add(
        //                    this.MakeServerMessage(
        //                        string.Format(timeoutfmt, users[guid].ToString())
        //                    )
        //                );

        //                users.Remove(guid);
        //            }
        //        }
        //        pings.Clear();
        //    }
        //}
        ///// <summary>
        ///// Gets the current list of
        ///// users
        ///// </summary>
        //public string[] Users
        //{
        //    get
        //    {
        //        string[] nicks = new string[users.Count];
        //        users.Values.CopyTo(nicks, 0);
        //        return nicks;
        //    }
        //}

        ///// <summary>
        ///// HTML Formatted user list
        ///// </summary>
        //public string UserList
        //{
        //    get
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        string[] nicks = new string[users.Count];
        //        users.Values.CopyTo(nicks, 0);

        //        foreach (string user in nicks)
        //        {
        //            sb.Append(string.Format(userlistfmt, user));
        //        }

        //        return sb.ToString();
        //    }
        //}
        ///// <summary>
        ///// Gets the current buffer of chat text
        ///// </summary>
        //public string BufferText
        //{
        //    get
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        foreach (string line in chat)
        //        {
        //            sb.Append(line);
        //        }

        //        return sb.ToString();
        //    }
        //}
        ///// <summary>
        ///// Does a user exist based on a guid
        ///// </summary>
        //public bool GuidExists(string guid)
        //{
        //    return users.ContainsKey(guid);
        //}
        ///// <summary>
        ///// Checks to see if the current user 
        ///// exists in the list
        ///// </summary>
        //public bool UserExists(string user)
        //{
        //    return users.ContainsValue(user);
        //}
        ///// <summary>
        ///// Adds a user to the list
        ///// </summary>
        //public void AddUser(string id, string user)
        //{
        //    lock (syncRoot)
        //        if (!UserExists(user))
        //        {
        //            users.Add(id, user);
        //            pings.Add(id);

        //            chat.Add(
        //                this.MakeServerMessage(
        //                    string.Format(joinedfmt, user)
        //                )
        //            );
        //        }
        //}

        ///// <summary>
        ///// Pings this chat server and keeps 
        ///// a user alive
        ///// </summary>
        //public void Ping(string guid)
        //{
        //    lock (syncRoot)
        //        pings.Add(guid);
        //}

        

        public void InviteUser(string roomId, int userId)
        {
            string users = GetUsersByRoom(roomId);
            if (!users.Contains(userId.ToString()))
                users += String.Format(" {0},", userId);

            usersByRoom[roomId] = users;
        }

        /// <summary>
        /// Adds text to the buffer
        /// </summary>
        public void AddMessage(string roomId, int userId, string userName, string text)
        {
            InviteUser(roomId, userId);

            if (!String.IsNullOrEmpty(text))
                messages.Add(MakeChatMessage(roomId, userId, userName, text));
        }




        

        public IEnumerable<ChatMessage> GetMessages(int userId)
        {
            IEnumerable<string> rooms = GetRoomsByUser(userId);
            return messages.Where(msg => rooms.Contains(msg.RoomId) && msg.UserId != userId && msg.ID > messages.Count() - 5);
        }

        
        public void Reset()
        {
            _engine = null;
        }


        //    while (chat.Count > maxbuffer)
        //    {
        //        chat.RemoveAt(0);
        //    }

        //    if (!pings.Contains(guid))
        //    {
        //        pings.Add(guid);
        //    }

        //    chat.Add(
        //        ParseText(users[guid].ToString(), text)
        //    );
        //}

        ///// <summary>
        ///// Format the users chat depending on whether 
        ///// a control command was supplied.
        ///// </summary>
        //private string ParseText(string user, string text)
        //{
        //    if (text.StartsWith("/me "))
        //    {
        //        return MakeActionMessage(
        //            string.Format(mefmt, user, text.Replace("/me", string.Empty))
        //            );
        //    }

        //    if (text.StartsWith("/admin "))
        //    {
        //        string command = text.Replace("/admin", string.Empty).Trim();
        //        string result = string.Empty;

        //        switch (command)
        //        {
        //            case "clear":
        //                chat.Clear();
        //                result = MakeServerMessage(
        //                    string.Format(clearfmt, user)
        //                    );
        //                break;
        //            default:
        //                result = MakeServerMessage(
        //                    string.Format(unknowcmdfmt, user)
        //                    );
        //                break;
        //        }
        //        return result;
        //    }


        //    if (text.StartsWith("/nick "))
        //    {
        //        string newnick = text.Replace("/nick", string.Empty).Trim();

        //        if (UserExists(newnick))
        //        {
        //            return MakeServerMessage(
        //                string.Format(nickinusefmt, user)
        //                );
        //        }

        //        string[] keys = new string[users.Count];
        //        users.Keys.CopyTo(keys, 0);

        //        foreach (string key in keys)
        //        {
        //            if (users[key].ToString() == user)
        //            {
        //                users[key] = newnick;
        //                return MakeServerMessage(
        //                    string.Format(nickfmt, user, newnick
        //                    )
        //                );
        //            }
        //        }

        //    }

        //    return MakeUserMessage(
        //        string.Format(txtfmt, user, text)
        //        );
        //}
        ///// <summary>
        ///// Remove the user
        ///// </summary>
        //public void Remove(string user, string password)
        //{
        //    //
        //    // TODO: Add your password here
        //    if (password != "Add password here!")
        //        return;

        //    if (this.UserExists(user))
        //    {
        //        string[] keys = new string[users.Count];
        //        foreach (DictionaryEntry e in users)
        //        {
        //            if (e.Value.ToString() == user)
        //            {
        //                chat.Add(
        //                    this.MakeServerMessage(
        //                        string.Format(killfmt, user)
        //                    )
        //                );

        //                users.Remove(e.Key);
        //                return;
        //            }
        //        }
        //    }

        //    if (pings.Contains(user))
        //        pings.Remove(user);
        //}

        ///// <summary>
        ///// Format a user message by wrapping text
        ///// in a list item with the user css class
        ///// selector
        ///// </summary>
        //private string MakeUserMessage(string text)
        //{
        //    return string.Format(msg, userstyle, text);
        //}
        ///// <summary>
        ///// Format a user action message by wrapping text
        ///// in a list item with the action css class
        ///// selector
        ///// </summary>
        //private string MakeActionMessage(string text)
        //{
        //    return string.Format(msg, actionstyle, text);
        //}


        #region Utils

        private IEnumerable<string> GetRoomsByUser(int userId)
        {
            return usersByRoom.Where(pair => pair.Value.Contains(String.Format(" {0},", userId))).Select(pair => pair.Key);
        }

        private string GetUsersByRoom(string roomId)
        {
            //
            // Create the room
            //
            if (!String.IsNullOrEmpty(roomId) && !usersByRoom.ContainsKey(roomId))
                usersByRoom.Add(roomId, String.Empty);

            return usersByRoom[roomId];
        }

        /// <summary>
        /// Format a server message by wrapping
        /// text in a list item with the server
        /// css class selector
        /// </summary>
        private ChatMessage MakeChatMessage(string roomId, int userId, string userName, string text)
        {
            lock (syncRoot)
            {
                ChatMessage message = new ChatMessage()
                {
                    RoomId = roomId,
                    Description = text,
                    UserId = userId,
                    UserName = userName,
                    ID = messages.Where(msg => msg.RoomId == roomId).Count(),
                    SentDate = DateTime.Now
                };
                return message;
            }
        }
        #endregion

    }

    [Serializable]
    public class ChatMessage
    {
        public int ID;
        public string RoomId;
        public string Description;
        public string UserName;
        public int UserId;
        public DateTime SentDate;
    }
}
