using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//Handles saving of all the data within the project
public static class SaveLoadManager
{
    private static string levelData = "LevelData";                  //name to the path of level saving
    private static string userSaveName = "UserData.das";            //name of the path for saving user data
    private static string extension = ".lvl";                       //extension for saving levels
    private static int saveNum = 0; //0 = level 1, 1 = level 2, 2 = level 3 and 3 = level 4


    #region Level Related
    //sets number for which the level file will be saved/generated
    public static void SetLevelNumber(int levelNum)
    {
        saveNum = levelNum;
    }

    //Responsible for saving each specific level based on set level number function
    public static void SaveLevelData(GameEditor data)
    {
        BinaryFormatter binaryFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + levelData + saveNum + extension, FileMode.Create);
        LevelData newData = new LevelData(data);
        binaryFormat.Serialize(fileStream, newData);
        fileStream.Close();
    }

    //Deleting the save file of the level based on assigned leve number
    public static void DeleteLevel()
    {
        File.Delete(Application.persistentDataPath + levelData + saveNum + extension);
    }

    #endregion

    #region Saving Player data
    //saving of the data for the player settings like sound or resolution settings
    public static void SaveData(SaveDataManager data)
    {
        BinaryFormatter binaryFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + userSaveName, FileMode.Create);
        GameData newData = new GameData(data);
        binaryFormat.Serialize(fileStream, newData);
        fileStream.Close();

    }

    //Loads the save file
    public static int[][] LoadLevelData()
    {
        if (File.Exists(Application.persistentDataPath + levelData + saveNum + extension))
        {
            BinaryFormatter binaryFormat = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + levelData + saveNum + extension, FileMode.Open);
            LevelData data = binaryFormat.Deserialize(fileStream) as LevelData;
            fileStream.Close();
            return data.allLevelData;
        }
        else
        {
            Debug.LogError("File does not exist");
            return null;
        }
    }




    //Deleting the save file which contains all user data like sounds and resolution settings
    public static void DeleteSave()
    {
        File.Delete(Application.persistentDataPath + userSaveName);
    }

    //Loads the save file
    public static float[] LoadUserData()
    {
        if (File.Exists(Application.persistentDataPath + userSaveName))
        {
            BinaryFormatter binaryFormat = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + userSaveName, FileMode.Open);
            GameData data = binaryFormat.Deserialize(fileStream) as GameData;
            fileStream.Close();
            return data.allUserData;
        }
        else
        {
            Debug.LogError("File does not exist");
            return null;
        }
    }
    #endregion
}

//Class that stores and grabs all the data that will be saved using the save function
[System.Serializable]
public class GameData
{
    public float[] allUserData;             //contains all user information

    public GameData(SaveDataManager data)
    {
        allUserData = new float[8];

        allUserData[0] = data.Master;
        allUserData[1] = data.Sound;
        allUserData[2] = data.Music;
        allUserData[3] = data.ResolutionIndex;
        allUserData[4] = data.IsFullScreen;
        allUserData[5] = data.DefaultMoney;
        allUserData[6] = data.Difficulties;
        allUserData[7] = data.UsingFreePlacement;
    }


}

//Class that stores and grabs all the level data that will be saved using the save function
[System.Serializable]
public class LevelData
{
    public int[][] allLevelData;            //stores all level information     
    public int[] paths;                     //stores the path of the level
    public int[] blocks;                    //stores the blocks within the level and their type
    public int[] sizeData;                  //stores all other values within the level like height and width
    public LevelData(GameEditor data)
    {
        allLevelData = new int[3][];
        sizeData = new int[2];
        sizeData[0] = data.SavedWidth;
        sizeData[1] = data.SavedHeight;
        paths = data.NumOfPath.ToArray();
        blocks = data.BlockIds.ToArray();
        allLevelData[0] = sizeData;
        allLevelData[1] = blocks;
        allLevelData[2] = paths;
    }


}
