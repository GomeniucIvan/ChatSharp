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
    }
}
