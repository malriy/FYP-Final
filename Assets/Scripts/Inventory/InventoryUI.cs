using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    private InventoryController inventory;
    private Transform invContainer;
    private Transform invSlot;
    private Player player;
    private PlayerController1 player1;


    private void Awake()
    {
        invContainer = transform.Find("InvContainer");
        invSlot = invContainer.Find("InvSlot");
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }
    public void SetPlayer(PlayerController1 player)
    {
        this.player1 = player;
    }


    public void SetInventory(InventoryController inventory)
    {
        if (this.inventory != null)
        {
            InventoryController.OnItemAdded -= Inventory_OnItemListChanged;
        }

        this.inventory = inventory;
        InventoryController.OnItemAdded += Inventory_OnItemListChanged;
        RefreshInvItems();
    }
    private void OnDestroy()
    {
        // Unsubscribe from the event when this object is destroyed
        if (inventory != null)
        {
            InventoryController.OnItemAdded -= Inventory_OnItemListChanged;
        }
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInvItems();
    }

    private void RefreshInvItems()
    {
        if (invContainer == null || invSlot == null) return;

        foreach (Transform child in invContainer)
        {
            if (child == invSlot) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float invSlotCellSize = 140f;
        foreach (Item item in inventory.GetItems())
        {
            RectTransform invSlotRectTransform = Instantiate(invSlot, invContainer).GetComponent<RectTransform>(); 
            invSlotRectTransform.gameObject.SetActive(true);

            EventTrigger eventTrigger = invSlotRectTransform.gameObject.AddComponent<EventTrigger>();

            // Create a new event entry for PointerClick
            EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
            pointerClickEntry.eventID = EventTriggerType.PointerClick;
            pointerClickEntry.callback.AddListener((eventData) => { HandleClick((PointerEventData)eventData, item); });

            // Add the event entry to the EventTrigger
            eventTrigger.triggers.Add(pointerClickEntry);

            invSlotRectTransform.anchoredPosition = new Vector2(x * invSlotCellSize, y * invSlotCellSize);
            Image image = invSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            Text amountText = invSlotRectTransform.Find("Amount").GetComponent<Text>();
            if (item.amount > 1)
            {
                amountText.text = item.amount.ToString();
            }
            else
            {
                amountText.text = "";
            }

            x++;
            if (x > 7)
            {
                x = 0;
                y--;
            }
        }
    }

    public void HandleClick(PointerEventData eventData, Item item)
    {
        // Check if left or right click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UseItem(item);
        }
    }

    public void UseItem(Item item)
    {
        inventory.UseItem(item);
    }

    public InventoryController GetInventoryController()
    {
        return inventory;
    }
}
