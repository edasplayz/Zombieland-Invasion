using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileDataWriter 
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    // before we cheate a new save file we must check to see if one of this character slot already exist (max 10 character slots)
    public bool CheckToSeeIfFileExist()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else 
        { 
            return false; 
        }
    }

    // used to delete character save files 
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    // used to create a save file upn starting a new game
    public void CreateNewCharaterSaveFile(CharacterSaveData characterData)
    {
        // make a path to the save file (a location on the machine)
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            // create the directory the file will be writen to, if it does not alreaddy exist
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("CREATING SAVE FILE, AT SAVE PATH:" + savePath);

            // serialze the c# game object into json
            string dataToStore = JsonUtility.ToJson(characterData, true);

            // write the file to our system
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream)) 
                { 
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("error while trying to save character data, game not saved" + savePath + "\n" + ex);
        }
    }

    // used to load a save file upon loading a previos game
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        // make a path to load file (a location on the machine)
        string LoadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if(File.Exists(LoadPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(LoadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deseliaze the data from json back to unity
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.Log("File is blank");
            }

        }

        return characterData;
        
    }
}
