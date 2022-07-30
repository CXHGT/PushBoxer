using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PushBoxer
{
    public static class ImageManager
    {
        public static ImageSource boxEntityImage1;
        public static ImageSource boxEntityImage2;
        public static ImageSource pointEntityImage;
        public static List<ImageSource> playerEntityImageSources;
        public static List<Bitmap> mapBitmaps;

        public static void Load()
        {
            boxEntityImage1 = GameImage.BitmapToBitmapImage(Resource.box1);
            boxEntityImage2 = GameImage.BitmapToBitmapImage(Resource.box2);
            pointEntityImage = GameImage.BitmapToBitmapImage(Resource.point4);
            playerEntityImageSources = new List<ImageSource>();
            mapBitmaps = new List<Bitmap>();

            Bitmap bitmap = Resource.Player;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Rectangle rectangle = new Rectangle(x * bitmap.Width / 4, y * bitmap.Height / 4, bitmap.Width / 4, bitmap.Height / 4);
                    Bitmap b = bitmap.Clone(rectangle, bitmap.PixelFormat);
                    b.SetResolution(96, 96);
                    playerEntityImageSources.Add(GameImage.BitmapToBitmapImage(b));
                    b.Dispose();
                }
            }
            //bitmap.Dispose();
            bitmap = Resource.MapImage1;
            int bitSize = 64;
            for (int y = 0; y < bitmap.Height / bitSize; y++)
            {
                for (int x = 0; x < bitmap.Width / bitSize; x++)
                {
                    Rectangle rectangle = new Rectangle(x * bitSize, y * bitSize, bitSize, bitSize);
                    Bitmap b = bitmap.Clone(rectangle, bitmap.PixelFormat);
                    b.SetResolution(96, 96);
                    mapBitmaps.Add(b);
                }
                //bitmap.Dispose();
            }



        }
    }
}
