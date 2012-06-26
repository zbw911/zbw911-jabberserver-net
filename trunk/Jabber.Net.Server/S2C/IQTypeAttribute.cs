using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    public class IQTypeAttribute : XmppValidationAttribute
    {
        private readonly IqType[] allowed;

        public ErrorCondition ErrorCondition
        {
            get;
            set;
        }


        public IQTypeAttribute(params IqType[] allowed)
        {
            this.allowed = allowed;
        }

        public override XmppHandlerResult ValidateElement(Element element, XmppSession session, XmppHandlerContext context)
        {
            var iq = element as IQ;
            if (iq != null)
            {
                if (!allowed.Contains(iq.Type))
                {
                    return Error(session, ErrorCondition, iq);
                }
            }
            return Success();
        }
    }
}
