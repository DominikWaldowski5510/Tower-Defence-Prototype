using UnityEngine;

//handles the punchthrough bullet logic
public class PunchthroughBullet : SpikeBullet
{
    //Punchthrough bullet collision, does not disable the bullet upon collision
    public override void HandleCollision(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyHealth healthOfEnemy = other.GetComponent<EnemyHealth>();
            healthOfEnemy.TakeDamage(Damage);
        }
    }
}
