using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    public Animator animator;
    public BoxCollider2D collider;

    public void Interact()
    {
        StartCoroutine(colliderOff());
    }

    IEnumerator colliderOff()
    {
        collider.enabled = false;
        animator.SetBool("isOpen", true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("isOpen", false);
        collider.enabled = true;
    }
}
