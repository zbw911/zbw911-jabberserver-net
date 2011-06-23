using System;
using System.Text;
using agsXMPP.util;
using agsXMPP.Xml;
using agsXMPP.Xml.Dom;
using agsXMPP.protocol;

namespace Jabber.Net
{
    public class XmppStreamParser
    {
        private readonly StreamParser agsParser;


        public event EventHandler<ParsedArgs> Parsed;

        public event EventHandler<ParseErrorArgs> Error;


        public XmppStreamParser()
        {
            agsParser = new StreamParser();
            agsParser.OnStreamStart += AgsParserOnStreamStart;
            agsParser.OnStreamElement += AgsParserOnStreamElement;
            agsParser.OnStreamEnd += AgsParserOnStreamElement;
            agsParser.OnError += AgsParserOnError;
            agsParser.OnStreamError += AgsParserOnError;
            Reset();
        }

        public XmppElement Parse(byte[] buffer)
        {
            return new XmppElement(ElementSerializer.DeSerializeElement<Node>(Encoding.UTF8.GetString(buffer)));
        }

        public void ParseAsync(byte[] buffer)
        {
            agsParser.Push(buffer, 0, buffer.Length);
        }

        public void Reset()
        {
            agsParser.Reset();
        }


        private void AgsParserOnStreamStart(object sender, Node e, string streamNamespace)
        {
            if (e is Stream)
            {
                ((Stream)e).SetAttribute("xmlns", streamNamespace);
            }
            AgsParserOnStreamElement(sender, e);
        }

        private void AgsParserOnStreamElement(object sender, Node e)
        {
            var ev = Parsed;
            if (ev != null)
            {
                ev(this, new ParsedArgs(new XmppElement(e)));
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
            public XmppElement XmppElement
            {
                get;
                private set;
            }


            public ParsedArgs(XmppElement xmppElement)
            {
                XmppElement = xmppElement;
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
