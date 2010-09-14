using System;
using System.Collections;

namespace InfoControl.Net.Mail.POP3
{
	/// <summary>
	/// Holds Pop33 messages info.
	/// </summary>
	public class Pop3MessagesInfo
	{
		private Hashtable m_pMessages = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Pop3MessagesInfo()
		{	
			m_pMessages = new Hashtable();
		}

		#region function Add

		internal void Add(string messageID,int messageNr,long messageSize)
		{
			m_pMessages.Add(messageNr,new Pop3MessageInfo(messageID,messageNr,messageSize));
		}

		#endregion


		#region function GetMessageInfo

		/// <summary>
		/// Gets specified message info.
		/// </summary>
		/// <param name="nr"></param>
		/// <returns></returns>
		public Pop3MessageInfo GetMessageInfo(int nr)
		{
			if(m_pMessages.ContainsKey(nr)){
				return (Pop3MessageInfo)m_pMessages[nr];
			}
			else{
				throw new Exception("No such message !");
			}
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// Gets total size of messages.
		/// </summary>
		public long TotalSize
		{
			get{ 
				long sizeTotal = 0;
				foreach(Pop3MessageInfo msg in this.Messages){
					sizeTotal += msg.MessageSize;
				}
				return sizeTotal; 
			}
		}

		/// <summary>
		/// Gets messages count.
		/// </summary>
		public int Count
		{
			get{ return m_pMessages.Count; }
		}

		/// <summary>
		/// Gets list of messages.
		/// </summary>
		public Pop3MessageInfo[] Messages
		{
			get{
				Pop3MessageInfo[] retVal = new Pop3MessageInfo[m_pMessages.Count];
				m_pMessages.Values.CopyTo(retVal,0);

				return retVal; 
			}
		}

		#endregion

	}
}
