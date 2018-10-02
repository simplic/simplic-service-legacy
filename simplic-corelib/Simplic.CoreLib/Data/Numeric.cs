using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Data
{
    /// <summary>
    /// A numeric data type which accepts all kinds of numeric types
    /// </summary>
    public struct Numeric : IComparable, IConvertible, IFormattable
    {
        private const int N = 8;
        private readonly double _value;
        private readonly Type _type;

        /// <summary>
        /// Initialize new <see cref="Numeric"/>
        /// </summary>
        /// <param name="value"></param>
        public Numeric(double value)
        {
            _value = value;
            _type = value.GetType();
        }

        /// <summary>
        /// Initialize new <see cref="Numeric"/>
        /// </summary>
        /// <param name="value"></param>
        public Numeric(Numeric value)
        {
            _value = value._value;
            _type = value._type;
        }

        // Example of one member of double:
        public static Numeric operator *(Numeric d1, Numeric d2)
        {
            return new Numeric(d1._value * d2._value);
        }

        public static Numeric operator +(Numeric d1, Numeric d2)
        {
            return new Numeric(d1._value + d2._value);
        }

        public static Numeric operator -(Numeric d1, Numeric d2)
        {
            return new Numeric(d1._value - d2._value);
        }

        public static Numeric operator /(Numeric d1, Numeric d2)
        {
            return new Numeric(d1._value / d2._value);
        }

        public override bool Equals(object obj)
        {
            return _value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            return _value.CompareTo(obj);
        }

        public static bool operator ==(Numeric c1, Numeric c2)
        {
            return c1._value == c2._value;
        }

        public static bool operator >=(Numeric c1, Numeric c2)
        {
            return c1._value >= c2._value;
        }

        public static bool operator <=(Numeric c1, Numeric c2)
        {
            return c1._value <= c2._value;
        }

        public static bool operator !=(Numeric c1, Numeric c2)
        {
            return c1._value != c2._value;
        }

        public static bool operator >(Numeric c1, Numeric c2)
        {
            return c1._value > c2._value;
        }

        public static bool operator <(Numeric c1, Numeric c2)
        {
            return c1._value < c2._value;
        }

        /// <summary>
        /// Implicit conversion from double to PrecisedDecimal. 
        /// Implicit: No cast operator is required.
        /// </summary>
        public static implicit operator Numeric(double value)
        {
            return new Numeric(value);
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to int. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator Numeric(int value)
        {
            return new Numeric(value);
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to decimal?. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator Numeric(decimal value)
        {
            return new Numeric((double)value);
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to int. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator int(Numeric value)
        {
            return (int)value._value;
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to float. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator float(Numeric value)
        {
            return (float)value._value;
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to double. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator double(Numeric value)
        {
            return value._value;
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to long. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator long(Numeric value)
        {
            return (long)value._value;
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to uint. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator uint(Numeric value)
        {
            return (uint)value._value;
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to ulong. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator ulong(Numeric value)
        {
            return (ulong)value._value;
        }


        public string ToString(string format)
        {
            return _value.ToString(format);
        }

        public string ToString(IFormatProvider provider)
        {
            return _value.ToString(provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return _value.ToString(format, provider);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        #region [IConvertible Implementation]
        public TypeCode GetTypeCode()
        {
            return TypeCode.Double;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return _value == 1.0;
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(_value);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(_value);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(_value);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(_value);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(_value);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(_value);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(_value);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(_value);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(_value);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(_value);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(_value);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(_value);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(_value);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(_value, conversionType);
        }
        #endregion
    }

    /// <summary>
    /// <see cref="Numeric"/> extensin methods
    /// </summary>
    public static class NumericExtension
    {
        /// <summary>
        /// Sum <see cref=Numeric""/>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Numeric Sum(this IEnumerable<Numeric> source)
        {
            if (!source.Any())
                return source.FirstOrDefault();

            return source.Aggregate((x, y) => x + y);
        }

        /// <summary>
        /// Sum <see cref=Numeric""/>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Numeric Sum<T>(this IEnumerable<T> source, Func<T, Numeric> selector)
        {
            if (!source.Select(selector).Any())
                return source.Select(selector).FirstOrDefault();

            return source.Select(selector).Aggregate((x, y) => x + y);
        }
    }
}
