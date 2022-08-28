using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class ROOM_GET_LOBBY_USER_LIST_REC : ReceiveGamePacket
    {
        public ROOM_GET_LOBBY_USER_LIST_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            Account player = _client._player;
            if (player == null)
                return;
            try
            {
                Channel ch = player.getChannel();
                if (ch != null)
                    _client.SendPacket(new ROOM_GET_LOBBY_USER_LIST_PAK(ch));
            }
            catch (Exception ex) 
            { 
                Logger.warning(ex.ToString()); 
            }
        }
    }
}