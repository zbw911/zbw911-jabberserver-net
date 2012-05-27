namespace Jabber.Net.Server.Connections
{
    public interface IXmppListener
    {
        void StartListen(XmppConnectionManager connectionManager);

        void StopListen();
    }
}
