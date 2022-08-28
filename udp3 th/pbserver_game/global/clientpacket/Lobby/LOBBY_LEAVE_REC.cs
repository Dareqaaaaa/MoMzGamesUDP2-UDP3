using Core;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class LOBBY_LEAVE_REC : ReceiveGamePacket
    {
        private uint erro;
        public LOBBY_LEAVE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            try
            {
                if (_client == null || _client._player == null)
                    return;
                Account player = _client._player;
                Channel channel = player.getChannel();
                if (player._room != null || player._match != null)
                    return;
                if (channel == null || player.Session == null || !channel.RemovePlayer(player))
                    erro = 0x80000000;
                _client.SendPacket(new LOBBY_LEAVE_PAK(erro));
                if (erro == 0)
                {
                    player.ResetPages();
                    player._status.updateChannel(255);
                    AllUtils.syncPlayerToFriends(player, false);
                    AllUtils.syncPlayerToClanMembers(player);
                }
                else
                    _client.Close(1000);
            }
            catch (Exception ex)
            {
                Logger.info("LOBBY_LEAVE_REC: " + ex.ToString());
                _client.SendPacket(new LOBBY_LEAVE_PAK(0x80000000));
                _client.Close(1000);
            }
        }
    }
}