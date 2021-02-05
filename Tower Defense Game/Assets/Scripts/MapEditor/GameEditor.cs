using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Handles user inputs, saving and deleting of the level as well as stores all values for level saving
public class GameEditor : MonoBehaviour
{
    private int pathNum = 0;            //keeps track of the total paths
    [SerializeField] private List<int> numOfPath = new List<int>();      //Stores all the ids of the path
    private MapEditorGenerator generator;
    [SerializeField] private Dropdown levelDropdown = null;             //stores dropdown that allows the user to select the level being worked on
    [Header("Docking Options")]
    [SerializeField] private Transform dockingPanel = null;             //panel that stores all editor tools
    [SerializeField] private Transform dockingButton = null;            //button that appears when editor panel is invisible
    [Header("Level Settings")]
    private int savedHeight, savedWidth;                                //the height and width that gets saved into the save system
    List<int> blockIds = new List<int>();
    private int currentlySelectedLevel = 0;         //id for currently active levle
    [SerializeField] private Text selectedBlockText = null;     //selected block text
    [SerializeField] private Text pathPointsMsg = null;         //message that gets displayed if not enough path points exist within the level
    private string[] selectedNames;                 //list of preset messages
    private int currentlySelectedId = 9999;         //stores id of selected block
    public int CurrentlySelectedLevel { get => currentlySelectedLevel; }
    public List<int> NumOfPath { get => numOfPath; set => numOfPath = value; }
    public int PathNum { get => pathNum; set => pathNum = value; }
    public MapEditorGenerator Generator { get => generator; set => generator = value; }
    public List<int> BlockIds { get => blockIds; set => blockIds = value; }
    public int SavedWidth { get => savedWidth; set => savedWidth = value; }
    public int SavedHeight { get => savedHeight; set => savedHeight = value; }

    //sets up default values of panels and names and level properties
    private void Start()
    {
        pathPointsMsg.gameObject.SetActive(false);
        generator = this.gameObject.GetComponent<MapEditorGenerator>();
        dockingPanel.gameObject.SetActive(true);
        dockingButton.gameObject.SetActive(false);
        currentlySelectedLevel = 0;
        selectedNames = new string[3]
            {"Selected Block: Grass",
            "Selected Block: Dirt",
                "Selected Block: Path"
            };


        selectedBlockText.text = "Selected Block: None";
        SetLevel();
    }

    #region Visibility Dock

    //makes the editor tools panel visible
    public void DockingOut()
    {
        dockingPanel.gameObject.SetActive(true);
        dockingButton.gameObject.SetActive(false);
    }

    //makes the editor tools panel invisible to have more space
    public void DockingIn()
    {
        dockingPanel.gameObject.SetActive(false);
        dockingButton.gameObject.SetActive(true);
    }
    #endregion

    #region saving and setting
    //sets the id of selected block, which allows user to change the type of block when interacted with map blocks
    public void SelectBlock(int _blockId)
    {
        if (_blockId != 9999)
        {
            currentlySelectedId = _blockId;
            selectedBlockText.text = selectedNames[currentlySelectedId];
        }
        else
        {
            selectedBlockText.text = "Selected Block: None";
            currentlySelectedId = _blockId;
        }
    }

    //sets the level id based on currently selected level
    public void SetLevel()
    {
        currentlySelectedLevel = levelDropdown.value;
    }

    //saves the current level to the user files
    public void SaveLevel()
    {
        if (pathNum > 2)
        {
            SaveLoadManager.SetLevelNumber(currentlySelectedLevel);
            blockIds.Clear();
            for (int i = 0; i < generator.MapBlocksList.Count; i++)
            {
                blockIds.Add((int)generator.MapBlocksList[i].typeOfTerrain);
            }
            savedHeight = generator.Height;
            SavedWidth = generator.Width;
            SaveLoadManager.SaveLevelData(this);
        }
        else
        {
            pathPointsMsg.gameObject.SetActive(true);
        }
    }

    //Resets the level to empty and deletes it from saved levels within user files
    public void RemoveLevel()
    {
        pathNum = 0;
        numOfPath.Clear();
        MapEditorGenerator.instance.ResetMsg();
        MapEditorGenerator.instance.DeleteMap();
        MapEditorGenerator.instance.DeleteMap();
        if (File.Exists(Application.persistentDataPath + "LevelData" + currentlySelectedLevel + ".lvl"))
        {
            SaveLoadManager.SetLevelNumber(currentlySelectedLevel);
            SaveLoadManager.DeleteLevel();
        }
    }

    //Draws the path by displaying it physically and adding points to the list that can be saved
    public void DrawPathFromSave(int pathId, EditorTile tileSelected)
    {
        numOfPath.Add(pathId);
        tileSelected.PathNumber.text = pathNum.ToString();
        tileSelected.PathNumber.gameObject.SetActive(true);
        pathNum++;
    }

    //resets the path by removing every point and defaulting all the values
    public void ResetPath()
    {
        pathNum = 0;
        numOfPath.Clear();
        for (int i = 0; i < MapEditorGenerator.instance.MapBlocksList.Count; i++)
        {
            MapEditorGenerator.instance.MapBlocksList[i].PathNumber.gameObject.SetActive(false);
        }
    }

    #endregion

    //Handles left mouse click which allows for placing map tiles as well as path points
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pathPointsMsg.gameObject.SetActive(false);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    //handles placing of a block type(grass,dirt,path)
                    if (currentlySelectedId != 9999)
                    {
                        hit.transform.GetComponent<EditorTile>().SetNewBlockType(currentlySelectedId);
                    }
                    //handles placing of path points on specific path block
                    else if (currentlySelectedId == 9999)
                    {
                        for (int i = 0; i < MapEditorGenerator.instance.MapBlocksList.Count; i++)
                        {
                            EditorTile tileSelected = hit.transform.GetComponent<EditorTile>();
                            if (MapEditorGenerator.instance.MapBlocksList[i] == tileSelected)
                            {
                                if (MapEditorGenerator.instance.MapBlocksList[i].typeOfTerrain == EditorTile.TerrainType.Path)
                                {
                                    DrawPathFromSave(i, tileSelected);
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    //returns to main menu when button gets pressed that calls this function
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
