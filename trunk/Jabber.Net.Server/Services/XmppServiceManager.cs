using System;
using System.Collections.Generic;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Services
{
    public class XmppServiceManager
    {
        private readonly IDictionary<string, IXmppService> services;
        private readonly XmppHandlerManager handlerManager;


        public XmppServiceManager(XmppHandlerManager handlerManager)
        {
            Args.NotNull(handlerManager, "handlerManager");

            services = new Dictionary<string, IXmppService>(10);
            this.handlerManager = handlerManager;
        }


        public void RegisterService(IXmppService service)
        {
            Args.NotNull(service, "service");

            services.Add(service.Jid, service);
            service.Register(handlerManager);
        }

        public void UnregisterService(string jid)
        {
            Args.Requires<ArgumentNullException>(!string.IsNullOrEmpty(jid), "jid");

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
