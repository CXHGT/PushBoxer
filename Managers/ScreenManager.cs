using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PushBoxer
{
    /// <summary>
    /// 整个游戏就一个Screen，这个文件可以不用管
    /// </summary>
    public static class ScreenManager
    {
        public static Grid mainScreen;
        public static GameWindow gameWindow;
        public static Dictionary<string, UIElement> ScreenDictionary = new Dictionary<string, UIElement>();

        public static void Load()
        {
            mainScreen = GameManager.mainWindow.MainScreen;
            gameWindow = new GameWindow();
            ScreenDictionary.Add("Game",gameWindow);
            Screen("Game",null) ;
        }

        private static void Screen(string screenName, object data = null)
        {
            UIElement uIElement = FindUIElement(screenName);
            if (uIElement is ContentControl contentControl)
            {
                mainScreen.Children.Add(contentControl);
                (contentControl as ILoadData).Load(data);
            }
            else
            {
                throw new Exception($"Not found the \"{screenName}\" Screen");
            }
        }

        public static UIElement FindUIElement(string name)
        {
            if (ScreenDictionary.TryGetValue(name, out UIElement uIElement))
            {
                return uIElement;
            }
            return null;
        }


    }
}