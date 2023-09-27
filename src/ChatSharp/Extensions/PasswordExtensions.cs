using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace ChatSharp.Extensions
{
    public class PasswordExtensions
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string CreatePasswordHash(string password, string saltKey)
        {
            var algorithm = (HashAlgorithm)CryptoConfig.CreateFromName("SHA1");
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash algorithm name.");

            var data = string.Concat(password, saltKey).GetBytes();
            return BitConverter.ToString(algorithm.ComputeHash(data)).Replace("-", string.Empty);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string CreateSaltKey(int size)
        {
            using var provider = RandomNumberGenerator.Create();
            var buff = new byte[size];
            provider.GetBytes(buff);

            // Make Base64
            return Convert.ToBase64String(buff);
        }
    }
}
