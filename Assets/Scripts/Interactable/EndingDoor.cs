using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDoor : MonoBehaviour, Interactable
{
    public Animator animator;
    public BoxCollider2D doorCollider;
    [SerializeField] private Dialog dialog;
    [SerializeField] private GameObject sensor;


    public void Interact()
    {
        if (EnochStart.EnochCompleted)  //ADD CHONG PEI'S "IS LEVEL FINISHED VARIABLE"
        {
            StartCoroutine(colliderOff());
        }
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
        }
    }

    IEnumerator colliderOff()
    {
        sensor.SetActive(true);
        doorCollider.enabled = false;
        animator.SetBool("isOpen", true);
        yield return null;
    }
}
