using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour, Interactable
{
    public string npcName;

    [SerializeField] public Dialog dialog;
    private Text nameText;

    [SerializeField] public bool willRoam = false; // Determines if the NPC should roam
    [SerializeField] private float roamDistance = 5f; // Distance the NPC can roam left and right
    [SerializeField] private float roamSpeed = 1f; // Speed of roaming
    [SerializeField] private float roamDelay = 2f; // Delay between roaming movements

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

    private void Awake()
    {
        nameText = GameObject.Find("NameText").GetComponent<Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //player = GameObject.Find("Player").GetComponent<PlayerController>();
        //player1 = GameObject.Find("Player").GetComponent<PlayerController1>();
    }

    private void Start()
    {
        startPosition = transform.position;
        leftPosition = startPosition + Vector3.left * roamDistance;
        rightPosition = startPosition + Vector3.right * roamDistance;

        if (willRoam)
        {
            StartCoroutine(Roam());
        }
    }

    private void Update()
    {
        if (animator != null)
        {
            
            animator.SetBool("moving", moving);
        }
    }
    private Coroutine talking;
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
        
        talking = StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
        GiftPlayer();
    }
    public bool moving;

    void GiftPlayer()
    {
        foreach (RewardItems rewardItems in gifts)
        {
            Item item = new Item { itemType = rewardItems.itemType, amount = rewardItems.amount };
            if (player != null)
            {
                player.inventory.AddItem(item);
            }
            if (player1 != null)
            {
                player1.inventory.AddItem(item);
            }
        }
    }

    private IEnumerator Roam()
    {
        while (willRoam)
        {
            // Set target position based on the current direction
            targetPosition = movingRight ? rightPosition : leftPosition;

            // Move towards the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
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
    }
}
