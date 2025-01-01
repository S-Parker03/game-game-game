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
        Debug.Log($"Clicked on item: {ItemName} (ID: {ItemGuid})");
        VisualElement m_Root = GameObject.Find("Inventory").GetComponent<UIDocument>().rootVisualElement;
        VisualElement m_Description = m_Root.Q<VisualElement>("ItemDescription");
        m_Description.Q<Label>().text = ItemDescription;
        VisualElement m_Name = m_Root.Q<VisualElement>("ItemName");
        m_Name.Q<Label>().text = ItemName;
        VisualElement m_Icon = m_Root.Q<VisualElement>("ItemImage");
        m_Icon.style.backgroundImage = new StyleBackground(Icon.sprite);

    }


    

#region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits> { }

    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
#endregion

}
