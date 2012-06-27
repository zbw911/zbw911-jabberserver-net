using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using Jabber.Net.Server.Storages;
using NUnit.Framework;

namespace Jabber.Net.Tests.Storages
{
    [TestFixture]
    public class XmppElementStorageTest
    {
        private readonly IXmppElementStorage storage;


        public XmppElementStorageTest()
        {
            storage = new XmppElementStorage("elements");
        }


        [Test]
        public void SingleElementTest()
        {
            var element = new Message()
            {
                From = new Jid("from@s/R"),
                To = new Jid("to@s/R"),
                Type = MessageType.headline,
                Body = "body",
                Subject = "subject",
            };
            var key = "key";

            storage.SaveSingleElement(element.To, key, element);

            var fromdb = storage.GetSingleElement(Jid.Empty, null);
            Assert.IsNull(fromdb);

            fromdb = storage.GetSingleElement(element.To, key);
            Assert.AreEqual(element.ToString(), fromdb.ToString());

            storage.RemoveSingleElement(element.To, key);

            fromdb = storage.GetSingleElement(element.To, key);
            Assert.IsNull(fromdb);
        }

        [Test]
        public void ElementsTest()
        {
            var element = new Message()
            {
                From = new Jid("from@s/R"),
                To = new Jid("to@s/R"),
                Type = MessageType.headline,
                Body = "body",
                Subject = "subject",
            };
            var key = "key";

            storage.RemoveElements(element.To, key);

            var fromdb = storage.GetElements(Jid.Empty, null);
            CollectionAssert.IsEmpty(fromdb);

            fromdb = storage.GetElements(element.To, key);
            CollectionAssert.IsEmpty(fromdb);

            storage.SaveElements(element.To, key, element);
            storage.SaveElements(element.To, key, element);
            fromdb = storage.GetElements(element.To, key);
            Assert.AreEqual(2, fromdb.Count());
            Assert.AreEqual(element.ToString(), fromdb.ElementAt(0).ToString());
            Assert.AreEqual(element.ToString(), fromdb.ElementAt(1).ToString());

            storage.RemoveElements(element.To, key);

            fromdb = storage.GetElements(element.To, key);
            CollectionAssert.IsEmpty(fromdb);
        }
    }
}
