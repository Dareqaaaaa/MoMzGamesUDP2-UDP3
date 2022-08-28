using Core;
using Core.filters;
using Core.managers;
using Core.models.account.players;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class LOBBY_CREATE_NICK_NAME_REC : ReceiveGamePacket
    {
        private string name;
        public LOBBY_CREATE_NICK_NAME_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }
        public override void read()
        {
            name = readS(readC());
        }
        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null || p.player_name.Length > 0 || string.IsNullOrEmpty(name) || name.Length < ConfigGS.minNickSize || name.Length > ConfigGS.maxNickSize)
                {
                    _client.SendPacket(new LOBBY_CREATE_NICK_NAME_PAK(0x80001013));
                    return;
                }
                foreach (string s in NickFilter._filter)
                    if (name.Contains(s))
                    {
                        _client.SendPacket(new LOBBY_CREATE_NICK_NAME_PAK(0x80001013));
                        return;
                    }
                if (!PlayerManager.isPlayerNameExist(name))
                {
                    if (AccountManager.updatePlayerName(name, p.player_id))
                    {
                        NickHistoryManager.CreateHistory(p.player_id, p.player_name, name, "First nick");
                        p.player_name = name;
                        List<ItemsModel> awards = BasicInventoryXML.creationAwards;
                        if (awards.Count > 0)
                        {
                            _client.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, p, awards));
                            _client.SendPacket(new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0));
                        }
                        _client.SendPacket(new LOBBY_CREATE_NICK_NAME_PAK(0));
                        _client.SendPacket(new BASE_QUEST_GET_INFO_PAK(p));
                    }
                    else _client.SendPacket(new LOBBY_CREATE_NICK_NAME_PAK(0x80001013));
                }
                else _client.SendPacket(new LOBBY_CREATE_NICK_NAME_PAK(0x80000113));
            }
            catch (Exception ex)
            {
                Logger.warning("[LOBBY_CREATE_NICK_NAME_REC] " + ex.ToString());
            }
        }
    }
}