using Core;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
    public static class KickAllPlayers
    {
        public static string KickPlayers()
        {
            int succ = 0;
            using (AUTH_ACCOUNT_KICK_PAK packet = new AUTH_ACCOUNT_KICK_PAK(0))
            {
                if (GameManager._socketList.Count > 0)
                {
                    byte[] data = packet.GetCompleteBytes("KickAllPlayers.genCode");
                    foreach (GameClient client in GameManager._socketList.Values)
                    {
                        Account p = client._player;
                        if (p != null && p._isOnline && (int)p.access <= 2)
                        {
                            p.SendCompletePacket(data);
                            p.Close(1000, true);
                            succ++;
                        }
                    }
                }
            }
            return Translation.GetLabel("KickAllWarn", succ);
        }
    }
}