using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Item
{
    public enum ItemType
    {
        HealthPotion,
        ManaPotion,
        BrownBuildingKey,
        Basketball,
        GunStoreKey,
        Lantern
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default: 
            case ItemType.HealthPotion:     return ItemAssets.Instance.healthPotSprite;
            case ItemType.ManaPotion:       return ItemAssets.Instance.manaPotSprite;
            case ItemType.BrownBuildingKey: return ItemAssets.Instance.BBKeySprite;
            case ItemType.Basketball:       return ItemAssets.Instance.basketballSprite;
            case ItemType.GunStoreKey:      return ItemAssets.Instance.gunKeySprite;
            case ItemType.Lantern:          return ItemAssets.Instance.lanternSprite;
        }
    }

    public bool IsStackable()
    {
        switch(itemType)
        {
            default:
            case ItemType.HealthPotion:
            case ItemType.ManaPotion: 
                return true;
            case ItemType.BrownBuildingKey:
            case ItemType.Basketball:
            case ItemType.GunStoreKey:
            case ItemType.Lantern:
                return false;
        }
    }
}