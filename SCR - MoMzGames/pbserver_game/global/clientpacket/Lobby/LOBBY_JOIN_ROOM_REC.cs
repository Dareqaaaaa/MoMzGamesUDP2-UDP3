/*
 * Arquivo: ROOM_JOIN_NORMAL_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 21/07/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class LOBBY_JOIN_ROOM_REC : ReceiveGamePacket
    {
        private int roomId, type;
        private string password;
        public LOBBY_JOIN_ROOM_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            roomId = readD();
            password = readS(4);
            type = readC2();
        }

        public override void run()
        {
            try
            {
                Account p = _client._player, leader;
                Channel ch;
                if (p != null && p.player_name.Length > 0 && p._room == null && 
                    p._match == null && p.getChannel(out ch))
                {
                    Room room = ch.getRoom(roomId);
                    if (room != null && room.getLeader(out leader))
                    {
                        if (room.room_type == 10)
                            _client.SendPacket(new LOBBY_JOIN_ROOM_PAK(0x8000107C)); //Tutorial
                        else if (room.password.Length > 0 && password != room.password && p._rank != 53 && !p.HaveGMLevel() && type != 1)
                            _client.SendPacket(new LOBBY_JOIN_ROOM_PAK(0x80001005));
                        else if (room.limit == 1 && (int)room._state >= 1 && !p.HaveGMLevel() || room.special == 5)
                            _client.SendPacket(new LOBBY_JOIN_ROOM_PAK(0x80001013)); //Entrada proibida com partida em andamento
                        else if (room.kickedPlayers.Contains(p.player_id) && !p.HaveGMLevel())
                            _client.SendPacket(new LOBBY_JOIN_ROOM_PAK(0x8000100C)); //Você foi expulso dessa sala.
                        else if (room.addPlayer(p) >= 0)
                        {
                            p.ResetPages();
                            using (ROOM_GET_SLOTONEINFO_PAK packet = new ROOM_GET_SLOTONEINFO_PAK(p))
                                room.SendPacketToPlayers(packet, p.player_id);
                            _client.SendPacket(new LOBBY_JOIN_ROOM_PAK(0, p, leader));
                        }
                        else _client.SendPacket(new LOBBY_JOIN_ROOM_PAK(0x80001003)); //SLOTFULL
                    }
                    else _client.SendPacket(new LOBBY_JOIN_ROOM_PAK(0x80001004)); //INVALIDROOM
                }
                else _client.SendPacket(new LOBBY_JOIN_ROOM_PAK(0x80001004));
            }
            catch (Exception ex)
            {
                Logger.warning("[ROOM_JOIN_NORMAL_REC] " + ex.ToString());
            }
        }
    }
}