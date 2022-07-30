using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PushBoxer
{
    public static class UpdateManager
    {
        public static Task task;
        private static bool run = true;

        private static System.Timers.Timer timer;

        private static List<string> messages = new List<string>(18);
        private static Stack<UserMessage> stack = new Stack<UserMessage>();
        private static Stack<UserMessage> strStack = new Stack<UserMessage>();
        private static Stack<UserMessage> strStack2 = new Stack<UserMessage>();
        public static void Load()
        {
            //使用计时器控制刷新
            timer = new System.Timers.Timer();
            timer.Interval = 30;    //每30毫秒刷新一次，大概一秒钟刷新33次
            timer.Enabled = true;
            timer.Elapsed += delegate
            {
                GameManager.Handle(Update);
            };
            UpdateManager.task = new Task(delegate ()
            {
                for (; ; )
                {
                    try
                    {
                        if (!UpdateManager.run)
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                        CmdManager.Update();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            });
            UpdateManager.task.Start();
            UpdateManager.run = true;
        }
        public static void Update()
        {
            if (MapManager.mainMap != null) MapManager.mainMap.Update();
        }
        public static void Close()
        {
            run = false;
            timer.Dispose();
        }

/*        public static void UpdateListBox()
        {
            JObject jt = JsonConvert.DeserializeObject<JObject>(Post("https://api.live.bilibili.com/xlive/web-room/v1/dM/gethistory", "Content-Disposition: form-data;roomid=" + ConfigManager.RoomID));
            foreach (JObject jt2 in jt["data"]["room"])
            {
                stack.Push(new UserMessage(jt2["timeline"].ToString().Split(' ')[1], jt2["nickname"].ToString(), jt2["text"].ToString()));
            }
            while (stack.Count > 0)
            {
                UserMessage message = stack.Pop();
                bool flag = false;
                for (int i = messages.Count - 1; i >= 0; i--)
                {
                    if (messages[i] == message.getString())
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                    break;
                else
                {
                    strStack.Push(message);
                    strStack2.Push(message);
                }
            }

            while (strStack.Count > 0)
            {
                if (messages.Count >= 16) messages.RemoveAt(0);
                UserMessage uMessage = strStack.Pop();
                messages.Add(uMessage.getString());
                CmdManager.Cmd(uMessage.username, uMessage.message);
            }
            GameManager.Handle(delegate
            {
                while (strStack2.Count > 0)
                {
                    if (ScreenManager.gameWindow.GameWindowListBox.Items.Count >= 14) ScreenManager.gameWindow.GameWindowListBox.Items.RemoveAt(0);
                    ScreenManager.gameWindow.ListBoxAddText(strStack2.Pop().getString());
                }
            });
        }

        public static string Post(string url, string content)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add("Host", "api.live.bilibili.com");
            req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:78.0) Gecko/20100101 Firefox/78.0");
            byte[] data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }*/
    }
}
