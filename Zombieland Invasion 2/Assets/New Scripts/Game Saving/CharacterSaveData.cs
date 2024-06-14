using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// since we want to reference this data for every save file, this scipt is not monobehavior and is instead serialzable
public class CharacterSaveData 
{
    [Header("Scene Index")]
    public int sceneIndex = 1;
    [Header("Character Name")]
    public string characterName = "Character";

    [Header("Time Played")]
    public float secondsPlayed;


    
    // answer: we can only save data from "basic" variables types (float, int, string, bool, ect)
    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Resources")]
    public int currentHealth;
    public float currentStamina;

    [Header("Stats")]
    public int vitality;
    public int endurance;

    [Header("Sites Of Grace")]
    public SerializableDictionary<int, bool> sitesOfGrace; // the int is the site of grace id the bool is the activated status


    [Header("Bosses")]
    public SerializableDictionary<int, bool> bossesAwakened; // the int is the boss id the bool is the awakened status 
    public SerializableDictionary<int, bool> bossesDefeated; // the int is the boss id the bool is the defeated status 

    public CharacterSaveData() 
    { 
        sitesOfGrace = new SerializableDictionary<int, bool>();
        bossesAwakened = new SerializableDictionary<int, bool>();
        bossesDefeated = new SerializableDictionary<int, bool>();

    }


}
