using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ChatSharp.Extensions
{
    public static class ObjectExtensions
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Convert<T>(this object value)
        {
            if (TryConvert(value, typeof(T), CultureInfo.InvariantCulture, out object result))
            {
                return (T)result;
            }

            return default;
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryConvert(object value, Type to, CultureInfo culture, out object convertedValue)
        {
            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            convertedValue = null;

            if (value == null || value == DBNull.Value)
            {
                return to == typeof(string) || to.IsBasicType() == false;
            }

            if (to != typeof(object) && to.IsInstanceOfType(value))
            {
                convertedValue = value;
                return true;
            }

            Type from = value.GetType();

            if (culture == null)
            {
                culture = CultureInfo.InvariantCulture;
            }

            try
            {
                // Get a converter for 'to' (value -> to)
                var converter = TypeDescriptor.GetConverter(to);
                if (converter != null && converter.CanConvertFrom(from))
                {
                    convertedValue = converter.ConvertFrom(null, culture, value);
                    return true;
                }

                // Try the other way round with a 'from' converter (to <- from)
                converter = TypeDescriptor.GetConverter(from);
                if (converter != null && converter.CanConvertTo(to))
                {
                    convertedValue = converter.ConvertTo(null, culture, value, to);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static bool IsBasicType(this Type type)
        {
            return
                type.IsPrimitive ||
                type.IsEnum ||
                type == typeof(string) ||
                type == typeof(decimal) ||
                type == typeof(DateTime) ||
                type == typeof(TimeSpan) ||
                type == typeof(Guid) ||
                type == typeof(byte[]);
        }
    }
}
