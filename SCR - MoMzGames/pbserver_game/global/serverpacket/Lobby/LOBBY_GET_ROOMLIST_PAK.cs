using Core.server;

namespace Game.global.serverpacket
{
    public class LOBBY_GET_ROOMLIST_PAK : SendPacket
    {
        private int _roomPage, _playerPage, _allPlayers, _allRooms, _count1, _count2;
        private byte[] _salas, _waiting;
        public LOBBY_GET_ROOMLIST_PAK(int allRooms, int allPlayers, int roomPage, int playerPage, int count1, int count2, byte[] rooms, byte[] players)
        {
            _allRooms = allRooms;
            _allPlayers = allPlayers;
            _roomPage = roomPage;
            _playerPage = playerPage;
            _salas = rooms;
            _waiting = players;
            _count1 = count1;
            _count2 = count2;
        }

        public override void write()
        {
            writeH(3074);
            writeD(_allRooms); //total
            writeD(_roomPage); //página atual(roomPages - 1)
            writeD(_count1); //15 salas por página_salas.Count - carregando atualmente
            writeB(_salas);

            writeD(_allPlayers); //total
            writeD(_playerPage); //página atual
            writeD(_count2); //10 por página
            writeB(_waiting);
        }
    }
}