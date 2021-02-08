using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Handles all game related features such as money, health all generic UI
public class GameManager : MonoBehaviour
{
    [SerializeField] private TowerAvailability towersCosts = null;          //stores reference to object that allows the tower locking to update automatically
    public static GameManager instance;                                      //instance of gamemanager script
    [Header("Player")]
    private int money = 100;                                //players available money to spend for towers and upgrades
    [SerializeField] private Text playerHealthText = null;                   //players total health amount text
    [SerializeField] private Text playerMoneyText = null;                    //players total money amount text
    [SerializeField] private Text endGameLostText = null;                 //Text that appears when we lose the game
    private bool gameLost = false;                                        //a value that sets to true when the game stops used for  checking lose/win state of the game
    private int playerMaxHealth = 10, playerHealth;                       //player health variables
    [SerializeField] private AudioSource healthLostSound = null;            //plays sound for health lost
    private int killCount;
    public int Money { get => money; set => money = value; }
    public int KillCount { get => killCount; set => killCount = value; }
    public bool GameLost { get => gameLost; }


    //creates instance of game manager class
    private void Awake()
    {
        instance = this;
    }

    //sets up default values and updates all start up UI elements
    private void Start()
    {
        killCount = 0;
        gameLost = false;
        endGameLostText.gameObject.SetActive(false);
        playerHealth = playerMaxHealth;
        playerHealthText.text = "Player Health: " + playerHealth + "/" + playerMaxHealth;

        if (SaveDataManager.instance.DefaultMoney != 0)
        {
            money = (int)SaveDataManager.instance.DefaultMoney;
        }
        else
        {
            money = 200;
        }
        UpdateMoney();

    }


    //updates player health when an enemy reaches the end of the path
    public void UpdatePlayerHealth()
    {
        playerHealth--;
        PlaySound();
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            if (gameLost == false)
            {
                GameOver();
            }
        }
        playerHealthText.text = "Player Health: " + playerHealth + "/" + playerMaxHealth;
    }

    //The game has now been won
    public void GameWon()
    {
        gameLost = true;
        endGameLostText.gameObject.SetActive(true);
        endGameLostText.text = "Game Over, You Win!\n Total Kills: " + KillCount + "\n Press Space to return to Main menu";
    }

    //Displays game over text when health reaches 0 and sets the game as lost
    private void GameOver()
    {
        gameLost = true;
        endGameLostText.gameObject.SetActive(true);
        endGameLostText.text = "Game Over\n Total Kills: " + KillCount + "\n Press Space to return to Main menu";
    }

    //allows user to leave to main menu when the game is lost
    private void Update()
    {
        if (gameLost == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    //updates amount of money user possesses in the ui text as well as updates the availability of towers
    public void UpdateMoney()
    {
        playerMoneyText.text = "Money:" + money + "$";
        towersCosts.UpdateAvailability();
    }

    //plays sound of the tower gun
    private void PlaySound()
    {
        healthLostSound.volume = SaveDataManager.instance.Sound;
        healthLostSound.Play();
    }
}
