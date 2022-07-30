using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Media;
using TiledSharp;

namespace PushBoxer
{
    public class GameTmxMap
    {
        public TmxMap TmxMap
        {
            get;
            private set;
        }
        public readonly ImageSource[] imageSource;
        public readonly bool[,] MapRoad;

        public GameTmxMap(Stream stream)
        {
            this.TmxMap = new TmxMap(stream);
            this.MapRoad = new bool[this.TmxMap.Width, this.TmxMap.Height];
            for (int y = 0; y < this.TmxMap.Height; y++)
            {
                for (int x = 0; x < this.TmxMap.Width; x++)
                {
                    int point = y * this.TmxMap.Width + x;
                    if (this.TmxMap.Layers[1].Tiles[point].Gid != 0)
                    {
                        this.MapRoad[x, y] = true;
                    }
                    else
                    {
                        this.MapRoad[x, y] = false;
                    }
                }
            }
            List<Bitmap> mapBitmaps = ImageManager.mapBitmaps;
            this.imageSource = new ImageSource[this.TmxMap.Layers.Count];
            Bitmap mapBitmap = new Bitmap(this.TmxMap.Width * this.TmxMap.TileWidth, this.TmxMap.Height * this.TmxMap.TileHeight);
            Graphics graphics = Graphics.FromImage(mapBitmap);
            for (int y = 0; y < this.TmxMap.Height; y++)
            {
                for (int x = 0; x < this.TmxMap.Width; x++)
                {
                    void draw(int gid)
                    {
                        if (gid != 0) graphics.DrawImage(mapBitmaps[gid - 1], x * this.TmxMap.TileWidth, y * this.TmxMap.TileHeight, this.TmxMap.TileWidth, this.TmxMap.TileHeight);
                    };
                    draw(this.TmxMap.Layers[0].Tiles[y * this.TmxMap.Width + x].Gid);
                    if (this.imageSource.Length > 1) draw(this.TmxMap.Layers[1].Tiles[y * this.TmxMap.Width + x].Gid);
                }
            }
            GameManager.Handle(delegate {
                this.imageSource[0] = GameImage.BitmapToBitmapImage(mapBitmap);
                graphics.Dispose();
                mapBitmap.Dispose();
            });

        }
    }
}
