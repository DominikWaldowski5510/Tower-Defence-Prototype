using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//handles gathering and saving user data throughout scenes
public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager instance;                 //instance of this object which allows us to reference it from different classes easier
    private float music;                                   //value of the music within the project
    private float sound;                                  //value of the sound within the project
    private float master;                                 //value of the master within the project
    private float isFullScreen;                           //value of the full screen or window mode
    private float resolutionIndex;                       //value of the index which corresponds to specific screen resolution
    private float defaultMoney;                          //start up money user receives when game starts
    private float usingFreePlacement;                     //free or restricted tower placement mode
    private float difficulties = 2;

    //sets the instance of the object and deletes any other ones if theres more than 1 present
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }

    //sets the volume of the music
    public void SetMusic(float musicVolume)
    {
        music = musicVolume;
    }

    //Sets up the value of start money in the game
    public void SetDefaultMoney(int money)
    {
        defaultMoney = money;
    }

    //Set up the tower placement type between free and restricted
    public void SetTowerPlacementType(bool placementMode)
    {
        usingFreePlacement = Convert.ToInt32(placementMode);
    }

    //sets difficulty of the enemies
    public void SetDifficulty(int difficultyNumber)
    {
        //difficulty is from 0 to 4
        difficulties = difficultyNumber;
    }

    //sets the sound effects of the game
    public void SetSound(float soundVolume)
    {
        sound = soundVolume;
    }

    //sets the master sound of the game
    public void SetMaster(float masterVolume)
    {
        master = masterVolume;
    }

    //sets the settings for the screen size and resolution
    public void SetScreenOptions(float _fullScreenIndex, float _resIndex)
    {
        isFullScreen = _fullScreenIndex;
        resolutionIndex = _resIndex;
    }

    //Saves all the user data
    public void Save()
    {
        SaveLoadManager.SaveData(this);
    }

    //Loads the user data
    public void Load()
    {
        float[] loadedData = SaveLoadManager.LoadUserData();
        //manually assign the data to their correct spot
        try
        {
            SetMaster(loadedData[0]);
            SetSound(loadedData[1]);
            SetMusic(loadedData[2]);
            SetScreenOptions(loadedData[4], loadedData[3]);
            SetDefaultMoney((int)loadedData[5]);
            SetDifficulty((int)loadedData[6]);
            bool placementTyp = Convert.ToBoolean(loadedData[7]);
            SetTowerPlacementType(placementTyp);
        }
        catch
        {
            Debug.LogWarning("Data was not loaded successfully");
        }
    }

    #region Accessors
    public float Music { get => music; set => music = value; }
    public float Sound { get => sound; set => sound = value; }
    public float Master { get => master; set => master = value; }
    public float IsFullScreen { get => isFullScreen; set => isFullScreen = value; }
    public float ResolutionIndex { get => resolutionIndex; set => resolutionIndex = value; }
    public float UsingFreePlacement { get => usingFreePlacement; set => usingFreePlacement = value; }
    public float DefaultMoney { get => defaultMoney; set => defaultMoney = value; }
    public float Difficulties { get => difficulties; set => difficulties = value; }

    #endregion
}
