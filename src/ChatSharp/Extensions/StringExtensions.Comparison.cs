using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ChatSharp.Extensions
{
    public static partial class StringExtensions
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsNoCase(this string value, string other)
        {
            return value.Contains(other, StringComparison.OrdinalIgnoreCase);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsNoCase(this string value, string other)
        {
            if (value.IsEmpty() && other.IsEmpty())
                return true;

            if (value.IsEmpty() && !other.IsEmpty())
                return false;

            if (!value.IsEmpty() && other.IsEmpty())
                return false;

            return value.Equals(other, StringComparison.OrdinalIgnoreCase);
        }
    }
}
