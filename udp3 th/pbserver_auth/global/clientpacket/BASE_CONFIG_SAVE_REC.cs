/*
 * Arquivo: BASE_CONFIG_SAVE_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 17/12/2016
 * Sintam inveja, não nos atinge
 */

using Auth.data.model;
using Core.managers;
using Core.models.account.players;
using Core.server;
using System.Collections.Generic;

namespace Auth.global.clientpacket
{
    public class BASE_CONFIG_SAVE_REC : ReceiveLoginPacket
    {
        private int type;
        private DBQuery query = new DBQuery();
        public BASE_CONFIG_SAVE_REC(LoginClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            Account p = _client._player;
            if (p == null || p._config == null)
                return;
            type = readD();
            if ((type & 1) == 1)
            {
                p._config.blood = readH();
                p._config.sight = readC();
                p._config.hand = readC();
                p._config.config = readD();
                p._config.audio_enable = readC();
                readB(5);
                p._config.audio1 = readC();
                p._config.audio2 = readC();
                p._config.fov = readH();
                p._config.sensibilidade = readC();
                p._config.mouse_invertido = readC();
                readC();
                readC();
                p._config.msgConvite = readC();
                p._config.chatSussurro = readC();
                p._config.macro = readC();
                readC();
                readC();
                readC();
            }
            if ((type & 2) == 2)
            {
                readB(5);
                byte[] keysBuffer = readB(215);
                p._config.keys = keysBuffer;
            }
            if ((type & 4) == 4)
            {
                p._config.macro_1 = readS(readC());
                p._config.macro_2 = readS(readC());
                p._config.macro_3 = readS(readC());
                p._config.macro_4 = readS(readC());
                p._config.macro_5 = readS(readC());
            }
        }

        public override void run()
        {
            Account p = _client._player;
            if (p == null || p._config == null)
                return;
            PlayerConfig config = p._config;
            if ((type & 1) == 1)
                PlayerManager.updateConfigs(query, config);
            if ((type & 2) == 2)
                query.AddQuery("keys", config.keys);
            if ((type & 4) == 4)
                PlayerManager.updateMacros(query, config, type);
            ComDiv.updateDB("player_configs", "owner_id", p.player_id, query.GetTables(), query.GetValues());
        }
    }
}