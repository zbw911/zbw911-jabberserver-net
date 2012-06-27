using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using agsXMPP;
using agsXMPP.util;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Data;
using Jabber.Net.Server.Data.Sql;

namespace Jabber.Net.Server.Storages
{
    public class XmppElementStorage : IXmppElementStorage
    {
        private readonly string connectionStringName;


        public XmppElementStorage(string connectionStringName)
        {
            Args.NotNull(connectionStringName, "connectionStringName");
            this.connectionStringName = connectionStringName;

            CreateSchema();
        }


        public Element GetSingleElement(Jid jid, string key)
        {
            return GetElements(true, jid, key).SingleOrDefault();
        }

        public void SaveSingleElement(Jid jid, string key, Element element)
        {
            Args.NotNull(element, "element");
            SaveElements(true, jid, key, element);
        }

        public void RemoveSingleElement(Jid jid, string key)
        {
            RemoveElements(true, jid, key);
        }


        public IEnumerable<Element> GetElements(Jid jid, string key)
        {
            return GetElements(false, jid, key);
        }

        public void SaveElements(Jid jid, string key, params Element[] elements)
        {
            Args.NotNull(elements, "elements");
            SaveElements(false, jid, key, elements);
        }

        public void RemoveElements(Jid jid, string key)
        {
            RemoveElements(false, jid, key);
        }


        private void CreateSchema()
        {
            var jabber_element = new SqlCreate.Table("jabber_element", true)
                .AddColumn(new SqlCreate.Column("jid", DbType.String, 3071).NotNull(true))
                .AddColumn(new SqlCreate.Column("element_key", DbType.String, 1024).NotNull(true))
                .AddColumn(new SqlCreate.Column("element_text", DbType.String, UInt16.MaxValue).NotNull(false))
                .PrimaryKey("jid", "element_key");

            var jabber_elements = new SqlCreate.Table("jabber_elements", true)
                .AddColumn(new SqlCreate.Column("id", DbType.Int32).NotNull(true).Autoincrement(true).PrimaryKey(true))
                .AddColumn(new SqlCreate.Column("jid", DbType.String, 3071).NotNull(true))
                .AddColumn(new SqlCreate.Column("element_key", DbType.String, 1024).NotNull(true))
                .AddColumn(new SqlCreate.Column("element_text", DbType.String, UInt16.MaxValue).NotNull(false))
                .AddIndex(new SqlCreate.Index("elements_index", "jabber_elements", "jid", "element_key"));

            using (var db = GetDb())
            {
                db.ExecuteNonQuery(jabber_element);
                db.ExecuteNonQuery(jabber_elements);
            }
        }

        private DbManager GetDb()
        {
            return new DbManager(connectionStringName);
        }

        private IEnumerable<Element> GetElements(bool single, Jid jid, string key)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(key, "key");

            var q = new SqlQuery(single ? "jabber_element" : "jabber_elements")
                .Select("element_text")
                .Where("jid", jid.Bare)
                .Where("element_key", key);
            using (var db = GetDb())
            {
                return db.ExecList(q)
                    .Select(r => ElementSerializer.DeSerializeElement<Element>((string)r[0]))
                    .ToList();
            }
        }

        private void SaveElements(bool single, Jid jid, string key, params Element[] elements)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(key, "key");
            
            using (var db = GetDb())
            using (var tx = db.BeginTransaction())
            {
                foreach (var e in elements)
                {
                    var i = new SqlInsert(single ? "jabber_element" : "jabber_elements", true)
                        .InColumnValue("jid", jid.Bare)
                        .InColumnValue("element_key", key)
                        .InColumnValue("element_text", e.ToString());
                    db.ExecuteNonQuery(i);
                }
                tx.Commit();
            }
        }

        private void RemoveElements(bool single, Jid jid, string key)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(key, "key");
            
            using (var db = GetDb())
            {
                var d = new SqlDelete(single ? "jabber_element" : "jabber_elements")
                    .Where("jid", jid.Bare)
                    .Where("element_key", key);
                db.ExecuteNonQuery(d);
            }
        }
    }
}
