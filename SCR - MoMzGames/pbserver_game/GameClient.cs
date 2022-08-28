/*
 * Arquivo: GameClient.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 05/08/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.global;
using Game.global.clientpacket;
using Game.global.serverpacket;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Game
{
    public class GameClient : IDisposable
    {
        public long player_id;
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
            player_id = 0;
            if (disposing)
                handle.Dispose();
            disposed = true;
        }
        public GameClient(Socket client)
        {
            _client = client;
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
                Close(0);
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
        public bool isSocketConnected()
        {
            return !((_client.Poll(1000, SelectMode.SelectRead) && (_client.Available == 0)) || !_client.Connected);
        }
        public void SendCompletePacket(byte[] data)
        {
            try
            {
                if (data.Length < 4)
                    return;
                if (ConfigGS.debugMode)
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
                Close(0);
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
                if (ConfigGS.debugMode)
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
                Close(0);
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
                    if (ConfigGS.debugMode)
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
                Close(0);
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
            catch// (Exception ex)
            {
                //Logger.warning(ex.ToString());
                Close(0);
            }
        }
        private void read()
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = _client;
                _client.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(OnReceiveCallback), state);
            }
            catch// (Exception ex)
            {
                //Logger.warning(ex.ToString());
                Close(0);
            }
        }
        private class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 8096;
            public byte[] buffer = new byte[BufferSize];
        }
        private void OnReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
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
            catch
            {
                Close(0);
            }
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
        public void Close(int time, bool kicked = false)
        {
            if (!closed)
            {
                try
                {
                    closed = true;
                    GameManager.RemoveSocket(this);
                    Account p = _player;
                    if (player_id > 0 && p != null)
                    {
                        Channel ch = p.getChannel();
                        Room room = p._room;
                        Match match = p._match;
                        p.setOnlineStatus(false);
                        if (room != null)
                            room.RemovePlayer(p, false, kicked ? 1 : 0);
                        if (match != null)
                            match.RemovePlayer(p);
                        if (ch != null)
                            ch.RemovePlayer(p);
                        p._status.ResetData(player_id);

                        AllUtils.syncPlayerToFriends(p, false);
                        AllUtils.syncPlayerToClanMembers(p);

                        p.SimpleClear();
                        p.updateCacheInfo();
                        _player = null;
                    }
                    player_id = 0;
                }
                catch (Exception ex)
                {
                    Logger.warning("Error by: " + player_id + "\n" + ex.ToString());
                }
            }
            if (_client != null)
                _client.Close(time);
            Dispose();
        }
        
        private void FirstPacketCheck(ushort packetId)
        {
            if (firstPacketId == 0)
            {
                firstPacketId = packetId;
                if (packetId != 2579 && packetId != 2644)
                {
                    Close(0, false);
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
            ReceiveGamePacket packet = null;
            switch (pacote)
            {
                case 275:
                    packet = new FRIEND_INVITE_FOR_ROOM_REC(this, buff); break;
                case 280:
                    packet = new FRIEND_ACCEPT_REC(this, buff); break;
                case 282:
                    packet = new FRIEND_INVITE_REC(this, buff); break;
                case 284:
                    packet = new FRIEND_DELETE_REC(this, buff); break;
                case 290:
                    packet = new AUTH_SEND_WHISPER_REC(this, buff); break;
                case 292:
                    packet = new AUTH_SEND_WHISPER2_REC(this, buff); break;
                case 297:
                    packet = new AUTH_FIND_USER_REC(this, buff); break;



                case 417:
                    packet = new BOX_MESSAGE_CREATE_REC(this, buff); break;
                case 419:
                    packet = new BOX_MESSAGE_REPLY_REC(this, buff); break;
                case 422:
                    packet = new BOX_MESSAGE_VIEW_REC(this, buff); break;
                case 424:
                    packet = new BOX_MESSAGE_DELETE_REC(this, buff); break;



                case 530:
                    packet = new SHOP_BUY_ITEM_REC(this, buff); break;
                case 534:
                    packet = new INVENTORY_ITEM_EQUIP_REC(this, buff); break;
                case 536:
                    packet = new INVENTORY_ITEM_EFFECT_REC(this, buff); break;
                case 540:
                    packet = new BOX_MESSAGE_GIFT_TAKE_REC(this, buff); break;
                case 542:
                    packet = new INVENTORY_ITEM_EXCLUDE_REC(this, buff); break;
                case 544:
                    packet = new AUTH_WEB_CASH_REC(this, buff); break;
                case 548:
                    packet = new AUTH_CHECK_NICKNAME_REC(this, buff); break;
                    //554 = Reparar arma


                case 1304:
                    packet = new CLAN_GET_INFO_REC(this, buff); break;
                case 1306:
                    packet = new CLAN_MEMBER_CONTEXT_REC(this, buff); break;
                case 1308:
                    packet = new CLAN_MEMBER_LIST_REC(this, buff); break;
                case 1310:
                    packet = new CLAN_CREATE_REC(this, buff); break;
                case 1312:
                    packet = new CLAN_CLOSE_REC(this, buff); break;
                case 1314:
                    packet = new CLAN_CHECK_CREATE_INVITE_REC(this, buff); break;
                case 1316:
                    packet = new CLAN_CREATE_INVITE_REC(this, buff); break;
                case 1318:
                    packet = new CLAN_PLAYER_CLEAN_INVITES_REC(this, buff); break;
                case 1320:
                    packet = new CLAN_REQUEST_CONTEXT_REC(this, buff); break;
                case 1322:
                    packet = new CLAN_REQUEST_LIST_REC(this, buff); break;
                case 1324:
                    packet = new CLAN_REQUEST_INFO_REC(this, buff); break;
                case 1326:
                    packet = new CLAN_REQUEST_ACCEPT_REC(this, buff); break;
                case 1329:
                    packet = new CLAN_REQUEST_DENIAL_REC(this, buff); break;
                case 1332:
                    packet = new CLAN_PLAYER_LEAVE_REC(this, buff); break;
                case 1334:
                    packet = new CLAN_DEMOTE_KICK_REC(this, buff); break;
                case 1337:
                    packet = new CLAN_PROMOTE_MASTER_REC(this, buff); break;
                case 1340:
                    packet = new CLAN_PROMOTE_AUX_REC(this, buff); break;
                case 1343:
                    packet = new CLAN_DEMOTE_NORMAL_REC(this, buff); break;
                case 1358:
                    packet = new CLAN_CHATTING_REC(this, buff); break;
                case 1360:
                    packet = new CLAN_CHECK_DUPLICATE_LOGO_REC(this, buff); break;
                case 1362:
                    packet = new CLAN_REPLACE_NOTICE_REC(this, buff); break;
                case 1364:
                    packet = new CLAN_REPLACE_INTRO_REC(this, buff); break;
                case 1372:
                    packet = new CLAN_SAVEINFO3_REC(this, buff); break;
                case 1381:
                    packet = new CLAN_ROOM_INVITED_REC(this, buff); break;
                case 1390:
                    packet = new CLAN_CHAT_1390_REC(this, buff); break;
                case 1392:
                    packet = new CLAN_MESSAGE_INVITE_REC(this, buff); break;
                case 1394:
                    packet = new CLAN_MESSAGE_REQUEST_INTERACT_REC(this, buff); break;
                case 1396:
                    packet = new CLAN_MSG_FOR_PLAYERS_REC(this, buff); break;



                case 1416:
                    packet = new CLAN_CREATE_REQUIREMENTS_REC(this, buff); break;
                case 1441:
                    packet = new CLAN_CLIENT_ENTER_REC(this, buff); break;
                case 1443:
                    packet = new CLAN_CLIENT_LEAVE_REC(this, buff); break;
                case 1445:
                    packet = new CLAN_CLIENT_CLAN_LIST_REC(this, buff); break;
                case 1447:
                    packet = new CLAN_CHECK_DUPLICATE_NAME_REC(this, buff); break;
                case 1451:
                    packet = new CLAN_CLIENT_CLAN_CONTEXT_REC(this, buff); break;



                case 1538:
                    packet = new CLAN_WAR_PARTY_CONTEXT_REC(this, buff); break;
                case 1540:
                    packet = new CLAN_WAR_PARTY_LIST_REC(this, buff); break;
                case 1542:
                    packet = new CLAN_WAR_MATCH_TEAM_CONTEXT_REC(this, buff); break;
                case 1544:
                    packet = new CLAN_WAR_MATCH_TEAM_LIST_REC(this, buff); break;
                case 1546:
                    packet = new CLAN_WAR_CREATE_TEAM_REC(this, buff); break;
                case 1548:
                    packet = new CLAN_WAR_JOIN_TEAM_REC(this, buff); break;
                case 1550:
                    packet = new CLAN_WAR_LEAVE_TEAM_REC(this, buff); break;
                case 1553:
                    packet = new CLAN_WAR_PROPOSE_REC(this, buff); break;
                case 1558:
                    packet = new CLAN_WAR_ACCEPT_BATTLE_REC(this, buff); break;
                case 1565:
                    packet = new CLAN_WAR_CREATE_ROOM_REC(this, buff); break;
                case 1567:
                    packet = new CLAN_WAR_JOIN_ROOM_REC(this, buff); break;
                case 1569:
                    packet = new CLAN_WAR_MATCH_TEAM_INFO_REC(this, buff); break;
                case 1571:
                    packet = new CLAN_WAR_UPTIME_REC(this, buff); break;
                case 1576:
                    packet = new CLAN_WAR_TEAM_CHATTING_REC(this, buff); break;



                case 2571:
                    packet = new BASE_CHANNEL_LIST_REC(this, buff); break;
                case 2573:
                    packet = new BASE_CHANNEL_ENTER_REC(this, buff); break;
                case 2575:
                    packet = new BASE_HEARTBEAT_REC(this, buff); break;
                case 2577:
                    packet = new BASE_SERVER_CHANGE_REC(this, buff); break;
                case 2579:
                    packet = new BASE_USER_ENTER_REC(this, buff); break;
                case 2581:
                    packet = new BASE_CONFIG_SAVE_REC(this, buff); break;
                //case 2584:
                // packet = new CM_2584(this, buff); break;
                case 2591:
                    packet = new BASE_GET_USER_STATS_REC(this, buff); break;



                case 2601:
                    packet = new BASE_MISSION_ENTER_REC(this, buff); break;
                case 2605:
                    packet = new BASE_QUEST_BUY_CARD_SET_REC(this, buff); break;
                case 2607:
                    packet = new BASE_QUEST_DELETE_CARD_SET_REC(this, buff); break;
                case 2619:
                    packet = new BASE_TITLE_GET_REC(this, buff); break;
                case 2621:
                    packet = new BASE_TITLE_USE_REC(this, buff); break;
                case 2623:
                    packet = new BASE_TITLE_DETACH_REC(this, buff); break;
                case 2627:
                    packet = new BASE_CHATTING_REC(this, buff); break;
                case 2635:
                    packet = new BASE_MISSION_SUCCESS_REC(this, buff); break;
                case 2639:
                    packet = new LOBBY_GET_PLAYERINFO_REC(this, buff); break;
                case 2642:
                    packet = new BASE_SERVER_LIST_REFRESH_REC(this, buff); break;
                case 2644:
                    packet = new BASE_SERVER_PASSW_REC(this, buff); break;
                case 2654:
                    packet = new BASE_USER_EXIT_REC(this, buff); break;
                case 2661:
                    packet = new EVENT_VISIT_CONFIRM_REC(this, buff); break;
                case 2663:
                    packet = new EVENT_VISIT_REWARD_REC(this, buff); break;
                case 2684:
                    packet = new GM_LOG_LOBBY_REC(this, buff); break;
                case 2686:
                    packet = new GM_LOG_ROOM_REC(this, buff); break;
                case 2694://?
                    break;


                case 2817:
                    packet = new SHOP_LEAVE_REC(this, buff); break;
                case 2819:
                    packet = new SHOP_ENTER_REC(this, buff); break;
                case 2821:
                    packet = new SHOP_LIST_REC(this, buff); break;



                case 3073:
                    packet = new LOBBY_GET_ROOMLIST_REC(this, buff); break;
                case 3077:
                    packet = new LOBBY_QUICKJOIN_ROOM_REC(this, buff); break;
                case 3079:
                    packet = new LOBBY_ENTER_REC(this, buff); break;
                case 3081:
                    packet = new LOBBY_JOIN_ROOM_REC(this, buff); break;
                case 3083:
                    packet = new LOBBY_LEAVE_REC(this, buff); break;
                case 3087:
                    packet = new LOBBY_GET_ROOMINFO_REC(this, buff); break;
                case 3089:
                    packet = new LOBBY_CREATE_ROOM_REC(this, buff); break;
                case 3094:
                    packet = new A_3094_REC(this, buff); break;
                    //3096 = New??
                case 3099:
                    packet = new LOBBY_GET_PLAYERINFO2_REC(this, buff); break;
                case 3101:
                    packet = new LOBBY_CREATE_NICK_NAME_REC(this, buff); break;


                case 3329: //Pode ser chamado caso state = 13| caso contrário é chamado o 3333
                    packet = new BATTLE_3329_REC(this, buff); break;
                case 3331:
                    packet = new BATTLE_READYBATTLE_REC(this, buff); break;
                case 3333:
                    packet = new BATTLE_STARTBATTLE_REC(this, buff); break;
                case 3337:
                    packet = new BATTLE_RESPAWN_REC(this, buff); break;
                case 3344:
                    packet = new BATTLE_SENDPING_REC(this, buff); break;
                case 3348:
                    packet = new BATTLE_PRESTARTBATTLE_REC(this, buff); break;
                case 3354:
                    packet = new BATTLE_DEATH_REC(this, buff); break;
                case 3356:
                    packet = new BATTLE_MISSION_BOMB_INSTALL_REC(this, buff); break;
                case 3358:
                    packet = new BATTLE_MISSION_BOMB_UNINSTALL_REC(this, buff); break;
                case 3368:
                    packet = new BATTLE_MISSION_GENERATOR_INFO_REC(this, buff); break;
                case 3372:
                    packet = new BATTLE_TIMERSYNC_REC(this, buff); break;
                case 3376:
                    packet = new BATTLE_CHANGE_DIFFICULTY_LEVEL_REC(this, buff); break;
                case 3378:
                    packet = new BATTLE_RESPAWN_FOR_AI_REC(this, buff); break;
                case 3384:
                    packet = new BATTLE_PLAYER_LEAVE_REC(this, buff); break;
                case 3386:
                    packet = new BATTLE_MISSION_DEFENCE_INFO_REC(this, buff); break;
                case 3390:
                    packet = new BATTLE_DINO_DEATHBLOW_REC(this, buff); break;
                case 3394:
                    packet = new BATTLE_ENDTUTORIAL_REC(this, buff); break;
                case 3396:
                    packet = new VOTEKICK_START_REC(this, buff); break;



                case 3400:
                    packet = new VOTEKICK_UPDATE_REC(this, buff); break;
                    //3413

                    //3421 -Pausar
                    //3423 -Recomeçar
                case 3428:
                    packet = new A_3428_REC(this, buff); break;

                case 3585:
                    packet = new INVENTORY_ENTER_REC(this, buff); break;
                case 3589:
                    packet = new INVENTORY_LEAVE_REC(this, buff); break;



                case 3841:
                    packet = new ROOM_GET_PLAYERINFO_REC(this, buff); break;
                case 3845:
                    packet = new ROOM_CHANGE_SLOT_REC(this, buff); break;
                case 3847:
                    packet = new BATTLE_ROOM_INFO_REC(this, buff); break;
                case 3849:
                    packet = new ROOM_CLOSE_SLOT_REC(this, buff); break;
                case 3854:
                    packet = new ROOM_GET_LOBBY_USER_LIST_REC(this, buff); break;
                case 3858:
                    packet = new ROOM_CHANGE_INFO2_REC(this, buff); break;
                case 3862:
                    packet = new BASE_PROFILE_ENTER_REC(this, buff); break;
                case 3864:
                    packet = new BASE_PROFILE_LEAVE_REC(this, buff); break;
                case 3866:
                    packet = new ROOM_REQUEST_HOST_REC(this, buff); break;
                case 3868:
                    packet = new ROOM_RANDOM_HOST2_REC(this, buff); break;
                case 3870:
                    packet = new ROOM_CHANGE_HOST_REC(this, buff); break;
                case 3872:
                    packet = new ROOM_RANDOM_HOST_REC(this, buff); break;
                case 3874:
                    packet = new ROOM_CHANGE_TEAM_REC(this, buff); break;
                case 3884:
                    packet = new ROOM_INVITE_PLAYERS_REC(this, buff); break;
                case 3886:
                    packet = new ROOM_CHANGE_INFO_REC(this, buff); break;

                case 3890:
                    packet = new A_3890_REC(this, buff); break;
                case 3894:
                    packet = new A_3894_REC(this, buff); break;
                case 3900:
                    packet = new A_3900_REC(this, buff); break;
                case 3902:
                    packet = new A_3902_REC(this, buff); break;

                case 3904:
                    packet = new BATTLE_LOADING_REC(this, buff); break;
                case 3906:
                    packet = new ROOM_CHANGE_PASSW_REC(this, buff); break;

                case 3910:
                    packet = new EVENT_PLAYTIME_REWARD_REC(this, buff); break;
                default:
                    {
                        StringUtil strU = new StringUtil();
                        strU.AppendLine("|[GC]| Opcode não encontrado " + pacote);
                        strU.AppendLine("Encry/SemLength/Cheio: " + BitConverter.ToString(simple));
                        strU.AppendLine("SemEnc/SemLength/Cheio: " + BitConverter.ToString(buff));
                        strU.AppendLine("Enc/ComLength/TUDO: " + BitConverter.ToString(lastCompleteBuffer));
                        strU.AppendLine("pId: " + player_id);
                        Logger.error(strU.getString());
                        break;
                    }
            }
            if (packet != null)
                new Thread(packet.run).Start();
        }
    }
}