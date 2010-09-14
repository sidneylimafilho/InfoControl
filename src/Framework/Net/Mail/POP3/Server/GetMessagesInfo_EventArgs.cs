using System;

namespace InfoControl.Net.Mail.POP3
{
	/// <summary>
	/// Provides data for the GetMessgesList event.
	/// </summary>
	public class GetMessagesInfo_EventArgs
	{
		private Pop3Session  m_pSession       = null;
		private Pop3Messages m_pPop3Messages = null;
		private string        m_UserName       = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="session">Reference to Pop33 session.</param>
		/// <param name="messages"></param>
		/// <param name="mailbox">Mailbox name.</param>
		public GetMessagesInfo_EventArgs(Pop3Session session,Pop3Messages messages,string mailbox)
		{
			m_pSession       = session;
			m_pPop3Messages = messages;
			m_UserName       = mailbox;
		}

		#region Properties Implementation

		/// <summary>
		/// Gets reference to Pop33 session.
		/// </summary>
		public Pop3Session Session
		{
			get{ return m_pSession; }
		}

		/// <summary>
		/// Gets referance to Pop33 messages info.
		/// </summary>
		public Pop3Messages Messages
		{
			get{ return m_pPop3Messages; }
		}

		/// <summary>
		/// User Name.
		/// </summary>
		public string UserName
		{
			get{ return m_UserName; }
		}

		/// <summary>
		/// Mailbox name.
		/// </summary>
		public string Mailbox
		{
			get{ return m_UserName; }
		}

		#endregion

	}
}
