using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public LayerMask interactableLayer;
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;

    public SwordAttack swordAttack;

    public float maxHealth = 100;
    public float curHealth = 100;
    public InventoryController inventory;
    [SerializeField] private InventoryUI inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = new InventoryController(UseItem);
        inventoryUI.SetInventory(inventory);
        inventoryUI.gameObject.SetActive(false);

        //ItemWorld.SpawnItemWorld(new Vector3(20, 20), new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
 
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                curHealth += 10f;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                Debug.Log($"Health Pots: {inventory.GetItems()}");
                break;
            case Item.ItemType.ManaPotion:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
                break;
        }
    }


    public void HandleUpdate()
    {
        // Player movement and sprite flipping

        if (canMove)
        {
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


            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }

        // Player Health and Stats
        if (curHealth >= maxHealth)
        {
            curHealth = maxHealth;
        }
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX") * 0.5f, animator.GetFloat("moveY") * 0.5f);
        var interactPos = transform.position + facingDir;

        Debug.DrawLine(transform.position, interactPos, Color.green, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.15f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
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
        animator.SetTrigger("swordAttack");
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

    public void SwordAttack()
    {
        LockMovement();

        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSword()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }
    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void PlayerDeath()
    {
        animator.SetTrigger("playerDeath");
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
