using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using ItemType = ItemInfo.ItemType;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour
{
    public List<InventorySlot> InventoryItems = new List<InventorySlot>();
    public Inventory Inventory;

    private VisualElement m_Root;

    private VisualElement m_SlotContainer;

    public ItemType currentPage = ItemType.Key;
    
    public bool guiNeedsUpdating = true;

    private void Awake(){
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        
    }

    private void OnGUI()
    {
        
        if (guiNeedsUpdating)
        {
            // Ensure the inventory object exists
            GameObject inventoryObject = GameObject.Find("Inventory");
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            GameObject settingsObj = GameObject.Find("Settings");
            GameObject menuObj = GameObject.Find("MainMenu");
            
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

            Button keyItemsButton = m_Root.Q<Button>("KeyItemsButton");
            Button loreItemsButton = m_Root.Q<Button>("LoreItemsButton");
            Button closeButton = m_Root.Q<Button>("CloseButton");
            Button settingsButton = m_Root.Q<Button>("SettingsButton");
            Button menuButton = m_Root.Q<Button>("QuitButton");
            keyItemsButton.RegisterCallback<ClickEvent>(OnNavButtonClick);
            loreItemsButton.RegisterCallback<ClickEvent>(OnNavButtonClick);
            closeButton.RegisterCallback<ClickEvent>(evt => {
                Pause pause = playerObj.GetComponent<Pause>();
                pause.resumeGame();
                inventoryObject.SetActive(false);
            
                pause.paused = false;
            });
            settingsButton.RegisterCallback<ClickEvent>(evt => {
                Debug.Log("settings clicked");
                settingsObj.GetComponent<SettingsManager>().previousMenu =  "Inventory";
                settingsObj.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.Flex;
                inventoryObject.SetActive(false);
            });
            menuButton.RegisterCallback<ClickEvent>(evt => {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
            

            if(currentPage == ItemType.Key){
                loreItemsButton.style.backgroundColor = new StyleColor(new Color(125f / 255f, 0f, 0f));
                keyItemsButton.style.backgroundColor = new StyleColor(new Color(72f / 255f, 17f / 255f, 17f / 255f));
                keyItemsButton.style.borderBottomColor = new StyleColor(new Color(72f / 255f, 17f / 255f, 17f / 255f));
                loreItemsButton.style.borderBottomColor = new StyleColor(new Color(125f / 255f, 0f, 0f));
                
            } else if (currentPage == ItemType.Lore){
                loreItemsButton.style.backgroundColor = new StyleColor(new Color(72f / 255f, 17f / 255f, 17f / 255f));
                keyItemsButton.style.backgroundColor = new StyleColor(new Color(125f / 255f, 0f, 0f));
                loreItemsButton.style.borderBottomColor = new StyleColor(new Color(72f / 255f, 17f / 255f, 17f / 255f));
                keyItemsButton.style.borderBottomColor = new StyleColor(new Color(125f / 255f, 0f, 0f));
                
            }

            DisplayItems(currentPage);
            guiNeedsUpdating = false;
        }
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

    private void OnNavButtonClick(ClickEvent evt){
        Button button = (Button)evt.target;
        Button otherButton = m_Root.Q<Button>(button.name == "KeyItemsButton" ? "LoreItemsButton" : "KeyItemsButton");
        string buttonName = button.name;
        Debug.Log("Button clicked: " + buttonName);
        if (buttonName == "KeyItemsButton")
        {
            currentPage = ItemType.Key;
            
            guiNeedsUpdating = true;
            
        }
        else if (buttonName == "LoreItemsButton")
        {
            currentPage = ItemType.Lore;
            guiNeedsUpdating = true;
        }
    }
}
