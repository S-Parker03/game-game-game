using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class InventorySlot : Button
{
    public Image Icon;
    public string ItemGuid = "";
     public string ItemName = "";
    public string ItemDescription = "";

    public InventorySlot()
    {
        //Create a new Image element and add it to the root
        Icon = new Image();
        Add(Icon);

        //Add USS style properties to the elements
        Icon.AddToClassList("slotIcon");
        AddToClassList("slotContainer");
        
        RegisterCallback<ClickEvent>(OnClick);
    }

    // Method to handle the click event
    private void OnClick(ClickEvent evt)
    {
        // Log the item name and ID to the console
        Debug.Log($"Clicked on item: {ItemName} (ID: {ItemGuid})");
        // Find the root visual element
        VisualElement m_Root = GameObject.Find("Inventory").GetComponent<UIDocument>().rootVisualElement;
        // Find the item description element
        VisualElement m_Description = m_Root.Q<VisualElement>("ItemDescription");
        // Set the text of the item description element to the item description
        m_Description.Q<Label>().text = ItemDescription;
        // Find the item name element
        VisualElement m_Name = m_Root.Q<VisualElement>("ItemName");
        // Set the text of the item name element to the item name
        m_Name.Q<Label>().text = ItemName;
        // Find the item image element
        VisualElement m_Icon = m_Root.Q<VisualElement>("ItemImage");
        // Set the background image of the item image element to the item sprite
        m_Icon.style.backgroundImage = new StyleBackground(Icon.sprite);

    }


    

#region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits> { }

    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
#endregion

}
