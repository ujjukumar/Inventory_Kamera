using System.Drawing.Imaging;

namespace InventoryKamera.Helpers;

public static class Extensions
{
    /// <summary>
    /// Calculates the center point of the specified rectangle. Replicates the Accord Center() method
    /// </summary>
    /// <param name="rect">The rectangle for which to determine the center point.</param>
    /// <returns>A <see cref="Point"/> representing the center of the rectangle. If the rectangle has zero width or height, the
    /// center will be at the midpoint of the respective sides.</returns>
    public static Point Center(this Rectangle rect)
    {
        return new Point(
            rect.X + rect.Width / 2,
            rect.Y + rect.Height / 2
        );
    }

    /// <summary>
    /// Converts a bitmap to Grayscale using BT.709 coefficients.
    /// Replaces Accord's new Grayscale(0.2125, 0.7154, 0.0721).Apply(bitmap)
    /// </summary>
    public static Bitmap ConvertToGrayscale(this Bitmap original)
    {
        Bitmap newBitmap = new Bitmap(original.Width, original.Height);

        using (Graphics g = Graphics.FromImage(newBitmap))
        {
            ColorMatrix colorMatrix = new ColorMatrix(
            [
                    [0.2125f, 0.2125f, 0.2125f, 0, 0],
                    [0.7154f, 0.7154f, 0.7154f, 0, 0],
                    [0.0721f, 0.0721f, 0.0721f, 0, 0],
                    [0,       0,       0,       1, 0],
                    [0,       0,       0,       0, 1]
            ]);

            using ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            g.DrawImage(original,
                new Rectangle(0, 0, original.Width, original.Height),
                0, 0, original.Width, original.Height,
                GraphicsUnit.Pixel, attributes);
        }

        return newBitmap;
    }

    /// <summary>
    /// Adjusts the contrast of the specified bitmap in place using the given contrast value.
    /// </summary>
    /// <remarks>This method modifies the original bitmap and does not create a copy. The contrast value is
    /// automatically clamped to the valid range of -255 to 255. The operation affects all color channels except alpha.
    /// For best results, use with bitmaps that support 24bpp or 32bpp pixel formats.</remarks>
    /// <param name="bmp">The bitmap image to modify. The contrast adjustment is applied directly to this instance.</param>
    /// <param name="contrast">The amount of contrast adjustment to apply, in the range -255 to 255. Values greater than 0 increase contrast;
    /// values less than 0 decrease contrast.</param>
    public static void AdjustContrast(this Bitmap bmp, int contrast)
    {
        // Clamp contrast value to valid range for this algorithm (-255 to 255)
        if (contrast < -255) contrast = -255;
        if (contrast > 255) contrast = 255;

        // 1. Calculate the factor
        double factor = (259.0 * (contrast + 255.0)) / (255.0 * (259.0 - contrast));

        // 2. Pre-calculate the Lookup Table (LUT) for performance
        //    (Calculating this for every pixel is too slow)
        byte[] lut = new byte[256];
        for (int i = 0; i < 256; i++)
        {
            double newValue = factor * (i - 128) + 128;

            // Clamp result to byte range
            if (newValue < 0) newValue = 0;
            else if (newValue > 255) newValue = 255;

            lut[i] = (byte)newValue;
        }

        // 3. Apply to the image using LockBits
        BitmapData data = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadWrite,
            bmp.PixelFormat);

