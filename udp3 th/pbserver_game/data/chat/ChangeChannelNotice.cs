﻿using Core;
using Game.data.xml;

namespace Game.data.chat
{
    public static class ChangeChannelNotice
    {
        public static string SetChannelNotice(string str)
        {
            int idx = str.IndexOf(" ");
            if (idx == -1)
                return Translation.GetLabel("ChangeChAnnounceFail");
            int channelId = int.Parse(str.Substring(7, idx));
            if (channelId < 1)
                return Translation.GetLabel("ChangeChAnnounceFail2");
            channelId--;
            string announce = str.Substring(idx + 1);
            bool result = ChannelsXML.updateNotice(ConfigGS.serverId, channelId, announce);
            if (result)
            {
                Logger.warning(Translation.GetLabel("ChangeChAnnounceWarn", (channelId + 1), (ConfigGS.serverId + 1), announce));
                return Translation.GetLabel("ChangeChAnnounceSucc");
            }
            else
                return Translation.GetLabel("ChangeChAnnounceFail");
        }
        public static string SetAllChannelsNotice(string str)
        {
            string announce = str.Substring(6);
            bool result = ChannelsXML.updateNotice(announce);
            if (result)
            {
                Logger.warning(Translation.GetLabel("ChangeChsAnnounceWarn", announce));
                return Translation.GetLabel("ChangeChsAnnounceSucc");
            }
            return Translation.GetLabel("ChangeChsAnnounceFail");
        }
    }
}