using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenBLive.Runtime.Data
{
    public class AppStartInfo
    {
        [JsonProperty("code")]
        public int Code;
        [JsonProperty("message")]
        public string Message;
        [JsonProperty("data")]
        public AppStartInfoData Data;
    }


    public class AppStartInfoData
    {
        /// <summary>
        /// 场次信息
        /// </summary>
        [JsonProperty("game_info")]
        public AppStartGameInfo GameInfo;
        /// <summary>
        /// 长连信息
        /// </summary>
        [JsonProperty("websocket_info")]
        public AppStartWebsocketInfo WebsocketInfo;
    }

    public class AppStartGameInfo
    {
        /// <summary>
        /// 场次id,心跳key(心跳保持20s-60s)调用一次,超过60s无心跳自动关闭,长连停止推送消息
        /// </summary>
        [JsonProperty("game_id")]
        public string GameId;
    }
    public class AppStartWebsocketInfo
    {
        /// <summary>
        /// 长连使用的请求json体 第三方无需关注内容,建立长连时使用即可
        /// </summary>
        [JsonProperty("auth_body")]
        public string AuthBody;
        /// <summary>
        ///  wss 长连地址
        /// </summary>
        [JsonProperty("wss_link")]
        public List<string> WssLink;
    }
}
