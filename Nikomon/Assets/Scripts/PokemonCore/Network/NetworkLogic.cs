using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using PokemonCore.Combat;
using UnityEditor;

namespace PokemonCore.Network
{
    /// <summary>
    /// 通过NetworkLogic来组织游戏中的网络链接逻辑
    /// </summary>
    public static class NetworkLogic
    {
        public static float TimeToFlush = 5;
        public static Dictionary<IPAddress,NetworkBroadcastData> usersBroadcast=new Dictionary<IPAddress, NetworkBroadcastData>();
        
        public static void PariOnBattle(int trainersNum=2,int pokemonsPerTrainer=1,string password="")
        {
            _password = password;
            _broadcastType = BroadcastType.SearchForBattle;
            _trainersNum = trainersNum;
            _pokemonsPerTrainer=pokemonsPerTrainer;
            _randomNum = Game.Random.Next();
            NetworkLocal.StartToDetect();
            NetworkLocal.OnDetectBroadcast += OnDetectedPair;
            NetworkLocal.StartToBroadCast(new NetworkBroadcastData(BroadcastType.SearchForBattle,_randomNum,password,Game.trainer,trainersNum,pokemonsPerTrainer));
        }

        private static string _password;
        private static BroadcastType _broadcastType;
        private static int _trainersNum;
        private static int _pokemonsPerTrainer;
        private static int _randomNum;

        public static void PairOff()
        {
            NetworkLocal.StopBroadCast();
            NetworkLocal.StopDetect();
        }

        private static List<string> _pairedAddr;
        /// <summary>
        /// 对于初次收到
        /// </summary>
        /// <param name="result"></param>
        /// <param name="password"></param>
        static void OnDetectedPair(UdpReceiveResult result, string password)
        {
            if (result.RemoteEndPoint.Address.Equals(IPAddress.Parse(NetworkLocal.GetAddressIP()))) return;

            NetworkBroadcastData data = JsonConvert.DeserializeObject<NetworkBroadcastData>(password);
            if (data == null) throw new Exception("Unknown data from online");
            if (data.password != _password && data.broadCastType!=_broadcastType) return;
            
            
            if (usersBroadcast.ContainsKey(result.RemoteEndPoint.Address))
                usersBroadcast[result.RemoteEndPoint.Address] = data;
            else
                usersBroadcast.Add(result.RemoteEndPoint.Address,data);

            //TODO: 加入对方取消链接时的处理情况
            if (usersBroadcast.Count == _trainersNum-1)
            {
                UnityEngine.Debug.Log("Have matched, start battle");
                int maxRandom = (from d in usersBroadcast.Values select d.randomNum).Max();
                bool isHost = false;
                if (maxRandom < _randomNum)
                {
                    //这个时候是本机当主机
                    Game.Random = new Random(_randomNum);
                    isHost = true;
                    BecomeHost();
                }
                else
                {
                    //这个时候是maxRandom机当主机
                    Game.Random = new Random(maxRandom);
                    var arr = (from d in usersBroadcast where d.Value.randomNum == maxRandom select d.Key).ToArray();
                    if (arr.Length != 1) throw new Exception("Error occured");
                    isHost = false;
                    BecomeClient(arr[0]);
                }
                NetworkLocal.StopDetect();

                System.Threading.Timer t=new Timer(new TimerCallback((o) =>
                {
                    NetworkLocal.StopBroadCast();
                    UnityEngine.Debug.Log("Stop broadcast");
                }),
                    null,1000,Timeout.Infinite);
                
                StartBattle(isHost);
            }
            
        }


        public static Action<List<Trainer>,List<Trainer>,List<Trainer>,List<Trainer>,bool,int> OnStartBattle;
        static void StartBattle(bool isHost)
        {
            var trainers = (from d in usersBroadcast select d.Value).ToList();
            trainers.Sort((o1, o2) => o1.randomNum - o2.randomNum);

            List<Trainer> allies = new List<Trainer>();
            List<Trainer> oppos = new List<Trainer>();
            

            if (_randomNum < trainers[trainers.Count / 2].randomNum)
            {
                for (int i = 0; i < trainers.Count/2; i++)
                {
                    allies.Add(trainers[i].Trainer);
                }
                for (int i = trainers.Count/2; i < trainers.Count; i++)
                {
                    oppos.Add(trainers[i].Trainer);
                }
            }
            else
            {
                for (int i = 0; i <= trainers.Count/2; i++)
                {
                    oppos.Add(trainers[i].Trainer);
                }
                for (int i = trainers.Count/2 + 1; i < trainers.Count; i++)
                {
                    allies.Add(trainers[i].Trainer);
                }
            }
            OnStartBattle?.Invoke(allies,oppos,null,allies.Union(oppos).ToList(),isHost,_pokemonsPerTrainer);
            // Game.Instance.StartBattle(allies,oppos,null,allies.Union(oppos).ToList(),isHost,_pokemonsPerTrainer);
            if (isHost)
            {
                Battle.Instance.OnUserChooseInstruction += ServerSendInstruction;
                UnityEngine.Debug.Log("Become Host");
            }
            else
            {
                Battle.Instance.OnUserChooseInstruction += ClientSendInstruction;
                UnityEngine.Debug.Log("Become Client");
            }
        }
        
        static void FlushPairs()
        {
            Thread.Sleep((int)(TimeToFlush*1000));
            _pairedAddr.Clear();
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

            NetworkLocal.OnServerReceiveMessage += (data) =>
            {
                string str = Encoding.UTF8.GetString(data);
                NetworkLocal.SendToClients(str);
                UnityEngine.Debug.Log("Server Receive Message");
                UnityEngine.Debug.Log(str);
                Instruction ins = JsonConvert.DeserializeObject<Instruction>(str);
                Battle.Instance.ReceiveInstruction(ins,false);
            };
        }

        static void BecomeClient(IPAddress ipAddress)
        {
            NetworkLocal.StartClient(ipAddress);
            NetworkLocal.OnClientReceiveMessage += (data) =>
            {
                string str = Encoding.UTF8.GetString(data);
                UnityEngine.Debug.Log("Client Receive Message");
                UnityEngine.Debug.Log(str);
                Instruction ins = JsonConvert.DeserializeObject<Instruction>(str);
                Battle.Instance.ReceiveInstruction(ins,false);
            };
        }
        static void ServerSendInstruction(Instruction instruction)
        {
            string str = JsonConvert.SerializeObject(instruction);
            UnityEngine.Debug.Log("Server send Instruction");
            UnityEngine.Debug.Log(str);
            NetworkLocal.SendToClients(str);
        }
        static void ClientSendInstruction(Instruction instruction)
        {
            string str = JsonConvert.SerializeObject(instruction);
            UnityEngine.Debug.Log("Client send Instruction");
            UnityEngine.Debug.Log(str);
            NetworkLocal.SendToServer(str);
        }
        

        
        
        
        
    }
}