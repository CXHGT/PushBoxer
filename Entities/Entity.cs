using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PushBoxer
{
    public class Entity : GameImage, IUpdateable, ILoadData
    {
        public bool canTrought = false;

        public List<EntityBehavior> entityBehaviors = new List<EntityBehavior>();

        public Vector2 Position
        {
            get
            {
                return new Vector2(this.Margin.Left, this.Margin.Top);
            }
            set
            {
                this.Margin = new System.Windows.Thickness(value.X, value.Y, 0, 0);
            }
        }

        public Point2 BasePosition
        {
            get
            {
                return new Point2((int)((this.Margin.Left + 0) / Width), (int)((this.Margin.Top + 0) / Height));
            }
            set
            {
                this.Margin = new System.Windows.Thickness(value.X * Width, value.Y * Height, 0, 0);
            }
        }
        public Entity()
        {
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
        }
        public virtual void Load(object data)
        {
        }
        public virtual object Save()
        {
            return null;
        }
        public override void Update()
        {
            foreach (EntityBehavior entityBehavior in this.entityBehaviors)
            {
                entityBehavior.Update();
            }
        }
        public T FindBehavior<T>() where T : class
        {
            return this.FindBehavior(typeof(T)) as T;
        }
        public EntityBehavior FindBehavior(Type type)
        {
            foreach (EntityBehavior entityBehavior in this.entityBehaviors)
            {
                if (IntrospectionExtensions.GetTypeInfo(type).IsAssignableFrom(IntrospectionExtensions.GetTypeInfo(entityBehavior.GetType())))
                {
                    return entityBehavior;
                }
            }
            return null;
        }
    }
}
