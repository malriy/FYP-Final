using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ItemEventArgs : EventArgs
{
    public Item Item { get; private set; }

    public ItemEventArgs(Item item)
    {
        Item = item;
    }
}

public class InventoryController : MonoBehaviour
{
    public static event EventHandler<ItemEventArgs> OnItemAdded;
    private List<Item> itemList;
    private Action<Item> useItemAction;

    public InventoryController(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool alreadyInInv = false;
            foreach (Item i in itemList)
            {
                if (i.itemType == item.itemType)
                {
                    i.amount += item.amount;
                    alreadyInInv = true;
                }
            }
            if (!alreadyInInv)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }

        OnItemAdded?.Invoke(this, new ItemEventArgs(item));
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInv = null;

            foreach (Item i in itemList)
            {
                if (i.itemType == item.itemType)
                {
                    i.amount -= item.amount;
                    itemInInv = i;
                }
            }
            if (itemInInv != null && itemInInv.amount <= 0)
            {
                itemList.Remove(itemInInv);
            }
        }
        else
        {
            itemList.Remove(item);
        }

        OnItemAdded?.Invoke(this, new ItemEventArgs(item));
    }

    public List<Item> GetItems()
    {
        return itemList;
    }

    public bool HasItem(Item item)
    {
        foreach (Item i in itemList)
        {
            if (i.itemType == item.itemType) { return true; }
        }
        return false;
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }
}