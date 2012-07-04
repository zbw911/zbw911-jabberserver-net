﻿using System.Collections.Generic;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.vcard;

namespace Jabber.Net.Server.Storages
{
    public interface IXmppUserStorage
    {
        XmppUser GetUser(string username);

        void SaveUser(XmppUser user);

        bool RemoveUser(string username);


        Vcard GetVCard(string username);

        void SetVCard(string username, Vcard vcard);


        IEnumerable<RosterItem> GetRosterItems(Jid user);

        IEnumerable<Presence> GetPendingPresences(Jid contact);

        IEnumerable<Jid> GetToJids(Jid contact);

        RosterItem GetRosterItem(Jid user, Jid contact);

        void SaveRosterItem(Jid user, RosterItem ri);

        bool RemoveRosterItem(Jid user, Jid contact);
    }
}
