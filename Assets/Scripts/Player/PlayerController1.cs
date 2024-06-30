using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController1 : Singleton<PlayerController1>
{
    public bool FacingLeft { get { return facingLeft; } }
    public PlayerHealth stats;
    private Stamina stamina;

    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private KnockBack knockback;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;

    //Inventory
    [NonSerialized] public InventoryController inventory;
    [SerializeField] private InventoryUI inventoryUI;

    public LayerMask interactableLayer;
    //[SerializeField] private TextMeshPro interactText;
    [SerializeField] public TextMeshPro interactText;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
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
        inventoryUI.gameObject.SetActive(false);
        startingMoveSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    public void Update()
    {
        PlayerInput();
        playerControls.Inventory.OpenInv.performed += _ => OpenInventory();
        playerControls.Movement.Interact.performed += _ => Interact();

        AdjustPlayerFacingDirection();
        Move();

        // E to interact text
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

    private void FixedUpdate()
    {

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

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack) { return; }
        if (movement == Vector2.zero)
        {
            myAnimator.SetBool("isMoving", false);
        }
        else
        {
            myAnimator.SetBool("isMoving", true);
        }
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerSecreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerSecreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
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
        if(!isDashing && stamina.CurrentStamina > 0){
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
