using System;

using agsXMPP.protocol.Base;

using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

namespace agsXMPP.protocol.component
{
    using Base;

    public enum LogType
	{
		NONE = -1,	
		warn,	
		info,	
		verbose,	
		debug,
		notice
	}

	/// <summary>
	/// Zusammenfassung f�r Log.
	/// </summary>
	public class Log : Stanza
	{
		public Log()
		{
			this.TagName	= "log";
			this.Namespace	= Uri.ACCEPT;	
		}

		/// <summary>
		/// creates a new Log Packet with the given message
		/// </summary>
		/// <param name="message"></param>
		public Log(string message) : this()
		{
			this.Value = message;
		}
		

		/// <summary>
		/// Gets or Sets the logtype
		/// </summary>
		public LogType Type
		{
			get 
			{ 
				return (LogType) GetAttributeEnum("type", typeof(LogType));
			}
			set 
			{ 
				if (value == LogType.NONE)
					RemoveAttribute("type");
				else
					SetAttribute("type", value.ToString()); 
			}
		}

		/// <summary>
		/// The namespace for logging
		/// </summary>
		public string LogNamespace
		{
			get { return GetAttribute("ns"); }
			set { SetAttribute("ns", value); }
		}	

	}


}
