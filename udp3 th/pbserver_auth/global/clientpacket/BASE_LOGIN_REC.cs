/*
 * Arquivo: BASE_LOGIN_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/12/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.managers;
using Auth.data.model;
using Auth.data.sync;
using Auth.data.sync.server_side;
using Auth.global.serverpacket;
using Core;
using Core.managers;
using Core.managers.server;
using Core.models.enums.errors;
using Core.models.enums.global;
using Core.models.servers;
using Core.server;
using Core.xml;
using System;
using System.Net.NetworkInformation;

namespace Auth.global.clientpacket
{
    public class BASE_LOGIN_REC : ReceiveLoginPacket
    {
        private string login, passN, passEnc, UserFileListMD5, d3d9MD5, GameVersion, PublicIP, LocalIP;
        private int loginSize, passSize, IsRealIP;
        private ulong key;
        private ClientLocale GameLocale;
        private PhysicalAddress MacAddress;
        public BASE_LOGIN_REC(LoginClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            GameVersion = readC() + "." + readH() + "." + readH();
            GameLocale = (ClientLocale)readC();
            loginSize = readC();
            passSize = readC();
            login = readS(loginSize);
            passN = readS(passSize);
            passEnc = ComDiv.gen5(passN);
            MacAddress = new PhysicalAddress(readB(6));
            readB(2);

            IsRealIP = readC(); //1-Local server|0-Public server (IsRealIP)
            LocalIP = readC() + "." + readC() + "." + readC() + "." + readC();
            key = readUQ();
            UserFileListMD5 = readS(32);
            readD();
            d3d9MD5 = readS(32);
            readC();
            PublicIP = _client.GetIPAddress();


        }

        public override void run()
        {
            try
            {
                //if (!RegionXML.regions.Contains(AllUtils.gI().getCountry(PublicIP)))
                //{
                //    _client.SendPacket(new BASE_LOGIN_PAK(EventErrorEnum.Login_BLOCK_COUNTRY, login, 0));
                //    Logger.LogLogin("Região do IP bloqueada [" + login + "]");
                //    _client.Close(0, false);
                //}
                bool isValid = DirectXML.IsValid(d3d9MD5);
                if (!isValid)
                    Logger.warning("No listed: " + d3d9MD5);
                ServerConfig cfg = LoginManager.Config;
                if (cfg == null || !ConfigGA.isTestMode && !ConfigGA.GameLocales.Contains(GameLocale) || login.Length < ConfigGA.minLoginSize || !ConfigGA.isTestMode && passN.Length < ConfigGA.minPassSize || LocalIP == "0.0.0.0" || MacAddress.GetAddressBytes() == new byte[6] || GameVersion != cfg.ClientVersion || ConfigGA.LauncherKey > 0 && key != ConfigGA.LauncherKey) //_UserFileListMD5 != cfg._UserFileList || 
                {
                    string msg = "";
                    if (cfg == null)
                        msg = "Config do servidor inválida [" + login + "]";
                    else if (!ConfigGA.isTestMode && !ConfigGA.GameLocales.Contains(GameLocale))
                        msg = "País: " + GameLocale + " do cliente bloqueado [" + login + "]";
                    else if (login.Length < ConfigGA.minLoginSize)
                        msg = "Login muito pequeno [" + login + "]";
                    else if (!ConfigGA.isTestMode && passN.Length < ConfigGA.minPassSize)
                        msg = "Senha muito pequena [" + login + "]";
                    else if (LocalIP == "0.0.0.0")
                        msg = "IP inválido. [" + login + "]";
                    else if (MacAddress.GetAddressBytes() == new byte[6])
                        msg = "MAC inválido. [" + login + "]";
                    else if (GameVersion != cfg.ClientVersion)
                        msg = "Versão: " + GameVersion + " não compatível [" + login + "]";
                    else if (ConfigGA.LauncherKey > 0 && key != ConfigGA.LauncherKey)
                        msg = "Chave: " + key + " não compatível [" + login + "]";
                    else
                        msg = "UserFileList: " + UserFileListMD5 + " não compatível [" + login + "]";
                    _client.SendPacket(new SERVER_MESSAGE_DISCONNECT_PAK(2147483904, false));
                    Logger.LogLogin(msg);
                    _client.Close(1000, true);
                }
                else
                {
                    _client._player = AccountManager.getInstance().getAccountDB(login, null, 0, 0);
                    if (_client._player == null && ConfigGA.AUTO_ACCOUNTS && !AccountManager.getInstance().CreateAccount(out _client._player, login, passEnc))
                    {
                        _client.SendPacket(new BASE_LOGIN_PAK(EventErrorEnum.Login_DELETE_ACCOUNT, login, 0));
                        Logger.LogLogin("Falha ao criar conta automaticamente [" + login + "]");
                        _client.Close(1000, false);
                    }
                    else
                    {
                        Account p = _client._player;
                        if (p == null || !p.ComparePassword(passEnc))
                        {
                            string msg = "";
                            if (p == null)
                                msg = "Conta retornada da DB é nula";
                            else if (!p.ComparePassword(passEnc))
                                msg = "Senha inválida";
                            _client.SendPacket(new BASE_LOGIN_PAK(EventErrorEnum.Login_DELETE_ACCOUNT, login, 0));
                            Logger.LogLogin(msg + " [" + login + "]");
                            _client.Close(1000, false);
                        }
                        else if (p.access >= 0)
                        {
                            if (p.MacAddress != MacAddress)
                                ComDiv.updateDB("accounts", "last_mac", MacAddress, "player_id", p.player_id);
                            bool macStatus, ipStatus;
                            BanManager.GetBanStatus(MacAddress.ToString(), PublicIP, out macStatus, out ipStatus);
                            if (macStatus || ipStatus)
                            {
                                if (macStatus)
                                    Logger.LogLogin("MAC banido [" + login + "]");
                                else
                                    Logger.LogLogin("IP banido [" + login + "]");
                                _client.SendPacket(new BASE_LOGIN_PAK(ipStatus ? EventErrorEnum.Login_BLOCK_IP : EventErrorEnum.Login_BLOCK_ACCOUNT, login, 0));
                                _client.Close(1000, false);
                            }
                            else if (p.IsGM() && cfg.onlyGM || p.access >= 0 && !cfg.onlyGM)
                            {
                                Account pCache = AccountManager.getInstance().getAccount(p.player_id, true);
                                if (!p._isOnline)
                                {
                                    BanHistory htb = BanManager.GetAccountBan(p.ban_obj_id);
                                    if (htb.endDate > DateTime.Now)
                                    {
                                        _client.SendPacket(new BASE_LOGIN_PAK(EventErrorEnum.Login_BLOCK_ACCOUNT, login, 0));
                                        Logger.LogLogin("Conta com ban ativo [" + login + "]");
                                        _client.Close(1000, false);
                                    }
                                    else
                                    {
                                        p.SetPlayerId(p.player_id, 31);
                                        p._clanPlayers = ClanManager.getClanPlayers(p.clan_id, p.player_id);
                                        _client.SendPacket(new BASE_LOGIN_PAK(0, login, p.player_id));
                                        _client.SendPacket(new AUTH_WEB_CASH_PAK(0));
                                        if (p.clan_id > 0)
                                            _client.SendPacket(new BASE_USER_CLAN_MEMBERS_PAK(p._clanPlayers));
                                        p._status.SetData(4294967295, p.player_id);
                                        p._status.updateServer(0);
                                        p.setOnlineStatus(true);
                                        if (pCache != null)
                                            pCache._connection = _client;
                                        SEND_REFRESH_ACC.RefreshAccount(p, true);
                                    }
                                }
                                else
                                {
                                    _client.SendPacket(new BASE_LOGIN_PAK(EventErrorEnum.Login_ALREADY_LOGIN_WEB, login, 0));
                                    Logger.LogLogin("Conta online [" + login + "]");
                                    if (pCache != null && pCache._connection != null)
                                    {
                                        pCache.SendPacket(new AUTH_ACCOUNT_KICK_PAK(1));
                                        pCache.SendPacket(new SERVER_MESSAGE_ERROR_PAK(2147487744));
                                        pCache.Close(1000);
                                    }
                                    else
                                        Auth_SyncNet.SendLoginKickInfo(p);
                                    _client.Close(1000, false);
                                }
                            }
                            else
                            {
                                _client.SendPacket(new BASE_LOGIN_PAK(EventErrorEnum.Login_TIME_OUT_2, login, 0));
                                Logger.LogLogin("Nível de acesso inválido [" + login + "]");
                                _client.Close(1000, false);
                            }
                        }
                        else
                        {
                            _client.SendPacket(new BASE_LOGIN_PAK(EventErrorEnum.Login_BLOCK_ACCOUNT, login, 0));
                            Logger.LogLogin("Banido permanente [" + login + "]");
                            _client.Close(1000, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.warning("[BASE_LOGIN_REC] " + ex.ToString());
            }
        }
        private void LoginQueue()
        {
            GameServerModel server = ServersXML.getServer(0);
            if (server._LastCount >= server._maxPlayers)
            {
                if (LoginManager._loginQueue.Count >= 100)
                {
                    _client.SendPacket(new BASE_LOGIN_PAK(EventErrorEnum.Login_SERVER_USER_FULL, login, 0));
                    Logger.LogLogin("Servidor cheio [" + login + "]");
                    _client.Close(1000, false);
                    return;
                }
                else
                {
                    int pos = LoginManager.EnterQueue(_client);
                    _client.SendPacket(new A_LOGIN_QUEUE_PAK(pos + 1, ((pos + 1) * 120)));
                }
            }
        }
    }
}