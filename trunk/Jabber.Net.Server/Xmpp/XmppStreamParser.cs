using System;
using System.Text;
using agsXMPP.util;
using agsXMPP.Xml;
using agsXMPP.Xml.Dom;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.session;
using agsXMPP.protocol.iq.vcard;

namespace Jabber.Net.Server.Xmpp
{
    public class XmppStreamParser
    {
        private readonly StreamParser agsParser;


        public event EventHandler<ParsedArgs> Parsed;

        public event EventHandler<ParseErrorArgs> Error;


        public XmppStreamParser()
        {
            agsParser = new StreamParser();
            agsParser.OnStreamStart += AgsParserOnStreamElement;
            agsParser.OnStreamElement += AgsParserOnStreamElement;
            agsParser.OnStreamEnd += AgsParserOnStreamElement;
            agsParser.OnError += AgsParserOnError;
            agsParser.OnStreamError += AgsParserOnError;
            Reset();
        }

        public Element Parse(byte[] buffer)
        {
            var e = ElementSerializer.DeSerializeElement<Element>(Encoding.UTF8.GetString(buffer));
            return CorrectIQType(e);
        }

        public byte[] ToBytes(Element e)
        {
            return Encoding.UTF8.GetBytes(e.ToString());
        }

        public void ParseAsync(byte[] buffer)
        {
            agsParser.Push(buffer, 0, buffer.Length);
        }

        public void Reset()
        {
            agsParser.Reset();
        }


        private void AgsParserOnStreamElement(object sender, Node e)
        {
            var ev = Parsed;
            if (ev != null && e is Element)
            {
                ev(this, new ParsedArgs(CorrectIQType((Element)e)));
            }
        }

        private void AgsParserOnError(object sender, Exception ex)
        {
            var ev = Error;
            if (ev != null)
            {
                ev(this, new ParseErrorArgs(ex));
            }
        }

        private Element CorrectIQType(Element element)
        {
            var iq = element as IQ;
            if (iq != null)
            {
                if (iq.SelectSingleElement<Bind>() != null)
                {
                    return new BindIq(iq);
                }
                if (iq.SelectSingleElement<Session>() != null)
                {
                    return new SessionIq(iq);
                }
                if (iq.SelectSingleElement("vCard") as Vcard != null)
                {
                    return new VcardIq(iq);
                }
            }
            return element;
        }


        public class ParsedArgs : EventArgs
        {
            public Element Element
            {
                get;
                private set;
            }


            public ParsedArgs(Element element)
            {
                Element = element;
            }
        }

        public class ParseErrorArgs : EventArgs
        {
            public Exception Error
            {
                get;
                private set;
            }


            public ParseErrorArgs(Exception error)
            {
                Error = error;
            }
        }
    }
}
