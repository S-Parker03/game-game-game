using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems; 

// STRUCTURE OF THE CODE
// DialogueManager is a singleton class that manages the dialogue system in the game
// It uses the Ink runtime to parse the JSON file and display the dialogue on the screen
// The StartDialogue method is called from the PlayerInteract script to start the dialogue
// The nextDialogue method is called to continue the dialogue - mouse click to continue
// The EndDialogue method is called to end the dialogue
// The DisplayChoices method is called to display the choices on the screen
// the game remains paused during the dialogue and resumes after the dialogue ends
public class DialogueManager : MonoBehaviour
{
    // Reference to the dialogue panel and text
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;

    // Reference to the current story - ink file
    private Story currentStory;

    private bool isdialogueActive;

    private GameObject basementPiece;


    // Reference to the choices UI
    [Header("Choices UI")]
    [SerializeField] private GameObject[] choiceList;

    private TextMeshProUGUI[] choiceTexts;
    public static DialogueManager instance;

    // Reference to the Pause script to resume the game after dialogue ends
    private Pause pauseScript;    

    // Awake method to initialise the singleton instance
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

 public static DialogueManager GetInstance() {
     return instance;
 }

 public void Start()
    {
        // Hide the dialogue panel at the start
        dialoguePanel.SetActive(false);
        isdialogueActive = false;

        // Get the TextMeshProUGUI component from each choice button
        choiceTexts = new TextMeshProUGUI[choiceList.Length];
        int index = 0;
        // Loop through each choice button and get the TextMeshProUGUI component
        foreach (GameObject choice in choiceList)
        {
            choiceTexts[index] = choice.GetComponentsInChildren<TextMeshProUGUI>()[0];
            index++;
        }
        // Get the Pause script reference from the same GameObject
        pauseScript = GetComponent<Pause>();
        if (pauseScript == null)
        {
            Debug.LogError("Pause script not found on the same GameObject.");
        }

        basementPiece = GameObject.Find("Basement");


    }


    public void Update()
    {
        if (!isdialogueActive)
        {
            return;
        }

        // Check for mouse button input to continue the dialogue
        if (Input.GetMouseButtonDown(0)) // 0 is for left mouse button
        {
            nextDialogue();
        }

    }

// function is called from the player interact script - to start the dialogue
    public void StartDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        isdialogueActive = true;
        dialoguePanel.SetActive(true);

        HideChoices(); // Hide the choice buttons at the start of the dialogue

        // call the next line of dialogue 
        nextDialogue();
    
    }

public void nextDialogue()
{
    if (currentStory.canContinue)
    {
        Debug.Log("Continuing story...");
        string nextLine = currentStory.Continue();
        Debug.Log("Next dialogue: " + nextLine);

        if (!string.IsNullOrEmpty(nextLine))
        {
            dialogueText.text = nextLine;
            dialogueText.ForceMeshUpdate(); // Ensure TextMeshPro updates
            // DisplayChoices();
        }
        else
        {
            Debug.LogWarning("Next line is empty!");
        }
        // Delay choice display until all lines are shown
        if (!currentStory.canContinue) 
        {
            DisplayChoices();
        }
    }
    else
    {
        Debug.Log("Ending dialogue...");
        EndDialogue();
    }
}


// function to end the dialogue
    public void EndDialogue()
    {
        // Hide the dialogue panel
        dialoguePanel.SetActive(false);
        isdialogueActive = false;
        dialogueText.text = "";

        // Call the ResumeDialogue function from the Pause script
        if (pauseScript != null)
        {
            pauseScript.resumeGame();
        }
        else
        {
            Debug.LogError("Pause script reference is null.");
        }

        // yellow king - make basement glass piece active after conversation
        // so player can pick it up.
        if (!basementPiece.activeSelf){
            basementPiece.SetActive(true);
        }
    }

// function to display the choices on the screen - as buttons 
    private void DisplayChoices()
    {
        List<Choice> choices = currentStory.currentChoices;

        if (choices.Count > choiceList.Length)
        {
            // to check if the number of choices is greater than the number of choice buttons - UI cannot handle
            Debug.LogError("there are more choices than the choice buttons");
            return;
        }   

        int index = 0;
        // Loop through each choice and display the text on the button
        foreach (Choice choice in choices)
        {
            choiceList[index].gameObject.SetActive(true); // Show the choice button
            choiceTexts[index].text = choice.text;
            index++;
        }

        // Hide the remaining choice buttons
        for (int i = index; i < choiceList.Length; i++)
        {
            choiceList[i].gameObject.SetActive(false);
        }
         StartCoroutine(SelectFirstChoice());

        // If there are no choices, continue the dialogue automatically
        // Automatically hide choice buttons if no choices are present
        if (choices.Count == 0)
        {
        foreach (GameObject choice in choiceList)
        {
            choice.SetActive(false);
            // nextDialogue();
        }
        nextDialogue(); // Continue the dialogue
    }

       
    }

    public void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log("Choice selected: " + choiceIndex);
        currentStory.ChooseChoiceIndex(choiceIndex);
        nextDialogue();
    }

    private IEnumerator SelectFirstChoice()
    {
       //this is the weird part of the unity event system
       //first have one selected and then wati for the end of the frame to select the actual choice
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choiceList[0].gameObject);
    }

// function to make a choice - called when the player clicks on a choice button
    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        Debug.Log("Choice selected: " + choiceIndex);
        // nextDialogue();
        foreach (GameObject choice in choiceList)
    {
        choice.SetActive(false);
    }

    // Continue the dialogue
    if (currentStory.canContinue)
    {
        dialogueText.text = currentStory.Continue(); // Update dialogue text with the next part
        // DisplayChoices(); // Check if there are more choices to display
    }
    else
    {
        Debug.Log("Ending dialogue...");
        EndDialogue(); // End dialogue if no more content
    }


    }

    private void HideChoices()
    {
        foreach (GameObject choice in choiceList)
        {
            choice.SetActive(false);
        }
    }
    

}
  





