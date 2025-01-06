using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems; 

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
    private Story currentStory;

    private bool isdialogueActive;

    [Header("Choices UI")]

    [SerializeField] private GameObject[] choiceList;

    private TextMeshProUGUI[] choiceTexts;
    public static DialogueManager instance;

    private Pause pauseScript;    

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
        dialoguePanel.SetActive(false);
        isdialogueActive = false;

        choiceTexts = new TextMeshProUGUI[choiceList.Length];
        int index = 0;
        foreach (GameObject choice in choiceList)
        {
            choiceTexts[index] = choice.GetComponentsInChildren<TextMeshProUGUI>()[0];
            index++;
        }

        pauseScript = GetComponent<Pause>();
        if (pauseScript == null)
        {
            Debug.LogError("Pause script not found on the same GameObject.");
        }


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

    public void StartDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        isdialogueActive = true;
        dialoguePanel.SetActive(true);
        
        nextDialogue();
    
    }


public void nextDialogue(){
    if (currentStory.canContinue)
        {
             Debug.Log("Continuing story...");
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }

        else
        {
            Debug.Log("Ending dialogue...");
            EndDialogue();
        }
        
}
    public void EndDialogue()
    {
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

    }

    private void DisplayChoices()
    {
        List<Choice> choices = currentStory.currentChoices;

        if (choices.Count > choiceList.Length)
{
    Debug.LogError("there are more choices than the choice buttons");
    return;
}

        int index = 0;
        foreach (Choice choice in choices)
        {
            choiceList[index].gameObject.SetActive(true);
            choiceTexts[index].text = choice.text;
            index++;
        }

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
    

}
  