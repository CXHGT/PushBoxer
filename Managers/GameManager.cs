using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Windows;

namespace PushBoxer
{
    public static class GameManager
    {
        public static MainWindow mainWindow;
        public static SoundPlayer soundPlayer;
        /// <summary>
        /// 加载的顺序很重要，不然游戏会出现个中国问题
        /// </summary>
        /// <param name="window"></param>
        public static void Load(MainWindow window)
        {
            mainWindow = window;
            //ConfigManager.Load();
            ImageManager.Load();
            ScreenManager.Load();
            MapManager.Load();
            CmdManager.Load();
            UpdateManager.Load();
            soundPlayer = new SoundPlayer(Resource.BackgroundSound);
            soundPlayer.PlayLooping();
            //soundPlayer.Dispose();
        }

        public static void Close()
        {
            MapManager.Save();
            UpdateManager.Close();
            Application.Current.Shutdown();
        }


        /// <summary>
        ///  子线程不能操作主线程的ui，所以用该方法将子线程的函数操作装换成主线程来操作
        /// </summary>
        /// <param name="action"></param>
        public static void Handle(Action action)
        {
            try
            {
                mainWindow.Dispatcher.Invoke(action);
            }
            catch
            {

            }
        }
    }
}
