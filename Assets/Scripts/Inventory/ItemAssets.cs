using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfItemWorld;
    public Sprite healthPotSprite;
    public Sprite manaPotSprite;
    public Sprite BBKeySprite;
    public Sprite basketballSprite;
    public Sprite gunKeySprite;
    public Sprite lanternSprite;
    public Sprite iceCreamSprite;
    public Sprite GymCardSprite;
    public Sprite KeysSprite;
    public Sprite BrushSprite;
    public Sprite MoonStoneSprite;
    public Sprite SunStoneSprite;
    public Sprite OrbSprite;
}
