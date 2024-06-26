using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNPC : NPCController
{
    private bool hasTalked = false;
    [SerializeField] private Dialog newDialog;

    public override void Interact()
    {
        if (hasTalked)
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(newDialog));
        }
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
            hasTalked = true;
        }
    }
}
