/*
 * Arquivo: Message.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/11/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.enums;
using System;
using System.Globalization;

namespace Core.models.account
{
    public class Message
    {
        public int object_id, clanId, type, state;
        public long sender_id, expireDate;
        public string sender_name = "", text = "";
        public NoteMessageClan cB;
        public int DaysRemaining;
        public Message()
        {
        }
        public Message(long expire, DateTime start)
        {
            expireDate = expire;
            SetDaysRemaining(start);
        }
        public Message(double days)
        {
            DateTime date = DateTime.Now.AddDays(days);
            expireDate = long.Parse(date.ToString("yyMMddHHmm"));
            SetDaysRemaining(date, DateTime.Now);
        }
        private void SetDaysRemaining(DateTime now)
        {
            DateTime end = DateTime.ParseExact(expireDate.ToString(), "yyMMddHHmm", CultureInfo.InvariantCulture);
            SetDaysRemaining(end, now);
        }
        private void SetDaysRemaining(DateTime end, DateTime now)
        {
            int days = (int)Math.Ceiling((end - now).TotalDays);
            DaysRemaining = (days < 0 ? 0 : days);
        }
    }
}