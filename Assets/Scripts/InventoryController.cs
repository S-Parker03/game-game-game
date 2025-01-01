using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using ItemType = ItemInfo.ItemType;

public class InventoryController : MonoBehaviour
{
    public List<InventorySlot> InventoryItems = new List<InventorySlot>();
    public Inventory Inventory;

    private VisualElement m_Root;
    private VisualElement m_SlotContainer;

    private void OnGUI()
    {
        // Ensure the inventory object exists
        GameObject inventoryObject = GameObject.Find("Inventory");
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (inventoryObject != null)
        {
            // Ensure the inventory object has an Inventory component
            Inventory = playerObj.GetComponent<Inventory>();
            if (Inventory == null)
            {
                Debug.LogError("Player object does not have an Inventory component.");
                return;
            }
        }
        else
        {
            Debug.LogError("Inventory object not found.");
            return;
        }

        // Ensure the UIDocument component exists
        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument != null)
        {
            m_Root = uiDocument.rootVisualElement;
            if (m_Root == null)
            {
                Debug.LogError("Root Visual Element is null.");
                return;
            }

            // Ensure the SlotContainer exists in the UI hierarchy
            m_SlotContainer = m_Root.Q<VisualElement>("SlotContainer");
            if (m_SlotContainer == null)
            {
                Debug.LogError("SlotContainer not found in the UI hierarchy.");
                return;
            }
        }
        else
        {
            Debug.LogError("UIDocument component not found.");
            return;
        }

        DisplayItems(ItemType.Key);
    }

    public void DisplayItems(ItemType page)
    {
        // Ensure m_SlotContainer is not null
        if (m_SlotContainer == null)
        {
            Debug.LogError("SlotContainer is null.");
            return;
        }

        // Clear the SlotContainer
        m_SlotContainer.Clear();

        // Create InventorySlots and add them as children to the SlotContainer
        if (page == ItemType.Key)
        {
            foreach (ItemInfo item in Inventory.KeyItems)
            {
                Debug.Log("Displaying item " + item.itemName);
                InventorySlot slot = new InventorySlot();
                slot.ItemGuid = item.itemID;
                slot.ItemName = item.itemName;
                slot.ItemDescription = item.itemDescription;
                slot.Icon.sprite = item.itemImage;
                InventoryItems.Add(slot);
                m_SlotContainer.Add(slot);
            }
        }
        else if (page == ItemType.Lore)
        {
            foreach (ItemInfo item in Inventory.LoreItems)
            {
                Debug.Log("Displaying item " + item.itemName);
                InventorySlot slot = new InventorySlot();
                slot.ItemGuid = item.itemID;
                slot.ItemName = item.itemName;
                slot.ItemDescription = item.itemDescription;
                slot.Icon.sprite = item.itemImage;
                InventoryItems.Add(slot);
                m_SlotContainer.Add(slot);
            }
        }
    }
}
