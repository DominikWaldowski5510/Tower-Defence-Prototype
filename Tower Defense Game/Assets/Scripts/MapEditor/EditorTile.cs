
using UnityEngine;


//the object that stores each individual tile within the editor
public class EditorTile : MonoBehaviour
{
    [SerializeField] private TextMesh pathNumber = null;                //compotent that stores a 3d number visual
    [SerializeField] private GameObject[] tileTypes = null;             //stores all available tile types which are grass, path and dirt see TerrainType enum

    //The default types of terrain available
    public enum TerrainType
    {
        Grass,
        Dirt,
        Path
    }
    public TerrainType typeOfTerrain;               //stores the currently set terrain type

    public TextMesh PathNumber { get => pathNumber; set => pathNumber = value; }

    //sets the default map block
    public void SetTile(int correctionBlock)
    {
        ResetBlocks();
        pathNumber.gameObject.SetActive(false);
        typeOfTerrain = (TerrainType)correctionBlock;
        SetCorrectBlock();
    }

    //sets the correct block based on the terrain type enum
    private void SetCorrectBlock()
    {
        tileTypes[(int)typeOfTerrain].SetActive(true);
    }

    //resets all the blocks by setting them to inactive
    private void ResetBlocks()
    {
        for (int i = 0; i < tileTypes.Length; i++)
        {
            tileTypes[i].SetActive(false);
        }
    }

    //Sets a new block, 
    public void SetNewBlockType(int blockId)
    {
        ResetBlocks();
        if (blockId == 0)
        {
            typeOfTerrain = TerrainType.Grass;
        }
        else if (blockId == 1)
        {
            typeOfTerrain = TerrainType.Dirt;
        }
        else if (blockId == 2)
        {
            typeOfTerrain = TerrainType.Path;
        }
        SetCorrectBlock();
    }
}
