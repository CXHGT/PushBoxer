using System;
using System.Collections.Generic;
using System.Text;

namespace PushBoxer
{
    public class MoveBehavior : EntityBehavior, IUpdateable, ILoadData
    {
        public float speed = 4;
        //protected Point2 point2;
        public MoveBehavior(Entity entity) : base(entity)
        {
        }
        public override void Update()
        {
            base.Update();
        }
        public virtual bool OnMove(MoveType moveType, bool trought = false)
        {
            if (CanMove(moveType))
            {
                switch (moveType)
                {
                    case MoveType.DOWN:
                        EntityMove(new Vector2(0, this.speed));
                        break;
                    case MoveType.UP:
                            EntityMove(new Vector2(0, -this.speed));
                        break;
                    case MoveType.LEFT:
                            EntityMove(new Vector2(-this.speed));
                        break;
                    case MoveType.RIGHT:
                            EntityMove(new Vector2(this.speed));
                        break;
                }
                return true;
            }
            return false;
        }
        public virtual bool CanMove(MoveType moveType)
        {
            Point2 point2 = this.entity.BasePosition;
            Vector2 vector2 = this.entity.Position;
            GameTmxMap gameTmxMap = MapManager.mainMap.gameTmxMap;
            bool flag = false;
            switch (moveType)
            {
                case MoveType.DOWN:
                    if (gameTmxMap.TmxMap.Height > point2.Y +1 &&  gameTmxMap.MapRoad[point2.X, point2.Y + 1])
                        flag = true;
                    break;
                case MoveType.UP:
                    vector2 += new Vector2(0,63);
                    point2 = new Point2((int)vector2.X / 64, (int)vector2.Y / 64);
                    if (point2.Y - 1 > 0 && gameTmxMap.MapRoad[point2.X, point2.Y - 1])
                        flag = true;
                    break;
                case MoveType.LEFT:
                    vector2 += new Vector2(63, 0);
                    point2 = new Point2((int)vector2.X / 64, (int)vector2.Y / 64);
                    if (point2.X - 1 > 0 && gameTmxMap.MapRoad[point2.X - 1, point2.Y])
                        flag = true;
                    break;
                case MoveType.RIGHT:
                    if (gameTmxMap.TmxMap.Width > point2.X + 1 && gameTmxMap.MapRoad[point2.X + 1, point2.Y])
                        flag = true;
                    break;
            }
            return flag;
        }
        public virtual void EntityMove(Vector2 movePosition)
        {
            entity.Position += movePosition;
        }
    }
}
