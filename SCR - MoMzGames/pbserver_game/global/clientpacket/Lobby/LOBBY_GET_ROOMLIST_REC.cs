using Core;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class LOBBY_GET_ROOMLIST_REC : ReceiveGamePacket
    {
        public LOBBY_GET_ROOMLIST_REC(GameClient client, byte[] data)
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
                Account p = _client._player;
                if (p == null)
                    return;
                Channel channel = p.getChannel();
                if (channel != null)
                {
                    channel.RemoveEmptyRooms();
                    List<Room> rooms = channel._rooms;
                    List<Account> waiting = channel.getWaitPlayers();
                    int Rpages = (int)Math.Ceiling(rooms.Count / 15d),
                        Apages = (int)Math.Ceiling(waiting.Count / 10d);
                    if (p.LastRoomPage >= Rpages)
                        p.LastRoomPage = 0;
                    if (p.LastPlayerPage >= Apages)
                        p.LastPlayerPage = 0;
                    int roomsCount = 0, playersCount = 0;
                    byte[] roomsArray = GetRoomListData(p.LastRoomPage, ref roomsCount, rooms);
                    byte[] waitingArray = GetPlayerListData(p.LastPlayerPage, ref playersCount, waiting);
                    _client.SendPacket(new LOBBY_GET_ROOMLIST_PAK(rooms.Count, waiting.Count, p.LastRoomPage++, p.LastPlayerPage++, roomsCount, playersCount, roomsArray, waitingArray));
                }
            }
            catch (Exception ex)
            {
                Logger.warning("[LOBBY_GET_ROOMLIST_REC] " + ex.ToString());
            }
        }
        private byte[] GetRoomListData(int page, ref int count, List<Room> list)
        {
            using (SendGPacket p = new SendGPacket())
            {
                for (int i = (page * 15); i < list.Count; i++)
                {
                    WriteRoomData(list[i], p);
                    if (++count == 15)
                        break;
                }
                return p.mstream.ToArray();
            }
        }
        private void WriteRoomData(Room room, SendGPacket p)
        {
            int restrictions = 0;
            p.writeD(room._roomId);
            p.writeS(room.name, 23);
            p.writeH((short)room.mapId);
            p.writeC(room.stage4v4);
            p.writeC(room.room_type);
            p.writeC((byte)room._state);
            p.writeC((byte)room.getAllPlayers().Count);
            p.writeC((byte)room.getSlotCount());
            p.writeC((byte)room._ping);
            p.writeC(room.weaponsFlag);
            if (room.random_map > 0) restrictions += 2;
            if (room.password.Length > 0) restrictions += 4;
            if (room.limit > 0 && (int)room._state > 0) restrictions += 128;
            p.writeC((byte)restrictions);
            p.writeC(room.special);
        }
        private void WritePlayerData(Account pl, SendGPacket p)
        {
            Clan clan = ClanManager.getClan(pl.clanId);
            p.writeD(pl.getSessionId());
            p.writeD(clan._logo);
            p.writeS(clan._name, 17);
            p.writeH((short)pl.getRank());
            p.writeS(pl.player_name, 33);
            p.writeC((byte)pl.name_color);
            p.writeC(31); //31
        }
        private byte[] GetPlayerListData(int page, ref int count, List<Account> list)
        {
            using (SendGPacket p = new SendGPacket())
            {
                for (int i = (page * 10); i < list.Count; i++)
                {
                    WritePlayerData(list[i], p);
                    if (++count == 10)
                        break;
                }
                return p.mstream.ToArray();
            }
        }
    }
}