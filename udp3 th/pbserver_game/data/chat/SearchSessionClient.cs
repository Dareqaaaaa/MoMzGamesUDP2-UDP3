using Core;
using Core.managers;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
    public static class SearchSessionClient
    {
        public static string genCode1(string str)
        {
            uint sessionId = uint.Parse(str.Substring(13));
            Account player = GameManager.SearchActiveClient(sessionId);
            if (player != null)
            {
                return "";
            }
            else
            {
                return "";
            }
        }
    }
}