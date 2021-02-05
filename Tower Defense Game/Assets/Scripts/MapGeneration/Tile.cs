using UnityEngine;

//Handles the block that is placed on the generated map
public class Tile : MonoBehaviour
{
    [SerializeField] private Transform[] blockTypes = null;          //stores different types of tile blocks
    [SerializeField] private Transform pathPoint = null;           //stores point for the path for enemies to move on
    [SerializeField] private GameObject startPoint, endPoint;      //used for determining last and first block in the map
    private GameObject placedObject;
    public Transform PathPoint { get => pathPoint; }
    public GameObject EndPoint { get => endPoint; set => endPoint = value; }
    public GameObject StartPoint { get => startPoint; set => startPoint = value; }
    public GameObject PlacedObject { get => placedObject; set => placedObject = value; }


    //sets all the blocks to be disabled
    private void ResetBlocks()
    {
        //sets all the blocks to invisible
        for (int i = 0; i < blockTypes.Length; i++)
        {
            blockTypes[i].transform.gameObject.SetActive(false);
        }
    }

    //sets the correct block as well as tags them appropriately
    public void SetTile(int typeId)
    {
        ResetBlocks();
        blockTypes[typeId].transform.gameObject.SetActive(true);
        if (typeId == 2)
        {
            this.gameObject.tag = "Path";
        }
        else if (typeId != 0)
        {
            this.gameObject.tag = "Unavailable";

        }
    }
}

