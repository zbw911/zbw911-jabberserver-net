
namespace Jabber.Net.Server.Handlers
{
    public abstract class XmppHandlerResult
    {
        public bool Handled
        {
            get;
            private set;
        }


        public XmppHandlerResult Merge(XmppHandlerResult result)
        {
            return null;
        }


        public abstract void Execute(XmppHandlerContext context);
    }
}
