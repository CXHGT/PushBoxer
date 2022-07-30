using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;

namespace PushBoxer
{
    public class CreatureEntity : Entity
    {
        public List<ImageSource> imageSources;
        public CreatureEntity()
        {
            this.Width = 64;
            this.Height = 64;
            imageSources = new List<ImageSource>();
        }
        public virtual void LoadImages(List<ImageSource> imageSources)
        {
            this.imageSources = imageSources;
            this.Source = this.imageSources[0];
        }

        public virtual void LoadImages(Bitmap bitmap)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Rectangle rectangle = new Rectangle(x * bitmap.Width / 4, y * bitmap.Height / 4, bitmap.Width / 4, bitmap.Height / 4);
                    Bitmap b = bitmap.Clone(rectangle, bitmap.PixelFormat);
                    b.SetResolution(96, 96);
                    this.imageSources.Add(BitmapToBitmapImage(b));
                    b.Dispose();
                }
            }
            this.Source = this.imageSources[0];
        }
    }
}
