using UnityEngine;
using UnityEngine.UI;

//Handles clicking the icon of the tower in the tower placement bar
public class TowerSelection : MonoBehaviour
{
    private Image img;                                                 //image attached to this object, handles the tower image display
    [SerializeField] private BaseTower tower = null;                  //reference to scriptable object that has all tower stats and pricing
    [SerializeField] private Text towerPrice = null;                  //text that displays the price of the tower
    [SerializeField] private Transform availabilityIcon = null;         //the locking image that shows that cant be purchased
    [SerializeField] private Pooling towerToPool = null;
    //sets up default values and grabs imagine reference
    private void Start()
    {
        img = this.gameObject.GetComponent<Image>();
        img.sprite = tower.towerIcon;
        towerPrice.text = "£" + tower.buyoutCost;
        CheckPricing();
    }

    //selects tower with a button click
    public void SelectTower()
    {
        PlayerInteraction.instace.DisableDocks();
        if (GameManager.instance.Money >= tower.buyoutCost)
        {
            PlayerInteraction.instace.EnableHighlight(tower, towerToPool.GetPooledObject());
        }
    }

    //Checks whenever the tower can be purchased or not
    public void CheckPricing()
    {
        if (GameManager.instance.Money >= tower.buyoutCost)
        {
            availabilityIcon.gameObject.SetActive(false);
        }
        else
        {
            availabilityIcon.gameObject.SetActive(true);
        }
    }

}
