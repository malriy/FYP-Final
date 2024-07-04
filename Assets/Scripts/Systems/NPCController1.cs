using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController1 : MonoBehaviour, Interactable
{
    public string npcName;
    public string npcID; // Unique ID for each NPC

    [SerializeField] public Dialog dialog;
    [SerializeField] private Dialog newDialog;

    private Text nameText;

    [SerializeField] public bool willRoam = false; // Determines if the NPC should roam
    [SerializeField] private float roamDistance = 5f; // Distance the NPC can roam left and right
    [SerializeField] private float roamSpeed = 1f; // Speed of roaming
    [SerializeField] private float roamDelay = 2f; // Delay between roaming movements

    [Header("Assign Player if NPC gift something")]
    public PlayerController1 player1;
    public Player player;
    [SerializeField] private List<RewardItems> gifts;

    private Vector3 startPosition;
    private Vector3 leftPosition;
    private Vector3 rightPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private bool playerInRange = false;

    private void Awake()
    {
        nameText = GameObject.Find("NameText").GetComponent<Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Uncomment the next line to reset the interaction state in the Unity Editor for testing purposes
        PlayerPrefs.DeleteKey(npcID);
        

        startPosition = transform.position;
        leftPosition = startPosition + Vector3.left * roamDistance;
        rightPosition = startPosition + Vector3.right * roamDistance;

        if (willRoam)
        {
            StartCoroutine(Roam());
        }

        // Load the interaction state
        hasTalked = PlayerPrefs.GetInt(npcID, 0) == 1;
        Debug.Log($"NPC {npcID} hasTalked: {hasTalked}");
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool("moving", moving);
        }
    }

    [NonSerialized] public bool hasTalked;
    public virtual void Interact()
    {
        if (animator != null)
        {
            StopCoroutine(Roam());
            animator.SetBool("moving", false);
        }

        if (npcName != null)
        {
            nameText.text = npcName;
        }
        else
        {
            nameText.text = "???";
        }

        if (!hasTalked)
        {
            StartCoroutine(DialogueManager1.Instance.ShowDialog(dialog));
            GiftPlayer();
            hasTalked = true;
            PlayerPrefs.SetInt(npcID, 1); // Save the interaction state
            Debug.Log($"NPC {npcID} hasTalked set to true");
        }
        else
        {
            StartCoroutine(DialogueManager1.Instance.ShowDialog(newDialog));
        }
    }

    [NonSerialized] public bool moving;

    void GiftPlayer()
    {
        foreach (RewardItems rewardItems in gifts)
        {
            Item item = new Item { itemType = rewardItems.itemType, amount = rewardItems.amount };
            PlayerController1.Instance.inventory.AddItem(item);
        }
    }

    private IEnumerator Roam()
    {
        while (willRoam)
        {
            if (!playerInRange)
            {
                // Set target position based on the current direction
                targetPosition = movingRight ? rightPosition : leftPosition;

                // Move towards the target position
                while (!playerInRange && Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, roamSpeed * Time.deltaTime);
                    moving = true;
                    yield return null;
                }

                // Flip direction and wait for a bit before moving again
                movingRight = !movingRight;
                spriteRenderer.flipX = movingRight ? false : true;

                moving = false;
                yield return new WaitForSeconds(roamDelay);
            }
            else
            {
                moving = false;
                yield return null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (animator != null)
            {
                animator.SetBool("moving", false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (willRoam)
            {
                StartCoroutine(Roam());
            }
        }
    }
}
