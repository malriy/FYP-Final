using UnityEngine;
using System.Collections.Generic;

public class Item
{
    public enum ItemType
    {
        HealthPotion,
        ManaPotion,
        BrownBuildingKey,
        Basketball
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
                return false;
        }
    }
}