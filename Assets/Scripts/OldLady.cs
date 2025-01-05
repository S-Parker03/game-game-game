using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLady : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON; // Reference to the specific Ink JSON file for this NPC

    public TextAsset GetInkJSON()
    {
        return inkJSON;
    }
}
