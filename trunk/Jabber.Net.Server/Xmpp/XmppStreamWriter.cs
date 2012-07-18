using System;
using System.IO;
using System.Text;
using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Xmpp
{
    class XmppStreamWriter
    {
        private readonly Stream stream;
        private readonly Encoding encoding;


        public event EventHandler<XmppStreamArgs> WriteElementComleted;


        public XmppStreamWriter(Stream stream)
        {
            Args.NotNull(stream, "stream");

            this.stream = stream;
            this.encoding = Encoding.UTF8;
        }


        public void WriteElementAsync(Element element)
        {
            try
            {
                var buffer = encoding.GetBytes(element.ToString());
                stream.BeginWrite(buffer, 0, buffer.Length, SendCallback, element);
            }
            catch (Exception ex)
            {
                OnError(element, ex);
            }
        }


        private void SendCallback(IAsyncResult ar)
        {
            var element = (Element)ar.AsyncState;
            try
            {
                stream.EndWrite(ar);
                stream.Flush();
                var ev = WriteElementComleted;
                if (ev != null)
                {
                    ev(this, new XmppStreamArgs(XmppStreamState.Success, element, null));
                }
            }
            catch (Exception ex)
            {
                OnError(element, ex);
            }
        }

        private void OnError(Element element, Exception ex)
        {
            var ev = WriteElementComleted;
            if (ev != null)
            {
                ev(this, new XmppStreamArgs(XmppStreamState.Error, element, ex));
            }
        }
    }
}
