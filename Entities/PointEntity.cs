using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace PushBoxer
{
    public class PointEntity : Entity
    {
        public PointEntity()
        {
            this.Width = 64;
            this.Height = 64;
            this.canTrought = true;
            this.Source = ImageManager.pointEntityImage;
            BoxMoveBehavior moveBehavior = new BoxMoveBehavior(this);
            moveBehavior.Load(null);
            this.entityBehaviors.Add(moveBehavior);
        }
    }
}
