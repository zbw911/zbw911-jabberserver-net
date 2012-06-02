
namespace Jabber.Net.Server.Connections
{
    public interface IXmppReciever
    {
        void OnRecive(byte[] buffer);

        void OnClose();
    }
}