        try
        {
            int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            int heightInPixels = data.Height;
            int widthInBytes = data.Width * bytesPerPixel;
            int stride = data.Stride;

            unsafe
            {
                byte* ptrFirstPixel = (byte*)data.Scan0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* currentLine = ptrFirstPixel + (y * stride);

                    for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                    {
                        // Apply the LUT to Blue, Green, Red
                        // (Note: Alpha is usually at x+3, we leave it alone)

                        currentLine[x] = lut[currentLine[x]];         // Blue
                        currentLine[x + 1] = lut[currentLine[x + 1]]; // Green
                        currentLine[x + 2] = lut[currentLine[x + 2]]; // Red
                    }
                }
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }
    }

    /// <summary>
    /// Inverts the colors of the specified bitmap in place by replacing each pixel's color with its complementary
    /// color.
    /// </summary>
    /// <remarks>This method modifies the original bitmap directly. The alpha channel, if present, is not
    /// affected. The bitmap must be writable; otherwise, an exception may be thrown.</remarks>
    /// <param name="bmp">The bitmap whose colors will be inverted. Cannot be null.</param>
    public static void InvertColors(this Bitmap bmp)
    {
        BitmapData data = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadWrite,
            bmp.PixelFormat);

        try
        {
            int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            int heightInPixels = data.Height;
            int widthInBytes = data.Width * bytesPerPixel;
            int stride = data.Stride;

            unsafe
            {
                byte* ptrFirstPixel = (byte*)data.Scan0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* currentLine = ptrFirstPixel + (y * stride);

                    for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                    {
                        // Invert Blue, Green, Red
                        // Formula: New = 255 - Old

                        currentLine[x] = (byte)(255 - currentLine[x]);     // Blue
                        currentLine[x + 1] = (byte)(255 - currentLine[x + 1]); // Green
                        currentLine[x + 2] = (byte)(255 - currentLine[x + 2]); // Red

                        // We typically leave the Alpha channel (x+3) alone
                    }
                }
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }
    }

    /// <summary>
    /// Applies a binary threshold to the specified bitmap, converting each pixel to black or white based on the given
    /// threshold value.
    /// </summary>
    /// <remarks>This method assumes the bitmap is in a format where the red, green, and blue channels
    /// represent intensity (such as a grayscale image). If used on a color image, the threshold is applied based on the
    /// red channel. The alpha channel, if present, is not modified. The operation modifies the original bitmap and is
    /// not thread-safe.</remarks>
    /// <param name="bmp">The bitmap to which the threshold will be applied. The bitmap is modified in place.</param>
    /// <param name="threshold">The intensity threshold value. Pixels with intensity greater than or equal to this value are set to white;
    /// others are set to black. Valid values are 0 to 255.</param>
    public static void ApplyThreshold(this Bitmap bmp, int threshold)
    {
        BitmapData data = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadWrite,
            bmp.PixelFormat);

        try
        {
            int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            int heightInPixels = data.Height;
            int widthInBytes = data.Width * bytesPerPixel;
            int stride = data.Stride;

            unsafe
            {
                byte* ptrFirstPixel = (byte*)data.Scan0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* currentLine = ptrFirstPixel + (y * stride);

                    for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                    {
                        // We check the Red component (currentLine[x+2]) 
                        // assuming the image is already Grayscale from previous steps.
                        // If it's not, this effectively thresholds based on the Red channel.

                        // Logic: If intensity >= threshold, set to White (255), else Black (0)
                        byte pixelValue = currentLine[x + 2];
                        byte binaryValue = (pixelValue >= threshold) ? (byte)255 : (byte)0;

                        currentLine[x] = binaryValue; // Blue
                        currentLine[x + 1] = binaryValue; // Green
                        currentLine[x + 2] = binaryValue; // Red

                        // Leave Alpha (x+3) unchanged
                    }
                }
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }
    }

    /// <summary>
    /// Applies the Kirsch edge detection filter to the specified bitmap and returns a new bitmap with the filter
    /// applied.
    /// </summary>
    /// <remarks>The Kirsch filter highlights edges in the image by emphasizing regions with high intensity
    /// changes in eight compass directions. The resulting image is in 32bpp ARGB format, with edge strength represented
    /// in grayscale. The original bitmap is not modified.</remarks>
    /// <param name="source">The source bitmap to which the Kirsch filter will be applied. Must not be null.</param>
    /// <returns>A new Bitmap instance containing the result of the Kirsch edge detection filter. The returned bitmap has the
    /// same dimensions as the source.</returns>
    public static Bitmap ApplyKirschFilter(this Bitmap source)
    {
        int width = source.Width;
        int height = source.Height;

        // Create destination bitmap
        Bitmap resultBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

        BitmapData srcData = source.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        BitmapData dstData = resultBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        try
        {
            int stride = srcData.Stride;
            int bytesPerPixel = 4; // 32bpp

            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;

                // We skip the 1-pixel border to avoid boundary checks inside the loop
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        // Get pointers to the 3x3 grid around the current pixel
                        // Pointers are: [row][col] relative to center
                        // r0 = y-1, r1 = y, r2 = y+1
                        byte* r0 = srcPtr + (y - 1) * stride + (x - 1) * bytesPerPixel;
                        byte* r1 = srcPtr + (y) * stride + (x - 1) * bytesPerPixel;
                        byte* r2 = srcPtr + (y + 1) * stride + (x - 1) * bytesPerPixel;

                        // Read luminance (grayscale) of the 3x3 grid
                        // We use Green (offset +1) as a cheap proxy for luminance, 
                        // or average (R+G+B)/3. Let's use average for accuracy.

                        int p00 = (r0[0] + r0[1] + r0[2]) / 3; // Top-Left
                        int p01 = (r0[4] + r0[5] + r0[6]) / 3; // Top-Mid
                        int p02 = (r0[8] + r0[9] + r0[10]) / 3; // Top-Right

                        int p10 = (r1[0] + r1[1] + r1[2]) / 3; // Mid-Left
                        // int p11 = center pixel, unused in Kirsch mask usually (multiplied by 0)
                        int p12 = (r1[8] + r1[9] + r1[10]) / 3; // Mid-Right

                        int p20 = (r2[0] + r2[1] + r2[2]) / 3; // Bot-Left
                        int p21 = (r2[4] + r2[5] + r2[6]) / 3; // Bot-Mid
                        int p22 = (r2[8] + r2[9] + r2[10]) / 3; // Bot-Right

                        // Kirsch Operator: Max of 8 compass directions
                        // Mask weights are 5 vs -3.

                        int maxGradient = 0;

                        // Direction N
                        int g = 5 * (p00 + p01 + p02) - 3 * (p20 + p21 + p22 + p10 + p12);
                        if (g > maxGradient) maxGradient = g;

                        // Direction W
                        g = 5 * (p00 + p10 + p20) - 3 * (p01 + p02 + p12 + p22 + p21);
                        if (g > maxGradient) maxGradient = g;

                        // Direction S
                        g = 5 * (p20 + p21 + p22) - 3 * (p00 + p01 + p02 + p10 + p12);
                        if (g > maxGradient) maxGradient = g;

                        // Direction E
                        g = 5 * (p02 + p12 + p22) - 3 * (p00 + p10 + p20 + p01 + p21);
                        if (g > maxGradient) maxGradient = g;

                        // Direction NW
                        g = 5 * (p01 + p00 + p10) - 3 * (p02 + p12 + p22 + p21 + p20);
                        if (g > maxGradient) maxGradient = g;

                        // Direction SW
                        g = 5 * (p10 + p20 + p21) - 3 * (p00 + p01 + p02 + p12 + p22);
                        if (g > maxGradient) maxGradient = g;

                        // Direction SE
                        g = 5 * (p21 + p22 + p12) - 3 * (p20 + p10 + p00 + p01 + p02);
                        if (g > maxGradient) maxGradient = g;

                        // Direction NE
                        g = 5 * (p12 + p02 + p01) - 3 * (p22 + p21 + p20 + p10 + p00);
                        if (g > maxGradient) maxGradient = g;

                        // Clamp to 255
                        byte val = (byte)(maxGradient > 255 ? 255 : (maxGradient < 0 ? 0 : maxGradient));

                        // Write to destination (set R, G, B to result)
                        byte* dstPixel = dstPtr + (y * stride) + (x * bytesPerPixel);
                        dstPixel[0] = val; // B
                        dstPixel[1] = val; // G
                        dstPixel[2] = val; // R
                        dstPixel[3] = 255; // Alpha
                    }
                }
            }
        }
        finally
        {
            source.UnlockBits(srcData);
            resultBitmap.UnlockBits(dstData);
        }

        return resultBitmap;
    }
}