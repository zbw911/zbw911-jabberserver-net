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

            storage.SaveSingleElement(element.To, element);

            var fromdb = storage.GetSingleElement(Jid.Empty, null, null);
            Assert.IsNull(fromdb);

            fromdb = storage.GetSingleElement(element.To, element.TagName, element.Namespace);
            Assert.AreEqual(element.ToString(), fromdb.ToString());

            storage.RemoveSingleElement(element.To, element.TagName, element.Namespace);

            fromdb = storage.GetSingleElement(element.To, element.TagName, element.Namespace);
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

            storage.RemoveElements(element.To, element.TagName, element.Namespace);

            var fromdb = storage.GetElements(Jid.Empty, null, null);
            CollectionAssert.IsEmpty(fromdb);

            fromdb = storage.GetElements(element.To, element.TagName, element.Namespace);
            CollectionAssert.IsEmpty(fromdb);

            storage.SaveElements(element.To, element);
            storage.SaveElements(element.To, element);
            fromdb = storage.GetElements(element.To, element.TagName, element.Namespace);
            Assert.AreEqual(2, fromdb.Count());
            Assert.AreEqual(element.ToString(), fromdb.ElementAt(0).ToString());
            Assert.AreEqual(element.ToString(), fromdb.ElementAt(1).ToString());

            storage.RemoveElements(element.To, element.TagName, element.Namespace);

            fromdb = storage.GetElements(element.To, element.TagName, element.Namespace);
            CollectionAssert.IsEmpty(fromdb);
        }
    }
}
