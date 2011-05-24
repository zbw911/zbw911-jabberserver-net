#if DEBUG
using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace Jabber.Net.Server
{
	[TestFixture]
	public class XTest
	{
		public XTest ()
		{
		}

		
		[Test]
		public void ParseXml ()
		{
			var s = "<iq type='result' id='reg1'>" +
				"<query xmlns='jabber:iq:register'>" +
				"<instructions>Choose a username and password for use with this service. Please also provide your email address." +
				"</instructions>" +
				"<username/>" +
				"<password/>" +
				"<email/>" +
				"</query>" +
				"</iq>";
			
			var xdoc = XElement.Parse(s);
			var first = xdoc.FirstNode;
		}
	}
}
#endif