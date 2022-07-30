using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PushBoxer
{
    /// <summary>
    /// GameScreen.xaml 的交互逻辑
    /// </summary>
    public partial class GameScreen : UserControl,IUpdateable,ILoadData
    {
        //public PlayerEntity player;
        public GameScreen()
        {
            InitializeComponent();
        }

        public void Update()
        {
        }

        public void UpdateCamrea(Entity entity)
        {
            if (entity != null)
            {
                if (entity.Position.Y > this.windowGrid.Height / 2f && entity.Position.Y < this.GameGrid.Height - this.windowGrid.Height / 2f)
                {
                    this.GameGrid.Margin = new Thickness(this.GameGrid.Margin.Left, this.windowGrid.Height / 2f - entity.Position.Y, 0, 0);
                }
                if (entity.Position.X > this.windowGrid.Width / 2f && entity.Position.X < this.GameGrid.Width - this.windowGrid.Width / 2f)
                {
                    this.GameGrid.Margin = new Thickness(this.windowGrid.Width / 2f - entity.Position.X, this.GameGrid.Margin.Top, 0, 0);
                }
            }
        }
        public void Load(object data = null)
        {
            this.GameGrid.Width = MapImage1.Width;
            this.GameGrid.Height = MapImage1.Height;
            this.EntityGrid.Width = MapImage1.Width;
            this.EntityGrid.Height = MapImage1.Height;
            if (data != null && data is Entity entity)
            {
                if (entity.Position.Y < this.windowGrid.Height / 2f)
                {
                    this.GameGrid.Margin = new Thickness(this.GameGrid.Margin.Left, 0, 0, 0);
                }
                else if (entity.Position.Y >= this.GameGrid.Height - this.windowGrid.Height / 2f)
                {
                    this.GameGrid.Margin = new Thickness(this.GameGrid.Margin.Left, this.windowGrid.Height - this.GameGrid.Height, 0, 0);
                }
                else
                {
                    this.GameGrid.Margin = new Thickness(this.GameGrid.Margin.Left, this.windowGrid.Height / 2f - entity.Position.Y, 0, 0);
                }
                if (entity.Position.X < this.windowGrid.Width / 2f)
                {
                    this.GameGrid.Margin = new Thickness(0, this.GameGrid.Margin.Top, 0, 0);
                }
                else if (entity.Position.X >= this.GameGrid.Width - this.windowGrid.Width / 2f)
                {
                    this.GameGrid.Margin = new Thickness(this.windowGrid.Width - this.GameGrid.Width, this.GameGrid.Margin.Top, 0, 0);
                }
                else
                {
                    this.GameGrid.Margin = new Thickness(this.windowGrid.Width / 2f - entity.Position.X, this.GameGrid.Margin.Top, 0, 0);
                }
            }


            /*            if (data is MapBase mapBase)
                        {
                            this.mapBase = mapBase;
                            MapManager.mainMap = mapBase;
                            this.mapBase.Load(this);
                        }
                        else
                        {
                            throw new Exception("the map data error");
                        }*/
        }

    }
}
