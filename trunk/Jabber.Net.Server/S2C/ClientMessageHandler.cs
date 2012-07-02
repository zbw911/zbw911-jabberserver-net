using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ClientMessageHandler : XmppHandler, IXmppHandler<Message>
    {
        /// <summary>
        /// Exchanging Messages.
        /// </summary>
        /// <see cref="http://xmpp.org/rfcs/rfc6121.html#rules-local-message"/>
        /// <remarks>
        /// Summary of Message Delivery Rules:
        /// +----------------------------------------------------------+
        /// | Condition        | Normal | Chat  | Groupchat | Headline |
        /// +----------------------------------------------------------+
        /// | ACCOUNT DOES NOT EXIST                                   |
        /// |  bare            |   S    |   S   |     E     |    S     |
        /// |  full            |   S    |   S   |     S     |    S     |
        /// +----------------------------------------------------------+
        /// | ACCOUNT EXISTS, BUT NO ACTIVE RESOURCES                  |
        /// |  bare            |   O    |   O   |     E     |    S     |
        /// |  full (no match) |   S    |   O   |     S     |    S     |
        /// +----------------------------------------------------------+
        /// | 1+ NEGATIVE RESOURCES BUT ZERO NON-NEGATIVE RESOURCES    |
        /// |  bare            |   O    |   O   |     E     |    S     |
        /// |  full match      |   D    |   D   |     D     |    D     |
        /// |  full no match   |   S    |   O   |     S     |    S     |
        /// +----------------------------------------------------------+
        /// | 1 NON-NEGATIVE RESOURCE                                  |
        /// |  bare            |   D    |   D   |     E     |    D     |
        /// |  full match      |   D    |   D   |     D     |    D     |
        /// |  full no match   |   S    |   D   |     S     |    S     |
        /// +----------------------------------------------------------+
        /// | 1+ NON-NEGATIVE RESOURCES                                |
        /// |  bare            |   M    |   M   |     E     |    A     |
        /// |  full match      |   D    |   D   |     D     |    D     |
        /// |  full no match   |   S    |   M   |     S     |    S     |
        /// +----------------------------------------------------------+
        /// </remarks>
        /// <param name="message">Client message.</param>
        /// <param name="session">Sended session.</param>
        /// <param name="context">Handler context.</param>
        /// <returns>Result of message processing.</returns>
        public XmppHandlerResult ProcessElement(Message message, XmppSession session, XmppHandlerContext context)
        {
            if (message.Type == MessageType.error)
            {
                // ignore
                return Void();
            }
            if (!message.HasTo)
            {
                return Error(session, ErrorCondition.BadRequest, message);
            }


            if (context.Storages.Users.GetUser(message.To.User) == null)
            {
                return Void();
            }



            return Void();
        }


        private enum SolutionResult
        {
            /// <summary>
            /// Silently ignoring the message
            /// </summary>
            S,

            /// <summary>
            /// Bouncing the message with a stanza error
            /// </summary>
            E,

            /// <summary>
            /// Storing the message offline 
            /// </summary>
            O,

            /// <summary>
            /// Delivering the message to the resource specified in the 'to' address
            /// </summary>
            D,

            /// <summary>
            /// Delivering the message to the "most available" resource or resources according to the server's implementation-specific algorithm, e.g., 
            /// treating the resource or resources with the highest presence priority as "most available"
            /// </summary>
            M,

            /// <summary>
            /// Delivering the message to all resources with non-negative presence priority
            /// </summary>
            A,
        }

        private class Solution
        {

        }
    }
}
