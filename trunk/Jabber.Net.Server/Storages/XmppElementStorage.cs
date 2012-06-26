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


        public Element GetSingleElement(Jid jid, string tag, string ns)
        {
            Args.NotNull(jid, "jid");
            return GetElements(true, jid, tag, ns).SingleOrDefault();
        }

        public void SaveSingleElement(Jid jid, Element element)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(element, "element");
            SaveElements(true, jid, element);
        }

        public void RemoveSingleElement(Jid jid, string tag, string ns)
        {
            Args.NotNull(jid, "jid");
            RemoveElements(true, jid, tag, ns);
        }


        public IEnumerable<Element> GetElements(Jid jid, string tag, string ns)
        {
            Args.NotNull(jid, "jid");
            return GetElements(false, jid, tag, ns);
        }

        public void SaveElements(Jid jid, params Element[] elements)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(elements, "elements");
            SaveElements(false, jid, elements);
        }

        public void RemoveElements(Jid jid, string tag, string ns)
        {
            Args.NotNull(jid, "jid");
            RemoveElements(false, jid, tag, ns);
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

        private IEnumerable<Element> GetElements(bool single, Jid jid, string tag, string ns)
        {
            var q = new SqlQuery(single ? "jabber_element" : "jabber_elements")
                .Select("element_text")
                .Where("jid", jid.Bare)
                .Where("element_key", tag + ns);
            using (var db = GetDb())
            {
                return db.ExecList(q)
                    .Select(r => ElementSerializer.DeSerializeElement<Element>((string)r[0]))
                    .ToList();
            }
        }

        private void SaveElements(bool single, Jid jid, params Element[] elements)
        {
            using (var db = GetDb())
            using (var tx = db.BeginTransaction())
            {
                foreach (var e in elements)
                {
                    var i = new SqlInsert(single ? "jabber_element" : "jabber_elements", true)
                        .InColumnValue("jid", jid.Bare)
                        .InColumnValue("element_key", e.TagName + e.Namespace)
                        .InColumnValue("element_text", e.ToString());
                    db.ExecuteNonQuery(i);
                }
                tx.Commit();
            }
        }

        private void RemoveElements(bool single, Jid jid, string tag, string ns)
        {
            using (var db = GetDb())
            {
                var d = new SqlDelete(single ? "jabber_element" : "jabber_elements")
                    .Where("jid", jid.Bare)
                    .Where("element_key", tag + ns);
                db.ExecuteNonQuery(d);
            }
        }
    }
}
