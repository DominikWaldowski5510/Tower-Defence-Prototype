using UnityEngine;

//bullet that does overtime damage
public class BurnBullet : Bullet
{
    private float burnDuration = 5;                    //the duration of the burn
    public float BurnDuration { get => burnDuration; }

    //Handles bullet collison of the burn bullet
    public override void HandleCollision(Collider other)
    {
        base.HandleCollision(other);
        if (other.tag == "Enemy")
        {
            float temDmg = Damage;
            temDmg = temDmg / 3;
            EnemyHealth healthOfEnemy = other.GetComponent<EnemyHealth>();
            ApplyingDebuff newDebuf = new ApplyingDebuff();
            newDebuf.typeOfDebuff = ApplyingDebuff.DebuffType.Fire;
            newDebuf.debuffDamage = temDmg;
            newDebuf.debuffDuration = burnDuration;
            healthOfEnemy.DealDebuff(newDebuf);
        }
    }
}
