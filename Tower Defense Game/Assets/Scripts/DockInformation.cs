using UnityEngine;
using UnityEngine.UI;

public class DockInformation : MonoBehaviour
{
    [Header("Generic Tab")]
    [SerializeField] private Image IconImage = null;                //reference to image of the object
    [SerializeField] private Image healthBar = null;                //reference to health bar of the object
    [SerializeField] private Text healthText = null;                //display of the health text in numbers
    [SerializeField] private Text theName = null;                   //the name of the object 
    [SerializeField] private Text description = null;               //for displaying description of the tower
    [SerializeField] private Transform healthStatsDisplay = null;           //used for displaying health of the enemy
    [Header("TowerTab Statistics")]
    [SerializeField] private Text damageText = null;                    //displays the damage of the tower
    [SerializeField] private Text rangeText = null;                      //displays the range of the tower
    [SerializeField] private Text rateOfFireText = null;                 //displays the rate of fire of the tower
    [Header("TowerTab Upgrade")]
    [SerializeField] private Text priceText = null;                     //displays the price of the tower
    [SerializeField] private Image upgradeLock = null;                  //handles the upgrade lock image 
    [SerializeField] private Text deletePrice = null;                   //displays the deletion price
    [Header("TowerTab FiringMode")]
    [SerializeField] private Sprite activeImage = null;                  //shows active image for firing mode
    [SerializeField] private Sprite inactiveImage = null;                //shows inactive image of firing mode
    [SerializeField] private Image[] firingModeButtons = null;              //stores all images that handle firing buttons
    //get all the images and determine currently active one
    [Header("Active Windows Manager")]
    [SerializeField] private Transform basicPanel = null;                   //stores reference to the basic panel which shows enemy + basic tower info
    [SerializeField] private Transform towerExtendedPanel = null;           //shows extended panel which has firing modes and upgrades
    private Tower tower;                                                    //reference of tower for holding stats of the tower
    private EnemyHealth enemy;

    //displays enemy stats and the basic panel
    public void DisplayEnemy(EnemyHealth _enemy)
    {
        enemy = _enemy;
        description.gameObject.SetActive(false);
        towerExtendedPanel.gameObject.SetActive(false);
        basicPanel.gameObject.SetActive(true);
        healthStatsDisplay.gameObject.SetActive(true);
        IconImage.sprite = enemy.Enemy.enemyIcon;
        theName.text = enemy.Enemy.enemyName;
        UpdateEnemyHealth();
    }

    private void UpdateEnemyHealth()
    {
        healthText.text = "Health: " + enemy.CurrentHealth.ToString("0") + "/" + enemy.Enemy.health;
        healthBar.fillAmount = (enemy.CurrentHealth / 100) / (enemy.Enemy.health / 100);
    }

    private void Update()
    {
        if(healthStatsDisplay.gameObject.activeInHierarchy == true)
        {
            UpdateEnemyHealth();
        }
    }

    //resets all buttons to default
    private void ResetActiveButtons()
    {
        for (int i = 0; i < firingModeButtons.Length; i++)
        {
            firingModeButtons[i].sprite = inactiveImage;
        }
    }

    //Upgrades the tower
    public void UpgradeTower()
    {
        if(GameManager.instance.Money >= tower.TowerStats.towerTier[tower.CurrentTowerTier].upgradeCost)
        {
            if (tower.TowerStats.towerTier.Length - 1 > tower.CurrentTowerTier)
            {
                GameManager.instance.Money -= tower.TowerStats.towerTier[tower.CurrentTowerTier].upgradeCost;
                GameManager.instance.UpdateMoney();
                tower.CurrentTowerTier++;
                tower.UpdateStats();
                DisplayTower(tower);
            }
        }
    }

