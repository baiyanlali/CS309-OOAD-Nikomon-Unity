using System;
using GamePlay.Utilities;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

namespace PokemonCore.Network
{
    public class ClientBehaviour : MonoSingleton<ClientBehaviour>
    {
        public NetworkDriver m_Driver;
        private NetworkConnection m_Connection;

        public Action OnConnectionDropped;
        

        private bool isActive;

        public void Init(string ip, ushort port, Action onConnectionDropped = null)
        {
            OnConnectionDropped = onConnectionDropped;
            m_Driver = NetworkDriver.Create();
            NetworkEndPoint endpoint = NetworkEndPoint.Parse(ip, port);

            m_Connection = m_Driver.Connect(endpoint);

            UnityEngine.Debug.Log("Try to connect to server on "+ endpoint.Address);
            isActive = true;
        }
        
        public void Shutdown(){
            if (isActive)
            {
                m_Driver.Dispose();
                m_Connection = default(NetworkConnection);
                isActive = false;
            }
            
        }
        
        
        void Update()
        {
            if (!isActive) return;
            
            m_Driver.ScheduleUpdate().Complete();

            if (!CheckAlive()) return;

            DataStreamReader stream;
            NetworkEvent.Type cmd;

            MessageUpdatePump();
        }

        private void MessageUpdatePump()
        {
            NetworkEvent.Type cmd;
            DataStreamReader stream;
            while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Connect)
                {
                    UnityEngine.Debug.Log("We are now connected to the server");
                    // uint value = 1;
                    // var writer = m_Driver.BeginSend(m_Connection);
                    // writer.WriteUInt(value);
                    // m_Driver.EndSend(writer);
                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    var value = stream.ReadFixedString128();
                    UnityEngine.Debug.Log("Got the value = " + value + " back from the server");
                    // m_Connection.Disconnect(m_Driver);
                    // m_Connection = default(NetworkConnection);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    UnityEngine.Debug.Log("Client got disconnected from server");
                    m_Connection = default(NetworkConnection);
                    Shutdown();
                }
            }
        }

        private bool CheckAlive()
        {
            if (!m_Connection.IsCreated)
            {
                UnityEngine.Debug.Log("Something went wrong during connect");
                OnConnectionDropped?.Invoke();
                Shutdown();
                return false;
            }

            return true;
        }
    }
    
    
}