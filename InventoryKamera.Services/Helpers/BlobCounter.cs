using System.Drawing.Imaging;

namespace InventoryKamera.Helpers;

public class BlobCounter : IDisposable
{
    // Configuration Properties
    public bool FilterBlobs { get; set; } = false;
    public int MinWidth { get; set; } = 0;
    public int MaxWidth { get; set; } = int.MaxValue;
    public int MinHeight { get; set; } = 0;
    public int MaxHeight { get; set; } = int.MaxValue;

    // Internal storage for detected blobs
    private List<Rectangle> _blobs = [];

    /// <summary>
    /// Detects blobs in the given binary image.
    /// </summary>
    public void ProcessImage(Bitmap image)
    {
        _blobs.Clear();

        int width = image.Width;
        int height = image.Height;
        bool[,] visited = new bool[width, height];

        BitmapData data = image.LockBits(
            new Rectangle(0, 0, width, height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);

        try
        {
            unsafe
            {
                byte* ptrBase = (byte*)data.Scan0;
                int stride = data.Stride;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (visited[x, y]) continue;

                        // Check if pixel is White (we assume binary image from Threshold)
                        // In 32bpp, R is at offset 2. If R > 128, it's white.
                        byte* ptrPixel = ptrBase + (y * stride) + (x * 4);
                        if (ptrPixel[2] > 128)
                        {
                            // Found a new blob, start tracing
                            Rectangle blobRect = TraceBlob(x, y, width, height, stride, ptrBase, visited);

                            if (ShouldAdd(blobRect))
                            {
                                _blobs.Add(blobRect);
                            }
                        }
                    }
                }
            }
        }
        finally
        {
            image.UnlockBits(data);
        }
    }

    /// <summary>
    /// Returns the list of detected rectangles.
    /// </summary>
    public Rectangle[] GetObjectsRectangles()
    {
        return _blobs.ToArray();
    }

    // Standard BFS Flood Fill to find the extent of the blob
    private unsafe Rectangle TraceBlob(int startX, int startY, int width, int height, int stride, byte* ptrBase, bool[,] visited)
    {
        int minX = startX, maxX = startX;
        int minY = startY, maxY = startY;

        Queue<Point> queue = new Queue<Point>();
        queue.Enqueue(new Point(startX, startY));
        visited[startX, startY] = true;

        while (queue.Count > 0)
        {
            Point pt = queue.Dequeue();

            if (pt.X < minX) minX = pt.X;
            if (pt.X > maxX) maxX = pt.X;
            if (pt.Y < minY) minY = pt.Y;
            if (pt.Y > maxY) maxY = pt.Y;

            // Check 8 neighbors (Connect-8)
            for (int ny = pt.Y - 1; ny <= pt.Y + 1; ny++)
            {
                for (int nx = pt.X - 1; nx <= pt.X + 1; nx++)
                {
                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    {
                        if (!visited[nx, ny])
                        {
                            byte* ptr = ptrBase + (ny * stride) + (nx * 4);
                            // Check if white
                            if (ptr[2] > 128)
                            {
                                visited[nx, ny] = true;
                                queue.Enqueue(new Point(nx, ny));
                            }
                        }
                    }
                }
            }
        }

        return new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
    }

    private bool ShouldAdd(Rectangle rect)
    {
        if (!FilterBlobs) return true;

        return rect.Width >= MinWidth &&
               rect.Width <= MaxWidth &&
               rect.Height >= MinHeight &&
               rect.Height <= MaxHeight;
    }

    public void Dispose()
    {
        // Nothing to dispose, but required to keep "using" statements valid
    }
}