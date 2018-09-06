using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data
{
    /// <summary>
    /// A decimal that is precise without rounding failure
    /// </summary>
    public struct PreciseDecimal : IComparable, IConvertible, IFormattable
    {
        const int N = 8;
        private readonly double _value;

        /// <summary>
        /// Initialize new <see cref="PreciseDecimal"/>
        /// </summary>
        /// <param name="value"></param>
        public PreciseDecimal(double value)
        {
            _value = Math.Round(value, N);
        }

        /// <summary>
        /// Initialize new <see cref="PreciseDecimal"/>
        /// </summary>
        /// <param name="value"></param>
        public PreciseDecimal(PreciseDecimal value)
        {
            _value = value._value;
        }

        // Example of one member of double:
        public static PreciseDecimal operator *(PreciseDecimal d1, PreciseDecimal d2)
        {
            return new PreciseDecimal(d1._value * d2._value);
        }

        public static PreciseDecimal operator +(PreciseDecimal d1, PreciseDecimal d2)
        {
            return new PreciseDecimal(d1._value + d2._value);
        }

        public static PreciseDecimal operator -(PreciseDecimal d1, PreciseDecimal d2)
        {
            return new PreciseDecimal(d1._value - d2._value);
        }

        public static PreciseDecimal operator /(PreciseDecimal d1, PreciseDecimal d2)
        {
            return new PreciseDecimal(d1._value / d2._value);
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

        public static bool operator ==(PreciseDecimal c1, PreciseDecimal c2)
        {
            return c1._value == c2._value;
        }

        public static bool operator >=(PreciseDecimal c1, PreciseDecimal c2)
        {
            return c1._value >= c2._value;
        }

        public static bool operator <=(PreciseDecimal c1, PreciseDecimal c2)
        {
            return c1._value <= c2._value;
        }

        public static bool operator !=(PreciseDecimal c1, PreciseDecimal c2)
        {
            return c1._value != c2._value;
        }

        public static bool operator >(PreciseDecimal c1, PreciseDecimal c2)
        {
            return c1._value > c2._value;
        }

        public static bool operator <(PreciseDecimal c1, PreciseDecimal c2)
        {
            return c1._value < c2._value;
        }

        /// <summary>
        /// Implicit conversion from double to PrecisedDecimal. 
        /// Implicit: No cast operator is required.
        /// </summary>
        public static implicit operator PreciseDecimal(double value)
        {
            return new PreciseDecimal(value);
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to int. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator PreciseDecimal(int value)
        {
            return new PreciseDecimal(value);
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to decimal?. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator PreciseDecimal(decimal value)
        {
            return new PreciseDecimal((double)value);
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to int. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator int(PreciseDecimal value)
        {
            return (int)value._value;
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to float. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator float(PreciseDecimal value)
        {
            return (float)value._value;
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to double. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator double(PreciseDecimal value)
        {
            return value._value;
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to long. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator long(PreciseDecimal value)
        {
            return (long)value._value;
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to uint. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator uint(PreciseDecimal value)
        {
            return (uint)value._value;
        }

        /// <summary>
        /// Explicit conversion from PrecisedDecimal to ulong. 
        /// Explicit: A cast operator is required.
        /// </summary>
        public static explicit operator ulong(PreciseDecimal value)
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
    /// <see cref="PreciseDecimal"/> extensin methods
    /// </summary>
    public static class PreciseDecimalExtension
    {
        /// <summary>
        /// Sum <see cref=PreciseDecimal""/>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PreciseDecimal Sum(this IEnumerable<PreciseDecimal> source)
        {
            if (!source.Any())
                return source.FirstOrDefault();

            return source.Aggregate((x, y) => x + y);
        }

        /// <summary>
        /// Sum <see cref=PreciseDecimal""/>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PreciseDecimal Sum<T>(this IEnumerable<T> source, Func<T, PreciseDecimal> selector)
        {
            if (!source.Select(selector).Any())
                return source.Select(selector).FirstOrDefault();

            return source.Select(selector).Aggregate((x, y) => x + y);
        }
    }
}
