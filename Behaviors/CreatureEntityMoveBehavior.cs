using System;
using System.Collections.Generic;
using System.Text;

namespace PushBoxer
{
    public class CreatureEntityMoveBehavior : MoveBehavior, IUpdateable, ILoadData
    {
        public int face = 0;
        public int anmFrame = 0;
        public long lastWalkTime = 0;
        public long walkInterval = 100;
        public bool stop = true;
        public CreatureEntity creatureEntity;
        public CreatureEntityMoveBehavior(Entity entity) : base(entity)
        {
            this.creatureEntity = entity as CreatureEntity;
        }
        public override void Update()
        {
            base.Update();
            if (this.stop && this.anmFrame != 0)
            {
                creatureEntity.Source = creatureEntity.imageSources[this.face * 4 + 0];
            }
            this.stop = true;
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
            Point2 point2 = this.entity.BasePosition;
            Vector2 vector2 = this.entity.Position;
            MapBase mapBase = MapManager.mainMap;
            this.stop = false;
            switch (moveType)
            {
                case MoveType.DOWN:
                    this.face = 0;
                    point2 += new Point2(0,1);
                    break;
                case MoveType.UP:
                    vector2 += new Vector2(0, 63);
                    point2 = new Point2((int)vector2.X / 64, (int)(vector2.Y / 64) - 1);
                    this.face = 3;
                    break;
                case MoveType.LEFT:
                    vector2 += new Vector2(63, 0);
                    point2 = new Point2((int)(vector2.X / 64) - 1, (int)vector2.Y / 64);
                    this.face = 1;
                    break;
                case MoveType.RIGHT:
                    point2 += new Point2(1, 0);
                    this.face = 2;
                    break;
            }
            foreach (Entity entity in mapBase.entities)
            {
                Point2 entityPoint = entity.BasePosition;
                Vector2 entityVecotr2 = entity.Position;
                switch (moveType)
                {
                    case MoveType.UP:
                        entityVecotr2 += new Vector2(0, 63);
                        entityPoint = new Point2((int)entityVecotr2.X / 64, (int)(entityVecotr2.Y / 64));
                        break;
                    case MoveType.LEFT:
                        entityVecotr2 += new Vector2(63, 0);
                        entityPoint = new Point2((int)(entityVecotr2.X / 64), (int)entityVecotr2.Y / 64);
                        break;
                }
                if (entityPoint == point2)
                {
                    if(entity is BoxEntity boxEntity && entity.canTrought == false)
                    {
                        MoveBehavior move = boxEntity.FindBehavior<MoveBehavior>();
                        if (move.OnMove(moveType) == false) return false;
                    }
                }
            }
            return base.OnMove(moveType);
        }
        public override void EntityMove(Vector2 movePosition)
        {
            base.EntityMove(movePosition);
            if (TimeManager.Time() - lastWalkTime > this.walkInterval)
            {
                this.lastWalkTime = TimeManager.Time();
                this.anmFrame = (this.anmFrame + 1) % 4;
                creatureEntity.Source = creatureEntity.imageSources[this.face * 4 + this.anmFrame];
            }
        }
    }
}
