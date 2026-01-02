using System.Drawing.Imaging;

namespace InventoryKamera.Helpers;

public static class BitmapHelper
{
    public static Color GetAverageColor(Bitmap bm)
    {
        // We lock the bits for high performance (GetPixel is too slow)
        BitmapData srcData = bm.LockBits(
            new Rectangle(0, 0, bm.Width, bm.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);

        int stride = srcData.Stride;
        int width = bm.Width;
        int height = bm.Height;

        long sumR = 0, sumG = 0, sumB = 0;
        long pixelCount = width * height;

        unsafe
        {
            byte* p = (byte*)srcData.Scan0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 32bpp structure is generally: B, G, R, A
                    int offset = (y * stride) + (x * 4);

                    byte b = p[offset];
                    byte g = p[offset + 1];
                    byte r = p[offset + 2];
                    // we ignore alpha (offset + 3) for color average usually

                    sumB += b;
                    sumG += g;
                    sumR += r;
                }
            }
        }

        bm.UnlockBits(srcData);

        return Color.FromArgb(
            255,
            (int)(sumR / pixelCount),
            (int)(sumG / pixelCount),
            (int)(sumB / pixelCount)
        );
    }

    public struct IntRange
    {
        public int Min;
        public int Max;

        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}