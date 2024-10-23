
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteractable : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Reference to the TextMeshProUGUI component for displaying dialogue
    public string[] lines; // Array to hold the lines of dialogue
    public float textSpeed; // Speed at which characters are typed out

    public int index = 0;  // Index to keep track of the current dialogue line
    
    void Start()
    {
        textComponent.text = string.Empty;
        // StartDialogue();
    }

   
    public void Update()
    { //// Check for mouse button click (left mouse button)
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine(); // Go to the next line of dialogue
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
            
        }
    }

    public void StartDialogue()
    {
        Debug.Log("Interact!"); //for debugging
        index = 0;
        StartCoroutine(TypeLine()); // Start the typing coroutine

    }

    IEnumerator TypeLine()
    { // Type out each character of the current line
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;  // Append the character to the text component
            yield return new WaitForSeconds(textSpeed); // Wait for  specified time before typing the next character
            
        }
    }

    public void NextLine()
    { // Check if there are more lines to display
        if (index < lines.Length -1)
        {
            index++;
            textComponent.text = string.Empty; // Clear the text for the new line
            StartCoroutine(TypeLine()); // Start typing the next line
        }
        else
        {
            gameObject.SetActive(false); // Deactivate the NPC object if there are no more lines
        }
    }
}

