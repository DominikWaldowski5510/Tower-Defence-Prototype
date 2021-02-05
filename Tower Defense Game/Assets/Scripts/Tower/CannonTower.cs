//Handles cannon tower functionality
using UnityEngine;

//handles ice/burn tower fire mode
public class CannonTower : Tower
{
    private bool isIce = true;                          //sets whenever its ice or fire type bullet

    //Performs shooting for the ice fire cannon with switch bullets
    public override void Shoot()
    {
        if (Target != null)
        {
            if (Target.gameObject.activeInHierarchy == true)
            {
                TimeElapsed += Time.deltaTime;
                if (TimeElapsed >= RateOfFire)
                {
                    TimeElapsed = 0;
                    if (BulletPool != null)
                    {
                        //plays  animation
                        if (Anim != null)
                        {
                            Anim.enabled = true;
                            Anim.Play(0, 0, 0);
                        }
                        //plays particle
                        if (GunFire != null)
                        {
                            GameObject particle = GunFire.GetPooledObject();
                            if (particle == null)
                                return;
                            particle.transform.position = Muzzle[0].transform.position;
                            particle.transform.rotation = Muzzle[0].transform.rotation;
                            particle.gameObject.SetActive(true);
                        }
                        //spawns the bullet and sets it to be positioned by muzzle
                        GameObject bulletObj = BulletPool.GetPooledObject();
                        bulletObj.transform.position = Muzzle[0].transform.position;
                        bulletObj.transform.rotation = Muzzle[0].transform.rotation;
                        bulletObj.SetActive(true);
                        Bullet newBullet = bulletObj.GetComponent<Bullet>();
                        newBullet.SetBullet(Damage, TowerStats.bulletSpeed, Target.transform, isIce);
                    }
                }
            }
        }
        else
        {
            //sets animation to disabled
            if (Anim != null)
            {
                Anim.enabled = false;
            }
        }
    }

    //sets element when icon is pressed
    public void SetElement(bool element)
    {
        isIce = element;
    }

}
