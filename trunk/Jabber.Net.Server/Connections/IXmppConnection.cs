namespace Jabber.Net.Server.Connections
{
    public interface IXmppConnection
    {
        void BeginRecieve(IXmppReciever reciever);

        void Send(byte[] buffer);

        void Close();
    }
}
