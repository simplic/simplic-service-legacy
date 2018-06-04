using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplic.Cache.Service;
using System;

namespace Simplic.Cache.Test
{
    [TestClass]
    public class CacheTest
    {
        private readonly UnityContainer container;

        public CacheTest()
        {
            container = new UnityContainer();
            container.RegisterType<ICacheService, CacheService>();
            container.RegisterType<IWeakReferenceCacheService, WeakReferenceCacheService>();
        }

        [TestMethod]
        public void SetGetTest()
        {
            var service = container.Resolve<ICacheService>();

            var key = "Sample_Cache_Key";
            var value = Guid.NewGuid();

            service.Set(key, value);

            Assert.AreEqual(value, service.Get<Guid>(key));

            Assert.AreEqual(default(Guid), service.Get<Guid>(Guid.NewGuid().ToString()));

            service.Remove<Guid>(key);
            Assert.AreEqual(default(Guid), service.Get<Guid>(key));
        }

        [TestMethod]
        public void SetGetWeakReferenceTest()
        {
            var service = container.Resolve<IWeakReferenceCacheService>();

            var key = "Sample_Cache_Key_WK";
            var value = Guid.NewGuid();

            service.Set(key, value);

            Assert.AreEqual(value, service.Get<Guid>(key));

            Assert.AreEqual(default(Guid), service.Get<Guid>(Guid.NewGuid().ToString()));

            service.Remove<Guid>(key);
            Assert.AreEqual(default(Guid), service.Get<Guid>(key));
        }

        [TestMethod]
        public void SetGetObjectTest()
        {
            var service = container.Resolve<ICacheService>();

            var obj = new CacheTestObject()
            {
                CacheKey = "Test_Key"
            };

            service.Set(obj);

            Assert.AreEqual(obj, service.Get<CacheTestObject>(obj.CacheKey));

            Assert.IsNull(service.Get<CacheTestObject>(Guid.NewGuid().ToString()));

            service.Remove<CacheTestObject>(obj.CacheKey);
            Assert.IsNull(service.Get<CacheTestObject>(obj.CacheKey));
        }

        [TestMethod]
        public void SetGetObjectWeakReferenceTest()
        {
            var service = container.Resolve<IWeakReferenceCacheService>();

            var obj = new CacheTestObject()
            {
                CacheKey = "Test_Key_WR"
            };

            service.Set(obj);

            Assert.AreEqual(obj, service.Get<CacheTestObject>(obj.CacheKey));

            Assert.IsNull(service.Get<CacheTestObject>(Guid.NewGuid().ToString()));

            service.Remove<CacheTestObject>(obj.CacheKey);
            Assert.IsNull(service.Get<CacheTestObject>(obj.CacheKey));
        }
    }
}
