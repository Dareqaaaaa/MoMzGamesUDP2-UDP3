/*
 * Arquivo: BASE_GIFT_LIST_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 20/05/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account;
using Core.server;
using System;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
    public class BASE_USER_GIFT_LIST_PAK : SendPacket
    {
        private int erro;
        private List<Message> gifts;
        public BASE_USER_GIFT_LIST_PAK(int erro, List<Message> gifts)
        {
            this.erro = erro;
            this.gifts = gifts;
        }

        public override void write()
        {
            writeH(529);
            writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm"))); //? data
            writeQ(0); //Nunca é usado.
            writeD(erro); //Count?/Existe presentes pendentes?|Valor<0  - Cancela o resto dos códigos abaixo
            writeC(0); //gifts.Count
            writeC((byte)gifts.Count); 
            writeC(0); //0 = Ativa algo? || O valor deve estar entre 0-99 || Valor acima de 100 = Erro GiftNouteCount Overflow
            foreach (Message gift in gifts)
            {
                writeD(gift.object_id); //Msg?
                writeD((uint)gift.sender_id); //Good da loja
                writeD(gift.state); //already get
                writeD((uint)gift.expireDate); //enddate
                writeC((byte)(gift.sender_name.Length + 1));
                writeS(gift.sender_name, gift.sender_name.Length + 1);
                writeC(1);
                writeS("", 1);
            }
        }
    }
}