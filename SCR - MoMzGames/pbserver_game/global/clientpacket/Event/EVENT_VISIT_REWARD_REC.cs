using Core;
using Core.managers;
using Core.managers.events;
using Core.models.account;
using Core.models.account.players;
using Core.models.enums.errors;
using Core.models.shop;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class EVENT_VISIT_REWARD_REC : ReceiveGamePacket
    {
        private EventErrorEnum erro = EventErrorEnum.VisitEvent_Success;
        private int eventId, type;
        public EVENT_VISIT_REWARD_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            eventId = readD();
            type = readC();
        }

        public override void run()
        {
            try
            {
                if (_client == null)
                    return;
                Account p = _client._player;
                if (p == null || p.player_name.Length == 0 || type > 1)
                    erro = EventErrorEnum.VisitEvent_UserFail;
                else if (p._event != null)
                {
                    if (p._event.LastVisitSequence1 == p._event.LastVisitSequence2)
                        erro = EventErrorEnum.VisitEvent_AlreadyCheck;
                    else
                    {
                        EventVisitModel eventv = EventVisitSyncer.getEvent(eventId);
                        if (eventv == null)
                        {
                            erro = EventErrorEnum.VisitEvent_Unknown;
                            goto Result;
                        }

                        if (eventv.EventIsEnabled())
                        {
                            VisitItem chI = eventv.getReward(p._event.LastVisitSequence2, type);
                            if (chI != null)
                            {
                                GoodItem good = ShopManager.getGood(chI.good_id);
                                if (good != null)
                                {
                                    p._event.NextVisitDate = int.Parse(DateTime.Now.AddDays(1).ToString("yyMMdd"));
                                    ComDiv.updateDB("player_events", "player_id", p.player_id, new string[] { "next_visit_date", "last_visit_sequence2" }, p._event.NextVisitDate, ++p._event.LastVisitSequence2);

                                    _client.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(good._item._id, good._item._category, good._item._name, good._item._equip, (uint)chI.count)));
                                }
                                else erro = EventErrorEnum.VisitEvent_NotEnough;
                            }
                            else erro = EventErrorEnum.VisitEvent_Unknown;
                        }
                        else erro = EventErrorEnum.VisitEvent_WrongVersion;
                    }
                }
                else erro = EventErrorEnum.VisitEvent_Unknown;
            Result:
                _client.SendPacket(new EVENT_VISIT_REWARD_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("[EVENT_VERIFICATION_REWARD_REC] " + ex.ToString());
            }
        }
    }
}