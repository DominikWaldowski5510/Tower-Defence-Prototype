using System.Collections.Generic;
using UnityEngine;

//handles the functionality of a tower
public class Tower : MonoBehaviour
{
    private List<ApplyingDebuff> appliedDBuffs = new List<ApplyingDebuff>();        //stores all buffs applied to the tower
    [SerializeField] private BaseTower towerStats = null;                    //all data for this specific tower
    [SerializeField] private LayerMask enemyLayer = 0;                       //layer on which all enemies are found
    [SerializeField] private GameObject rotator = null;                   //area that has to be rotated to face the enemy
    private float rotationSpeed = 20;                                     //how fast the tower rotates
    private float range;                                            //how large radius of the tower is
    private GameObject target = null;                          //who is the target to shoot
    [Header("Shooting")]
    [SerializeField] private Pooling bulletPool = null;                     //pooling of our bullets
    [SerializeField] private GameObject[] muzzle = null;         //point from which bullet spawns
    [SerializeField] private bool usesSmokeAnim = true;             //smoking animation that comes off from the gun muzzle
    [SerializeField] private bool freezeXRotation = false;
    //Tower stats for shooting
    private float rateOfFire = 0.4f;
    private float timeElapsed = 0;
    private float damage;
    private Pooling gunFire;
    private int currentTowerTier = 0;                                   //level upgrade of the tower
    [SerializeField] private AudioClip soundClip = null;                //sound that will be played
    [SerializeField] private AudioSource source = null;                 //source object that the sound is played from

    //used for setting up the mode in which we fire
    public enum FiringMode
    {
        ClosestTarget,
        FurthestTarget,
        LowestHealthTarget,
        HighestHealthTarget
    }
    public FiringMode modeOfFire = FiringMode.ClosestTarget;

