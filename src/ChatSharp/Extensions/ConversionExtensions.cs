using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChatSharp.Extensions
{
    public static class ConversionExtensions
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(this string value, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(value);
        }
    }
}
