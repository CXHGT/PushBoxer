using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace PushBoxer
{
    public static class MapManager
    {
        public static Queue<MoveType> queue = new Queue<MoveType>();
        public static MapBase mainMap = null;
        public static List<string> MapFiles = new List<string>();
        public static int MapIndex = 0;
        public static int MoveNumber = 0;
        public const string MapPath = "Resource/Maps";
        private static MapBase nextMapBase;
        public static bool IsOnMove = false;

        private static Stack<List<Point2>> saveEntityPointStack = new Stack<List<Point2>>(100);
        private static Stack<int> saveEntityMoveNumber = new Stack<int>(100);

        private static List<int> bestMoveNumber;

        public static void Load()
        {
            if (!Directory.Exists(MapPath))
            {
                throw new DirectoryNotFoundException("地图文件夹不存在");
            }
            foreach (string file in Directory.GetFiles(MapPath))
            {
                string extenName = Path.GetExtension(file);
                if (extenName == ".tmx")
                {
                    MapFiles.Add(Path.GetFileName(file));
                }
            }

            #region 数据加载，通过data.dat文件读取每一关最低步数记录
            bestMoveNumber = new List<int>(MapFiles.Count);
            int fileDataNum = 0;
            void updateBestMove()
            {
                bestMoveNumber.Clear();
                for(int i = 0; i< MapFiles.Count; i++)
                {
                    bestMoveNumber.Add(9999);
                }
            }
            if (File.Exists("data.dat"))
            {
                Stream stream = new FileStream("data.dat",FileMode.Open);
                BinaryReader binaryReader = new BinaryReader(stream);
                try
                {
                    fileDataNum = binaryReader.ReadInt32();
                    for(int i = 0; i< fileDataNum; i++)
                    {
                        bestMoveNumber.Add(binaryReader.ReadInt32());
                    }
                }
                catch
                {
                    updateBestMove();
                }
                binaryReader.Dispose();
                stream.Dispose();
            }
            else
            {
                updateBestMove();
            }
            for(int i = fileDataNum; i< MapFiles.Count; i++)
            {
                bestMoveNumber.Add(9999);
            }
            #endregion

            ScreenManager.gameWindow.AllGuanKaNumberLabel.Content = MapFiles.Count.ToString();


            MapIndex = 0;
            mainMap = CreateMap(MapIndex);
            LoadMap(mainMap);
            nextMapBase = CreateMap(MapIndex+1);
        }

        /// <summary>
        /// 保存每一关的最低步数记录
        /// </summary>
        public static void Save()
        {
            Stream stream = new FileStream("data.dat", FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            try
            {
                binaryWriter.Write(MapFiles.Count);
                for (int i = 0; i < MapFiles.Count; i++)
                {
                    binaryWriter.Write(bestMoveNumber[i]);
                }
            }
            catch
            {
            }
            binaryWriter.Dispose();
            stream.Dispose();
        }

        public static bool restart = false;
        public static void ReStart()
        {
            mainMap.ReCreate();
            LoadMap(mainMap);
        }

        /// <summary>
        /// 判断是否有卡死的箱子存在，不过有撤回后这个基本用不上了
        /// </summary>
        /// <returns></returns>
        public static bool IsGG()
        {
            foreach(Entity entity in mainMap.entities)
            {
                if(entity is BoxEntity boxEntity && boxEntity.isOnPoint == false)
                {
                    Point2 point = boxEntity.BasePosition;
                    if (mainMap.gameTmxMap.MapRoad[point.X, point.Y + 1] == false || mainMap.gameTmxMap.MapRoad[point.X, point.Y - 1] == false) {
                        if (mainMap.gameTmxMap.MapRoad[point.X + 1, point.Y] == false || mainMap.gameTmxMap.MapRoad[point.X - 1, point.Y] == false)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void SaveEntityPoint()
        {
            if(saveEntityPointStack.Count > 255)
            {
                ScreenManager.gameWindow.ListBoxAddText("撤回指令缓存已经清空处理");
                saveEntityPointStack.Clear();
                saveEntityMoveNumber.Clear();
            }
            List<Point2> point2s = new List<Point2>(mainMap.entities.Count);
            for (int i = 0;i< mainMap.entities.Count; i++)
            {
                point2s.Add(mainMap.entities[i].BasePosition);
            }
            saveEntityPointStack.Push(point2s);
            saveEntityMoveNumber.Push(queue.Count);
            
        }
        public static void LoadEntityPoint()
        {
            if(saveEntityPointStack.Count > 0)
            {
                List<Point2> point2s = saveEntityPointStack.Pop();
                for (int i = 0; i < point2s.Count; i++)
                {
                    mainMap.entities[i].BasePosition = point2s[i];
                }
            }
            if (saveEntityMoveNumber.Count > 0)
            {
                MoveNumber -= saveEntityMoveNumber.Pop();
                ScreenManager.gameWindow.MoveNumber.Content = "行走步数：" + (++MoveNumber);
            }
        }

        public static MoveType? GetMoveType()
        {
            if (queue.Count > 0)
            {
                IsOnMove = true;
                ScreenManager.gameWindow.Label_1.Content = "移动中，剩余："+queue.Count;
                ScreenManager.gameWindow.MoveNumber.Content = "行走步数："+ (++MoveNumber);
                return queue.Dequeue();
            }
            IsOnMove = false;
            ScreenManager.gameWindow.Label_1.Content = "等待指令中...";
            return null;
        }

        /// <summary>
        /// 地图箱子到位后调用该函数记录步数并进入下一关
        /// </summary>
        public static void FinishMap()
        {
            if(bestMoveNumber[MapIndex] > MoveNumber)
            {
                bestMoveNumber[MapIndex] = MoveNumber;
            }
            Next();
        }

        /// <summary>
        /// 下一关
        /// </summary>
        public static void Next()
        {
            mainMap = nextMapBase;
            if (++MapIndex >= MapFiles.Count)
            {
                MapIndex = 0;
            }
            LoadMap(mainMap);
            int nextIndex = MapIndex + 1;
            if(nextIndex == MapFiles.Count)
            {
                nextIndex = 0;
            }
            //采用另一个线程加载下一张地图，防止切换地图时的加载会造成卡顿
            Task.Run(delegate {
                nextMapBase = CreateMap(nextIndex);
            });
        }

        public static MapBase CreateMap(int index)
        {
            if (index < MapFiles.Count)
            {
                MapBase map = new MapBase(MapPath + "/" + MapFiles[index]);
                return map;
            }
            return null;
        }

        public static void LoadMap(MapBase map)
        {
            saveEntityPointStack.Clear();
            saveEntityMoveNumber.Clear();
            MoveNumber = 0;
            ScreenManager.gameWindow.MoveNumber.Content = "行走步数：0";
            queue.Clear();
            ScreenManager.gameWindow.GuanKaNumberLabel.Content = (MapIndex + 1).ToString().PadLeft(3, '0');

            ScreenManager.gameWindow.BestMoveNumber.Content = bestMoveNumber[MapIndex].ToString().PadLeft(4,'0');


            ScreenManager.gameWindow.gameScreen.EntityGrid.Children.Clear();
            ScreenManager.gameWindow.gameScreen.MapImage1.Source = map.gameTmxMap.imageSource[0];
            ScreenManager.gameWindow.gameScreen.MapImage1.Height = map.gameTmxMap.imageSource[0].Height;
            ScreenManager.gameWindow.gameScreen.MapImage1.Width = map.gameTmxMap.imageSource[0].Width;
            foreach (Entity entity in map.entities)
            {
                ScreenManager.gameWindow.gameScreen.EntityGrid.Children.Add(entity);
            }
            mainMap = map;
        }
    }
}
