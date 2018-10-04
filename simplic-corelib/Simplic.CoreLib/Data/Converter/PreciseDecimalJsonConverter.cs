using Newtonsoft.Json;
using System;

namespace Simplic.Data.Converter
{
    /// <summary>
    /// Custom converter for PreciseDecimal, only needed for serialize
    /// </summary>
    internal class PreciseDecimalJsonConverter : JsonConverter
    {
        /// <summary>
        /// Sets what type can convert
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <summary>
        /// Handles the PreciseDecimal properties
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value;
            if (reader.ValueType == typeof(float) || reader.ValueType == typeof(double) || reader.ValueType == typeof(decimal))
            {
                PreciseDecimal newValue = new PreciseDecimal(Convert.ToDouble(value));
                return newValue;
            }
            return serializer.Deserialize(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}