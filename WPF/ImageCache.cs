using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WPF
{
    public static class ImageCache
    {
        private static Dictionary<string, Bitmap> _bitmapCache;

        public static void Initialize()
        {
            _bitmapCache = new Dictionary<string, Bitmap>();
        }

        public static Bitmap GetImgBitmap(string pathName)
        {
            if (!_bitmapCache.ContainsKey(pathName))
                _bitmapCache.Add(pathName, new Bitmap(pathName));
            return _bitmapCache[pathName];
        }
        public static Bitmap CreateEmptyBitmap(int width, int height)
        {
            string emptyKey = "empty";

            if (_bitmapCache.ContainsKey(emptyKey)) return (Bitmap)_bitmapCache[emptyKey].Clone();

            _bitmapCache.Add(emptyKey, new Bitmap(width, height));
            Graphics g = Graphics.FromImage(_bitmapCache[emptyKey]);
            g.FillRectangle(new SolidBrush(System.Drawing.Color.LightGreen), 0, 0, width, height);
            return (Bitmap)_bitmapCache[emptyKey].Clone();
        }
        public static void ClearCache()
        {
            _bitmapCache.Clear();
        }
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
