using Newtonsoft.Json;
using System;

namespace Simplic.Data.Converter
{
    /// <summary>
    /// Custom converter for PreciseDecimal, only needed for serialize
    /// </summary>
    public class PreciseDecimalJsonConverter : JsonConverter
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
            if (objectType == typeof(PreciseDecimal) || objectType == typeof(PreciseDecimal?))
            {
                if (value == null) return null;
                return new PreciseDecimal(Convert.ToDouble(value));
            }

            return serializer.Deserialize(reader);
        }

        /// <summary>
        /// Not required  
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}