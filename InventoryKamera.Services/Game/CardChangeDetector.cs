using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
#nullable enable

namespace InventoryKamera
{
    /// <summary>
    /// Adaptive frame change detector for 16:9 1080p Genshin Impact inventory scanning.
    /// Replaces fixed blind thread sleeps by polling a representative item card slice,
    /// exiting immediately once the UI renders the newly selected item.
    /// </summary>
    public static class CardChangeDetector
    {
        // 1080p 16:9 Card sample slice:
        // Positioned over the card header and stat region (X: 1350, Y: 130, Width: 64, Height: 256)
        private const int SliceX = 1350;
        private const int SliceY = 130;
        private const int SliceWidth = 64;
        private const int SliceHeight = 256;
        private const int TotalPixels = SliceWidth * SliceHeight; // 16,384

        // 24bpp RGB = 3 bytes per pixel. Stride = 192 bytes per row.
        private const int RowStride = 192;
        private const int BufferSize = RowStride * SliceHeight; // 49,152 bytes

        // Pre-allocated buffers to guarantee zero heap allocations during hot scanning loop
        private static readonly byte[] _prevBuffer = new byte[BufferSize];
        private static readonly byte[] _currBuffer = new byte[BufferSize];
        private static Bitmap? _sliceBitmap;
        private static Graphics? _sliceGraphics;
        private static readonly object _lock = new object();

        public static void Initialize()
        {
            lock (_lock)
            {
                Cleanup();
                _sliceBitmap = new Bitmap(SliceWidth, SliceHeight, PixelFormat.Format24bppRgb);
                _sliceGraphics = Graphics.FromImage(_sliceBitmap);
            }
        }

        public static void Cleanup()
        {
            lock (_lock)
            {
                _sliceGraphics?.Dispose();
                _sliceGraphics = null;
                _sliceBitmap?.Dispose();
                _sliceBitmap = null;
            }
        }

        /// <summary>
        /// Captures the current card slice into the baseline buffer prior to clicking.
        /// </summary>
        public static void TakeSample()
        {
            lock (_lock)
            {
                EnsureInitialized();
                CaptureSliceToBuffer(_prevBuffer);
            }
        }

        /// <summary>
        /// Polls until the card region differs from the pre-click baseline, or until timeout.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="minWaitMs">Minimum wait for client input & draw pipeline (default: 15ms)</param>
        /// <param name="pollIntervalMs">Polling interval between frame samples (default: 8ms)</param>
        /// <returns>True if change was detected; false if timed out (e.g. duplicate item)</returns>
        public static bool WaitUntilChanged(CancellationToken cancellationToken, int minWaitMs = 15, int pollIntervalMs = 8)
        {
            lock (_lock)
            {
                EnsureInitialized();

                int elapsed = 0;
                int maxWaitMs = Math.Max(100, (int)(200 * Navigation.GetDelay()));

                // Initial wait for Genshin input & render pipeline
                if (minWaitMs > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Thread.Sleep(minWaitMs);
                    elapsed += minWaitMs;
                }

                while (elapsed < maxWaitMs)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    CaptureSliceToBuffer(_currBuffer);

                    if (HasChanged(_prevBuffer, _currBuffer))
                    {
                        // Card has changed! Small 5ms settle delay to ensure complete render stability
                        Thread.Sleep(5);
                        return true;
                    }

                    Thread.Sleep(pollIntervalMs);
                    elapsed += pollIntervalMs;
                }

                // Timed out (e.g. identical consecutive items or first item on page)
                return false;
            }
        }

        private static void CaptureSliceToBuffer(byte[] targetBuffer)
        {
            if (_sliceBitmap == null || _sliceGraphics == null) return;

            try
            {
                int screenX = Navigation.GetPosition().Left + SliceX;
                int screenY = Navigation.GetPosition().Top + SliceY;

                _sliceGraphics.CopyFromScreen(screenX, screenY, 0, 0, _sliceBitmap.Size);

                var rect = new Rectangle(0, 0, SliceWidth, SliceHeight);
                BitmapData data = _sliceBitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                try
                {
                    Marshal.Copy(data.Scan0, targetBuffer, 0, BufferSize);
                }
                finally
                {
                    _sliceBitmap.UnlockBits(data);
                }
            }
            catch (Exception)
            {
                // Ignore capture failure (e.g. window minimized) during poll
            }
        }

        private static bool HasChanged(byte[] before, byte[] after)
        {
            // Count pixels with color difference > 30.
            // Threshold: >= 150 differing pixels out of 16,384 (~0.9%) to ignore particle shimmer.
            int diffCount = 0;
            const int diffThreshold = 150;
            const int colorThreshold = 30;

            for (int i = 0; i < BufferSize - 2; i += 3)
            {
                int delta = Math.Abs(before[i] - after[i])
                          + Math.Abs(before[i + 1] - after[i + 1])
                          + Math.Abs(before[i + 2] - after[i + 2]);

                if (delta > colorThreshold)
                {
                    diffCount++;
                    if (diffCount >= diffThreshold)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void EnsureInitialized()
        {
            if (_sliceBitmap == null || _sliceGraphics == null)
            {
                Initialize();
            }
        }
    }
}
