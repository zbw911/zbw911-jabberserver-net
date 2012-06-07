using System;
using System.Text;
using agsXMPP.util;
using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Xmpp
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
            return ElementSerializer.DeSerializeElement<Element>(Encoding.UTF8.GetString(buffer));
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
                ev(this, new ParsedArgs((Element)e));
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
