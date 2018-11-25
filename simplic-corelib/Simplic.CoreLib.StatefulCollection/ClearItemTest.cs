using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplic.Collections.Generic;

namespace Simplic.CoreLib.StatefulCollection
{
    [TestClass]
    public class ClearItemTest
    {
        [TestMethod]
        public void ClearEventCancel()
        {
            var cancel = true;
            var collection = new StatefulCollection<string>(new[] { "Hello", "World", "Simplic" });
            collection.ClearCollection += (s, e) =>
            {
                e.Cancel = cancel;
            };

            collection.Clear();

            Assert.AreEqual(collection.Count, 3);
            Assert.AreEqual(collection.CountNewItems, 0);
            Assert.AreEqual(collection.CountRemovedItems, 0);

            cancel = false;
            collection.Clear();

            Assert.AreEqual(collection.Count, 0);
            Assert.AreEqual(collection.CountNewItems, 0);
            Assert.AreEqual(collection.CountRemovedItems, 0);

            // Test without event
            collection = new StatefulCollection<string>(new[] { "Hello", "World", "Simplic" });

            collection.Clear();

            Assert.AreEqual(collection.Count, 0);
            Assert.AreEqual(collection.CountNewItems, 0);
            Assert.AreEqual(collection.CountRemovedItems, 0);
        }
    }
}