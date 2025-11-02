using System;
using System.IO;

namespace GestionEmpleadosApp.Helpers
{
    public static class ImageHelper
    {
        // Convierte un Stream de imagen a Base64
        public static async Task<string> StreamToBase64Async(Stream stream)
        {
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            byte[] bytes = ms.ToArray();
            return Convert.ToBase64String(bytes);
        }

        // Convierte Base64 a ImageSource para mostrar en UI
        public static ImageSource Base64ToImageSource(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64)) return null;
            byte[] bytes = Convert.FromBase64String(base64);
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }
    }
}

