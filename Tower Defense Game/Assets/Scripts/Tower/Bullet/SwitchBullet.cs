using UnityEngine;

//handles mehcanics of the bullet that switches types from ice to fire
public class SwitchBullet : CannonBullet
{
    [SerializeField] private Material[] typeMaterial = null;         //stores 2 materials that represent ice and fire
    [SerializeField] private Renderer rendereMat = null;             //reference to renderer to be able to switch materials in run time
    private bool isIce;                 //checks for element of the bullet

    //handles how the cannon bullet collides using the aoe 
    public override void HandleCollision(Collider other)
    {
        if (other.gameObject.tag != "Enemy" || other.gameObject.tag != "Tower")
        {
            //Calculate the Aoe damage
            Collider[] targets = Physics.OverlapSphere(this.transform.position, Range, EnemyLayer);
            for (int i = 0; i < targets.Length; i++)
            {
                EnemyHealth healthOfEnemy = targets[i].GetComponent<EnemyHealth>();
                float damageCalculation;
                Vector3 explostionToTarget = targets[i].transform.position - this.transform.position;
                float explosionDist = explostionToTarget.magnitude;
                float relativeDistance = (Range - explosionDist) / Range;
                damageCalculation = relativeDistance * Damage;
                damageCalculation = Mathf.FloorToInt(damageCalculation);
                healthOfEnemy.TakeDamage(damageCalculation);
                ApplyingDebuff newDebuf = new ApplyingDebuff();
                if (isIce)
                {
                    newDebuf.debuffDamage = 0;
                    newDebuf.debuffDuration = 4f;
                    newDebuf.typeOfDebuff = ApplyingDebuff.DebuffType.Freeze;
                }
                else
                {
                    float temDmg = Damage;
                    temDmg = temDmg / 3;
                    newDebuf.typeOfDebuff = ApplyingDebuff.DebuffType.Fire;
                    newDebuf.debuffDamage = temDmg;
                    newDebuf.debuffDuration = 3;

                }
                healthOfEnemy.DealDebuff(newDebuf);
            }



            DisableBullet();
        }

    }

    //Sets bullet target and damage values
    public override void SetBullet(float _damage, float _bulletSpeed, Transform _target, bool _isIce)
    {
        base.SetBullet(_damage, _bulletSpeed, _target, _isIce);
        EndPos = _target.position;
        isIce = _isIce;
        if (isIce)
        {
            SetIceEffect();
        }
        else
        {
            SetFireEffect();
        }
    }

    //sets the material to be fire effect colour
    private void SetFireEffect()
    {
        rendereMat.material = typeMaterial[1];
    }

    //sets the material to be ice effect colour
    private void SetIceEffect()
    {
        rendereMat.material = typeMaterial[0];
    }
}
