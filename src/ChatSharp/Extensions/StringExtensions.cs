using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ChatSharp.Extensions
{
    public static partial class StringExtensions
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }
    }
}
