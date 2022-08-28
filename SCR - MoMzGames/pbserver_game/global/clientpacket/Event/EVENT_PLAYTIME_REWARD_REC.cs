/*
 * Arquivo: EVENT_PLAYTIME_REWARD_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 01/07/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.managers;
using Core.managers.events;
using Core.models.account.players;
using Core.models.shop;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class EVENT_PLAYTIME_REWARD_REC : ReceiveGamePacket
    {
        private int goodId;
        public EVENT_PLAYTIME_REWARD_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            goodId = readD();
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player == null)
                    return;
                PlayerEvent pev = player._event;
                GoodItem good = ShopManager.getGood(goodId);
                if (good == null || pev == null)
                    return;
                PlayTimeModel eventPt = EventPlayTimeSyncer.getRunningEvent();
                if (eventPt != null)
                {
                    uint count = (uint)eventPt.GetRewardCount(goodId);
                    if (pev.LastPlaytimeFinish == 1 && count > 0 && ComDiv.updateDB("player_events", "last_playtime_finish", 2, "player_id", _client.player_id))
                    {
                        pev.LastPlaytimeFinish = 2;
                        _client.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, player, new ItemsModel(good._item._id, good._item._category, "Playtime reward", good._item._equip, count)));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.info("EVENT_PLAYTIME_REWARD_REC] " + ex.ToString());
            }
        }
    }
}