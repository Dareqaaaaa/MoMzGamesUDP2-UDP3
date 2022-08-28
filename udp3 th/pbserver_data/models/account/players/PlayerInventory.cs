/*
 * Arquivo: PlayerInventory.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 31/07/2017
 * Sintam inveja, não nos atinge
 */

using Core.xml;
using System.Collections.Generic;

namespace Core.models.account.players
{
    public class PlayerInventory
    {
        public List<ItemsModel> _items = new List<ItemsModel>();
        public ItemsModel getItem(int id)
        {
            lock (_items)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    ItemsModel item = _items[i];
                    if (item._id == id)
                        return item;
                }
            }
            return null;
        }
        public ItemsModel getItem(long obj)
        {
            lock (_items)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    ItemsModel item = _items[i];
                    if (item._objId == obj)
                        return item;
                }
            }
            return null;
        }
        public void LoadBasicItems()
        {
            lock (_items)
            {
                _items.AddRange(BasicInventoryXML.basic);
            }
        }
        public List<ItemsModel> getItemsByType(int type)
        {
            List<ItemsModel> list = new List<ItemsModel>();
            lock (_items)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    ItemsModel item = _items[i];
                    if (item._category == type || item._id > 1200000000 && item._id < 1300000000 && type == 4)
                        list.Add(item);
                }
            }
            return list;
        }
        /// <summary>
        /// Remove um item específico do inventário do jogador através do número do objeto. (Proteção de Thread-Safety)
        /// </summary>
        /// <param name="obj">Número do objeto</param>
        public void RemoveItem(long obj)
        {
            lock (_items)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    ItemsModel item = _items[i];
                    if (item._objId == obj)
                    {
                        _items.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Remove um item específico do inventário do jogador através do item. (Proteção de Thread-Safety)
        /// </summary>
        /// <param name="item">Modelo do item</param>
        public bool RemoveItem(ItemsModel item)
        {
            lock (_items)
            {
                return _items.Remove(item);
            }
        }
        /// <summary>
        /// Adiciona um item no inventário do jogador. (Proteção de Thread-Safety)
        /// </summary>
        /// <param name="item">Modelo do item</param>
        public void AddItem(ItemsModel item)
        {
            lock (_items)
            {
                _items.Add(item);
            }
        }
    }
}