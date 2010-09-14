using System;
using System.Collections;

namespace InfoControl.Net.Mail.POP3
{
	/// <summary>
	/// Pop33 messages collection.
	/// </summary>
	public class Pop3Messages
	{
		private ArrayList m_Pop3Messages = null;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Pop3Messages()
		{	
			m_Pop3Messages = new ArrayList();
		}


		#region function AddMessage

		/// <summary>
		/// Adds new message to message list.
		/// </summary>
		/// <param name="messageID">Message ID.</param>
		/// <param name="uid">Message UID. This UID is reported in UIDL command.</param>
		/// <param name="messageSize">Message size in bytes.</param>
		public void AddMessage(string messageID,string uid,int messageSize)
		{
			AddMessage(messageID,uid,messageSize,null);
		}

		/// <summary>
		/// Adds new message to message list.
		/// </summary>
		/// <param name="messageID">Message ID.</param>
		/// <param name="uid">Message UID. This UID is reported in UIDL command.</param>
		/// <param name="messageSize">Message size in bytes.</param>
		/// <param name="tag">User data for message.</param>
		public void AddMessage(string messageID,string uid,int messageSize,object tag)
		{
			Pop3Message msg = new Pop3Message(this);
			msg.MessageUID   = uid;
			msg.MessageID    = messageID;
			msg.MessageSize  = messageSize;
			msg.Tag          = tag;

			m_Pop3Messages.Add(msg);
		}

		#endregion

		#region function GetMessage

		/// <summary>
		/// Gets specified message from message list.
		/// </summary>
		/// <param name="messageNr">Number of message which to get.</param>
		/// <returns></returns>
		public Pop3Message GetMessage(int messageNr)
		{
			return (Pop3Message)m_Pop3Messages[messageNr];
		}

		#endregion

		#region function MessageExists

		/// <summary>
		/// Checks if message exists. NOTE marked for delete messages returns false.
		/// </summary>
		/// <param name="nr">Number of message which to check.</param>
		/// <returns></returns>
		public bool MessageExists(int nr)
		{
			try
			{
				if(nr > 0 && nr <= m_Pop3Messages.Count){
					Pop3Message msg = (Pop3Message)m_Pop3Messages[nr-1];
					if(!msg.MarkedForDelete){
						return true;
					}
				}
			}
			catch{
			}
			
			return false;			
		}

		#endregion

		#region function GetTotalMessagesSize

		/// <summary>
		/// Gets messages total sizes. NOTE messages marked for deletion is excluded.
		/// </summary>
		/// <returns></returns>
		public int GetTotalMessagesSize()
		{
			int totalSize = 0;
			foreach(Pop3Message msg in m_Pop3Messages){
				if(!msg.MarkedForDelete){
					totalSize += msg.MessageSize;
				}
			}

			return totalSize;
		}

		#endregion


		#region function ResetDeleteFlags

		/// <summary>
		/// Unmarks all messages, which are marked for deletion.
		/// </summary>
		public void ResetDeleteFlags()
		{
			foreach(Pop3Message msg in m_Pop3Messages){
				msg.MarkedForDelete = false;
			}
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// Gets count of messages. NOTE messages marked for deletion are excluded.
		/// </summary>
		public int Count
		{
			get{
				int messageCount = 0;
				foreach(Pop3Message msg in m_Pop3Messages){
					if(!msg.MarkedForDelete){
						messageCount++;
					}
				}
				return messageCount; 
			}
		}

		/// <summary>
		/// Gets messages, which aren't marked for deletion.
		/// </summary>
		public Pop3Message[] ActiveMessages
		{			
			get{
				//--- Make array of unmarked messages --------//
				ArrayList activeMessages = new ArrayList();
				foreach(Pop3Message msg in m_Pop3Messages){
					if(!msg.MarkedForDelete){
						activeMessages.Add(msg);
					}
				}
				//--------------------------------------------//
				
				Pop3Message[] retVal = new Pop3Message[activeMessages.Count];
				activeMessages.CopyTo(retVal);
				return retVal; 
			}
		}


		/// <summary>
		/// Referance to Messages ArrayList.
		/// </summary>
		internal ArrayList Messages
		{
			get{ return m_Pop3Messages; }
		}

		/// <summary>
		/// Gets specified message.
		/// </summary>
		internal Pop3Message this[int messageNr]
		{
			get{ return (Pop3Message)m_Pop3Messages[messageNr-1]; }
		}

		#endregion

	}
}
