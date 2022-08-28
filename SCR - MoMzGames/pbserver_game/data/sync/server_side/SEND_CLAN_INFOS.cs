using Core.models.account.clan;
using Core.models.servers;
using Core.server;
using Core.xml;
using Game.data.model;

namespace Game.data.sync.server_side
{
    public class SEND_CLAN_INFOS
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pl">Conta principal</param>
        /// <param name="member"></param>
        /// <param name="type">0=Zerar players e clanId|1=Adicionar|2=Remover|3=att clanid e aux</param>
        public static void Load(Account pl, Account member, int type)
        {
            if (pl == null)
                return;
            GameServerModel gs = Game_SyncNet.GetServer(pl._status);
            if (gs == null)
                return;

            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(16);
                pk.writeQ(pl.player_id);
                pk.writeC((byte)type);
                if (type == 1) //adicionar
                {
                    pk.writeQ(member.player_id);
                    pk.writeC((byte)(member.player_name.Length + 1));
                    pk.writeS(member.player_name, member.player_name.Length + 1);
                    pk.writeB(member._status.buffer);
                    pk.writeC((byte)member._rank);
                }
                else if (type == 2) //remover
                    pk.writeQ(member.player_id);
                else if (type == 3) //atualizar id do clã e aux
                {
                    pk.writeD(pl.clanId);
                    pk.writeC((byte)pl.clanAccess);
                }
                Game_SyncNet.SendPacket(pk.mstream.ToArray(), gs.Connection);
            }
        }
        public static void Update(Clan clan, int type)
        {
            foreach (GameServerModel gs in ServersXML._servers)
            {
                if (gs._id == 0 || gs._id == ConfigGS.serverId)
                    continue;

                using (SendGPacket pk = new SendGPacket())
                {
                    pk.writeH(22);
                    pk.writeC((byte)type);
                    if (type == 0)
                        pk.writeQ(clan.owner_id);
                    else if (type == 1)
                    {
                        pk.writeC((byte)(clan._name.Length + 1));
                        pk.writeS(clan._name, clan._name.Length + 1);
                    }
                    else if (type == 2)
                        pk.writeC((byte)clan._name_color);
                    Game_SyncNet.SendPacket(pk.mstream.ToArray(), gs.Connection);
                }
            }
        }
        public static void Load(Clan clan, int type)
        {
            foreach (GameServerModel gs in ServersXML._servers)
            {
                if (gs._id == 0 || gs._id == ConfigGS.serverId)
                    continue;

                using (SendGPacket pk = new SendGPacket())
                {
                    pk.writeH(21);
                    pk.writeC((byte)type);
                    pk.writeD(clan._id);
                    if (type == 0)
                    {
                        pk.writeQ(clan.owner_id);
                        pk.writeD(clan.creationDate);
                        pk.writeC((byte)(clan._name.Length + 1));
                        pk.writeS(clan._name, clan._name.Length + 1);
                        pk.writeC((byte)(clan._info.Length + 1));
                        pk.writeS(clan._info, clan._info.Length + 1);
                    }
                    Game_SyncNet.SendPacket(pk.mstream.ToArray(), gs.Connection);
                }
            }
        }
    }
}