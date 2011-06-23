using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Services
{
    public class XmppServiceManager
    {
        private readonly IDictionary<string, IXmppService> services;
        private readonly XmppHandlerManager handlerManager;


        public XmppServiceManager(XmppHandlerManager handlerManager)
        {
            Contract.Requires<ArgumentNullException>(handlerManager != null, "handlerManager");

            services = new Dictionary<string, IXmppService>(10);
            this.handlerManager = handlerManager;
        }


        public void RegisterService(IXmppService service)
        {
            Contract.Requires<ArgumentNullException>(service != null, "service");

            services.Add(service.Jid, service);
            service.Register(handlerManager);
        }

        public void UnregisterService(string jid)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(jid), "jid");

            if (services.ContainsKey(jid))
            {
                var service = services[jid];
                try
                {
                    service.Unregister(handlerManager);
                }
                finally
                {
                    services.Remove(jid);
                }
            }
        }
    }
}
