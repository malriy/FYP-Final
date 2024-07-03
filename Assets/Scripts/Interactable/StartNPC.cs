using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNPC : NPCController
{
    [SerializeField] private Dialog onEnochComplete;
    [SerializeField] private Dialog onCpComplete;
    [SerializeField] private Dialog onGameComplete;

    [SerializeField] private GameObject bars;
    public static int lastLevelCompleted;

    public override void Interact()
    {
        if (hasTalked)
        {
            Destroy(bars);
            StartCoroutine(DialogueManager.Instance.ShowDialog(newDialog));
        }
        else
        {
            if (EnochStart.EnochCompleted && true) // ADD CP FINISH LEVEL
            {
                StartCoroutine(DialogueManager.Instance.ShowDialog(onGameComplete));
            }
            else if (EnochStart.EnochCompleted && lastLevelCompleted == 2)
            {
                StartCoroutine(DialogueManager.Instance.ShowDialog(onEnochComplete));
            }
            else if (lastLevelCompleted == 1)
            {
                StartCoroutine(DialogueManager.Instance.ShowDialog(onCpComplete));
            }
            else
            {
                StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
            }

            hasTalked = true;
        }
    }
}
