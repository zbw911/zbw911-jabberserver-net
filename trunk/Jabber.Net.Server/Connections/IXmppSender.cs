
namespace Jabber.Net.Server.Connections
{
    public interface IXmppSender
    {
        string SessionId
        {
            get;
            set;
        }


        void Send(XmppElement e);

        void SendAndClose(XmppElement e);

        void Close();
    }
}
