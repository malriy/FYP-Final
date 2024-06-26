using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController1 : Singleton<PlayerController1>
{
    public bool FacingLeft { get { return facingLeft; } }
    public PlayerHealth stats;

    [SerializeField] private float moveSpeed = 1f;
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

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<KnockBack>();

        inventory = new InventoryController(UseItem);
        inventoryUI.SetInventory(inventory);
    }

    private void Start()
    {
        OpenInventory();
        playerControls.Combat.Dash.performed += _ => Dash();

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
                stats.currentHealth += 10;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                Debug.Log($"Health Pots: {inventory.GetItems()}");
                break;
            case Item.ItemType.ManaPotion:
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
        var facingDir = new Vector3(myAnimator.GetFloat("moveX"), myAnimator.GetFloat("moveY")).normalized;
        Debug.Log(facingDir);
        var interactPos = transform.position + facingDir * 1.5f;
        Debug.Log(interactPos);
        Debug.DrawLine(transform.position, interactPos, Color.blue, 5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.5f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    private void Dash()
    {
        if (!isDashing)
        {
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

}
