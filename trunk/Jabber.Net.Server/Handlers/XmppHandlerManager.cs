using System;
using Jabber.Net.Server.Connections;
using Jabber.Net.Xmpp;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        public XmppHandlerManager()
        {
        }
        

        public void ProcessXmppElement(IXmppEndPoint endpoint, XmppElement e)
        {
            try
            {
                
            }
            catch (Exception error)
            {
                ProcessError(endpoint, error);
            }
        }

        public void ProcessClose(IXmppEndPoint endpoint)
        {
            try
            {

            }
            catch (Exception error)
            {
                Log.Error(error);
            }
        }

        public void ProcessError(IXmppEndPoint endpoint, Exception error)
        {
            try
            {

            }
            catch (Exception innererror)
            {
                Log.Error(innererror);
            }
        }
    }
}
