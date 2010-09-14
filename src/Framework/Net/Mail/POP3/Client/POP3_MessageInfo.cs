using System;

namespace InfoControl.Net.Mail.POP3
{
	/// <summary>
	/// Holds Pop33 message info.
	/// </summary>
	public class Pop3MessageInfo
	{
		private string m_MessageID   = "";
		private int    m_MessageNo   = 0;
		private long   m_MessageSize = 0;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="messageID"></param>
		/// <param name="messageNo"></param>
		/// <param name="messageSize"></param>
		public Pop3MessageInfo(string messageID,int messageNo,long messageSize)
		{	
			m_MessageID   = messageID;
			m_MessageNo   = messageNo;
			m_MessageSize = messageSize;
		}

		#region Properties Implementation

		/// <summary>
		/// Gets message unique ID returned by Pop33 server.
		/// </summary>
		public string MessegeID
		{
			get{ return m_MessageID; }
		}

		/// <summary>
		/// Gets message number in Pop33 server.
		/// </summary>
		[Obsolete("Use MessageNumber instead !")]
		public int MessageNr
		{
			get{ return m_MessageNo; }
		}

		/// <summary>
		/// Gets message number in Pop33 server.
		/// </summary>
		public int MessageNumber
		{
			get{ return m_MessageNo; }
		}

		/// <summary>
		/// Gets message size.
		/// </summary>
		public long MessageSize
		{
			get{ return m_MessageSize; }
		}

		#endregion

	}
}
