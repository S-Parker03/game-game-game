using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIDialogues : MonoBehaviour
{
    public UIDocument DialogueBox; // Reference to the UI Document component
    [SerializeField] public string[] lines;        // Array to hold the lines of dialogue
    public float textSpeed = 0.05f; // Speed at which characters are typed out

    private Label DialogueLabel;  // Reference to the Label for displaying dialogues
    private int index = 0;        // Index to keep track of the current dialogue line
    private bool isDialogueActive = false; // Flag to check if dialogue has started
    private bool isTyping = false; // Flag to check if typing is in progress

    private Pause pauseScript;
    private VisualElement root;

    private GameObject playerObj;

    void Start()
    {
        var root = DialogueBox.rootVisualElement;
    
    if (root == null)
    {
        Debug.LogError("Root VisualElement is null. Make sure the UIDocument is properly assigned.");
        return;
    }

    // Find the Label by its name (ensure the name in the UI Builder is correct)
    DialogueLabel = root.Q<Label>("DialogueLabel");

    if (DialogueLabel == null)
    {
        Debug.LogError("DialogueLabel not found. Check the UI Builder for the correct name.");
    }
    else
    {
        DialogueLabel.text = string.Empty; // Ensure the label starts empty
        Debug.Log("Both root Visual element and rootvisual element were found");
    }

    playerObj = GameObject.FindGameObjectWithTag("Player");

    // Pause pauseScript = FindObjectOfType<Pause>(); 
    pauseScript = GetComponent<Pause>(); // Get the Pause script attached to the same GameObject
        if (pauseScript == null)
        {
            Debug.LogError("Pause script not found in the scene.");
        }
        else
        {
            Debug.Log("Pause Script is found");
        }
    }

    public void StartDialogue()
    {
        
        if (!isDialogueActive)
        {
            // Start dialogue if it hasn't started yet
            Debug.Log("Interact!"); // For debugging
            isDialogueActive = true;
            index = 0;
            DialogueLabel.text = string.Empty;
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
        DialogueLabel.text = string.Empty;

        // Type out each character of the current line
        foreach (char c in lines[index].ToCharArray())
        {
            DialogueLabel.text += c; // Append the character to the Label text
            yield return new WaitForSecondsRealtime(textSpeed); // Wait for specified time before typing the next character
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
        DialogueLabel.text = string.Empty;
        Debug.Log("Dialogue ended!");
        pauseScript.resumeGame();
    }
}