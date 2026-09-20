using System.Collections.Generic;

namespace InventoryKamera
{
    /// <summary>
    /// Deterministic 16:9 1080p inventory grid calculator.
    /// Provides zero-allocation, pre-calculated bounding boxes for Genshin Impact inventory items,
    /// completely eliminating expensive Kirsch filter and BlobCounter loops on standard 1080p displays.
    /// </summary>
    public static class GenshinInventoryGrid
    {
        public const int ColumnsCount = 8;
        public const int ArtifactRowsCount = 4;
        public const int WeaponRowsCount = 5;

        // Exact calibrated 1080p 16:9 geometry
        private const int FirstColCenterX = 183;
        private const int ColSpacing = 147;

        // Row center Y coordinates: Row 0: 245, Row 1: 420 (245+175), Row 2: 595 (420+175), Row 3: 772 (595+177), Row 4: 947 (772+175)
        private static readonly int[] RowCenterY = [245, 420, 595, 772, 947];

        public const int ItemWidth = 125;
        public const int ItemHeight = 153;

        private static readonly List<Rectangle> _artifactGrid1080p;
        private static readonly List<Rectangle> _weaponGrid1080p;

        static GenshinInventoryGrid()
        {
            _artifactGrid1080p = BuildGrid(ColumnsCount, ArtifactRowsCount);
            _weaponGrid1080p = BuildGrid(ColumnsCount, WeaponRowsCount);
        }

        private static List<Rectangle> BuildGrid(int cols, int rows)
        {
            var list = new List<Rectangle>(cols * rows);
            for (int r = 0; r < rows; r++)
            {
                int yCenter = RowCenterY[r];
                int y = yCenter - (ItemHeight / 2); // 76

                for (int c = 0; c < cols; c++)
                {
                    int xCenter = FirstColCenterX + (c * ColSpacing);
                    int x = xCenter - (ItemWidth / 2); // 62

                    list.Add(new Rectangle(x, y, ItemWidth, ItemHeight));
                }
            }
            return list;
        }

        /// <summary>
        /// Checks whether the current game window matches 1920x1080 16:9 resolution.
        /// </summary>
        public static bool IsSupported1080p()
        {
            return Navigation.GetWidth() == 1920 && Navigation.GetHeight() == 1080 && Navigation.IsNormal;
        }

        /// <summary>
        /// Attempts to get the deterministic pre-calibrated grid for standard 1080p resolution.
        /// </summary>
        public static bool TryGetGrid(InventoryPage page, out List<Rectangle> rectangles, out int cols, out int rows)
        {
            if (!IsSupported1080p())
            {
                rectangles = null!;
                cols = 0;
                rows = 0;
                return false;
            }

            cols = ColumnsCount;
            if (page == InventoryPage.Artifacts)
            {
                rows = ArtifactRowsCount;
                rectangles = _artifactGrid1080p;
            }
            else
            {
                rows = WeaponRowsCount;
                rectangles = _weaponGrid1080p;
            }

            return true;
        }
    }
}
