using Core;

namespace Game.data.chat
{
    public static class AFK_Interaction
    {
        public static string GetAFKCount(string str)
        {
            double hours = double.Parse(str.Substring(9));
            int count = GameManager.KickCountActiveClient(hours);

            return Translation.GetLabel("AFK_Count_Success", count);
        }
        public static string KickAFKPlayers(string str)
        {
            double hours = double.Parse(str.Substring(8));
            int count = GameManager.KickActiveClient(hours);

            return Translation.GetLabel("AFK_Kick_Success", count);
        }
    }
}