using Core;
using Core.managers;
using Core.models.account.title;
using Core.xml;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BASE_TITLE_USE_REC : ReceiveGamePacket
    {
        private byte slotIdx, titleId;
        private uint erro;
        public BASE_TITLE_USE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            slotIdx = readC();
            titleId = readC();
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null)
                    return;
                PlayerTitles t = p._titles;
                TitleQ titleQ = TitlesXML.getTitle(titleId),
                    eq1, eq2, eq3;
                TitlesXML.get3Titles(t.Equiped1, t.Equiped2, t.Equiped3, out eq1, out eq2, out eq3, false);
                if (slotIdx >= 3 || titleId >= 45 || t == null || titleQ == null || titleQ._classId == eq1._classId && slotIdx != 0 || titleQ._classId == eq2._classId && slotIdx != 1 || titleQ._classId == eq3._classId && slotIdx != 2 || !t.Contains(titleQ._flag) || t.Equiped1 == titleId || t.Equiped2 == titleId || t.Equiped3 == titleId)
                    erro = 0x80000000;
                else
                {
                    if (TitleManager.getInstance().updateEquipedTitle(t.ownerId, slotIdx, titleId))
                        t.SetEquip(slotIdx, titleId);
                    else
                        erro = 0x80000000;
                }
                _client.SendPacket(new BASE_TITLE_USE_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("BASE_TITLE_USE_REC: " + ex.ToString());
            }
        }
    }
}