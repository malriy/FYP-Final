using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStore : MonoBehaviour, Interactable
{
    [SerializeField] private Dialog dialog;
    public Player player;
    public Vector2 newPos;
    public static bool locked = true;

    public void Interact()
    {
        if (!locked)
        {
            Vector2 pos = new Vector2(newPos.x, newPos.y);

            player.transform.position = pos;
        }
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
        }
    }
}
