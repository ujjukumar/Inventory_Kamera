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

        try
        {
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
        }
        finally
        {
            bm.UnlockBits(srcData);
        }

        return Color.FromArgb(
            255,
            (int)(sumR / pixelCount),
            (int)(sumG / pixelCount),
            (int)(sumB / pixelCount)
        );
    }

    public static unsafe Tesseract.Pix ConvertToPix(Bitmap bmp)
    {
        int width = bmp.Width;
        int height = bmp.Height;
        var pix = Tesseract.Pix.Create(width, height, 32);
        var pixData = pix.GetData();

        BitmapData bmpData = bmp.LockBits(
            new Rectangle(0, 0, width, height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);

        try
        {
            uint* pPixBase = (uint*)pixData.Data;
            int pixWordsPerLine = pixData.WordsPerLine;

            byte* pBmpBase = (byte*)bmpData.Scan0;
            int bmpStride = bmpData.Stride;

            for (int y = 0; y < height; y++)
            {
                uint* pPixLine = pPixBase + (y * pixWordsPerLine);
                byte* pBmpLine = pBmpBase + (y * bmpStride);

                for (int x = 0; x < width; x++)
                {
                    int x4 = x * 4;
                    byte b = pBmpLine[x4];
                    byte g = pBmpLine[x4 + 1];
                    byte r = pBmpLine[x4 + 2];
                    byte a = pBmpLine[x4 + 3];

                    // Leptonica EncodeAsRGBA: ((uint)r << 24) | ((uint)g << 16) | ((uint)b << 8) | a
                    pPixLine[x] = ((uint)r << 24) | ((uint)g << 16) | ((uint)b << 8) | a;
                }
            }

            if (bmp.HorizontalResolution > 0) pix.XRes = (int)Math.Round(bmp.HorizontalResolution);
            if (bmp.VerticalResolution > 0) pix.YRes = (int)Math.Round(bmp.VerticalResolution);
        }
        finally
        {
            bmp.UnlockBits(bmpData);
        }

        return pix;
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