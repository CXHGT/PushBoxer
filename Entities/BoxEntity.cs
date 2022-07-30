using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace PushBoxer
{
    public class BoxEntity : Entity,ILoadData
    {
        public ImageSource image1, image2;
        public bool isOnPoint
        {
            get;
            private set;
        }
        public BoxMoveBehavior moveBehavior
        {
            get;
            set;
        }
        public BoxEntity()
        {
            this.Width = 64;
            this.Height = 64;
            image1 = ImageManager.boxEntityImage1;
            image2 = ImageManager.boxEntityImage2;
            this.Source = image1;
            moveBehavior = new BoxMoveBehavior(this);
            moveBehavior.Load(null);
            this.entityBehaviors.Add(moveBehavior);
        }

        public override void Load(object data)
        {
            base.Load(data);
        }

        public override void Update()
        {
            base.Update();
            this.Source = this.image1;
            bool flag = false;
            foreach (Entity entity in MapManager.mainMap.entities)
            {
                if (entity is PointEntity && this.Position == entity.Position)
                {
                    this.Source = this.image2;
                    flag = true;
                    break;
                }
            }
            isOnPoint = flag;
        }
    }
}
