using Core;
using Core.models.servers;
using Core.xml;

namespace Game.data.chat
{
    public static class PlayersCountInServer
    {
        public static string GetMyServerPlayersCount()
        {
            return Translation.GetLabel("UsersCount", GameManager._socketList.Count, ConfigGS.serverId);
        }
        public static string GetServerPlayersCount(string str)
        {
            int serverId = int.Parse(str.Substring(9));
            GameServerModel server = ServersXML.getServer(serverId);
            if (server != null)
                return Translation.GetLabel("UsersCount2", server._LastCount, server._maxPlayers, serverId);
            else
                return Translation.GetLabel("UsersInvalid");
        }
    }
}