using System.Collections;
using UnityEngine;
using TMPro;

public class NPCInteractable : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Reference to the TextMeshProUGUI component for displaying dialogue
    public string[] lines; // Array to hold the lines of dialogue
    public float textSpeed; // Speed at which characters are typed out

    private int index = 0; // Index to keep track of the current dialogue line
    private bool isDialogueActive = false; // Flag to check if dialogue has started
    private bool isTyping = false; // Flag to check if typing is in progress

    void Start()
    {
        textComponent.text = string.Empty;
    }

    public void StartDialogue()
    {
        if (!isDialogueActive)
        {
            // Start dialogue if it hasn't started yet
            Debug.Log("Interact!"); // For debugging
            isDialogueActive = true;
            index = 0;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else if (!isTyping && index < lines.Length) 
        {
            // Continue to the next line if not typing and dialogue is active
            NextLine();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;

        // Type out each character of the current line
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c; // Append the character to the text component
            yield return new WaitForSeconds(textSpeed); // Wait for specified time before typing the next character
        }

        isTyping = false;
    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        textComponent.text = string.Empty;
        Debug.Log("Dialogue ended!");
    }
}
