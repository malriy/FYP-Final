using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    public Animator animator;
    public BoxCollider2D doorCollider;

    public void Interact()
    {
        StartCoroutine(colliderOff());
    }

    IEnumerator colliderOff()
    {
        doorCollider.enabled = false;
        animator.SetBool("isOpen", true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("isOpen", false);
        doorCollider.enabled = true;
    }
}
