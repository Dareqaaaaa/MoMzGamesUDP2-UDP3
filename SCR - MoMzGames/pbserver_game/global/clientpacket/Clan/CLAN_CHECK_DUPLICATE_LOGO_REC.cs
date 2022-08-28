using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class CLAN_CHECK_DUPLICATE_LOGO_REC : ReceiveGamePacket
    {
        private uint logo, erro;
        public CLAN_CHECK_DUPLICATE_LOGO_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            logo = readUD();
        }

        public override void run()
        {
            Account p = _client._player;
            if (p == null || ClanManager.getClan(p.clanId)._logo == logo ||
                ClanManager.isClanLogoExist(logo))
                erro = 0x80000000;
            _client.SendPacket(new CLAN_CHECK_DUPLICATE_MARK_PAK(erro));
        }
    }
}