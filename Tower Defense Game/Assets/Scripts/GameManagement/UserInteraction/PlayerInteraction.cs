using UnityEngine;
using UnityEngine.EventSystems;

//Handles user left and right mouse clicks
public class PlayerInteraction : MonoBehaviour
{
    private Camera cam;                     //reference to camera used in the game scene
    [Header("Tower Placements")]
    private GameObject selectedToPlaceObj;             //selected object that will be placed(tower)
    private GameObject selectionHighlight;             //tower outline when placing a tower
    private GameObject selectedObj;                    //selected already placed object like a tower or an enemy
    public static PlayerInteraction instace;                            //instance of the class so it can be accessed easier
    [Header("Dockable UI")]
    [SerializeField] private DockInformation dockingPoint = null;                 //reference to UI that displays all stats for towers and enemies
    [SerializeField] private GameObject[] hightedTowers = null;                  //all highlights of the towers
    [Header("Selectors")]
    [SerializeField] private GameObject rangeDisplayCircle = null;              //Circle of range that is used for displaying current range of tower to be placed
    [SerializeField] private GameObject enemySelectedByTower = null;              //Circle of range that is used for displaying current range of tower to be placed
    public GameObject SelectedToPlaceObj { get => selectedToPlaceObj; set => selectedToPlaceObj = value; }
    public GameObject RangeDisplayCircle { get => rangeDisplayCircle; set => rangeDisplayCircle = value; }

    //setting up the instance
    private void Awake()
    {
        instace = this;
    }

    //assigns camera  and resetting all default Ui elements
    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        DisableDocks();
        DeselectHightlight();
    }

   

    //Enables the visual of the tower that will be placed(after selecting the tower icon it enables its highlight)
    public void EnableHighlight(BaseTower towerDetails, GameObject objToPool)
    {
        DeselectHightlight();
        hightedTowers[towerDetails.towerId].gameObject.SetActive(true);
        selectionHighlight = hightedTowers[towerDetails.towerId];
        selectedToPlaceObj = objToPool;
    }

    //handles player mouse input
    private void Update()
    {
        TowerPlacementPlacers();
        LeftMouseClicks();
        RightMouseClick();
        CheckForSelectedEnemy();
    }

    //Display the currently selected tower
    private void TowerPlacementPlacers()
    {
        if (selectionHighlight != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (AreaCheck.CheckRadius(selectionHighlight.transform, selectionHighlight.GetComponent<HightSelectionManager>().GroundTag) == true)
                {
                    selectionHighlight.transform.GetComponent<HightSelectionManager>().SetMaterials(true);
                }
                else
                {
                    selectionHighlight.transform.GetComponent<HightSelectionManager>().SetMaterials(false);
                }
                selectionHighlight.transform.position = hit.point;
            }

        }
    }

    //checks if a tower currently has a target and displays a circle around it
    private void CheckForSelectedEnemy()
    {
        if (selectedObj != null)
        {
            if (selectedObj.tag != "Enemy")
            {
                Tower selectedTower = selectedObj.GetComponent<Tower>();
                if (selectedTower.Target != null)
                {
                    enemySelectedByTower.gameObject.SetActive(true);
                    Vector3 adjustedPosition = selectedTower.Target.transform.position;
                    enemySelectedByTower.transform.position = adjustedPosition;
                }
                else
                {
                    enemySelectedByTower.gameObject.SetActive(false);
                }
            }
        }
    }

    //handles raycasts for left mouse clicks
    private void LeftMouseClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Vector3 mousePos = Vector3.one;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                mousePos = hit.point;
                if (selectionHighlight == null)
                {
                    rangeDisplayCircle.gameObject.SetActive(false);
                    DisableDocks();
                }


                //places the selected tower
                if (selectedToPlaceObj != null)
                {


                    if (AreaCheck.CheckRadius(selectionHighlight.transform, selectionHighlight.GetComponent<HightSelectionManager>().GroundTag))
                    {
                        Vector3 placementPosition;

                        if (SaveDataManager.instance.UsingFreePlacement == 1)
                        {
                            placementPosition = new Vector3(mousePos.x, 0.70f, mousePos.z);
                        }
                        else
                        {
                            placementPosition = hit.transform.GetComponent<Tile>().PathPoint.transform.position;
                            placementPosition.y = 0.70f;
                        }
                        selectedToPlaceObj.transform.position = placementPosition;
                        selectedToPlaceObj.SetActive(true);
                        if (selectedToPlaceObj.GetComponent<Tower>() != null)
                        {
                            GameManager.instance.Money -=
                                 selectedToPlaceObj.GetComponent<Tower>().TowerStats.buyoutCost;
                            GameManager.instance.UpdateMoney();
                        }
                        else
                        {
                            GameManager.instance.Money -=
                                 selectedToPlaceObj.GetComponent<RoadSpike>().Spike.buyoutCost;
                            GameManager.instance.UpdateMoney();
                        }
                        selectedToPlaceObj = null;
                        DeselectHightlight();
                    }
                }
                //Selects a tower or an enemy that are already in the scene, while enabling stats panel
                if (selectedToPlaceObj == null)
                {
                    if (hit.transform.tag == "Enemy" || hit.transform.tag == "Tower")
                    {
                        DisableDocks();
                        selectedObj = hit.transform.gameObject;
                        selectedObj.GetComponent<SelectableObject>().OnSelectObject();
                        if (hit.transform.tag == "Enemy")
                        {
                            dockingPoint.transform.gameObject.SetActive(true);
                            dockingPoint.DisplayEnemy(hit.transform.GetComponent<EnemyHealth>());
                        }
                        else if (hit.transform.tag == "Tower")
                        {
                            Tower towerStats = hit.transform.GetComponent<Tower>();
                            dockingPoint.transform.gameObject.SetActive(true);
                            dockingPoint.DisplayTower(towerStats);
                            rangeDisplayCircle.gameObject.SetActive(true);
                            rangeDisplayCircle.transform.position = new Vector3(hit.transform.position.x, 0.50f, hit.transform.position.z);
                            rangeDisplayCircle.transform.localScale = new Vector3(towerStats.Range * 2, rangeDisplayCircle.transform.localScale.y, towerStats.Range * 2);
                        }
                    }
                }

            }
        }
    }

    //handles right mouse clicks
    private void RightMouseClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DeselectHightlight();
            selectedToPlaceObj = null;
            if (selectedObj != null)
            {
                selectedObj.GetComponent<SelectableObject>().OnDeselectObject();
                selectedObj = null;
            }
            DisableDocks();
        }
    }

    #region Visible Panels
    //Disables the highlight and range circle
    private void DeselectHightlight()
    {
        rangeDisplayCircle.gameObject.SetActive(false);
        selectionHighlight = null;
        for (int i = 0; i < hightedTowers.Length; i++)
        {
            hightedTowers[i].gameObject.SetActive(false);
        }
    }

    //Disables the docking UI element as well as deselects selected object such as enemy or tower
    public void DisableDocks()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<SelectableObject>().OnDeselectObject();
        }
        dockingPoint.transform.gameObject.SetActive(false);
        enemySelectedByTower.gameObject.SetActive(false);
    }
    #endregion 
}
