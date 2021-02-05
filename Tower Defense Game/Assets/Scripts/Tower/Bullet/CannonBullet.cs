using System;
using UnityEngine;

//handles the cannon bullet that moves in a parabola
public class CannonBullet : Bullet
{
    [SerializeField] private float bulletTopPoint = 2;               //how high the bullet will travel
    private float startTime;                                  //The initial time when the bullet spawns
    private Vector3 startPos;                                //the initial position of the bullet
    private Vector3 endPos;                               //desired end position for the bullet
    private float journeyLength;                          //time in which the bullet travels
    [SerializeField] private float range = 2;                 //how big the splash radius is of the cannon
    [SerializeField] private LayerMask enemyLayer = 0;          //layer mask of the object of interaction
    private Pooling particlePool;                                   //pooling explosion particle

    public float Range { get => range; set => range = value; }
    public LayerMask EnemyLayer { get => enemyLayer; set => enemyLayer = value; }
    public Vector3 EndPos { get => endPos; set => endPos = value; }

    //Enables all default bullet values as well as sets up for parabola movement
    public override void OnEnable()
    {
        base.OnEnable();
        startTime = Time.time;
        startPos = this.transform.position;
        journeyLength = Vector3.Distance(startPos, endPos);
        particlePool = GameObject.Find("CannonParticlePool").GetComponent<Pooling>();
    }

    //disables all default bullet features
    public override void OnDisable()
    {
        base.OnDisable();
    }

    //Sets bullet target and damage values
    public override void SetBullet(float _damage, float _bulletSpeed, Transform _target, bool isIce)
    {
        base.SetBullet(_damage, _bulletSpeed, _target);
        endPos = _target.position;
    }

    //handles how the cannon bullet collides using the aoe 
    public override void HandleCollision(Collider other)
    {
        if (other.gameObject.tag != "Enemy" || other.gameObject.tag != "Tower")
        {
            //Calculate the Aoe damage
            Collider[] targets = Physics.OverlapSphere(this.transform.position, range, enemyLayer);
            for (int i = 0; i < targets.Length; i++)
            {
                EnemyHealth healthOfEnemy = targets[i].GetComponent<EnemyHealth>();
                float damageCalculation;
                Vector3 explostionToTarget = targets[i].transform.position - this.transform.position;
                float explosionDist = explostionToTarget.magnitude;
                float relativeDistance = (range - explosionDist) / range;
                damageCalculation = relativeDistance * Damage;
                damageCalculation = Mathf.FloorToInt(damageCalculation);
                healthOfEnemy.TakeDamage(damageCalculation);
            }

            DisableBullet();
        }

    }

    //Makes the bullet move in desired parabola way
    public override void BulletTravel()
    {
        float distCovered = (Time.time - startTime) * BulletSpeed;
        journeyLength = Vector3.Distance(startPos, endPos);
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Parabola(startPos, endPos, bulletTopPoint, fractionOfJourney);
    }


    //The parabola function
    private Vector3 Parabola(Vector3 _startPos, Vector3 _endPos, float _height, float _time)
    {
        Func<float, float> f = x => -4 * _height * x * x + 4 * _height * x;
        var mid = Vector3.Lerp(_startPos, _endPos, _time);
        return new Vector3(mid.x, f(_time) + Mathf.Lerp(_startPos.y, _endPos.y, _time), mid.z);
    }

    //disables the bullet
    public override void DisableBullet()
    {
        GameObject particle = particlePool.GetPooledObject();
        if (particle != null)
        {
            particle.transform.position = this.transform.position;
            particle.gameObject.SetActive(true);
        }
        base.DisableBullet();
    }

    //draws gizmos to see the range of the bullet used for testing only
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
