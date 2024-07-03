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
        Lantern,
        IceCream,
        GymCard,
        Keys,
        Brush,
        MoonStone,
        SunStone,
        Orb
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
            case ItemType.IceCream:         return ItemAssets.Instance.iceCreamSprite;
            case ItemType.GymCard:          return ItemAssets.Instance.GymCardSprite;
            case ItemType.Keys:             return ItemAssets.Instance.KeysSprite;
            case ItemType.Brush:            return ItemAssets.Instance.BrushSprite;
            case ItemType.MoonStone:        return ItemAssets.Instance.MoonStoneSprite;
            case ItemType.SunStone:         return ItemAssets.Instance.SunStoneSprite;
            case ItemType.Orb:              return ItemAssets.Instance.OrbSprite;
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
            case ItemType.IceCream:
            case ItemType.GymCard:
            case ItemType.Keys:
            case ItemType.Brush:
            case ItemType.MoonStone:
            case ItemType.SunStone:
            case ItemType.Orb:
                return false;
        }
    }
}