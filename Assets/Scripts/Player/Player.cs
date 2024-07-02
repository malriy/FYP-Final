using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } }
    public PlayerHealth1 stats;
    private Stamina stamina;
    public static bool isDead = false;

    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private KnockBack knockback;
    private float startingMoveSpeed;
    bool canMove = true;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    private bool facingLeft = false;
    private bool isDashing = false;

    //Inventory
    [NonSerialized] public InventoryController inventory;
    [SerializeField] private InventoryUI inventoryUI;

    public LayerMask interactableLayer;
    //[SerializeField] private TextMeshPro interactText;
    [SerializeField] public TextMeshPro interactText;
    [SerializeField] private QuestUI questUI;

    protected void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<KnockBack>();
        stamina = GetComponent<Stamina>();

        inventory = new InventoryController(UseItem);
        inventoryUI.SetInventory(inventory);
        inventoryUI.SetPlayer(this);
    }

    private void Start()
    {
        OpenInventory();
        playerControls.Combat.Dash.performed += _ => Dash();
        interactText.gameObject.SetActive(false);
        questUI.gameObject.SetActive(false);
        startingMoveSpeed = moveSpeed;
        
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            stats.TakeDamage(stats.maxHealth, this.transform);
        }

        if (canMove && !isDead)
        {
            AdjustPlayerFacingDirection();
            if (movementInput != Vector2.zero)
            {
                animator.SetFloat("moveX", movementInput.x);
                animator.SetFloat("moveY", movementInput.y);
                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }

        float detectionRadius = 1.5f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, interactableLayer);
        if (colliders.Length > 0)
        {
            interactText.gameObject.SetActive(true);
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                stats.HealPlayer();
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;
            case Item.ItemType.ManaPotion:
                stamina.RefreshStamina();
                inventory.RemoveItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
                break;
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (movementInput != Vector2.zero)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
    }

    void OnInteract()
    {
        Interact();
    }
    void OnNextDialogue()
    {
        TriggerEvent.Trigger();
        Debug.Log("Dialogging");
    }

    void OnOpenInv()
    {
        if (inventoryUI.gameObject.activeInHierarchy)
        {
            inventoryUI.gameObject.SetActive(false);
            Time.timeScale = 1f;

        }
        else
        {
            inventoryUI.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void OnQuest()
    {
        if (questUI.gameObject.activeInHierarchy)
        {
            questUI.gameObject.SetActive(false);
            Time.timeScale = 1f;

        }
        else
        {
            questUI.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerSecreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerSecreenPoint.x)
        {
            spriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void OpenInventory()
    {
        if (inventoryUI.gameObject.activeInHierarchy)
        {
            inventoryUI.gameObject.SetActive(false);
            Time.timeScale = 1f;

        }
        else
        {
            inventoryUI.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void Interact()
    {
        float detectionRadius = 1.5f;

        // Detect colliders in the specified radius around the player
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, interactableLayer);

        if (colliders.Length > 0)
        {
            Collider2D closestCollider = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider2D collider in colliders)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }

            // Interact with the closest collider
            closestCollider?.GetComponent<Interactable>()?.Interact();
        }
    }

    private void Dash()
    {
        if (!isDashing && stamina.CurrentStamina > 0 && !DialogueManager.Instance.isDialogActive)
        {
            stamina.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.gameObject.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }
}
