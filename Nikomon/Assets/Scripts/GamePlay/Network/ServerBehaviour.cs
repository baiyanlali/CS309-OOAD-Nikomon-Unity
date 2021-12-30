using System;
using GamePlay.Utilities;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace PokemonCore.Network
{
    public class ServerBehaviour : MonoSingleton<ServerBehaviour>
    {
        public NetworkDriver m_Driver;
        private NativeList<NetworkConnection> m_Connections;
        
        public Action OnConnectionDropped;

        private bool isActive;
        public Action<string,NetworkConnection> OnMsgReceived;

        public void Init(ushort port,Action<string,NetworkConnection> onMsgReceived,Action onConnectionDropped=null)
        {
            OnConnectionDropped = onConnectionDropped;

            this.OnMsgReceived = onMsgReceived;
            m_Driver = NetworkDriver.Create();
            var endpoint = NetworkEndPoint.AnyIpv4;
            endpoint.Port = port;
            if (m_Driver.Bind(endpoint) != 0)
                UnityEngine.Debug.Log("Failed to bind to port 9000");
            else
                m_Driver.Listen();

            m_Connections = new NativeList<NetworkConnection>(4, Allocator.Persistent);
            isActive = true;
        }

        public void ShutDown()
        {
            if (isActive)
            {
                isActive = false;
                m_Driver.Dispose();
                m_Connections.Dispose();
            }
        }

        public void OnDestroy()
        {
            ShutDown();
        }

        void Update()
        {
            if (!isActive) return;
            m_Driver.ScheduleUpdate().Complete();
            
            CleanUpConnections();

            AcceptNewConnections();

            UpdateMessagePump();
        }

        public void SendToClient(NetworkConnection connection, string msg)
        {
            DataStreamWriter writer;
            m_Driver.BeginSend(connection, out writer);
            writer.WriteFixedString128(msg);
            m_Driver.EndSend(writer);
        }

        public void Broadcast(string msg, NetworkConnection except)
        {
            for (int i = 0; i < m_Connections.Length; i++)
            {
                if (m_Connections[i].IsCreated && m_Connections[i] != except)
                {
                    UnityEngine.Debug.Log($"Sending to client: {msg}, to {m_Connections[i].InternalId}");
                    SendToClient(m_Connections[i],msg);
                }
            }
        }
        
        public void Broadcast(string msg)
        {
            for (int i = 0; i < m_Connections.Length; i++)
            {
                if (m_Connections[i].IsCreated)
                {
                    UnityEngine.Debug.Log($"Sending to client: {msg}, to {m_Connections[i].InternalId}");
                    SendToClient(m_Connections[i],msg);
                }
            }
        }

        private void UpdateMessagePump()
        {
            DataStreamReader stream;
            for (int i = 0; i < m_Connections.Length; i++)
            {
                Assert.IsTrue(m_Connections[i].IsCreated);

                NetworkEvent.Type cmd;
                while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
                {
                    if (cmd == NetworkEvent.Type.Data)
                    {
                        
                        var value = stream.ReadFixedString128();
                        UnityEngine.Debug.Log($"Got the value = {value.Value} back from client\n{m_Driver.RemoteEndPoint(m_Connections[i]).Address}" );
                        
                        OnMsgReceived?.Invoke(value.Value,m_Connections[i]);
                        // uint number = stream.ReadUInt();
                        //
                        // UnityEngine.Debug.Log("Got " + number + " from the Client adding + 2 to it.");
                        // number += 2;

                        // var writer = m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[i]);
                        // writer.WriteUInt(number);
                        // m_Driver.EndSend(writer);
                    }
                    else if (cmd == NetworkEvent.Type.Disconnect)
                    {
                        UnityEngine.Debug.Log("Client disconnected from server");
                        m_Connections[i] = default(NetworkConnection);
                        OnConnectionDropped?.Invoke();
                    }
                }
            }
        }

        void CleanUpConnections()
        {
            // CleanUpConnections
            for (int i = 0; i < m_Connections.Length; i++)
            {
                if (!m_Connections[i].IsCreated)
                {
                    m_Connections.RemoveAtSwapBack(i);
                    --i;
                }
            }
        }

        private void AcceptNewConnections()
        {
            // AcceptNewConnections
            NetworkConnection c;
            while ((c = m_Driver.Accept()) != default(NetworkConnection))
            {
                m_Connections.Add(c);
                UnityEngine.Debug.Log("Accepted a connection");
            }
        }
    }
}