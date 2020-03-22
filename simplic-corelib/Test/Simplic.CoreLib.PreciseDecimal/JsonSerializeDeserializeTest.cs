using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Simplic.Data.Converter;

namespace Simplic.CoreLib.PreciseDecimal
{
    public class SamplePoco
    {
        [JsonConverter(typeof(PreciseDecimalJsonConverter))]
        public Data.PreciseDecimal Value
        {
            get;
            set;
        }
    }

    [TestClass]
    public class JsonSerializeDeserializeTest
    {
        [TestMethod]
        public void SerializeDeserialize()
        {
            var poco = new SamplePoco
            {
                Value = 47.121
            };

            var json = JsonConvert.SerializeObject(poco);
            var deserializedPoco = JsonConvert.DeserializeObject<SamplePoco>(json);

            Assert.AreEqual(deserializedPoco.Value, poco.Value);
        }
    }
}
