/*
 * Arquivo: Account.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 15/08/2017
 * Sintam inveja, não nos atinge
 */

using Core.managers;
using Core.models.account;
using Core.models.account.players;
using Core.models.account.title;
using Core.models.enums;
using Core.models.enums.flags;
using Core.server;
using Game.data.managers;
using Game.data.sync;
using Game.data.xml;
using System;
using System.Collections.Generic;
using System.Net;

namespace Game.data.model
{
    public class Account
    {
        public byte[] LocalIP = new byte[4];
        public bool _isOnline, HideGMcolor, AntiKickGM, LoadedShop, DebugPing;
        public string player_name = "", password, login;
        public long player_id, ban_obj_id;
        public uint LastRankUpDate, LastLoginDate;
        public IPAddress PublicIP;
        public CupomEffects effects;
        public PlayerSession Session;
        public int LastRoomPage, LastPlayerPage,
            tourneyLevel, channelId = -1, clanAccess, clanDate, _exp, _gp, clanId, _money, brooch, insignia, medal, blue_order, _slotId = -1, name_color, _rank, pc_cafe, matchSlot = -1;
        public PlayerEquipedItems _equip = new PlayerEquipedItems();
        public PlayerInventory _inventory = new PlayerInventory();
        public PlayerConfig _config;
        public GameClient _connection;
        public Room _room;
        public PlayerBonus _bonus;
        public Match _match;
        public AccessLevel access;
        public PlayerMissions _mission = new PlayerMissions();
        public PlayerStats _statistic = new PlayerStats();
        public FriendSystem FriendSystem = new FriendSystem();
        public PlayerTitles _titles = new PlayerTitles();
        public AccountStatus _status = new AccountStatus();
        public PlayerEvent _event;
        public DateTime LastSlotChange, LastLobbyEnter, LastPingDebug;
        public Account()
        {
            LastSlotChange = DateTime.Now;
            LastLobbyEnter = DateTime.Now;
        }
        /// <summary>
        /// Eventos, Títulos, Missões, Inventário, Status, Lista de amigos (Clean), Sessão, ClanMatch, Sala, Config, Connection,
        /// </summary>
        public void SimpleClear()
        {
            _titles = new PlayerTitles();
            _mission = new PlayerMissions();
            _inventory = new PlayerInventory();
            _status = new AccountStatus();
            FriendSystem.CleanList();
            Session = null;
            _event = null;
            _match = null;
            _room = null;
            _config = null;
            _connection = null;
        }
        public void SetPublicIP(IPAddress address)
        {
            if (address == null)
                PublicIP = new IPAddress(new byte[4]);
            PublicIP = address;
        }
        public void SetPublicIP(string address)
        {
            PublicIP = IPAddress.Parse(address);
        }
        public Channel getChannel()
        {
            return ChannelsXML.getChannel(channelId);
        }
        public void ResetPages()
        {
            LastRoomPage = 0;
            LastPlayerPage = 0;
        }
        public bool getChannel(out Channel channel)
        {
            channel = ChannelsXML.getChannel(channelId);
            return channel != null;
        }
        public void setOnlineStatus(bool online)
        {
            if (_isOnline != online && ComDiv.updateDB("accounts", "online", online, "player_id", player_id))
                _isOnline = online;
        }
        public void updateCacheInfo()
        {
            if (player_id == 0)
                return;
            lock (AccountManager._contas)
            {
                AccountManager._contas[player_id] = this;
            }
        }
        public int getRank()
        {
            return _bonus == null || _bonus.fakeRank == 55 ? _rank : _bonus.fakeRank;
        }
        public void Close(int time, bool kicked = false)
        {
            if (_connection != null)
                _connection.Close(time, kicked);
        }
        /// <summary>
        /// Envia pacotes para o jogador. Caso ele não esteja conectado, os dados não serão enviados.
        /// </summary>
        /// <param name="data">Dados</param>
        public void SendPacket(SendPacket sp)
        {
            if (_connection != null)
                _connection.SendPacket(sp);
        }
        public void SendPacket(SendPacket sp, bool OnlyInServer)
        {
            if (_connection != null)
                _connection.SendPacket(sp);
            else if (!OnlyInServer && (_status.serverId != 255 && _status.serverId != ConfigGS.serverId))
                Game_SyncNet.SendBytes(player_id, sp, _status.serverId);
        }
        /// <summary>
        /// Envia pacotes para o jogador. Caso ele não esteja conectado, os dados não serão enviados.
        /// </summary>
        /// <param name="data">Dados</param>
        public void SendPacket(byte[] data)
        {
            if (_connection != null)
                _connection.SendPacket(data);
        }
        public void SendPacket(byte[] data, bool OnlyInServer)
        {
            if (_connection != null)
                _connection.SendPacket(data);
            else if (!OnlyInServer && (_status.serverId != 255 && _status.serverId != ConfigGS.serverId))
                Game_SyncNet.SendBytes(player_id, data, _status.serverId);
        }
        /// <summary>
        /// Envia pacotes para o jogador. Caso ele não esteja conectado, os dados não serão enviados.
        /// </summary>
        /// <param name="data">Dados</param>
        public void SendCompletePacket(byte[] data)
        {
            if (_connection != null)
                _connection.SendCompletePacket(data);
        }
        public void SendCompletePacket(byte[] data, bool OnlyInServer)
        {
            if (_connection != null)
                _connection.SendCompletePacket(data);
            else if (!OnlyInServer && (_status.serverId != 255 && _status.serverId != ConfigGS.serverId))
                Game_SyncNet.SendCompleteBytes(player_id, data, _status.serverId);
        }
        public void LoadInventory()
        {
            lock (_inventory._items)
            {
                _inventory._items.AddRange(PlayerManager.getInventoryItems(player_id));
            }
        }
        public void LoadMissionList()
        {
            PlayerMissions m = MissionManager.getInstance().getMission(player_id, _mission.mission1, _mission.mission2, _mission.mission3, _mission.mission4);
            if (m == null)
                MissionManager.getInstance().addMissionDB(player_id);
            else
                _mission = m;
        }
        public void LoadPlayerBonus()
        {
            PlayerBonus bonus = PlayerManager.getPlayerBonusDB(player_id);
            if (bonus.ownerId == 0)
            {
                PlayerManager.CreatePlayerBonusDB(player_id);
                bonus.ownerId = player_id;
            }
            _bonus = bonus;
        }
        public uint getSessionId()
        {
            return Session != null ? Session._sessionId : 0;
        }
        public void SetPlayerId(long id)
        {
            player_id = id;
            GetAccountInfos(35);
        }
        public void SetPlayerId(long id, int LoadType)
        {
            player_id = id;
            GetAccountInfos(LoadType);
        }
        /// <summary>
        /// Gera informações adicionais a conta\n0 = Nada\n1 = Títulos\n2 = Bônus\n4 = Amigos (Completo)\n8 = Eventos\n16 = Configurações\n32 = Amigos (Básico)
        /// </summary>
        /// <param name="LoadType">Detalhes</param>
        public void GetAccountInfos(int LoadType)
        {
            if (LoadType > 0 && player_id > 0)
            {
                if ((LoadType & 1) == 1)
                    _titles = TitleManager.getInstance().getTitleDB(player_id);
                if ((LoadType & 2) == 2)
                    _bonus = PlayerManager.getPlayerBonusDB(player_id);
                if ((LoadType & 4) == 4)
                {
                    List<Friend> fs = PlayerManager.getFriendList(player_id);
                    if (fs.Count > 0)
                    {
                        FriendSystem._friends = fs;
                        AccountManager.getFriendlyAccounts(FriendSystem);
                    }
                }
                if ((LoadType & 8) == 8)
                    _event = PlayerManager.getPlayerEventDB(player_id);
                if ((LoadType & 16) == 16)
                    _config = PlayerManager.getConfigDB(player_id);
                if ((LoadType & 32) == 32)
                {
                    List<Friend> fs = PlayerManager.getFriendList(player_id);
                    if (fs.Count > 0)
                        FriendSystem._friends = fs;
                }
            }
        }
        public bool UseChatGM()
        {
            return !HideGMcolor && (_rank == 53 || _rank == 54);
        }
        /// <summary>
        /// Retorna TRUE se rank == 53 ou 54 ou HaveGMLevel.
        /// </summary>
        /// <returns></returns>
        public bool IsGM()
        {
            return _rank == 53 || _rank == 54 || HaveGMLevel();
        }
        public bool HaveGMLevel()
        {
            return (int)access > 2;
        }
        public bool HaveAcessLevel()
        {
            return (int)access > 0;
        }
    }
}