using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrownBuilding : MonoBehaviour, IAdvanced
{
    public Player player;
    public Vector2 newPos;
    public Dialog dialog;

    public void Interact()
    {
        if (player.inventory.HasItem(new Item { itemType = Item.ItemType.BrownBuildingKey }))
        {
            Vector2 pos = new Vector2(newPos.x, newPos.y);

            player.transform.position = pos;
        }
        else
        {
            NotYetInteract();
        }
    }

    public void NotYetInteract()
    {
        StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
    }
}
