using UnityEngine;


//scriptable object for a new tower
[CreateAssetMenu(fileName = "New Tower", menuName = "Tower/New Tower", order = 1)]
public class BaseTower : ScriptableObject
{
    [Header("Identification")]
    public int towerId;                 //identification number for selecting this specific tower
    public string towerName;            //name of the tower
    public string description;         //description which tells what the tower does
    [Header("Upgrades")]
    public UpgradeSystem[] towerTier;   //The upgrade level of the tower
    [Range(0.1f, 30)]
    public float bulletSpeed = 5;           //how fast the bullet moves
    [Header("Pricing Details")]
    public int buyoutCost;          //the buyout cost to place the tower
    public int selloutCost;         //the sell cost to sell the existing tower
    [Header("Visuals")]
    public Sprite towerIcon;                //image of the tower
}

//handles the upgrade system of the tower
[System.Serializable]
public class UpgradeSystem
{
    public float rateOfFire;
    public float damage;            //damage that the tower does
    public float range;             //the range that the tower has
    public int upgradeCost;         //how much it costs to upgrade to next tier
}
