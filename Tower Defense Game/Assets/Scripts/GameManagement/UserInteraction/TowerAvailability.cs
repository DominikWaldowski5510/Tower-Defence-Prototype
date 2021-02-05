using UnityEngine;

//updates availability icon on every single placeable tower image
public class TowerAvailability : MonoBehaviour
{
    [SerializeField]
    private TowerSelection[] availableTowers = null;                //stores references to all available tower icons

    //loops through all available tower selection icons to see which can be afforded by the player
    public void UpdateAvailability()
    {
        for (int i = 0; i < availableTowers.Length; i++)
        {
            availableTowers[i].CheckPricing();
        }
    }
}
