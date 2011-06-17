using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Jabber.Net.Server.Tests
{
    [TestFixture]
    public class TmpTest
    {
        [Test]
        public void TmpMethod()
        {
            var e = Activator.CreateInstance(typeof(Exception), 3);
        }
    }
}