    //Deletes the currently placed tower
    public void DeleteTower()
    {
        GameManager.instance.Money += CalculateDeleteCosts();
        GameManager.instance.UpdateMoney();
        //Removes and resets the panels
        tower.gameObject.SetActive(false);
        tower = null;
        PlayerInteraction.instace.RangeDisplayCircle.SetActive(false);
        towerExtendedPanel.gameObject.SetActive(false);
        basicPanel.gameObject.SetActive(false);
    }

    //Displays all the tower details
    public void DisplayTower(Tower currentTowerStats)
    {
        tower = currentTowerStats;
        towerExtendedPanel.gameObject.SetActive(true);
        basicPanel.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
        healthStatsDisplay.gameObject.SetActive(false);
        description.text = currentTowerStats.TowerStats.description;
        IconImage.sprite = currentTowerStats.TowerStats.towerIcon;
        ResetActiveButtons();
        CalculateDeletePrice();
        firingModeButtons[(int)currentTowerStats.modeOfFire].sprite = activeImage;
        //display tower information
        OffButtonHighlight();
        theName.text = currentTowerStats.TowerStats.towerName;
        //Checking of the level of the tower for upgrade button
        if (currentTowerStats.TowerStats.towerTier.Length -1 <= currentTowerStats.CurrentTowerTier)
        {
            upgradeLock.gameObject.SetActive(true);
            priceText.text = "MAX";
        }
        else
        {
            upgradeLock.gameObject.SetActive(false);
            priceText.text = "$" + currentTowerStats.TowerStats.towerTier[currentTowerStats.CurrentTowerTier].upgradeCost;
        }
    }

    //Change the mode of fire for the tower
    public void SetFiringMode(int firingModeNum)
    {
        ResetActiveButtons();
        tower.SetFiringMode(firingModeNum);
        firingModeButtons[firingModeNum].sprite = activeImage;
    }

    //shows the changed statistics of the tower when hovered over with mouse on upgrade button
    public void OnButtonHighlight()
    {
        if (tower.TowerStats.towerTier.Length - 1 <= tower.CurrentTowerTier)
        {
            return;
        }
            //set default values
            float tempDamage, tempRange;
        tempDamage = tower.Damage;
        tempRange = tower.Range;
        //remove damage from the scriptable values
        //what this does is counts difference when the tower is buffed by buffing tower
        //gets difference between actual and buffed value
        tempDamage -=
        tower.TowerStats.towerTier[tower.CurrentTowerTier].damage;
        tempRange -=
        tower.TowerStats.towerTier[tower.CurrentTowerTier].range;
        //gets difference between current and upgraded value whilst still including buff change
        tempRange += tower.TowerStats.towerTier[tower.CurrentTowerTier + 1].range;
        tempDamage += tower.TowerStats.towerTier[tower.CurrentTowerTier + 1].damage;
        //showcases the change of statistics
        damageText.text = "Damage: " + tower.Damage + "->" + tempDamage;
        rateOfFireText.text = "Rate Of fire: " + tower.RateOfFire + "->" + tower.TowerStats.towerTier[tower.CurrentTowerTier + 1].rateOfFire;
        rangeText.text = "Range: " + tower.Range + "->" + tempRange;
    }

    //resets the display of statistics to default when mouse leaves the upgrade button
    public void OffButtonHighlight()
    {
        damageText.text = "Damage: " + tower.Damage;
        rateOfFireText.text = "Rate Of fire: " + tower.RateOfFire;
        rangeText.text = "Range: " + tower.Range;
    }

    //Displays the amount of money that will be returned when the tower gets sold
    private void CalculateDeletePrice()
    {
        deletePrice.text = "Return: " + CalculateDeleteCosts();
    }

    //calculates the costs of selling the tower
    private int CalculateDeleteCosts()
    {
        int returnedMoney = 0;
        if (tower.CurrentTowerTier > 0)
        {
            for (int i = 0; i < tower.CurrentTowerTier; i++)
            {
                returnedMoney += (tower.TowerStats.towerTier[i].upgradeCost / 2);
            }
        }
       return returnedMoney += tower.TowerStats.selloutCost;
    }
}
