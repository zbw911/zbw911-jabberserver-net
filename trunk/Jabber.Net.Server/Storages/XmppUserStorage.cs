using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.iq.last;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.vcard;
using agsXMPP.util;
using Jabber.Net.Server.Data;
using Jabber.Net.Server.Data.Sql;

namespace Jabber.Net.Server.Storages
{
    public class XmppUserStorage : IXmppUserStorage
    {
        private readonly string connectionStringName;
        private readonly IXmppElementStorage elements;


        public XmppUserStorage(string connectionStringName, IXmppElementStorage elements)
        {
            Args.NotNull(connectionStringName, "connectionStringName");
            Args.NotNull(elements, "elements");

            this.connectionStringName = connectionStringName;
            this.elements = elements;

            CreateSchema();
        }


        public XmppUser GetUser(string username)
        {
            CheckUsername(username);
            using (var db = GetDb())
            {
                return db
                    .ExecList(new SqlQuery("jabber_user").Select("username", "userpass").Where("username", username))
                    .Select(r => new XmppUser((string)r[0], (string)r[1]))
                    .SingleOrDefault();
            }
        }

        public void SaveUser(XmppUser user)
        {
            Args.NotNull(user, "user");
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(user.Name), "User name can not be empty.");
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(user.Password), "User password can not be empty.");

            using (var db = GetDb())
            using (var tx = db.BeginTransaction())
            {
                var exists = 0 < db.ExecScalar<int>(new SqlQuery("jabber_user").SelectCount().Where("username", user.Name));
                var q = exists ?
                    (ISqlInstruction)new SqlUpdate("jabber_user").Set("userpass", user.Password).Where("username", user.Name) :
                    (ISqlInstruction)new SqlInsert("jabber_user").InColumnValue("username", user.Name).InColumnValue("userpass", user.Password);
                db.ExecuteNonQuery(q);
                tx.Commit();
            }
        }

        public bool RemoveUser(string username)
        {
            CheckUsername(username);

            var affected = 0;
            using (var db = GetDb())
            {
                affected = db.ExecuteNonQuery(new SqlDelete("jabber_user").Where("username", username));
            }
            elements.RemoveSingleElement(new Jid(username), "%");
            elements.RemoveElements(new Jid(username), "%");
            return 0 < affected;
        }


        public Vcard GetVCard(string username)
        {
            CheckUsername(username);
            return (Vcard)elements.GetSingleElement(new Jid(username), "vcard");
        }

        public void SetVCard(string username, Vcard vcard)
        {
            CheckUsername(username);
            if (vcard == null)
            {
                elements.RemoveSingleElement(new Jid(username), "vcard");
            }
            else
            {
                elements.SaveSingleElement(new Jid(username), "vcard", vcard);
            }
        }


        public IEnumerable<RosterItem> GetRosterItems(string username)
        {
            CheckUsername(username);
            return elements.Get(new Jid(username), "roster|%").OfType<RosterItem>();
        }

        public RosterItem GetRosterItem(string username, Jid jid)
        {
            CheckUsername(username);
            Args.NotNull(jid, "jid");
            return elements.GetSingleElement(new Jid(username), "roster|" + jid.Bare) as RosterItem;
        }

        public void SaveRosterItem(string username, RosterItem ri)
        {
            CheckUsername(username);
            Args.NotNull(ri, "ri");
            elements.SaveElements(new Jid(username), "roster|" + ri.Jid.Bare, ri);
        }

        public bool RemoveRosterItem(string username, Jid jid)
        {
            CheckUsername(username);
            return elements.RemoveElements(new Jid(username), "roster|" + jid.Bare);
        }


        private void CheckUsername(string username)
        {
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(username), "User name can not be empty.");
        }

        private DbManager GetDb()
        {
            return new DbManager(connectionStringName);
        }

        private void CreateSchema()
        {
            var jabber_user = new SqlCreate.Table("jabber_user", true)
                .AddColumn(new SqlCreate.Column("username", DbType.String, 1071).NotNull(true).PrimaryKey(true))
                .AddColumn(new SqlCreate.Column("userpass", DbType.String, 128).NotNull(true))
                .AddColumn(new SqlCreate.Column("uservcard", DbType.String, UInt16.MaxValue).NotNull(false));

            using (var db = GetDb())
            {
                db.ExecuteNonQuery(jabber_user);
            }
        }
    }
}
