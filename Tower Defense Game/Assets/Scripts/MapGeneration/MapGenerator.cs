using System.Collections.Generic;
using UnityEngine;

//generates the map in game scene
public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private Level[] levels = null;         //reference to all existing levels
    [SerializeField] private GameObject spawnObjTile = null;        //object that spawns on load
    private GameObject parent = null;                               //parent for the map blocks so they dont make the hierarchy messy
    private int levelId = 0;                                        //id of the level to be loaded
    private List<Tile> allSpawnedTiles = new List<Tile>();          //all the tiles that have been spawned within the level
    private int width, height;                                      //how tall and wide the map will be
    private int[] blockIds;                                         //used for loading default block data on loaded level
    private int[] pathIds;                                          //used for loading default pathing for enemy from loaded level

    public int Height { get => height; }
    public int Width { get => width; }

    //sets up the default values and loads settings from player prefs
    private void Start()
    {
        parent = this.gameObject;
        levelId = PlayerPrefs.GetInt("level");
        int levelType = PlayerPrefs.GetInt("LevelData");
        GenerateLevel(levelType);
    }

    //Generates the level based on saved data in scriptable level object from editor
    private void GenerateLevel(int levelTyp)
    {
        //if our level is default it uses scriptable data to assign width and height otherwise it loads all data from save file
        if (levelTyp == 0)
        {
            width = levels[levelId].width;
            height = levels[levelId].height;
        }
        else
        {
            SaveLoadManager.SetLevelNumber(levelId);
            int[][] loadedMapData = SaveLoadManager.LoadLevelData();
            height = loadedMapData[0][1];
            width = loadedMapData[0][0];
            blockIds = loadedMapData[1];
            pathIds = loadedMapData[2];
        }
        //generates the map based on our height and width assigned
        Tile[,] mapBlocks = new Tile[height, width];
        int additive = 0;
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                GameObject blockOnTerrain = Instantiate(spawnObjTile) as GameObject;
                blockOnTerrain.transform.localPosition = new Vector3(x, 0, y);
                blockOnTerrain.transform.SetParent(parent.transform);
                blockOnTerrain.name = x + "/" + y;
                Tile tile = blockOnTerrain.GetComponent<Tile>();
                mapBlocks[x, y] = tile;
                if (levelTyp == 0)
                {
                    mapBlocks[x, y].SetTile(levels[levelId].blockIds[additive]);
                }
                else
                {
                    mapBlocks[x, y].SetTile(blockIds[additive]);
                }
                allSpawnedTiles.Add(tile);
                additive++;
            }
        }

        //sets the path of the level based the method in which the level was loaded
        if (levelTyp == 0)
        {
            for (int t = 0; t < levels[levelId].path.Count; t++)
            {
                for (int i = 0; i < allSpawnedTiles.Count; i++)
                {
                    if (i == levels[levelId].path[t])
                    {

                        WaveManager.instance.PathTiles.Add(allSpawnedTiles[i]);

                    }
                }
            }
        }
        else
        {
            for (int t = 0; t < pathIds.Length; t++)
            {
                for (int i = 0; i < allSpawnedTiles.Count; i++)
                {
                    if (i == pathIds[t])
                    {

                        WaveManager.instance.PathTiles.Add(allSpawnedTiles[i]);

                    }
                }
            }
        }

        //sets the starting and end block of our pathing for the enemy
        WaveManager.instance.PathTiles[0].GetComponent<Tile>().StartPoint.SetActive(true);
        WaveManager.instance.PathTiles[WaveManager.instance.PathTiles.Count - 1].GetComponent<Tile>().EndPoint.SetActive(true);
    }
}
