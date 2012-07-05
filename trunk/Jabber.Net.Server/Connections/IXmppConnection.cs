using System;
using System.Security.Cryptography.X509Certificates;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppConnection
    {
        bool SupportTls { get; }


        void BeginRecieve(IXmppReciever reciever);

        void Send(byte[] buffer, Action<byte[]> error);

        void Close();

        void StartTls(X509Certificate certificate);
    }
}
