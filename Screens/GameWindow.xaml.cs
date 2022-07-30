using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PushBoxer
{
    /// <summary>
    /// GameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GameWindow : UserControl, IUpdateable, ILoadData
    {
        public GameWindow()
        {
            InitializeComponent();
        }

        public void Update()
        {
        }
        public void Load(object data = null)
        {
        }

        public void ListBoxAddText(string text)
        {
            if (GameWindowListBox.Items.Count >= 14) GameWindowListBox.Items.RemoveAt(0);
            GameWindowListBox.Items.Add(text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CmdManager.Cmd("LocalhostCmd",textBox.Text);
            textBox.Text = "";
        }

        private void GameWindowListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MapManager.Next();
        }
    }
}
