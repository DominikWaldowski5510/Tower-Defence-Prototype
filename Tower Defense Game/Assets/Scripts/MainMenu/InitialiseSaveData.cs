using System.IO;
using UnityEngine;

//Initialises and handles default data saving and generating files
public class InitialiseSaveData : MonoBehaviour
{
    private SaveDataManager dataManager;                    //references the game data holding script
    private const int defaultMoney = 200;                   //default start up money
    //loads a save file if it exists if not it generates a new save file
    private void Start()
    {
        dataManager = GameObject.Find("SaveLoadManager").GetComponent<SaveDataManager>();
        //Checks if any user settings exist if not generate new file from scratch
        if (File.Exists(Application.persistentDataPath + "UserData.das"))
        {
            //load existing data
            dataManager.Load();
        }
        else
        {
            // create new set of data
            NewGameSave();
        }
    }

    //creates new game save
    private void NewGameSave()
    {
        //audio settings
        dataManager.SetSound(0);
        dataManager.SetMusic(0);
        dataManager.SetMaster(0);
        //visual settings
        dataManager.SetScreenOptions(1, 0);
        //gameplay settings
        dataManager.SetTowerPlacementType(true);
        dataManager.SetDifficulty(2);
        dataManager.SetDefaultMoney(defaultMoney);
        //saving process
        dataManager.Save();
    }
}
