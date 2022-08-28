using Core.server;
using Game.data.model;
using Game.data.xml;

namespace Game.global.serverpacket
{
    public class BASE_CHANNEL_LIST_PAK : SendPacket
    {
        public BASE_CHANNEL_LIST_PAK()
        {
        }

        public override void write()
        {
            writeH(2572);
            writeD(ChannelsXML._channels.Count);
            writeD(ConfigGS.maxChannelPlayers);
            foreach (Channel channel in ChannelsXML._channels)
                writeD(channel._players.Count);
        }
    }
}