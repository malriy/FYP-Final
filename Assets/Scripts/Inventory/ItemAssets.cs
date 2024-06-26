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


}
