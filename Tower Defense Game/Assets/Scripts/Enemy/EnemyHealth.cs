using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles any damage dealt to the enemy as well as any debuffs applied
public class EnemyHealth : MonoBehaviour
{
    private Camera cam;                                                                      //reference to the active camera on scene
    private List<ApplyingDebuff> appliedDebuffs = new List<ApplyingDebuff>();               //stores all the debuffs that are currently applied to the enemy
    private Pooling deathPool = null;                                                       //stores the pooling script that triggers the death object when the enemy dies
    [SerializeField] private Image healthBar = null;                                         //stores reference to health bar of the enemy
    [SerializeField] private GameObject burnEffect = null;                                    //the visual representation of the enemy being under the burn debuff
    [SerializeField] private Transform healthTransform = null;                               //UI canvas attached to the enemy that displays health above the enemy
    [Header("Health")]
    [SerializeField] BaseEnemy enemy = null;                                                 //scriptable object that contains all enemy information such as health or speed values
    private float currentHealth = 0;                                                         //stores current health of the enemy so we can subtract from it when enemy gets hit
    private float maxHealth = 0;
    private float[] difficultyMultiplayer = new float[5] { 0.5f, 0.75f, 1f, 1.25f, 1.5f };
    public BaseEnemy Enemy { get => enemy; }
    public float CurrentHealth { get => currentHealth; }
    public List<ApplyingDebuff> AppliedDebuffs { get => appliedDebuffs; }


    //gets the main camera that is currently on the scene
    private void Start()
    {
        cam = Camera.main;
    }

    //sets the default start values such as the health and health bar
    private void OnEnable()
    {
        deathPool = GameObject.Find("DeathPool").GetComponent<Pooling>();
        maxHealth = enemy.health * difficultyMultiplayer[(int)SaveDataManager.instance.Difficulties];
        currentHealth = maxHealth;
        InvokeRepeating("DebuffCounter", 0, 1f);
        UpdateUI();
    }

    //when the object is disabled it stops the debuff check function 
    private void OnDisable()
    {
        CancelInvoke("DebuffCounter");
    }

    //checks through all debuffs and applies their effects while substracting from their remaining duration of debuff
    private void DebuffCounter()
    {
        if (PauseMenu.instance.IsPaused == false)
        {
            if (appliedDebuffs.Count > 0)
            {
                for (int i = 0; i < appliedDebuffs.Count; i++)
                {
                    TakeDamage(appliedDebuffs[i].debuffDamage);
                    appliedDebuffs[i].debuffDuration -= 1;
                    if (appliedDebuffs[i].debuffDuration <= 0)
                    {
                        appliedDebuffs.RemoveAt(i);
                    }
                }
            }
        }
    }


    //forces the health to look at the camera, and runs the fire debuff if burned
    private void Update()
    {
        healthTransform.LookAt(cam.transform);

        bool hasBurn = false;
        for (int i = 0; i < appliedDebuffs.Count; i++)
        {
            if (appliedDebuffs[i].typeOfDebuff == ApplyingDebuff.DebuffType.Fire)
            {
                hasBurn = true;
            }
        }
        if (hasBurn == false)
        {

            burnEffect.SetActive(false);
        }
        else if (hasBurn == true)
        {
            burnEffect.SetActive(true);
        }
    }

    //adds a debuff to the list of debuff if the current debuff doesnt already exist within the debuffs list
    public void DealDebuff(ApplyingDebuff debuffBeingApplied)
    {
        if (!appliedDebuffs.Contains(debuffBeingApplied))
            appliedDebuffs.Add(debuffBeingApplied);
    }

    //passes a variable that removes health from current health pool of the enemy, kills the enemy if health reaches 0
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateUI();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameObject deadObj = deathPool.GetPooledObject();
            deadObj.transform.position = this.transform.position;
            deadObj.SetActive(true);
            GameManager.instance.Money += enemy.value;
            GameManager.instance.KillCount += 1;
            GameManager.instance.UpdateMoney();
            Destroy(this.gameObject);
        }
    }

    //updates the health bar based on current and maximum health
    private void UpdateUI()
    {
        healthBar.fillAmount = (currentHealth / 100) / (maxHealth / 100);
    }
}

