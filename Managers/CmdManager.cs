using System;
using System.Collections.Generic;
using System.Text;
using System.Media;

namespace PushBoxer
{
    public static class CmdManager
    {
        private static List<string> ggUsers = new List<string>(5);
        private static List<string> nextUsers = new List<string>(5);
        private static List<string> limitUsers = new List<string>(10);
        private static Dictionary<string, string> jbUser = new Dictionary<string, string>(25);
        private static int MapIndex = 0;
        private static long ggLastWaitTime = 0;
        private static long nextLastWaitTime = 0;
        private static long jbLastWaitTime = 0;
        private static long limitLastWaitTime = 0;

        /// <summary>
        /// 占用者的名称
        /// </summary>
        private static string occupiedName = "";
        /// <summary>
        /// 剩余占用时间
        /// </summary>
        private static int occupiedTime = 0;
        /// <summary>
        /// 用于计算时间差
        /// </summary>
        private static long occupiedLastTime = 0;

        public static void Load()
        {
            jbLastWaitTime = TimeManager.Time();
        }

        public static bool IsMoveCmd(string cmd)
        {
            foreach (char c in cmd)
            {
                if (c != 'w' && c != 's' && c != 'a' && c != 'd')
                {
                    return false;
                }
            }
            return true;
        }
        public static void Cmd(string username,string text)
        {
            if (IsMoveCmd(text))
            {
                if (limitUsers.Contains(username))
                {
                    GameManager.Handle(delegate { ScreenManager.gameWindow.ListBoxAddText(username + "的指令执行失败"); });
                    return;
                }
                if(occupiedTime > 0 && occupiedName != username)
                {
                    GameManager.Handle(delegate { ScreenManager.gameWindow.ListBoxAddText(occupiedName + "占用中"); });
                    return;
                }
                if(occupiedTime <= 0)
                {
                    occupiedTime = 16;
                    occupiedName = username;
                }
                if (MapManager.queue.Count == 0)
                {
                    foreach (char c in text)
                    {
                        switch (c)
                        {
                            case 'w':
                                MapManager.queue.Enqueue(MoveType.UP);
                                break;
                            case 'a':
                                MapManager.queue.Enqueue(MoveType.LEFT);
                                break;
                            case 'd':
                                MapManager.queue.Enqueue(MoveType.RIGHT);
                                break;
                            case 's':
                                MapManager.queue.Enqueue(MoveType.DOWN);
                                break;
                        }
                    }

                    GameManager.Handle(delegate
                    {
                        MapManager.SaveEntityPoint();
                    });

                }
            }
            if (limitUsers.Contains(username))
            {
                return;
            }
            #region ch
            if (text == "ch")
            {
                if (occupiedTime > 0 && occupiedName != username)
                {
                    GameManager.Handle(delegate { ScreenManager.gameWindow.ListBoxAddText(occupiedName + "占用中"); });
                    return;
                }
                if (occupiedTime <= 0)
                {
                    occupiedTime = 12;
                    occupiedName = username;
                }
                if (MapManager.IsOnMove)
                {
                    GameManager.Handle(delegate
                    {
                        ScreenManager.gameWindow.ListBoxAddText("等待移动指令执行完毕才能撤回");
                    });
                }
                else
                {
                    GameManager.Handle(delegate
                    {
                        MapManager.LoadEntityPoint();
                    });
                }
            }
            #endregion

            if (text == "music")
            {
                GameManager.soundPlayer.Dispose();
                GameManager.soundPlayer = new SoundPlayer(Resource.BackgroundSound);
                GameManager.soundPlayer.PlayLooping();
            }

            #region 重置当前关卡
            if (text == "gg")
            {
                if (ggUsers.Count == 0)
                {
                    ggLastWaitTime = TimeManager.Time();
                }
                //设置某个玩家的指令无需投票就可以重开
/*                if(username == "CXHGT")
                {
                    ggUsers.Clear();
                    nextUsers.Clear();
                    GameManager.Handle(delegate
                    {
                        ScreenManager.gameWindow.ListBoxAddText(username + "重开了本关");
                        MapManager.ReStart();
                    });
                    return;
                }*/

                if (ggUsers.Contains(username) == false)
                {
                    if (ggUsers.Count >= 3)
                    {
                        ggUsers.Clear();
                        nextUsers.Clear();
                        GameManager.Handle(MapManager.ReStart);
                    }
                    else
                    {
                        GameManager.Handle(delegate
                        {
                            ScreenManager.gameWindow.ListBoxAddText(username + "发起了重开本关");
                            ScreenManager.gameWindow.ListBoxAddText((ggUsers.Count + 1) + "/4 , 40秒内集其即重开");
                        });
                        ggUsers.Add(username);
                    }
                }
                /*                GameManager.Handle(delegate
                                {
                                    if (MapManager.IsGG())
                                    {
                                        ggUsers.Clear();
                                        nextUsers.Clear();
                                        GameManager.Handle(MapManager.ReStart);
                                        ScreenManager.gameWindow.ListBoxAddText("系统判定关卡凉凉，直接重开");
                                    }
                                });*/
            }
            #endregion

            #region 跳关
            if (text == "next")
            {
                if (nextUsers.Count == 0)
                {
                    nextLastWaitTime = TimeManager.Time();
                }
                //设置某个玩家的指令无需投票就可以跳关
                /*                if (username == "CXHGT")
                                {
                                    ggUsers.Clear();
                                    nextUsers.Clear();
                                    GameManager.Handle(delegate
                                    {
                                        ScreenManager.gameWindow.ListBoxAddText(username + "跳过本关");
                                        MapManager.Next();
                                    });
                                    return;
                                }*/
                if (nextUsers.Contains(username) == false)
                {
                    if (nextUsers.Count >= 4)
                    {
                        ggUsers.Clear();
                        nextUsers.Clear();
                        GameManager.Handle(MapManager.Next);
                    }
                    else
                    {
                        GameManager.Handle(delegate
                        {
                            ScreenManager.gameWindow.ListBoxAddText(username + "发起了跳过本关");
                            ScreenManager.gameWindow.ListBoxAddText((nextUsers.Count + 1) + "/5 , 40秒内集其即跳关");
                        });
                        nextUsers.Add(username);
                    }
                }
            }
            #endregion


            #region 举报，有点小问题，但是懒得去解决了
/*            if (text.Length > 3 && text[0] == 'j' && text[1] == 'b')
            {
                string[] strs = text.Split(' ');
                if (strs.Length == 2)
                {
                    string beJbUser = strs[1];
                    int i = 0;
                    foreach (string keyValue in jbUser.Values)
                    {
                        if (keyValue == beJbUser)
                        {
                            i++;
                        }
                    }
                    if (i >= 2)
                    {
                        if (limitUsers.Contains(beJbUser) == false)
                        {
                            limitUsers.Add(beJbUser);
                            GameManager.Handle(delegate { ScreenManager.gameWindow.ListBoxAddText(beJbUser + "被多人举报，现被限制"); });
                        }
                    }
                    if (jbUser.ContainsKey(username) == false)
                    {
                        jbUser.Add(username, beJbUser);
                        jbLastWaitTime = TimeManager.Time();
                        GameManager.Handle(delegate { ScreenManager.gameWindow.ListBoxAddText(username + "举报了" + beJbUser + " " + (i + 1) + "/3"); });
                    }
                }
            }*/
            #endregion
        }

