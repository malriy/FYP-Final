using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnims : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void DeathAnim()
    {
        animator.SetTrigger("droneDeath");
    }
}
