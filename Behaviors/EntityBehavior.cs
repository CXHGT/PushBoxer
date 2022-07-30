using System;
using System.Collections.Generic;
using System.Text;

namespace PushBoxer
{
    public class EntityBehavior : IUpdateable, ILoadData
    {
        public Entity entity;
        public EntityBehavior(Entity entity)
        {
            this.entity = entity;
        }
        public virtual void Load(object data)
        {
        }
        public virtual object Save()
        {
            return null;
        }
        public virtual void Update()
        {
        }
    }
}
