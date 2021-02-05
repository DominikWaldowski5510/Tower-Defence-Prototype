using UnityEngine;


//handles see through highlight movement and availability
public class HightSelectionManager : MonoBehaviour
{
    [SerializeField]
    private Renderer[] activeMaterial = null;
    [SerializeField] private Material[] highlightMaterials = null;   //0 = good material, 1 = bad material
    [SerializeField] private BaseTower towerRangeDisplay = null;        //takes reference to tower values to be able to display correct range size
    [SerializeField] private GameObject rangeDisplay = null;            //reference to gameobject that is used for drawing the range
    [SerializeField] private string groundTag = "Tile";                 //unique tag used for placing the object on that specific ground type

    public string GroundTag { get => groundTag;  }

    //sets scale of the object based on range of that specific tower and enables range indicator
    private void OnEnable()
    {
        if (rangeDisplay != null)
        {
            rangeDisplay.gameObject.SetActive(true);
            rangeDisplay.transform.localScale = new Vector3(towerRangeDisplay.towerTier[0].range * 2, rangeDisplay.transform.localScale.y, towerRangeDisplay.towerTier[0].range * 2);
        }

    }

    //moves the range circle with the object itself
    private void Update()
    {
        if (rangeDisplay != null)
        {
            rangeDisplay.transform.position = this.gameObject.transform.position;
        }
    }

    //changes materials to show availability and inavailability of placement
    public void SetMaterials(bool isAvailable)
    {
        if(isAvailable == true)
        {
            for (int i = 0; i < activeMaterial.Length; i++)
            {
                activeMaterial[i].material = highlightMaterials[0];
            }
        }
        else
        {
            for (int i = 0; i < activeMaterial.Length; i++)
            {
                activeMaterial[i].material = highlightMaterials[1];
            }
        }
    }
}
