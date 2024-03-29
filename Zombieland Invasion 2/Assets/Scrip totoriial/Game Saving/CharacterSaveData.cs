using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// since we want to reference this data for every save file, this scipt is not monobehavior and is instead serialzable
public class CharacterSaveData 
{
    [Header("Character Name")]
    public string characterName;

    [Header("Time Played")]
    public float secondsPlayed;

    // qustion why noit use vector3?
    // answer: we can only save data from "basic" variables types (float, int, string, bool, ect)
    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;
}
