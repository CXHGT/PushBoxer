using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PushBoxer
{
    public class ConfigManager
    {

        public static string RoomID = "2413771"; 
        /// <summary>
        /// 读取roomid.txt文件内第一行的数据，若roomid.txt不存在的话默认用2413771。
        /// </summary>
        public static void Load()
        {
            if (File.Exists("roomid.txt"))
            {
                Stream stream = new FileStream("roomid.txt", FileMode.Open);
                StreamReader streamReader = new StreamReader(stream);
                try
                {
                    RoomID = streamReader.ReadLine();
                }
                catch
                {
                }
                streamReader.Dispose();
                stream.Dispose();
            }
            else
            {
                Stream stream = new FileStream("roomid.txt", FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(stream);
                try
                {
                    streamWriter.WriteLine(RoomID);
                }
                catch
                {
                }
                streamWriter.Dispose();
                stream.Dispose();
            }
        }
    }
}
