/*
 * Arquivo: LoginClient.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 05/08/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.model;
using Auth.data.sync;
using Auth.data.sync.server_side;
using Auth.global;
using Auth.global.clientpacket;
using Auth.global.serverpacket;
using Core;
using Core.server;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Auth
{
    public class LoginClient : IDisposable
    {
        public Socket _client;
        public Account _player;
        public DateTime ConnectDate;
        public uint SessionId;
        public int Shift, firstPacketId;
        private byte[] lastCompleteBuffer;
        bool disposed = false, closed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            _player = null;
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
            if (disposing)
                handle.Dispose();
            disposed = true;
        }
        public LoginClient(Socket client)
        {
            _client = client;
            _client.NoDelay = true;
        }
        public void Start()
        {            
            Shift = (int)(SessionId % 7 + 1);
            new Thread(init).Start();
            new Thread(read).Start();
            new Thread(ConnectionCheck).Start();
            ConnectDate = DateTime.Now;
        }
        private void ConnectionCheck()
        {
            Thread.Sleep(10000);
            if (_client != null && firstPacketId == 0)
            {
                Close(0, true);
                Logger.warning("Connection destroyed due to no responses.");
            }
        }
        public string GetIPAddress()
        {
            if (_client != null && _client.RemoteEndPoint != null)
                return ((IPEndPoint)_client.RemoteEndPoint).Address.ToString();
            return "";
        }
        public IPAddress GetAddress()
        {
            if (_client != null && _client.RemoteEndPoint != null)
                return ((IPEndPoint)_client.RemoteEndPoint).Address;
            return null;
        }
        private void init()
        {
            SendPacket(new BASE_SERVER_LIST_PAK(this));
        }
        public void SendCompletePacket(byte[] data)
        {
            try
            {
                if (data.Length < 4)
                    return;
                if (ConfigGA.debugMode)
                {
                    ushort opcode = BitConverter.ToUInt16(data, 2);
                    string debugData = "";
                    foreach (string str2 in BitConverter.ToString(data).Split('-', ',', '.', ':', '\t'))
                        debugData += " " + str2;
                    Logger.warning("[" + opcode + "]" + debugData);
                }
                _client.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), _client);
            }
            catch
            {               
                Close(0, true);
            }
        }
        public void SendPacket(byte[] data)
        {
            try
            {
                if (data.Length < 2)
                    return;
                ushort size = Convert.ToUInt16(data.Length - 2);
                List<byte> list = new List<byte>(data.Length + 2);
                list.AddRange(BitConverter.GetBytes(size));
                list.AddRange(data);
                byte[] result = list.ToArray();
                if (ConfigGA.debugMode)
                {
                    ushort opcode = BitConverter.ToUInt16(data, 0);
                    string debugData = "";
                    foreach (string str2 in BitConverter.ToString(result).Split('-', ',', '.', ':', '\t'))
                        debugData += " " + str2;
                    Logger.warning("[" + opcode + "]" + debugData);
                }
                if (result.Length > 0)
                    _client.BeginSend(result, 0, result.Length, SocketFlags.None, new AsyncCallback(SendCallback), _client);
                list.Clear();
            }
            catch
            {
                Close(0, true);
            }
        }
        public void SendPacket(SendPacket bp)
        {
            try
            {
                using (bp)
                {
                    bp.write();
                    byte[] data = bp.mstream.ToArray();
                    if (data.Length < 2)
                        return;
                    ushort size = Convert.ToUInt16(data.Length - 2);
                    List<byte> list = new List<byte>(data.Length + 2);
                    list.AddRange(BitConverter.GetBytes(size));
                    list.AddRange(data);
                    byte[] result = list.ToArray();
                    if (ConfigGA.debugMode)
                    {
                        ushort opcode = BitConverter.ToUInt16(data, 0);
                        string debugData = "";
                        foreach (string str2 in BitConverter.ToString(result).Split('-', ',', '.', ':', '\t'))
                            debugData += " " + str2;
                        Logger.warning("[" + opcode + "]" + debugData);
                    }
                    if (result.Length > 0)
                        _client.BeginSend(result, 0, result.Length, SocketFlags.None, new AsyncCallback(SendCallback), _client);
                    bp.mstream.Close();
                    list.Clear();
                }
            }
            catch
            {
                Close(0, true);
            }
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                if (handler != null && handler.Connected)
                    handler.EndSend(ar);
            }
            catch
            {
                Close(0, true);
            }
        }
        /// <summary>
        /// Aguarda a chegada de um novo pacote. É necessário após a leitura de um pacote.
        /// </summary>
        private void read()
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = _client;
                _client.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(OnReceiveCallback), state);
            }
            catch
            {
                Close(0, true);
            }
        }
        private class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 8096;
            public byte[] buffer = new byte[BufferSize];
        }
        /// <summary>
        /// Fecha a conexão do cliente.
        /// </summary>
        /// <param name="destroyConn">Destruir conexão?</param>
        /// <param name="time">Tempo de espera para cancelar o recebimento de pacotes (milissegundos)</param>
        public void Close(int time, bool destroyConnection)
        {
            if (closed)
                return;

            try
            {
                closed = true;
                LoginManager.RemoveSocket(this);
                Account player = _player;
                if (destroyConnection)
                {
                    if (player != null)
                    {
                        player.setOnlineStatus(false);
                        if (player._status.serverId == 0)
                            SEND_REFRESH_ACC.RefreshAccount(player, false);
                        player._status.ResetData(player.player_id);
                        player.SimpleClear();
                        player.updateCacheInfo();
                        _player = null;
                    }
                    _client.Close(time);
                    Thread.Sleep(time);
                    Dispose();
                }
                else if (player != null)
                {
                    player.SimpleClear();
                    player.updateCacheInfo();
                    _player = null;
                }
                Auth_SyncNet.UpdateGSCount(0);
            }
            catch (Exception ex)
            {
                Logger.warning("[LoginClient.Close] " + ex.ToString());
            }
        }
        private void OnReceiveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            try
            {
                int bytesCount = state.workSocket.EndReceive(ar);
                if (bytesCount > 0)
                {
                    byte[] babyBuffer = new byte[bytesCount];
                    Array.Copy(state.buffer, 0, babyBuffer, 0, bytesCount);

                    int length = BitConverter.ToUInt16(babyBuffer, 0) & 0x7FFF;

                    byte[] buffer = new byte[length + 2];
                    Array.Copy(babyBuffer, 2, buffer, 0, buffer.Length);

                    lastCompleteBuffer = babyBuffer;
                    byte[] decrypted = ComDiv.decrypt(buffer, Shift);

                    RunPacket(decrypted, buffer);
                    checkoutN(babyBuffer, length);
                    new Thread(read).Start();
                }
            }
            catch (ObjectDisposedException ex)
            { }
            catch
            { Close(0, true); }
        }        
        public void checkoutN(byte[] buffer, int FirstLength)
        {
            int tamanho = buffer.Length;
            try
            {
                byte[] newPacketENC = new byte[tamanho - FirstLength - 4];
                Array.Copy(buffer, FirstLength + 4, newPacketENC, 0, newPacketENC.Length);
                if (newPacketENC.Length == 0)
                    return;

                int lengthPK = BitConverter.ToUInt16(newPacketENC, 0) & 0x7FFF;

                byte[] newPacketENC2 = new byte[lengthPK + 2];
                Array.Copy(newPacketENC, 2, newPacketENC2, 0, newPacketENC2.Length);


                byte[] newPacketGO = new byte[lengthPK + 2];

                Array.Copy(ComDiv.decrypt(newPacketENC2, Shift), 0, newPacketGO, 0, newPacketGO.Length);

                RunPacket(newPacketGO, newPacketENC);
                checkoutN(newPacketENC, lengthPK);
            }
            catch
            {
            }
        }
        
        private void FirstPacketCheck(ushort packetId)
        {
            if (firstPacketId == 0)
            {
                firstPacketId = packetId;
                if (packetId != 2561)
                {
                    Close(0, true);
                    Logger.warning("Connection destroyed due to Unknown first packet. [" + packetId + "]");
                }
            }
        }
        private void RunPacket(byte[] buff, byte[] simple)
        {
            UInt16 pacote = BitConverter.ToUInt16(buff, 0);
            FirstPacketCheck(pacote);
            if (closed)
                return;
            ReceiveLoginPacket packet = null;
            switch (pacote)
            {
                case 528:
                    packet = new BASE_USER_GIFTLIST_REC(this, buff); break;
                case 2561:
                case 2563:
                    packet = new BASE_LOGIN_REC(this, buff); break;
                case 2565:
                case 2666:
                    packet = new BASE_USER_INFO_REC(this, buff); break;
                case 2567:
                    packet = new BASE_USER_CONFIGS_REC(this, buff); break;
                case 2575:
                    break;
                case 2577:
                    packet = new BASE_SERVER_CHANGE_REC(this, buff); break;
                case 2579:
                    packet = new BASE_USER_ENTER_REC(this, buff); break;
                case 2581:
                    packet = new BASE_CONFIG_SAVE_REC(this, buff); break;
                case 2642:
                    packet = new BASE_SERVER_LIST_REFRESH_REC(this, buff); break;
                case 2654:
                    packet = new BASE_USER_EXIT_REC(this, buff); break;
                case 2678:
                    packet = new A_2678_REC(this, buff); break;
                case 2698:
                    packet = new BASE_USER_INVENTORY_REC(this, buff); break;
                default:
                    {
                        StringUtil strU = new StringUtil();
                        strU.AppendLine("|[LC]| Opcode não encontrado " + pacote);
                        strU.AppendLine("Encry/SemLength/Cheio: " + BitConverter.ToString(simple));
                        strU.AppendLine("SemEnc/SemLength/Cheio: " + BitConverter.ToString(buff));
                        strU.AppendLine("Enc/ComLength/TUDO: " + BitConverter.ToString(lastCompleteBuffer));
                        Logger.error(strU.getString());
                        break;
                    }
            }
            if (packet != null)
                new Thread(packet.run).Start();
        }
    }
}