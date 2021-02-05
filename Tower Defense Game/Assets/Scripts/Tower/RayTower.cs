using UnityEngine;

//Ray tower which is a simplified version of the freeze tower only dealing damage
public class RayTower : FreezeTower
{
    //deals damage over time while connected to enemy
    public override void Shoot()
    {
        if (Target != null)
        {
            if (!LineRenderer.enabled)
                LineRenderer.enabled = true;
            LineRenderer.SetPosition(0, Muzzle[0].transform.position);
            LineRenderer.SetPosition(1, Target.transform.position);
            EndRayParticle.transform.position = Target.transform.position;
            EndRayParticle.gameObject.SetActive(true);
            EnemyHealth health = Target.GetComponent<EnemyHealth>();
            health.TakeDamage(Damage * Time.deltaTime);

        }
        else
        {
            LineRenderer.enabled = false;
            EndRayParticle.gameObject.SetActive(false);
        }
    }
}
