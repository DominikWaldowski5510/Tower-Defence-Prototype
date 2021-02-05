using UnityEngine;

//Tower that boosts range of nearby towers
public class SignalTower : Tower
{
    private float repeatRate = 10f;              //how often the buff repeats its function
    [SerializeField] private int buffId = 2; //2 = range, 3 = damage

    //starts invoking function that buffs the allies over period of time
    private void Start()
    {
        InvokeRepeating("ExtendRangeOfAllies", 1, repeatRate);
    }

    //disables the invoke so it doesnt happen when the object is inactive
    private void OnDisable()
    {
        CancelInvoke("ExtendRangeOfAllies");
    }

    //checks for all nearby enemies and provides them with the buff corresponding to the id
    private void ExtendRangeOfAllies()
    {
        Collider[] allies = Physics.OverlapSphere(this.transform.position, Range, EnemyLayer);
        foreach (Collider ally in allies)
        {
            if (ally.gameObject != this.gameObject)
            {
                ApplyingDebuff buffType = new ApplyingDebuff();
                buffType.debuffDamage = TowerStats.towerTier[CurrentTowerTier].damage;
                buffType.debuffDuration = repeatRate;
                buffType.typeOfDebuff = (ApplyingDebuff.DebuffType)buffId;
                ally.GetComponent<Tower>().AddBuff(buffType);
            }
        }

    }

}
