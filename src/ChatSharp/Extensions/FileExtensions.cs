using Microsoft.Extensions.FileProviders;

namespace ChatSharp.Extensions
{
    public static class FileExtensions
    {
        public static async Task<string> ReadAllTextAsync(this IFileInfo file)
        {
            using (var stream = file.CreateReadStream())
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
