using UnityEngine;

//class that handles the functionality of the spike tower which shoots bullets in various directions
public class StationaryTower : Tower
{
    //changes how the tower shoots, by storing multiple muzzle poitns and shooting in all of those points, also aims by changing for range
    public override void Shoot()
    {
        Collider[] targets = Physics.OverlapSphere(this.transform.position, Range, EnemyLayer);
        if (targets.Length > 0)
        {
            TimeElapsed += Time.deltaTime;
            if (TimeElapsed >= RateOfFire)
            {
                TimeElapsed = 0;
                if (BulletPool != null)
                {
                    for (int i = 0; i < Muzzle.Length; i++)
                    {
                        GameObject bulletObj = BulletPool.GetPooledObject();
                        bulletObj.transform.position = Muzzle[i].transform.position;
                        bulletObj.transform.rotation = Muzzle[i].transform.rotation;
                        bulletObj.SetActive(true);
                        Bullet newBullet = bulletObj.GetComponent<Bullet>();
                        newBullet.SetBullet(Damage, TowerStats.bulletSpeed);
                    }
                }
            }
        }
    }
}
