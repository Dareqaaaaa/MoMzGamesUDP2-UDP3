/*
 * Arquivo: LoginManager.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 18/10/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.model;
using Core;
using Core.managers.server;
using Core.server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Auth
{
    public class LoginManager
    {
        public static ServerConfig Config;
        public static Socket mainSocket;
        public static bool ServerIsClosed;
        public static ConcurrentDictionary<uint, LoginClient> _socketList = new ConcurrentDictionary<uint, LoginClient>();
        public static List<LoginClient> _loginQueue = new List<LoginClient>();
        public static bool Start()
        {
            try
            {
                Config = ServerConfigSyncer.GenerateConfig(ConfigGA.configId);
                mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint Local = new IPEndPoint(IPAddress.Parse(ConfigGA.authIp), ConfigGA.authPort);
                mainSocket.Bind(Local);
                mainSocket.Listen(10);
                mainSocket.BeginAccept(new AsyncCallback(AcceptCallback), mainSocket);
                return true;
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
                return false;
            }
        }
        private static void AcceptCallback(IAsyncResult result)
        {
            if (ServerIsClosed)
                return;
            Socket clientSocket = (Socket)result.AsyncState;
            try
            {
                Socket handler = clientSocket.EndAccept(result);
                if (handler != null)
                {
                    LoginClient client = new LoginClient(handler);
                    AddSocket(client);
                    if (client == null)
                        Console.WriteLine("LoginClient destruído após falha ao adicionar na lista.");
                    Thread.Sleep(5);
                }
            }
            catch
            {
                Logger.warning("[Failed a LC connection] " + DateTime.Now.ToString("dd/MM/yy HH:mm"));
            }
            mainSocket.BeginAccept(new AsyncCallback(AcceptCallback), mainSocket);
        }
        public static void AddSocket(LoginClient sck)
        {
            if (sck == null)
                return;
            uint idx = 0;
            while (true)
            {
                if (idx >= 100000)
                    break;
                uint valor = ++idx;
                if (!_socketList.ContainsKey(valor) && _socketList.TryAdd(valor, sck))
                {
                    sck.SessionId = valor;
                    sck.Start();
                    return;
                }
            }
            sck.Close(500, true);
        }
        public static int EnterQueue(LoginClient sck)
        {
            if (sck == null)
                return -1;
            lock (_loginQueue)
            {
                if (_loginQueue.Contains(sck))
                    return -1;
                _loginQueue.Add(sck);
                return _loginQueue.IndexOf(sck);
            }
        }
        public static bool RemoveSocket(LoginClient sck)
        {
            if (sck == null || sck.SessionId == 0)
                return false;
            if (_socketList.ContainsKey(sck.SessionId) && _socketList.TryGetValue(sck.SessionId, out sck))
                return _socketList.TryRemove(sck.SessionId, out sck);
            //Logger.warning("Session #" + session + " disconnected. (" + work + ")");
            return false;
        }
        public static int SendPacketToAllClients(SendPacket packet)
        {
            int count = 0;
            if (_socketList.Count > 0)
            {
                byte[] code = packet.GetCompleteBytes("GameManager.SendPacketToAllClients");
                foreach (LoginClient client in _socketList.Values)
                {
                    Account player = client._player;
                    if (player != null && player._isOnline)
                    {
                        player.SendCompletePacket(code);
                        count++;
                    }
                }
            }
            return count;
        }
        public static Account SearchActiveClient(long accountId)
        {
            if (_socketList.Count == 0)
                return null;
            foreach (LoginClient client in _socketList.Values)
            {
                Account player = client._player;
                if (player != null && player.player_id == accountId)
                    return player;
            }
            return null;
        }
    }
}