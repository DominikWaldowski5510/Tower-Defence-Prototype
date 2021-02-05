using UnityEngine;

//chain bullet burns the first target and spreads damage to nearby enemies
public class ChainBullet : BurnBullet
{
    [SerializeField] private LayerMask enemyLayer = default;        //layermask to affect spread
    private float spreadRadius = 2;             //how far the chain can spread


    //changes default collision function
    public override void HandleCollision(Collider other)
    {

        ChainHits(other.transform.gameObject);
        DisableBullet();
    }

    //does damage to target by applying burn effect and dealing damage separately
    private void DoDamage(Transform other)
    {
        float temDmg = Damage;
        temDmg = temDmg / 3;
        EnemyHealth healthOfEnemy = other.GetComponent<EnemyHealth>();
        ApplyingDebuff newDebuf = new ApplyingDebuff();
        newDebuf.typeOfDebuff = ApplyingDebuff.DebuffType.Fire;
        newDebuf.debuffDamage = temDmg;
        newDebuf.debuffDuration = BurnDuration;
        healthOfEnemy.DealDebuff(newDebuf);
        healthOfEnemy.TakeDamage(Damage);
    }


    //finds all enemies within range and deals damage to them
    private void ChainHits(GameObject spreadLocation)
    {
        Collider[] targets = Physics.OverlapSphere(spreadLocation.transform.position, spreadRadius, enemyLayer);
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (Vector3.Distance(this.transform.position, targets[i].transform.position) < spreadRadius)
                {
                    DoDamage(targets[i].transform);
                }
            }
        }

    }
}
