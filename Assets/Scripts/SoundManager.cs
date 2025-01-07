using System.Collections;
using System.Collections.Generic;
// using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    // using the singleton - so there is now going to be only 1 sound manager in the entire game
    // public instance so other scripts can access it 
    public static SoundManager instance;

    public float volumeMultiplier = 1f;

    public GameObject player;

    // gameObject that has the audio clips (better than directly giving a audioSource to the sound manager)
    // for each sound effect, a game object is created and the audio clip is assgined to it
    // cannot keep a single gameobject because if sounds overlap, it will mess up everything - in this case
    // the adudioclip will not be null so it will not initiate the sound manager
    [SerializeField] private AudioSource SoundDoorOpenObject;
    [SerializeField] private AudioSource SoundDoorCloseObject;

    [SerializeField] private AudioSource SoundBeginningObject;

    [SerializeField] private AudioSource SoundSanityPickUpObject;

    [SerializeField] private AudioSource SoundItemPickUpObject;

    [SerializeField] private AudioSource SoundDamageObject;

    public AudioSource footsteps;

    // CODE STRUCTURE
    // Awake method - instalises the soundmanager instance, if it is null
    // Doop Open method accepts the audioclip, transform of the gameobject(reset rn) and volume (rn 1f)
    // the method creates a new instance of the sound object at the transform
    // the method at last assigns volume and plays the sound and destroys this game object that was just created
    // destroying the object is important other clone objects are created (more doors, more clone)


    // intitalises the soundmanager instance 
    private void Awake() {
        
        if (instance == null)
        {
            instance = this;
            Debug.Log("SoundManager instance intiated");
        }
        player = GameObject.FindGameObjectWithTag("Player");

        

        
    }

    void Update()
    {
        SettingsManager settings = GameObject.Find("Settings").GetComponent<SettingsManager>();
        // testing the sound manager
        volumeMultiplier = settings.volumePercent / 100.0f;
        footsteps.volume = 1f * volumeMultiplier;    
    }

    // function to play the sound of the door opening
    public void PlayDoorOpenClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // testing null exception for audioclip
         if (audioClip == null)
    {
        Debug.LogError("AudioClip is not there in PlaySoundClip!");
        return;
    }
    // testing null exception for audioclip
    if (spawnTransform == null)
    {
        Debug.LogError("spawnTransform is not there in PlaySoundClip!");
        return;
    }
        // spawn a gameobject
        AudioSource audioSource = Instantiate(SoundDoorOpenObject, spawnTransform.position,Quaternion.identity);
        
        // assign the audioclip to the game object
        audioSource.clip = audioClip;

        // assign volume to it
        audioSource.volume = volume*volumeMultiplier;

        // play sound
        audioSource.Play();

        // get length of the sound to know when to destroy the object 
        float clipLength =audioSource.clip.length;

        // destroy the clip after it is done playing 
        // otherwise it creates clones of the same gameobject
        // destroy the object after the clip ends playing (cliplength)
        Destroy(audioSource.gameObject, clipLength);
    }    



    // function for sound of door closing - same function needed for all the sound effects
       public void PlayDoorCloseClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(SoundDoorCloseObject, spawnTransform.position,Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume*volumeMultiplier;
        audioSource.Play();
        float clipLength =audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }    
 

     public void PlaySanityPickUpClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(SoundSanityPickUpObject, spawnTransform.position,Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume*volumeMultiplier;
        audioSource.Play();
        float clipLength =audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }    

    public void PlayItemPickUpClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(SoundItemPickUpObject, spawnTransform.position,Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume*volumeMultiplier;
        audioSource.Play();
        float clipLength =audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }    

    public void PlayDamageClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(SoundDamageObject, spawnTransform.position,Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume*volumeMultiplier;
        audioSource.Play();
        float clipLength =audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }    
}
