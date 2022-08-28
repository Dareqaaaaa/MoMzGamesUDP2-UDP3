using Core;
using Core.managers.events;
using Core.models.enums.errors;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class EVENT_VISIT_CONFIRM_REC : ReceiveGamePacket
    {
        private EventErrorEnum erro = EventErrorEnum.VisitEvent_Success;
        private int eventId;
        private EventVisitModel eventv;
        public EVENT_VISIT_CONFIRM_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            eventId = readD();
        }

        public override void run()
        {
            try
            {
                if (_client == null)
                    return;
                Account p = _client._player;
                if (p == null || string.IsNullOrEmpty(p.player_name))
                    erro = EventErrorEnum.VisitEvent_UserFail;
                else if (p._event != null)
                {
                    int dateNow = int.Parse(DateTime.Now.ToString("yyMMdd"));
                    if (p._event.NextVisitDate <= dateNow)
                    {
                        eventv = EventVisitSyncer.getEvent(eventId);
                        if (eventv == null)
                        {
                            erro = EventErrorEnum.VisitEvent_Unknown;
                            goto Result;
                        }

                        if (eventv.EventIsEnabled())
                        {
                            p._event.NextVisitDate = int.Parse(DateTime.Now.AddDays(1).ToString("yyMMdd"));
                            ComDiv.updateDB("player_events", "player_id", p.player_id, new string[] { "next_visit_date", "last_visit_sequence1" }, p._event.NextVisitDate, ++p._event.LastVisitSequence1);
                            bool IsReward = false;
                            try
                            {
                                IsReward = eventv.box[p._event.LastVisitSequence2].reward1.IsReward;
                            }
                            catch { }
                            if (!IsReward)
                                ComDiv.updateDB("player_events", "last_visit_sequence2", ++p._event.LastVisitSequence2, "player_id", p.player_id);
                        }
                        else erro = EventErrorEnum.VisitEvent_WrongVersion;
                    }
                    else erro = EventErrorEnum.VisitEvent_AlreadyCheck;
                }
                else erro = EventErrorEnum.VisitEvent_Unknown;
            Result:
                _client.SendPacket(new EVENT_VISIT_CONFIRM_PAK(erro, eventv, p._event));
            }
            catch (Exception ex)
            {
                Logger.info("EVENT_VERIFICATION_CHECK_REC] " + ex.ToString());
            }
        }
    }
}