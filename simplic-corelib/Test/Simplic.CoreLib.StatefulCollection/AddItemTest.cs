using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplic.Collections.Generic;

namespace Simplic.CoreLib.StatefulCollection
{
    [TestClass]
    public class AddItemTest
    {
        [TestMethod]
        public void AddEventCancel()
        {
            var collection = new StatefulCollection<string>(new[] { "Hello", "World" });
            collection.AddItem += (s, e) =>
            {
                e.Cancel = e.Item.ToString() != "simplic";

                Assert.IsNotNull(e.Item);
            };

            collection.Add("test");

            Assert.AreEqual(collection.Count, 2);
            Assert.AreEqual(collection.CountNewItems, 0);
            Assert.AreEqual(collection.CountRemovedItems, 0);

            collection.Add("simplic");

            Assert.AreEqual(collection.Count, 2);
            Assert.AreEqual(collection.CountNewItems, 1);
            Assert.AreEqual(collection.CountRemovedItems, 0);

            collection.Commit();

            Assert.AreEqual(collection.Count, 3);
            Assert.AreEqual(collection.CountNewItems, 0);
            Assert.AreEqual(collection.CountRemovedItems, 0);
        }
    }
}