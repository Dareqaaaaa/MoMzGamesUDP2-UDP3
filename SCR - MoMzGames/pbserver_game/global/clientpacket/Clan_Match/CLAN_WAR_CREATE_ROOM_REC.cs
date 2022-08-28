using Core;
using Core.models.enums;
using Core.models.enums.match;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_WAR_CREATE_ROOM_REC : ReceiveGamePacket
    {
        private Match MyMatch, EnemyMatch;
        private int roomId = -1;
        public CLAN_WAR_CREATE_ROOM_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            Account p = _client._player;
            if (p == null || p.clanId == 0)
                return;
            Channel channel = p.getChannel();
            MyMatch = p._match;
            if (channel == null || MyMatch == null)
                return;
            int match = readH();
            int channel1 = readD();
            int channel2 = readD();
            EnemyMatch = channel.getMatch(match);
            try
            {
                if (EnemyMatch == null)
                    return;
                lock (channel._rooms)
                    for (int i = 0; i < 300; i++)
                        if (channel.getRoom(i) == null)
                        {
                            Room room = new Room(i, channel);
                            readH();
                            room.name = readS(23);
                            room.mapId = readH();
                            room.stage4v4 = readC();
                            room.room_type = readC();
                            readH();
                            room.initSlotCount(readC());
                            readC();
                            room.weaponsFlag = readC();
                            room.random_map = readC();
                            room.special = readC();
                            room.password = "";
                            room.killtime = 3;
                            room.addPlayer(p);
                            channel.AddRoom(room);
                            _client.SendPacket(new LOBBY_CREATE_ROOM_PAK(0, room, p));
                            roomId = i;
                            return;
                        }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
        }

        public override void run()
        {
            if (roomId == -1)
                return;
            using (CLAN_WAR_ENEMY_INFO_PAK packet = new CLAN_WAR_ENEMY_INFO_PAK(EnemyMatch))
            using (CLAN_WAR_JOINED_ROOM_PAK packet2 = new CLAN_WAR_JOINED_ROOM_PAK(EnemyMatch, roomId, 0))
            {
                byte[] data = packet.GetCompleteBytes("CLAN_WAR_CREATE_ROOM_REC-1");
                byte[] data2 = packet2.GetCompleteBytes("CLAN_WAR_CREATE_ROOM_REC-2");
                foreach (Account pM in MyMatch.getAllPlayers(MyMatch._leader))
                {
                    if (pM._match != null)
                    {
                        pM.SendCompletePacket(data);
                        pM.SendCompletePacket(data2);
                        MyMatch._slots[pM.matchSlot].state = SlotMatchState.Ready;
                    }
                }
            }
            using (CLAN_WAR_ENEMY_INFO_PAK packet = new CLAN_WAR_ENEMY_INFO_PAK(MyMatch))
            using (CLAN_WAR_JOINED_ROOM_PAK packet2 = new CLAN_WAR_JOINED_ROOM_PAK(MyMatch, roomId, 1))
            {
                byte[] data = packet.GetCompleteBytes("CLAN_WAR_CREATE_ROOM_REC-3");
                byte[] data2 = packet2.GetCompleteBytes("CLAN_WAR_CREATE_ROOM_REC-4");
                foreach (Account pM in EnemyMatch.getAllPlayers())
                {
                    if (pM._match != null)
                    {
                        pM.SendCompletePacket(data);
                        pM.SendCompletePacket(data2);
                        MyMatch._slots[pM.matchSlot].state = SlotMatchState.Ready;
                    }
                }
            }
        }
    }
}