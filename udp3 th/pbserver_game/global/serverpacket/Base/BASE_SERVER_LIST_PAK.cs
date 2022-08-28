using Core.models.servers;
using Core.server;
using Core.xml;
using Game.data.xml;
using System.Net;

namespace Game.global.serverpacket
{
    public class BASE_SERVER_LIST_PAK : SendPacket
    {
        private IPAddress _ip;
        private uint _sessionId;
        public BASE_SERVER_LIST_PAK(GameClient gc)
        {
            _sessionId = gc.SessionId;
            _ip = gc.GetAddress();
        }

        public override void write()
        {
            writeH(2049);
            writeD(_sessionId); //sessionId
            writeIP(_ip);
            writeH(29890);
            writeH(0); //?
            for (int i = 0; i < 10; i++)
                writeC((byte)ChannelsXML._channels[i]._type);
            writeC(1);
            writeD(ServersXML._servers.Count);
            for (int i = 0; i < ServersXML._servers.Count; i++)
            {
                GameServerModel server = ServersXML._servers[i];
                writeD(server._state);
                writeIP(server.Connection.Address);
                writeH(server._port);
                writeC((byte)server._type);
                writeH((ushort)server._maxPlayers);
                writeD(server._LastCount);
            }
            writeH(1); //Quantidade de algo
            writeH(300);
            writeD(200);
            writeD(100);
            /*
             * writeH(0);
             * writeD(0);
             * writeD(0);
             * 
             * */
            writeC(1); //Events Count
            writeD(3); //Type
            writeD(100); //Exp
            writeD(150); //Point
        }
    }
}