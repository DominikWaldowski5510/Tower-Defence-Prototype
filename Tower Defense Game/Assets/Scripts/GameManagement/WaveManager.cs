using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Spawns waves and handles what gets spawned
public class WaveManager : MonoBehaviour
{
    [Header("Wave button system")]
    [SerializeField] private Image availabilityButton = null;                             //image of the button that shows if the next wave can be triggered or not
    [SerializeField] private Sprite availableWave = null, unavailableWave = null;         //sprites that show whenever the object is active or inactive 
    [Header("Initializers")]
    public static WaveManager instance;                                                     //instance of the object to make it accessible easier
    private List<GameObject> enemiesInWave = new List<GameObject>();       //how many enemies are currently being spawned
    private float delayBetweenSpawn = 0.5f;                                                 //delay between spawning each enemy
    [SerializeField] private List<SingleWave> availableWaves = new List<SingleWave>();       //all available waves that can spawn within the game session
    private bool isWaveStarted = false;                                                     // checks whenever new wave can be started
    [Header("Wave Management")]
    private int waveNumber;
    [SerializeField] private Text WaveNumberText = null;                                   //Text that shows our current wave
    private List<Tile> pathTiles = new List<Tile>();                     //The path that the enemies travel by
    public List<Tile> PathTiles { get => pathTiles; set => pathTiles = value; }


    //setting up the instance of this class
    private void Awake()
    {
        instance
            = this;
    }

    //default all wave values to 0
    private void Start()
    {
        waveNumber = 0;
        isWaveStarted = false;
        WaveNumberText.gameObject.SetActive(false);
    }

    //Updates the number of the current wave
    private void UpdateWaveText()
    {
        WaveNumberText.gameObject.SetActive(true);
        WaveNumberText.text = "Current Wave: " + waveNumber + "/100";
    }
    //Controls spawning of the enemies.
    private IEnumerator SpawnEnemy()
    {
        //set up the default values of the wave, like button colour resetting list 
        availabilityButton.sprite = unavailableWave;
        isWaveStarted = true;
        enemiesInWave.Clear();
        delayBetweenSpawn =
        availableWaves[waveNumber - 1].spawnBetween;
        //spawns every enemy from the single wave scriptable object, disables them and adds them to list of enemies in wave
        for (int i = 0; i < availableWaves[waveNumber - 1].enemiesToSpawn.Length; i++)
        {
            GameObject go = Instantiate(availableWaves[waveNumber - 1].enemiesToSpawn[i]) as GameObject;
            go.transform.position = pathTiles[0].PathPoint.transform.position;
            go.name = "Enemy";
            enemiesInWave.Add(go);
            go.SetActive(false);
        }
        //enabled enemies from list of enemies with a time interval in between for each enemy in list
        int currentlyActive = 0;
        while (currentlyActive < availableWaves[waveNumber - 1].enemiesToSpawn.Length)
        {
            enemiesInWave[currentlyActive].SetActive(true);
            currentlyActive++;
            if (currentlyActive > enemiesInWave.Count)
            {
                currentlyActive = enemiesInWave.Count;
            }
            yield return new WaitForSeconds(delayBetweenSpawn);
        }
        isWaveStarted = false;
        availabilityButton.sprite = availableWave;

        yield return null;
    }

    //starts the wave with the button click
    public void StartWave()
    {
        if (GameManager.instance.GameLost == false)
        {
            if (isWaveStarted == false)
            {
                waveNumber++;
                UpdateWaveText();
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    //grabs the path for newly spawned enemy
    public Transform[] GetPath()
    {
        List<Transform> enemyPath = new List<Transform>();
        for (int i = 0; i < pathTiles.Count; i++)
        {
            enemyPath.Add(pathTiles[i].PathPoint);
        }
        return enemyPath.ToArray();
    }
}
