using System;
using UnityEngine;
using UnityEngine.UI;

//stores all gameplay changing settings
public class GameplaySettings : MonoBehaviour
{
    [Header("User Difficulty")]
    private int startMoney = 0;                                            //money that user will get when game starts
    [SerializeField] private Dropdown difficultyDropdown = null;                  //dropdown object that stores all difficulties
    [SerializeField] private Toggle freePlacementToggle = null;                    //toggle that stores user input for placement
    [SerializeField] private InputField startMoneyInputField = null;                   //input field that allows user to enter amount of starting money in round
    private enum Difficulties
    {
        VeryEasy,               //0.5 health from normal
        Easy,                   //0.75 health from normal
        Normal,                 //default 1 ratio of health
        Hard,                   //1.25 health from normal
        VeryHard                //1.5 health from normal
    }
    private Difficulties setDifficulty = Difficulties.Normal;                                     //enum that corresponds to the difficulty dropdown
    private bool usingFreePlacement = true;                                 //used for determining whenever free placement or square placement is used


    //sets default values
    private void Start()
    {

        startMoney = (int)SaveDataManager.instance.DefaultMoney;
        int diffValue = (int)SaveDataManager.instance.Difficulties;
        usingFreePlacement = Convert.ToBoolean(SaveDataManager.instance.UsingFreePlacement);
        setDifficulty = (Difficulties)diffValue;
        difficultyDropdown.value = (int)setDifficulty;
        freePlacementToggle.isOn = usingFreePlacement;
    }

    //sets up the placement value to be same as the toggle
    public void SelectPlacementType()
    {
        usingFreePlacement = freePlacementToggle.isOn;
    }

    //sets up difficulty from the dropdown menu
    public void SelectDropDownDifficulty()
    {
        setDifficulty = (Difficulties)difficultyDropdown.value;
    }

    //saves default money set by the game at round start
    public void SetMoney()
    {
        startMoney = Convert.ToInt32(startMoneyInputField.text);
    }


    //confirms gameplay settings and saves them to a file
    public void ConfirmGameplaySettings()
    {
        SaveDataManager.instance.SetDefaultMoney(startMoney);
        SaveDataManager.instance.SetDifficulty((int)setDifficulty);
        SaveDataManager.instance.SetTowerPlacementType(usingFreePlacement);
        SaveDataManager.instance.Save();
    }
}
