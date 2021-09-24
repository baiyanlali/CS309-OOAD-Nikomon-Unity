using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;

namespace PokemonCore.Network
{
    /// <summary>
    /// 通过NetworkLogic来组织游戏中的网络链接逻辑
    /// </summary>
    public static class NetworkLogic
    {
        public static float TimeToFlush = 5;
        public static Dictionary<IPAddress,string> usersBroadcast=new Dictionary<IPAddress, string>();

        public static void StartLocalNetwork(string name="Baron")
        {
            
        }

        public static void CloseLocalNetwork()
        {
            NetworkLocal.StopBroadCast();
            NetworkLocal.StopDetect();
        }

        public static void PariOn(string password="NONE")
        {
            new Thread(PairOnInner).Start(password);
            
        }

        private static bool HavePaired;
        private static string Password;
        private static void PairOnInner(object obj)
        {
            string password = obj as string;
            Password = password;
            NetworkLocal.StartToDetect();
            NetworkLocal.OnDetectBroadcast += OnDetectedPair;
            Thread.Sleep(500);
            if(!HavePaired)
                NetworkLocal.StartToBroadCast(password);
            // NetworkLocal.OnClientReceiveMessage += (str)=>UnityEngine.Debug.Log(Encoding.UTF8.GetString(str));
            // NetworkLocal.OnServerReceiveMessage += (str)=>UnityEngine.Debug.Log(Encoding.UTF8.GetString(str));
        }

        public static void PairOff()
        {
            NetworkLocal.StopBroadCast();
            NetworkLocal.StopDetect();
        }

        private static List<string> pairedAddr;
        /// <summary>
        /// 对于初次收到
        /// </summary>
        /// <param name="result"></param>
        /// <param name="password"></param>
        static void OnDetectedPair(UdpReceiveResult result, string password)
        {
            if (result.RemoteEndPoint.Address.Equals(IPAddress.Parse(NetworkLocal.GetAddressIP()))) return;

            if (password != Password) return;

            HavePaired = true;
            
            BecomeClient(result.RemoteEndPoint);
            
            PairOff();

        }
        
        static void FlushPairs()
        {
            Thread.Sleep((int)(TimeToFlush*1000));
            pairedAddr.Clear();
        }
        
        static void OnDetectedBroadcast(UdpReceiveResult result,string password)
        {
            //判断接收到的信息是不是自己发的
            if (result.RemoteEndPoint.Address.Equals(IPAddress.Parse(NetworkLocal.GetAddressIP()))) return;
            
            
            if (usersBroadcast.ContainsKey(result.RemoteEndPoint.Address))
                usersBroadcast[result.RemoteEndPoint.Address] = password;
            else
                usersBroadcast.Add(result.RemoteEndPoint.Address,password);
            Thread t = new Thread(new ParameterizedThreadStart(FlushUsers));
            t.Start(result.RemoteEndPoint.Address);
        }

        static void FlushUsers(object ipAddress)
        {
            IPAddress address=ipAddress as IPAddress;
            Thread.Sleep((int)(TimeToFlush*1000));
            if(address!=null)
                usersBroadcast.Remove(address);
        }
        /// <summary>
        /// 玩家无法通过游戏界面选择自己是Host还是Client,所有的配对都通过UDP广播来完成，然后由第一个UDP广播的作为Host
        /// </summary>
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