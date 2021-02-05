using UnityEngine;

//class that handles bullet flight
public class Bullet : MonoBehaviour
{
    private float damage;                                           //local version of the bullets damage
    [SerializeField] private Transform target = null;                              //local reference to target
    private float bulletSpeed = 10f;                             //how fast the bullet travels
    [SerializeField] private float disappearTime = 5;           //how long it takes for bullet to disappear after it spawns
    [Header("Trail Settings")]
    [SerializeField] private TrailRenderer trail = null;            //reference to the trail of the bullet
    [SerializeField] private float trailTime = 0.05f, trailDistance = 0.05f;         //how long the trail lasts for and how long the trail spreads to
    [SerializeField] private AudioClip soundClip = null;                //sound that will be played
    [SerializeField] private AudioSource source = null;                 //source object that the sound is played froms
    public Transform Target { get => target; }
    public float BulletSpeed { get => bulletSpeed; }
    public float Damage { get => damage; }
    public float DisappearTime { get => disappearTime; }


    //sets up default values when object is enabled and starts disappearing function
    public virtual void OnEnable()
    {
        if (trail != null)
        {
            trail.time = trailTime;
            trail.minVertexDistance = trailDistance;
            trail.enabled = true;
        }
        Invoke("DisableBullet", disappearTime);
    }

    //disables all values by resetting them
    public virtual void OnDisable()
    {
        if (trail != null)
        {
            trail.time = 0;
            trail.minVertexDistance = 0;
            trail.Clear();
            trail.enabled = false;
        }
        CancelInvoke("DisableBullet");
    }


    //sets up bullet once its instantiated
    public virtual void SetBullet(float _damage, float _bulletSpeed, Transform _target = null, bool elementType = false)
    {
        damage = _damage;
        target = _target;
        bulletSpeed = _bulletSpeed;
    }

    //handles the logic of the b ullet moving
    private void Update()
    {
        BulletTravel();
    }

    //handles the movement of the bullet
    public virtual void BulletTravel()
    {
        if (target != null && target.gameObject.activeInHierarchy == true)
        {
            Vector3 moveDIr = this.transform.position - target.transform.position;
            float magnitide = moveDIr.magnitude;
            transform.position -= (moveDIr / magnitide) * bulletSpeed * Time.deltaTime;
        }
        else
        {
            DisableBullet();
        }
    }

    //detects collision for enemies to take damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Bullet" || other.gameObject.tag != "Tower")
        {
            HandleCollision(other);
        }
    }

    //handles collsion between bullet and target, disables bullet on contact and deals damage to the enemy
    public virtual void HandleCollision(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyHealth healthOfEnemy = other.GetComponent<EnemyHealth>();
            healthOfEnemy.TakeDamage(damage);
            DisableBullet();
        }
    }

    //disables the bullet 
    public virtual void DisableBullet()
    {
        if (soundClip != null)
        {
            PlayButtonSound();
        }
        this.gameObject.SetActive(false);
    }

    //plays sound of the tower gun
    private void PlayButtonSound()
    {
        source.clip = soundClip;
        source.volume = SaveDataManager.instance.Sound;
        source.Play();
    }

}
