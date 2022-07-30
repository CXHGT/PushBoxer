using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TiledSharp;

namespace PushBoxer
{
    public class MapBase : IUpdateable
    {
        public GameTmxMap gameTmxMap = null;
        public List<Entity> entities = new List<Entity>();
        private List<TmxObject> cen1 = new List<TmxObject>();
        private List<TmxObject> cen2 = new List<TmxObject>();
        private List<TmxObject> cen3 = new List<TmxObject>();

        public MapBase(string filePath)
        {
            Stream stream = new FileStream(filePath,FileMode.Open);
            gameTmxMap = new GameTmxMap(stream);
            stream.Dispose();


            foreach (TmxObjectGroup tmxObjectGroup in this.gameTmxMap.TmxMap.ObjectGroups)
            {
                foreach (TmxObject tmxObject in tmxObjectGroup.Objects)
                {
                    switch (tmxObject.Tile.Gid)
                    {
                        case 64:
                            cen1.Add(tmxObject);
                            break;
                        case 65:
                            cen2.Add(tmxObject);
                            break;
                        case 66:
                            cen3.Add(tmxObject);
                            break;
                    }
                }
            }
            GameManager.Handle(CreateEntities);
            //CreateEntities();
        }

        public void ReCreate()
        {
            this.entities.Clear();
            CreateEntities();
        }

        public void CreateEntities()
        {
            AddEntities(cen3);
            AddEntities(cen2);
            AddEntities(cen1);
        }

        private void AddEntities(List<TmxObject> list)
        {
            foreach (TmxObject tmxObject in list)
            {
                Entity entity = tmxObject.Tile.Gid switch
                {
                    64 => new PlayerEntity(),
                    65 => new BoxEntity(),
                    66 => new PointEntity(),
                    _ => throw new Exception("实体加载错误"),
                };
                entity.BasePosition = new Point2((int)(tmxObject.X) / 64, (int)(tmxObject.Y - 64) / 64);
                this.entities.Add(entity);
            }
        }

        public virtual void Update()
        {
            bool flag = true;
            foreach (Entity entity in entities)
            {
                entity.Update();
                if(entity is BoxEntity boxEntity)
                {
                    if (boxEntity.isOnPoint == false)
                    {
                        flag = false;
                    }
                }
            }
            if (flag)
            {
                MapManager.FinishMap();
            }
        }
    }
}
