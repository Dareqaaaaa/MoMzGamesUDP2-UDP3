using Core;

namespace Game.data.chat
{
    public static class ChangeServerMode
    {
        public static string EnableTestMode()
        {
            if (ConfigGS.isTestMode)
                return Translation.GetLabel("AlreadyTestModeOn");
            else
            {
                ConfigGS.isTestMode = true;
                return Translation.GetLabel("TestModeOn");
            }
        }
        public static string EnablePublicMode()
        {
            if (!ConfigGS.isTestMode)
                return Translation.GetLabel("AlreadyTestModeOff");
            else
            {
                ConfigGS.isTestMode = false;
                return Translation.GetLabel("TestModeOff");
            }
        }
    }
}