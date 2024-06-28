using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNPC : NPCController
{
    private bool hasTalked = false;
    [SerializeField] private Dialog newDialog;
    [SerializeField] private GameObject bars;

    public override void Interact()
    {
        if (hasTalked)
        {
            bars.SetActive(false);
            StartCoroutine(DialogueManager.Instance.ShowDialog(newDialog));
        }
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
            hasTalked = true;
        }
    }
}