        public static void Update()
        {
            if(MapIndex != MapManager.MapIndex)
            {
                MapIndex = MapManager.MapIndex;
                nextUsers.Clear();
                ggUsers.Clear();
                occupiedTime = 1;
            }
            //1000ms=1s，在刷新时计算时间差，并更新剩余占用时间
            if(occupiedTime > 0 && TimeManager.Time() - occupiedLastTime > 1000)
            {
                --occupiedTime;
                GameManager.Handle(delegate {
                    ScreenManager.gameWindow.Label_2.Content = occupiedName + "占用中，剩余时间:" + occupiedTime;
                });
            }
            if(ggUsers.Count > 0 && TimeManager.Time() - ggLastWaitTime > 41000)
            {
                GameManager.Handle(delegate { ScreenManager.gameWindow.ListBoxAddText("重开时间到，未达到4个，重开取消"); });
                ggUsers.Clear();
                return;
            }
            if (nextUsers.Count > 0 && TimeManager.Time() - nextLastWaitTime > 41000)
            {
                GameManager.Handle(delegate { ScreenManager.gameWindow.ListBoxAddText("跳关时间到，未达到5个，跳关取消"); });
                nextUsers.Clear();
                return;
            }
            if (jbUser.Count > 0 && TimeManager.Time() - jbLastWaitTime > 35000)
            {
                jbUser.Clear();
            }
            if (TimeManager.Time() - limitLastWaitTime > 120000)
            {
                limitLastWaitTime = TimeManager.Time();
                limitUsers.Clear();
            }
        }
    }
}
