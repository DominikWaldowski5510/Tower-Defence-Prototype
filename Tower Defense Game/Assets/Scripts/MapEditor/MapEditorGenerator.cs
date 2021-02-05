using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//creates the editor map or loads from the files
public class MapEditorGenerator : MonoBehaviour
{
    private GameEditor gameEditor;
    [SerializeField] private Slider widthField = null, heightField = null;
    [SerializeField] private Text heightText = null, widthText = null, generatedText = null;
    private string genRecMsg = "Generate/Recreate Level";
    private string unloadMsg = "Unload Level";
    private string widMsg = "Map Width: ";
    private string heiMsg = "Map Height: ";
    public static MapEditorGenerator instance;
    private int height;         //how tall the map is
    private int width;          //how wide the map is
    [SerializeField] private GameObject tileBlock = null;          //the block that we place in our map
    private EditorTile[,] mapBlocks;            //array of all map componets
    [SerializeField] private List<EditorTile> mapBlocksList = new List<EditorTile>();           //stores all the blocks that have been added, used for saving and getting block number
    private Transform mapParent;                                                                //a transform which acts as parent used to parent blocks to it so the hierarchy is not as messy
    private bool isGenerated;                                                                   //checks if a map is already generated or not
    private int[] blockIds;                                                                     //stores all the blocks by their id used for saving the level
    public int Width { get => width; }
    public int Height { get => height; }
    public EditorTile[,] MapBlocks { get => mapBlocks; }
    public List<EditorTile> MapBlocksList { get => mapBlocksList; }

    //sets default values and instance of this class
    private void Awake()
    {
        isGenerated = false;
        gameEditor = this.gameObject.GetComponent<GameEditor>();
        instance = this;

    }

    //sets the generated/unload level button to default state
    private void OnEnable()
    {
        ResetMsg();
    }

    //sets the sets the generated/unload to generate level message
    public void ResetMsg()
    {
        generatedText.text = genRecMsg;
    }

    //sets default values of the map and the parent
    private void Start()
    {
        mapParent = this.gameObject.transform;
        SetWidth();
        SetHeight();
    }

    //deletes the map visual blocks 
    public void DeleteMap()
    {
        if (mapBlocks != null)
        {
            foreach (EditorTile block in mapBlocks)
            {
                Destroy(block.gameObject);
            }
        }
        mapBlocks = null;
        isGenerated = false;
    }

    //sets the height of the map to be the height value of the slider
    public void SetHeight()
    {
        if (isGenerated == false)
        {
            height = (int)heightField.value;
            heightText.text = heiMsg + height;
        }
        else
        {
            heightField.value = height;
        }
    }

    //sets the width of the map to be the width value of the slider
    public void SetWidth()
    {
        if (isGenerated == false)
        {
            width = (int)widthField.value;
            widthText.text = widMsg + width;
        }
        else
        {
            widthField.value = width;
        }
    }

    //checks how the map will be generated and proceeds to generation method
    public void GenerateMap()
    {
        if (isGenerated == false)
        {
            if (File.Exists(Application.persistentDataPath + "LevelData" + gameEditor.CurrentlySelectedLevel + ".lvl"))
            {
                LoadMap();
            }
            else
            {
                NewCreateMap(false);
            }
        }
        else if (isGenerated == true)
        {
            //delete the map instead
            gameEditor.ResetPath();
            DeleteMap();
            ResetMsg();
        }

    }

    //shows all the numbers for the paths when loaded a map with already existing path
    private void SetNumbersForPath()
    {
        int tempId = 0;
        for (int t = 0; t < gameEditor.NumOfPath.Count; t++)
        {
            for (int i = 0; i < MapBlocksList.Count; i++)
            {
                if (i == gameEditor.NumOfPath[t])
                {
                    MapBlocksList[i].PathNumber.gameObject.SetActive(true);
                    MapBlocksList[i].PathNumber.text = tempId.ToString();
                    tempId++;
                }
            }
        }
    }

    //loads map from save file
    private void LoadMap()
    {
        SaveLoadManager.SetLevelNumber(gameEditor.CurrentlySelectedLevel);
        int[][] loadedMapData = SaveLoadManager.LoadLevelData();
        height = loadedMapData[0][1];
        width = loadedMapData[0][0];
        blockIds = loadedMapData[1];
        NewCreateMap(true);
        gameEditor.ResetPath();
        int[] pathIds = loadedMapData[2];
        for (int i = 0; i < pathIds.Length; i++)
        {
            for (int t = 0; t < mapBlocksList.Count; t++)
            {
                if (pathIds[i] == t)
                {
                    gameEditor.DrawPathFromSave(i, mapBlocksList[t]);
                }
            }
        }
    }

    //generates the map
    private void NewCreateMap(bool isPreGenerated)
    {
        mapBlocksList.Clear();
        mapBlocks = new EditorTile[height, width];
        int additive = 0;
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                GameObject blockOnTerrain = Instantiate(tileBlock) as GameObject;
                blockOnTerrain.transform.localPosition = new Vector3(x, 0, y);
                blockOnTerrain.transform.SetParent(mapParent);
                blockOnTerrain.name = x + "/" + y;
                EditorTile tile = blockOnTerrain.GetComponent<EditorTile>();
                mapBlocks[x, y] = tile;
                mapBlocksList.Add(tile);
                if (isPreGenerated == false)
                {
                    mapBlocks[x, y].SetTile(0);
                }
                else if (isPreGenerated == true)
                {
                    mapBlocks[x, y].SetTile(blockIds[additive]);
                    additive++;
                }
            }
        }
        generatedText.text = unloadMsg;
        isGenerated = true;
    }
}