    [SerializeField] private Animator anim = null;
    public BaseTower TowerStats { get => towerStats; }
    public GameObject Target { get => target; set => target = value; }
    public GameObject[] Muzzle { get => muzzle; set => muzzle = value; }
    public float Damage { get => damage; set => damage = value; }
    public float RateOfFire { get => rateOfFire; }
    public float Range { get => range; }
    public int CurrentTowerTier { get => currentTowerTier; set => currentTowerTier = value; }
    public float TimeElapsed { get => timeElapsed; set => timeElapsed = value; }
    public Pooling BulletPool { get => bulletPool; set => bulletPool = value; }
    public LayerMask EnemyLayer { get => enemyLayer; set => enemyLayer = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public Pooling GunFire { get => gunFire; set => gunFire = value; }

    //sets up default tower valeus
    private void OnEnable()
    {
        currentTowerTier = 0;
        UpdateStats();
        if (anim != null)
        {
            if (anim != null)
                anim.speed = rateOfFire;
            anim.enabled = false;
        }
        if (usesSmokeAnim == true)
        {
            gunFire = GameObject.Find("GunFire").GetComponent<Pooling>();
        }
        InvokeRepeating("DebuffCounter", 1, 1f);
        InvokeRepeating("FiringType", 0, 0.4f);
    }

    //disables buff check function
    private void OnDisable()
    {
        CancelInvoke("DebuffCounter");
        CancelInvoke("FiringType");
    }

    //sets firing mode of the tower
    public void SetFiringMode(int firingModeNum)
    {
        modeOfFire = (FiringMode)firingModeNum;
    }

    //Determines which mode of fire the tower will use
    private void FiringType()
    {
        switch (modeOfFire)
        {
            case FiringMode.ClosestTarget:
                FindClosestTarget();
                break;
            case FiringMode.FurthestTarget:
                FindFurthestTarget();
                break;
            case FiringMode.LowestHealthTarget:
                FindLowestHealthTarget();
                break;
            case FiringMode.HighestHealthTarget:
                FindHighestHealthTarget();
                break;
        }
    }

   

    //handles aiming shooting and firing per frame
    private void Update()
    {
        if (rotator != null)
        {
            Aiming();
        }
        Shoot();
    }

    #region firing Modules
    //finds the nearest target to our turret
    private void FindClosestTarget()
    {
        Collider[] targets = Physics.OverlapSphere(this.transform.position, range, enemyLayer);
        Transform nearestTarget = null;
        float maxDistance = Mathf.Infinity;
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (Vector3.Distance(this.transform.position, targets[i].transform.position) < maxDistance)
                {
                    nearestTarget = targets[i].transform;
                    maxDistance = Vector3.Distance(this.transform.position, targets[i].transform.position);
                }
            }
            if (nearestTarget != this.transform)
            {
                target = nearestTarget.gameObject;
            }
        }
        else
        {
            target = null;
        }
    }

    //Aim at the furthest away enemy
    private void FindFurthestTarget()
    {
        Collider[] targets = Physics.OverlapSphere(this.transform.position, range, enemyLayer);
        Transform furthestTarget = null;
        float minDistance = 0;
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (Vector3.Distance(this.transform.position, targets[i].transform.position) > minDistance)
                {
                    furthestTarget = targets[i].transform;
                    minDistance = Vector3.Distance(this.transform.position, targets[i].transform.position);
                }
            }
            if (furthestTarget != this.transform)
            {
                target = furthestTarget.gameObject;
            }
        }
        else
        {
            target = null;
        }
    }

    //aim at the enemy with lowest health in range
    private void FindHighestHealthTarget()
    {
        Collider[] targets = Physics.OverlapSphere(this.transform.position, range, enemyLayer);
        float highestHealth = 0;
        Transform furthestTarget = null;

        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                float health = targets[i].GetComponent<EnemyHealth>().CurrentHealth;
                if (health > highestHealth)
                {
                    if (furthestTarget != this.transform)
                    {
                        highestHealth = health;
                        furthestTarget = targets[i].transform;
                    }
                }

            }
            target = furthestTarget.gameObject;
        }
        else
        {
            target = null;
        }
    }

    //aim at the enemy with lowest health in range
    private void FindLowestHealthTarget()
    {
        Collider[] targets = Physics.OverlapSphere(this.transform.position, range, enemyLayer);
        float lowestHealth = Mathf.Infinity;
        Transform nearestTarget = null;

        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                float health = targets[i].GetComponent<EnemyHealth>().CurrentHealth;
                if (health < lowestHealth)
                {
                    lowestHealth = health;
                    nearestTarget = targets[i].transform;
                }

            }
            target = nearestTarget.gameObject;
        }
        else
        {
            target = null;
        }
    }

    #endregion

    //rotates the top turret of the tower
    private void Aiming()
    {
        if (target != null)
        {
            if (target.gameObject.activeInHierarchy == true)
            {
                Vector3 targetDirection = target.transform.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
                Vector3 rotation = Quaternion.Lerp(rotator.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
                if (freezeXRotation == false)
                {
                    rotator.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
                }
                else
                {
                    rotator.transform.rotation = Quaternion.Euler(rotator.transform.rotation.x, rotation.y, rotator.transform.rotation.z);
                }
            }
        }
    }

    //handles the shooting logic
    public virtual void Shoot()
    {
        if (target != null)
        {
            if (target.gameObject.activeInHierarchy == true)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= rateOfFire)
                {
                    timeElapsed = 0;
                    if (bulletPool != null)
                    {
                        //plays  animation
                        if (anim != null)
                        {
                            anim.enabled = true;
                            anim.Play(0, 0, 0);
                        }
                        //plays particle
                        if (gunFire != null)
                        {
                            GameObject particle = gunFire.GetPooledObject();
                            if (particle == null)
                                return;
                            particle.transform.position = muzzle[0].transform.position;
                            particle.transform.rotation = muzzle[0].transform.rotation;
                            particle.gameObject.SetActive(true);
                        }
                        //plays sound
                        if (soundClip != null)
                        {
                            PlayButtonSound();
                        }
                        //spawns the bullet and sets it to be positioned by muzzle
                        GameObject bulletObj = bulletPool.GetPooledObject();
                        bulletObj.transform.position = muzzle[0].transform.position;
                        bulletObj.transform.rotation = muzzle[0].transform.rotation;
                        bulletObj.SetActive(true);
                        Bullet newBullet = bulletObj.GetComponent<Bullet>();
                        newBullet.SetBullet(damage, towerStats.bulletSpeed, target.transform);
                    }
                }
            }
        }
        else
        {
            //sets animation to disabled
            if (anim != null)
            {
                anim.enabled = false;
            }
        }
    }

    //plays sound of the tower gun
    private void PlayButtonSound()
    {
        source.clip = soundClip;
        source.volume = SaveDataManager.instance.Sound;
        source.Play();
    }

    //adds buff to our list of buffs
    public void AddBuff(ApplyingDebuff newBuff)
    {
        if (!appliedDBuffs.Contains(newBuff))
            appliedDBuffs.Add(newBuff);
    }

    //checks through the buffs to remove if necessary
    private void DebuffCounter()
    {
        if (PauseMenu.instance.IsPaused == false)
        {
            if (appliedDBuffs.Count > 0)
            {
                for (int i = 0; i < appliedDBuffs.Count; i++)
                {
                    if (appliedDBuffs[i].typeOfDebuff == ApplyingDebuff.DebuffType.RangeBoost)
                    {
                        range = towerStats.towerTier[CurrentTowerTier].range + appliedDBuffs[i].debuffDamage;
                    }
                    else if (appliedDBuffs[i].typeOfDebuff == ApplyingDebuff.DebuffType.DamageBoost)
                    {
                        damage = towerStats.towerTier[CurrentTowerTier].damage + appliedDBuffs[i].debuffDamage;
                    }
                    appliedDBuffs[i].debuffDuration -= 1;
                    if (appliedDBuffs[i].debuffDuration <= 0)
                    {
                        if (appliedDBuffs[i].typeOfDebuff == ApplyingDebuff.DebuffType.RangeBoost)
                        {
                            range = towerStats.towerTier[CurrentTowerTier].range;
                        }
                        else if (appliedDBuffs[i].typeOfDebuff == ApplyingDebuff.DebuffType.DamageBoost)
                        {
                            damage = towerStats.towerTier[CurrentTowerTier].damage;
                        }
                        appliedDBuffs[i].debuffDuration = 0;
                        appliedDBuffs.RemoveAt(i);
                    }
                }
            }
        }
    }

    //updates the statistics of the tower
    public void UpdateStats()
    {
        damage = towerStats.towerTier[currentTowerTier].damage;
        range = towerStats.towerTier[currentTowerTier].range;
        rateOfFire = towerStats.towerTier[currentTowerTier].rateOfFire;
    }

    //draws range so it can be visible in inspector
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }


}
