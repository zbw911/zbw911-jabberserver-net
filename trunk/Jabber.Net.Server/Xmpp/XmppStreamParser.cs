﻿using System;
using System.Text;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.iq.last;
using agsXMPP.protocol.iq.@private;
using agsXMPP.protocol.iq.register;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.session;
using agsXMPP.protocol.iq.time;
using agsXMPP.protocol.iq.vcard;
using agsXMPP.protocol.iq.version;
using agsXMPP.util;
using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Xmpp
{
    class XmppStreamParser
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
            return CorrectType(e);
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
                ev(this, new ParsedArgs(CorrectType((Element)e)));
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

        private Element CorrectType(Element element)
        {
            var iq = element as IQ;
            if (iq != null)
            {
                if (iq.SelectSingleElement<Bind>() != null)
                {
                    return new BindIq(iq);
                }
                else if (iq.SelectSingleElement<Session>() != null)
                {
                    return new SessionIq(iq);
                }
                else if (iq.SelectSingleElement("vCard") as Vcard != null)
                {
                    return new VcardIq(iq);
                }
                else if (iq.SelectSingleElement<DiscoInfo>() != null)
                {
                    return new DiscoInfoIq(iq);
                }
                else if (iq.SelectSingleElement<DiscoItems>() != null)
                {
                    return new DiscoItemsIq(iq);
                }
                else if (iq.SelectSingleElement<Last>() != null)
                {
                    return new LastIq(iq);
                }
                else if (iq.SelectSingleElement<agsXMPP.protocol.iq.version.Version>() != null)
                {
                    return new VersionIq(iq);
                }
                else if (iq.SelectSingleElement<Private>() != null)
                {
                    return new PrivateIq(iq);
                }
                else if (iq.SelectSingleElement<Register>() != null)
                {
                    return new RegisterIq(iq);
                }
                else if (iq.SelectSingleElement<Roster>() != null)
                {
                    return new RosterIq(iq);
                }
                else if (iq.SelectSingleElement<EntityTime>() != null)
                {
                    return new EntityTimeIq(iq);
                }
            }
            var stream = element as agsXMPP.protocol.Base.Stream;
            if (stream != null)
            {
                if (agsParser.DefaultNamespace == agsXMPP.Uri.CLIENT)
                {
                    return new Stream(stream, agsParser.DefaultNamespace);
                }
                if (agsParser.DefaultNamespace == agsXMPP.Uri.ACCEPT)
                {
                    return new agsXMPP.protocol.component.Stream(stream, agsParser.DefaultNamespace);
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
