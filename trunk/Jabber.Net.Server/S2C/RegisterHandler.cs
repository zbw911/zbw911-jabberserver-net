using agsXMPP.Idn;
using agsXMPP.protocol;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.register;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Resources;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class RegisterHandler : XmppHandler,
        IXmppHandler<RegisterIq>,
        IXmppRegisterHandler
    {
        public void OnRegister(XmppHandlerContext context)
        {
            context.Sessions.SupportRegister = true;
        }

        [IQType(IqType.get, IqType.set)]
        public XmppHandlerResult ProcessElement(RegisterIq element, XmppSession session, XmppHandlerContext context)
        {
            if (element.Type == IqType.get)
            {
                element.Query.Instructions = RS.RegisterInstructions;
                element.Query.Username = string.Empty;
                element.Query.Password = string.Empty;
                element.Type = IqType.result;
                if (session.Jid.HasUser && context.Storages.Users.GetUser(session.Jid.User) != null)
                {
                    element.Query.Username = session.Jid.User;
                    element.Query.AddChild(new Element("registered"));
                    element.SwitchDirection();
                    element.From = null;
                }
                else
                {
                    element.From = element.To = null;
                }
                return Send(session, element);
            }
            else
            {
                element.Type = IqType.result;

                if (element.Query.RemoveAccount)
                {
                    if (!session.Authenticated || !session.Jid.HasUser)
                    {
                        return Error(session, StreamErrorCondition.NotAuthorized);
                    }

                    context.Storages.Users.RemoveUser(session.Jid.User);
                    var component = Component();
                    foreach (var s in context.Sessions.FindSessions(session.Jid.BareJid))
                    {
                        if (!session.Equals(s))
                        {
                            component.AddResult(Error(s, StreamErrorCondition.Conflict));
                        }
                    }

                    element.Query.RemoveAllChildNodes();
                    element.SwitchDirection();
                    component.AddResult(Send(session, element));
                    return component;
                }

                if (string.IsNullOrEmpty(element.Query.Username) ||
                    string.IsNullOrEmpty(element.Query.Password) ||
                    Stringprep.NamePrep(element.Query.Username) != element.Query.Username)
                {
                    var error = new JabberStanzaException(ErrorCondition.NotAcceptable, element);
                    if (string.IsNullOrEmpty(element.Query.Username))
                    {
                        error = new JabberStanzaException(ErrorCondition.NotAcceptable, element, RS.RegisterEmptyUsername);
                    }
                    else if (string.IsNullOrEmpty(element.Query.Password))
                    {
                        error = new JabberStanzaException(ErrorCondition.NotAcceptable, element, RS.RegisterEmptyPassword);
                    }
                    else if (Stringprep.NamePrep(element.Query.Username) != element.Query.Username)
                    {
                        error = new JabberStanzaException(ErrorCondition.NotAcceptable, element, RS.RegisterInvalidCharacter);
                    }
                    return Error(session, error);
                }

                if (context.Storages.Users.GetUser(element.Query.Username) != null)
                {
                    return Error(session, ErrorCondition.Conflict, element);
                }

                context.Storages.Users.SaveUser(new XmppUser(element.Query.Username, element.Query.Password));

                element.Query.RemoveAllChildNodes();
                if (session.Authenticated)
                {
                    element.SwitchDirection();
                }
                else
                {
                    element.To = null;
                }
                element.From = null;
                return Send(session, element);
            }
        }
    }
}
