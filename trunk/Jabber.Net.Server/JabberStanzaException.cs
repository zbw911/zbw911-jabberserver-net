using System;
using System.Runtime.Serialization;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server
{
    [Serializable]
    public class JabberStanzaException : JabberException
    {
        private readonly ErrorCode error;
        private readonly Stanza stanza;


        public override bool CloseStream
        {
            get { return false; }
        }


        public JabberStanzaException(ErrorCode error, Stanza stanza)
        {
            this.error = error;
            this.stanza = stanza;
        }


        protected JabberStanzaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public override Element ToElement()
        {
            var e = new Error(error);
            if (error == ErrorCode.InternalServerError)
            {
                e.Message = this.Message;
            }
            if (stanza != null)
            {
                if (!stanza.Switched)
                {
                    stanza.SwitchDirection();
                }
                stanza.SetAttribute("type", "error");
                stanza.ReplaceChild(e);
                return stanza;
            }
            else
            {
                return e;
            }
        }
    }
}
