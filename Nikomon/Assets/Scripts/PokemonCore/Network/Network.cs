using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using PokemonCore.Utility;

namespace PokemonCore.Network
{
    /// <summary>
    /// NetworkLocal只提供最基础的链接方法，不提供游戏中具体的连接逻辑
    /// </summary>
    public static class NetworkLocal
    {
        private static UdpClient UDPsend;
        public static Socket ServerSocket;
        public static Socket ClientSocket;
        public static int BroadPort = 42424;
        public static int ServerPort = 42425;
        public static bool CanDetect = true;
        private static List<Socket> Clients;

        public static int TransMaxSize = 1024 * 1024 * 32;

        /// <summary>
        /// Include the host himself
        /// </summary>
        public static int maxPlayers = 6;

        public static Action<byte[]> OnServerReceiveMessage;
        public static Action<byte[]> OnClientReceiveMessage;
        public static Action<UdpReceiveResult,string> OnDetectBroadcast;

        private static int currentPlayerNum = 1;

        private static Thread _BroadcastThread;

        /// <summary>
        /// 用于广播消息
        /// </summary>
        public static void StartToBroadCast(string name)
        {
            if (UDPsend == null)
                UDPsend = new UdpClient(new IPEndPoint(IPAddress.Parse(GetAddressIP()), BroadPort));
            //用于广播
            _BroadcastThread = new Thread(BroadCast);
            _BroadcastThread.IsBackground = true;
            _BroadcastThread.Start(name);
            //用于接受广播信息
        }


        /// <summary>
        /// 广播时用
        /// </summary>
        private static void BroadCast(object name)
        {
            string na = name as string;
            byte[] buf = Encoding.Default.GetBytes($"Trainer: {na}");
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), BroadPort);
            // UnityEngine.Debug.Log($"Broad From IP:{UDPsend.Client}");
            while (true)
            {
                UnityEngine.Debug.Log("I'm sending message");
                UDPsend.Send(buf, buf.Length, endPoint);
                Thread.Sleep(1000);
            }
        }

        
        /// <summary>
        /// 用于探测当前是否有广播
        /// </summary>
        public static async void StartToDetect()
        {
            if (UDPsend == null) UDPsend = new UdpClient(new IPEndPoint(IPAddress.Parse(GetAddressIP()), BroadPort));
            UnityEngine.Debug.Log("Start to receive");
            string strs = "";
            UdpReceiveResult result;
            CanDetect = true;

            while (CanDetect)
            {
                try
                {
                    result = await UDPsend.ReceiveAsync();
                    strs = Encoding.UTF8.GetString(result.Buffer);
                }
                catch (Exception e)
                {
                    if (!(e is SocketException))
                    {
                        return;
                    }
                }

                if (!(string.IsNullOrEmpty(strs)))
                {
                    OnDetectBroadcast(result, strs);
                    // UnityEngine.Debug.Log($"{result.RemoteEndPoint.Address}:{result.RemoteEndPoint.Port}, {strs}");
                }
            }
        }

        public static void StopBroadCast()
        {
            // UDPsend?.Close();
            _BroadcastThread?.Abort();
        }

        public static void StopDetect()
        {
            CanDetect = false;
        }

        #region 当作host时的方法

        /// <summary>
        /// 建立一个Host
        /// </summary>
        public static void BuildHost()
        {
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(new IPEndPoint(IPAddress.Parse(GetAddressIP()), ServerPort));
            ServerSocket.Listen(maxPlayers - 1);
            Clients = new List<Socket>();
            new Thread(ListenClientConnect).Start();
        }

        /// <summary>
        /// 链接client
        /// </summary>
        static void ListenClientConnect()
        {
            while (currentPlayerNum < maxPlayers)
            {
                Socket client = ServerSocket.Accept();
                currentPlayerNum++;
                Clients.Add(client);
                Thread receiveThread = new Thread(ReceiveMessageFromClient);
                receiveThread.Start(client);
            }
        }

        /// <summary>
        /// 听取每个client的信息
        /// </summary>
        /// <param name="client"></param>
        static void ReceiveMessageFromClient(object client)
        {
            Socket sock = client as Socket;
            byte[] result = new byte[TransMaxSize];
            while (true)
            {
                try
                {
                    var len = sock.Receive(result, result.Length, SocketFlags.None);
                    if(len>0)
                        OnServerReceiveMessage?.Invoke(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    sock.Close();
                    sock.Dispose();
                }
            }
        }

        /// <summary>
        /// 目前逻辑上不支持向指定的单个Client发送信息
        /// </summary>
        /// <param name="data"></param>
        public static void SendToClients(string data)
        {
            foreach (var client in Clients.OrEmptyIfNull())
            {
                SendToClient(data, client);
            }
        }

        static void SendToClient(string data, Socket socket)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                socket.Send(bytes, SocketFlags.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region 当作client时用

        public static void StartClient(IPEndPoint ipEndPoint)
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ClientSocket.Connect(ipEndPoint);
            new Thread(ReceiveFromServer).Start();
        }

        public static void SendToServer(string strs)
        {
            if (ClientSocket == null) return;
            byte[] data = Encoding.UTF8.GetBytes(strs);
            ClientSocket.Send(data);
        }

        static void ReceiveFromServer()
        {
            if (ClientSocket == null) return;
            byte[] data = new byte[TransMaxSize];
            int len = ClientSocket.Receive(data, SocketFlags.None);
            if(len>0)
                OnClientReceiveMessage?.Invoke(data);
        }

        #endregion

        /// <summary>
        /// 获取本地IP，通用方法
        /// </summary>
        /// <returns></returns>
        private static string GetAddressIP()
        {
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                //TODO: 找办法识别wsl和ether net
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                    break;
                }
            }

            return AddressIP;
        }
    }
}