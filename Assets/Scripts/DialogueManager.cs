using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

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

    }


    public void Update()
    {
        if (!isdialogueActive)
        {
            return;
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
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }

        else
        {
            EndDialogue();
        }
        
}
    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isdialogueActive = false;
        dialogueText.text = "";

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
    }

}
  