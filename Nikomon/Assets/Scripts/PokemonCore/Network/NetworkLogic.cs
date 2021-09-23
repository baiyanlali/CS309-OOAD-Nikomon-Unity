using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PokemonCore.Network
{
    /// <summary>
    /// 通过NetworkLogic来组织游戏中的网络链接逻辑
    /// </summary>
    public static class NetworkLogic
    {
        public static float TimeToFlush = 2;
        public static Dictionary<IPAddress,string> usersBroadcast=new Dictionary<IPAddress, string>();

        public static void StartLocalNetwork()
        {
            NetworkLocal.StartToBroadCast("Baron");
            NetworkLocal.OnDetectBroadcast += OnDetectedBroadcast;
            NetworkLocal.StartToDetect();
            NetworkLocal.OnClientReceiveMessage += (str)=>UnityEngine.Debug.Log(Encoding.UTF8.GetString(str));
            NetworkLocal.OnServerReceiveMessage += (str)=>UnityEngine.Debug.Log(Encoding.UTF8.GetString(str));
        }

        public static void CloseLocalNetwork()
        {
            NetworkLocal.StopBroadCast();
            NetworkLocal.StopDetect();
        }
        
        static void OnDetectedBroadcast(UdpReceiveResult result,string name)
        {
            if (usersBroadcast.ContainsKey(result.RemoteEndPoint.Address))
                usersBroadcast[result.RemoteEndPoint.Address] = name;
            else
                usersBroadcast.Add(result.RemoteEndPoint.Address,name);
            Thread t = new Thread(new ParameterizedThreadStart(FlushUsers));
            t.Start(result.RemoteEndPoint.Address);
        }

        static void FlushUsers(object ipAddress)
        {
            IPAddress address=ipAddress as IPAddress;
            Thread.Sleep((int)TimeToFlush*1000);
            if(address!=null)
                usersBroadcast.Remove(address);
        }

        static void BecomeHost()
        {
            NetworkLocal.BuildHost();
        }

        static void BecomeClient(IPEndPoint ipEndPoint)
        {
            NetworkLocal.StartClient(ipEndPoint);
        }
        
        
        
    }
}