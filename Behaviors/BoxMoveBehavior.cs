using System;
using System.Collections.Generic;
using System.Text;

namespace PushBoxer
{
    public class BoxMoveBehavior : MoveBehavior, IUpdateable, ILoadData
    {
        public BoxEntity boxEntity;
        public BoxMoveBehavior(Entity entity) : base(entity)
        {
            this.boxEntity = entity as BoxEntity;
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Load(object data)
        {
        }
        public override object Save()
        {
            return base.Save();
        }
        public override bool OnMove(MoveType moveType, bool trought = false)
        {
            return base.OnMove(moveType);
        }

        public override bool CanMove(MoveType moveType)
        {
            Point2 point2 = this.entity.BasePosition;
            Vector2 vector2 = this.entity.Position;
            MapBase mapBase = MapManager.mainMap;
            GameTmxMap gameTmxMap = mapBase.gameTmxMap;
            bool flag = false;
            switch (moveType)
            {
                case MoveType.DOWN:
                    point2 += new Point2(0,1);
                    if (gameTmxMap.TmxMap.Height > point2.Y && gameTmxMap.MapRoad[point2.X, point2.Y])
                        flag = true;
                    break;
                case MoveType.UP:
                    vector2 += new Vector2(0, 63);
                    point2 = new Point2((int)vector2.X / 64, (int)vector2.Y / 64 - 1);
                    if (point2.Y > 0 && gameTmxMap.MapRoad[point2.X, point2.Y])
                        flag = true;
                    break;
                case MoveType.LEFT:
                    vector2 += new Vector2(63, 0);
                    point2 = new Point2((int)vector2.X / 64 - 1, (int)vector2.Y / 64);
                    if (point2.X  > 0 && gameTmxMap.MapRoad[point2.X, point2.Y])
                        flag = true;
                    break;
                case MoveType.RIGHT:
                    point2 += new Point2(1, 0);
                    if (gameTmxMap.TmxMap.Width > point2.X && gameTmxMap.MapRoad[point2.X, point2.Y])
                        flag = true;
                    break;
            }
            foreach (Entity entity in mapBase.entities)
            {
                if (entity.BasePosition == point2 && entity.canTrought == false && entity != this.entity) flag = false;
            }
            return flag;
        }
    }
}
