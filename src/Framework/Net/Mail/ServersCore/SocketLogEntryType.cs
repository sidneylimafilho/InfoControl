using System;

namespace InfoControl.Net.Mail
{
	/// <summary>
	/// Log entry type.
	/// </summary>
	public enum SocketLogEntryType
	{
		ReadFromRemoteEP = 0,
		SendToRemoteEP = 1,
		FreeText = 2,
	}
}
