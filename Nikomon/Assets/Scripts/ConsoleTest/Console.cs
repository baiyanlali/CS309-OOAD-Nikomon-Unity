using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ConsoleDebug
{
    public static class Console
    {
        private static bool is_OpenDebug = true;
        private static string m_UDPServerURL = "127.0.0.1";
        private static string m_UPDServerPort = "10086";

        private static UdpClient m_UDPClient = null;
        private static IPEndPoint m_EndPoint = null;
        private static Process m_ChildProcess;
        private static Application.LogCallback m_LogCallBack;
        private static UnityEngine.LogType m_LastLogType;

        public static Action<string> OnMessageEntered;

        #region OutInteface

        public static void Init()
        {
            if (!is_OpenDebug) return;
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor)
            {
                RunPythonConsole();
            }

            m_LogCallBack = (condition, stackTrace, type) =>
            {
                if (m_LastLogType != type)
                {
                    m_LastLogType = type;
                    SendUDP("#" + (int)type);
                }
                // Debug.Log("Condition: "+condition);
                SendUDP(condition);
                if (type != UnityEngine.LogType.Log)
                {
                    SendUDP(stackTrace);
                }
            };

            Application.logMessageReceived += m_LogCallBack;

        }

        public static void OnDestroy()
        {
            if (!(m_UDPClient is null))
            {
                m_UDPClient.Close();
                m_UDPClient.Dispose();
                m_UDPClient = null;
            }

            if (!(m_ChildProcess is null))
            {
                try
                {
                    m_ChildProcess.Kill();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                    throw;
                }

                m_ChildProcess = null;
            }

            if (is_OpenDebug)
                Application.logMessageReceived -= m_LogCallBack;
        }

        #endregion

        #region In

        static void RunPythonConsole()
        {
            string cmd = $"{Environment.CurrentDirectory}\\Assets\\Scripts\\ConsoleTest\\DebugTool.py";
            m_ChildProcess = new System.Diagnostics.Process();
            m_ChildProcess.StartInfo.FileName = cmd;
            m_ChildProcess.StartInfo.Arguments = m_UPDServerPort;
            m_ChildProcess.Start();
            Debug.Log(cmd);
        }

        static void SendUDP(string sendString)
        {
            if (m_UDPClient is null)
            {
                IPAddress remoteIP = IPAddress.Parse(m_UDPServerURL);
                m_EndPoint = new IPEndPoint(remoteIP, Convert.ToInt32(m_UPDServerPort));
                m_UDPClient = new UdpClient();
                AsyncReceive();
            }
            // Debug.Log("sendString:"+sendString);
            byte[] sendData = Encoding.UTF8.GetBytes(sendString);
            m_UDPClient.Send(sendData, sendData.Length, m_EndPoint);
        }

        static async void AsyncReceive()
        {
            string strs = "";
            try
            {
                UdpReceiveResult result = await m_UDPClient.ReceiveAsync();
                strs = Encoding.UTF8.GetString(result.Buffer);
            }
            catch (Exception e)
            {
                if (!(e is SocketException))
                {
                    return;
                }
            }

            AsyncReceive();
            if (!(string.IsNullOrEmpty(strs)))
            {
                ParseReceive(strs);
            }
        }

        static void ParseReceive(string strs)
        {
            OnMessageEntered(strs);
            // UnityEngine.Debug.Log(strs);
        }

        #endregion
    }
}
        
    