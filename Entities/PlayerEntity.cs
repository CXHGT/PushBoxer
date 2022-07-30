using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PushBoxer
{
    public class PlayerEntity : CreatureEntity
    {
        CreatureEntityMoveBehavior moveBehavior;
        private int moveNumber = 0;
        private MoveType moveType = MoveType.UP;
        public PlayerEntity()
        {
            moveBehavior = new CreatureEntityMoveBehavior(this);
            moveBehavior.Load(null);
            this.entityBehaviors.Add(moveBehavior);
            LoadImages(ImageManager.playerEntityImageSources);
        }
        public override void Load(object data)
        {

        }
        public override void Update()
        {
            base.Update();
            if(moveNumber > 0)
            {
                moveBehavior.OnMove(moveType);
                moveNumber--;
                ScreenManager.gameWindow.gameScreen.UpdateCamrea(this);
            }
            else
            {
                MoveType? type = MapManager.GetMoveType();
                if(type != null){
                    this.moveType = (MoveType)type;
                    moveNumber = 64 / (int)moveBehavior.speed;
                }
            }
            
        }
    }
}
