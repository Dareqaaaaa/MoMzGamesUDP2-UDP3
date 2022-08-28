using Core;
using Core.models.enums;

namespace Game.data.chat
{
    public static class ChangeUdpType
    {
        public static string SetUdpType(string str)
        {
            int udpT = int.Parse(str.Substring(4));
            if ((SERVER_UDP_STATE)udpT == ConfigGS.udpType)
                return Translation.GetLabel("ChangeUDPAlready");
            else if (udpT < 1 || udpT > 4)
                return Translation.GetLabel("ChangeUDPWrongValue");
            else
            {
                ConfigGS.udpType = (SERVER_UDP_STATE)udpT;
                return Translation.GetLabel("ChangeUDPSuccess", ConfigGS.udpType);
            }
        }
    }
}