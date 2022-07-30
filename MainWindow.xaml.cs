using Newtonsoft.Json;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 项目id，自行补充
        static string appId = "";
        private string gameId;

        private WebSocketBLiveClient m_WebSocketBLiveClient;
        private InteractivePlayHeartBeat m_PlayHeartBeat;

        private string upCode;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gameLoad();
        }

        private async Task gameLoad()
        {
            Input input = new Input();
            Hide();
            input.ShowDialog();
            if (input.DialogResult == false)
            {
                Environment.Exit(0);
            }


            // 开发者密钥，自行补充
            SignUtility.accessKeySecret = "";
            SignUtility.accessKeyId = "";

            this.upCode = input.upCodeText;

            Console.WriteLine(input.upCodeText + "     " + appId);

            BApi.isTestEnv = false;

            try
            {

                var ret = await BApi.StartInteractivePlay(upCode, appId);
                var retObject = JsonConvert.DeserializeObject<AppStartInfo>(ret);



                //获取房间信息
                m_WebSocketBLiveClient = new WebSocketBLiveClient(retObject);
                m_WebSocketBLiveClient.OnDanmaku += WebSocketBLiveClientOnDanmaku;
                m_WebSocketBLiveClient.OnGift += WebSocketBLiveClientOnGift;
                m_WebSocketBLiveClient.OnGuardBuy += WebSocketBLiveClientOnGuardBuy;
                m_WebSocketBLiveClient.OnSuperChat += WebSocketBLiveClientOnSuperChat;
                m_WebSocketBLiveClient.Connect();


                gameId = retObject.Data.GameInfo.GameId;


                if (gameId != null)
                {
                    Console.WriteLine("成功开启，开始心跳，游戏ID: " + retObject.Data.GameInfo.GameId);

                    m_PlayHeartBeat = new InteractivePlayHeartBeat(gameId);
                    m_PlayHeartBeat.HeartBeatError += delegate { };
                    m_PlayHeartBeat.HeartBeatSucceed += delegate { };
                    m_PlayHeartBeat.Start();
                }
                else
                {
                    Console.WriteLine("开启游戏错误: " + retObject);
                    throw new Exception("失败");
                }
            }catch (Exception ex)
            {
                MessageBox.Show("连接失败，请稍后再尝试，正常的游戏关闭后一般需要等30s后才可重新连接");
                Application.Current.Shutdown();
            }

            Show();
            GameManager.Load(this);


            //注释掉下面3行关闭最大化显示

            /*
            this.WindowState = System.Windows.WindowState.Normal;     //还原窗口（非最小化和最大化）
            this.WindowStyle = System.Windows.WindowStyle.None;         //仅工作区可见，不显示标题栏和边框
            this.ResizeMode = System.Windows.ResizeMode.NoResize;       //不显示最大化和最小化按钮
            */

            //  this.Topmost = true;    //窗口在最前
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }
        //BNI5E0ZOWQ2O7
        private void Window_Closing(object sender, EventArgs e)
        {

            var ret = BApi.EndInteractivePlay(appId, gameId);

            if (this.m_WebSocketBLiveClient != null)
            {
                this.m_WebSocketBLiveClient.Dispose();
            }
            if (this.m_PlayHeartBeat != null)
            {
                this.m_PlayHeartBeat.Stop();
            }
            GameManager.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private static void WebSocketBLiveClientOnSuperChat(SuperChat superChat)
        {
            StringBuilder sb = new StringBuilder("收到SC!");
            sb.AppendLine();
            sb.Append("来自用户：");
            sb.AppendLine(superChat.userName);
            sb.Append("留言内容：");
            sb.AppendLine(superChat.message);
            sb.Append("金额：");
            sb.Append(superChat.rmb);
            sb.Append("元");
            Logger.Log(sb.ToString());
        }

        private static void WebSocketBLiveClientOnGuardBuy(Guard guard)
        {
            StringBuilder sb = new StringBuilder("收到大航海!");
            sb.AppendLine();
            sb.Append("来自用户：");
            sb.AppendLine(guard.userInfo.userName);
            sb.Append("赠送了");
            sb.Append(guard.guardUnit);
            Logger.Log(sb.ToString());
        }

        private static void WebSocketBLiveClientOnGift(SendGift sendGift)
        {
            StringBuilder sb = new StringBuilder("收到礼物!");
            sb.AppendLine();
            sb.Append("来自用户：");
            sb.AppendLine(sendGift.userName);
            sb.Append("赠送了");
            sb.Append(sendGift.giftNum);
            sb.Append("个");
            sb.Append(sendGift.giftName);
            Logger.Log(sb.ToString());
        }

        private static void WebSocketBLiveClientOnDanmaku(Dm dm)
        {
            StringBuilder sb = new StringBuilder("收到弹幕!");
            sb.AppendLine();
            sb.Append("用户：");
            sb.AppendLine(dm.userName);
            sb.Append("弹幕内容：");
            sb.Append(dm.msg);
            Logger.Log(sb.ToString());

            Application.Current.Dispatcher.Invoke(delegate
            {
                CmdManager.Cmd(dm.userName, dm.msg);
                if (ScreenManager.gameWindow.GameWindowListBox.Items.Count >= 14) ScreenManager.gameWindow.GameWindowListBox.Items.RemoveAt(0);
                ScreenManager.gameWindow.ListBoxAddText(dm.userName + " : " + dm.msg);
            });



        }
    }
}
