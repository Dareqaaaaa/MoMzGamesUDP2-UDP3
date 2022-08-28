using Core;
using Core.managers;
using Core.managers.server;
using Game.data.model;

namespace Game.data.chat
{
    public static class EnableMissions
    {
        public static string genCode1(string str, Account player)
        {
            bool activate = bool.Parse(str.Substring(8));
            bool result = ServerConfigSyncer.updateMission(GameManager.Config, activate);
            if (result)
            {
                Logger.warning(Translation.GetLabel("ActivateMissionsWarn", activate, player.player_name));
                return Translation.GetLabel("ActivateMissionsMsg1");
            }
            else
                return Translation.GetLabel("ActivateMissionsMsg2");
        }
    }
}