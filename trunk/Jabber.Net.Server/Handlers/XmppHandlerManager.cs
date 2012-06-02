using System;
using System.Collections.Generic;
using Jabber.Net.Server.Connections;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        public XmppHandlerManager()
        {
        }
        

        public void ProcessElement(IXmppSender sender, XmppElement e)
        {
            try
            {

            }
            catch (Exception error)
            {
                ProcessError(sender, error);
            }
        }

        public void ProcessClose(IXmppSender sender, IEnumerable<XmppElement> notSended)
        {
            try
            {

            }
            catch (Exception error)
            {
                Log.Error(error);
            }
        }

        public void ProcessError(IXmppSender sender, Exception error)
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
