using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles the first scene that loads when the game opens which is the main menu
public class MainMenu : MonoBehaviour
{
    private LoadingScreen loading;
    [SerializeField] private Transform buttonsPanel = null, optionsPanel = null, levelSelection = null, loadingPanel = null;         //stores all the panels that get triggered when the buttons within the menu get presed

    //triggers function which disables all the panels beside the default one when game loads
    private void Start()
    {
        loading = this.gameObject.GetComponent<LoadingScreen>();
        loadingPanel.gameObject.SetActive(false);
        ReturnButton();
    }
    //Opens up the panel that allows the user to select a level 
    public void StartGame()
    {
        levelSelection.gameObject.SetActive(true);
    }

    //sets the selected level as player prefab so it can be loaded into the game scene, loads game scene and checks if that level is defualt or user made
    public void SetLevel(int levelId)
    {
        PlayerPrefs.SetInt("level", levelId);
        if (File.Exists(Application.persistentDataPath + "LevelData" + levelId + ".lvl"))
        {
            PlayerPrefs.SetInt("LevelData", 1);
        }
        else
        {
            PlayerPrefs.SetInt("LevelData", 0);
        }
        //opens up the loading sequence
        loadingPanel.gameObject.SetActive(true);
        loading.LoadButton();
    }

    //Opens up the map editor scene
    public void StartMapEditor()
    {
        SceneManager.LoadScene(2);
    }

    //used as return button to go back to default state of main menu
    public void ReturnButton()
    {
        levelSelection.gameObject.SetActive(false);
        optionsPanel.gameObject.SetActive(false);
        buttonsPanel.gameObject.SetActive(true);
    }

    //enables the options panel and disables the default state of main menu
    public void OptionsButton()
    {
        optionsPanel.gameObject.SetActive(true);
        buttonsPanel.gameObject.SetActive(false);
    }

    //quits the application
    public void QuitGame()
    {
        Application.Quit();
    }
}
