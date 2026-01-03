using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace InventoryKamera.WinUI.Helpers;

public static class ImageHelper
{
    // Converts System.Drawing.Bitmap to a WinUI-compatible ImageSource
    public static async Task<SoftwareBitmapSource> ToWinUI3ImageAsync(Bitmap bmp)
    {
        // 1. Lock the GDI+ bitmap pixels
        // We use ReadOnly because we are just copying OUT of the bitmap.
        BitmapData data = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);

        try
        {
            // 2. Create a generic WinRT SoftwareBitmap
            // WinUI needs Bgra8 (Blue-Green-Red-Alpha), which matches GDI+'s 32bppArgb.
            var softwareBitmap = new SoftwareBitmap(
                BitmapPixelFormat.Bgra8,
                bmp.Width,
                bmp.Height,
                BitmapAlphaMode.Premultiplied);

            // 3. COPY THE PIXELS
            // Calculate total bytes: Stride (width * bytes_per_pixel + padding) * Height
            int totalBytes = data.Stride * data.Height;
            byte[] pixelBuffer = new byte[totalBytes];

            // Copy from unmanaged memory (Scan0) -> managed array (pixelBuffer)
            Marshal.Copy(data.Scan0, pixelBuffer, 0, totalBytes);

            // 4. Fill the SoftwareBitmap from the managed array
            softwareBitmap.CopyFromBuffer(pixelBuffer.AsBuffer());

            // 5. Bind to the XAML Source
            // This must run on the UI thread, but SetBitmapAsync handles the switch automatically.
            var source = new SoftwareBitmapSource();
            await source.SetBitmapAsync(softwareBitmap);

            return source;
        }
        finally
        {
            // Always unlock!
            bmp.UnlockBits(data);
        }
    }
}
