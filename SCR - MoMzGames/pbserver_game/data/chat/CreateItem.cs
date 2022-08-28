using Core;
using Core.models.account.players;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.data.chat
{
    public static class CreateItem
    {
        public static string CreateItemYourself(string str, Account player)
        {
            int id = int.Parse(str.Substring(3));
            if (id < 100000000)
                return Translation.GetLabel("CreateItemWrongID");
            else if (player != null)
            {
                int category = ComDiv.GetItemCategory(id);
                player.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, player, new ItemsModel(id, category, "Command item", category == 3 ? 1 : 3, 1)));
                player.SendPacket(new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0));
                return Translation.GetLabel("CreateItemSuccess");
            }
            else
                return Translation.GetLabel("CreateItemFail");
        }
        public static string CreateItemByNick(string str, Account player)
        {
            string txt = str.Substring(str.IndexOf(" ") + 1);
            string[] split = txt.Split(' ');
            string nick = split[0];
            int item_id = Convert.ToInt32(split[1]);
            if (item_id < 100000000)
                return Translation.GetLabel("CreateItemWrongID");
            else
            {
                Account playerO = AccountManager.getAccount(nick, 1, 0);
                if (playerO == null)
                    return Translation.GetLabel("CreateItemFail");
                if (playerO.player_id == player.player_id)
                    return Translation.GetLabel("CreateItemUseOtherCMD");
                else
                {
                    int category = ComDiv.GetItemCategory(item_id);
                    playerO.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, playerO, new ItemsModel(item_id, category, "Command item", category == 3 ? 1 : 3, 1)), false);
                    playerO.SendPacket(new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0), false);
                    return Translation.GetLabel("CreateItemSuccess");
                }
            }
        }
        public static string CreateItemById(string str, Account player)
        {
            string txt = str.Substring(str.IndexOf(" ") + 1);
            string[] split = txt.Split(' ');
            int item_id = Convert.ToInt32(split[1]);
            long player_id = Convert.ToInt64(split[0]);
            if (item_id < 100000000)
                return Translation.GetLabel("CreateItemWrongID");
            else
            {
                Account playerO = AccountManager.getAccount(player_id, 0);
                if (playerO != null)
                {
                    if (playerO.player_id == player.player_id)
                        return Translation.GetLabel("CreateItemUseOtherCMD");
                    else
                    {
                        int category = ComDiv.GetItemCategory(item_id);
                        playerO.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, playerO, new ItemsModel(item_id, category, "Command item", category == 3 ? 1 : 3, 1)), false);
                        playerO.SendPacket(new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0), false);
                        return Translation.GetLabel("CreateItemSuccess");
                    }
                }
                else
                    return Translation.GetLabel("CreateItemFail");
            }
        }

        public static string CreateGoldCupom(string str)
        {
            string txt = str.Substring(str.IndexOf(" ") + 1);
            string[] split = txt.Split(' ');
            int gold = Convert.ToInt32(split[1]);
            long player_id = Convert.ToInt64(split[0]);
            if (gold.ToString().EndsWith("00"))
            {
                if (gold < 100 || gold > 99999999)
                    return Translation.GetLabel("CreateSItemWrongID");
                Account playerO = AccountManager.getAccount(player_id, 0);
                if (playerO != null)
                {
                    int cuponId = ComDiv.createItemId(15, (gold / 1000000), (gold % 1000) / 100, (gold % 1000000) / 1000);
                    playerO.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, playerO, new ItemsModel(cuponId, 3, "Gold CMD item", 1, 1)), false);
                    playerO.SendPacket(new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0), false);
                    return Translation.GetLabel("CreateSItemSuccess", gold);
                }
                else
                    return Translation.GetLabel("CreateItemFail");
            }
            else return Translation.GetLabel("CreateSItemFail");
        }
    }
}