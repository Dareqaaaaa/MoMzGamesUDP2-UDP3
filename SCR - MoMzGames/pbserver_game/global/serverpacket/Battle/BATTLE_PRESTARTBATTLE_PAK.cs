using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BATTLE_PRESTARTBATTLE_PAK : SendPacket
    {
        private Account player, leader;
        private Room room;
        private bool isPreparing, LoadHits;
        private uint UniqueRoomId;
        private int gen2;
        public BATTLE_PRESTARTBATTLE_PAK(Account p, Account l, bool more, int gen2)
        {
            player = p;
            leader = l;
            LoadHits = more;
            this.gen2 = gen2;
            room = p._room;
            if (room != null)
            {
                isPreparing = room.isPreparing();
                UniqueRoomId = room.UniqueRoomId;
            }
        }
        public BATTLE_PRESTARTBATTLE_PAK()
        {
        }
        public override void write()
        {
            writeH(3349);
            writeD(isPreparing);
            if (!isPreparing)
                return;

            writeD(player._slotId);
            writeC((byte)ConfigGS.udpType);
            /* RENDEZVOUS (Udp not in operation) only for Tutorial mode;
             * CLIENT (all calculation is performed on player host client) only for AI and Infection mode;
             * RELAY (all calculation is performed on server) for other modes. */
            writeIP(leader.PublicIP);
            writeH(29890);
            writeB(leader.LocalIP);
            writeH(29890);
            writeC(0);
            writeIP(player.PublicIP);
            writeH(29890);
            writeB(player.LocalIP);
            writeH(29890);
            writeC(0);
            writeIP(room.UDPServer.Connection.Address);
            writeH((ushort)room.UDPServer.Port);
            writeD(UniqueRoomId);
            writeD(gen2);
            if (LoadHits)
            {
                //writeB(room.HitParts);
                writeB(new byte[35] //hitparts
				{
					0x20, 0x15, 0x16, 0x17, 
                    0x18, 0x19, 0x11, 0x1B, 
                    0x1C, 0x1D, 0x1A, 0x1F, 
                    0x09, 0x21, 0x0E, 0x1E, 
                    0x01, 0x02, 0x03, 0x04, 
                    0x05, 0x06, 0x07, 0x08, 
                    0x14, 0x0A, 0x0B, 0x0C, 
                    0x0D, 0x22, 0x0F, 0x10, 
                    0x00, 0x12, 0x13
				});
            }
        }
    }
}