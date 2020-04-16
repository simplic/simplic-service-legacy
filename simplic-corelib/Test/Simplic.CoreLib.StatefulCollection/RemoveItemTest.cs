using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplic.Collections.Generic;

namespace Simplic.CoreLib.StatefulCollection
{
    [TestClass]
    public class RemoveItemTestItemTest
    {
        [TestMethod]
        public void RemoveEventCancel()
        {
            var collection = new StatefulCollection<string>(new[] { "Hello", "World", "Simplic" });
            collection.RemoveItem += (s, e) =>
            {
                e.Cancel = e.Item.ToString() != "Simplic";

                Assert.IsNotNull(e.Item);
                Assert.AreNotEqual(e.Item, "NotInList");
            };

            collection.Remove("NotInList");

            Assert.AreEqual(collection.Count, 3);
            Assert.AreEqual(collection.CountNewItems, 0);
            Assert.AreEqual(collection.CountRemovedItems, 0);

            collection.Remove("Hello");

            Assert.AreEqual(collection.Count, 3);
            Assert.AreEqual(collection.CountNewItems, 0);
            Assert.AreEqual(collection.CountRemovedItems, 0);

            collection.Remove("Simplic");

            Assert.AreEqual(collection.Count, 2);
            Assert.AreEqual(collection.CountNewItems, 0);
            Assert.AreEqual(collection.CountRemovedItems, 1);

            collection.Commit();

            Assert.AreEqual(collection.Count, 2);
            Assert.AreEqual(collection.CountNewItems, 0);
            Assert.AreEqual(collection.CountRemovedItems, 0);
        }
    }
}